using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Gui;
using JsonFx.Json;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public struct UpdateInfo
    {
        public string Changelog;
        public bool IsAvailable;
        public string Version;
    }

    public static class FiddlerUtils
    {
        private const string ERROR_UNPATCHED = "ERR_UNPATCHED";
        private const string ERROR_OLD_PATCH = "ERR_OLD_PATCH";
        private const string ERROR_CHECK_FAILED = "ERR_CHECK_FAILED";
        private const string ERROR_OLD_GAME_VERSION = "ERR_OLD_GAME_VERSION";
        private static bool errorThrown;

        private static readonly Regex versionPattern =
                new Regex(@"^(?<prefix>[A-z\-]+)(?<major>\d+)\.(?<minor>\d+)(\.(?<patch>\d+))?$");

        public static int GameVersion => (int) typeof(Misc).GetField(nameof(Misc.GAME_VERSION)).GetValue(null);
        public static bool PlusPack2Installed => GameUty.CheckPackFlag(PluginData.Type.PP002);
        public static bool PlusPackInstalled => GameUty.CheckPackFlag(PluginData.Type.PP001);
        public static UpdateInfo UpdateInfo { get; private set; }
        public static bool UpdatesChecked { get; private set; }

        public static Dictionary<string, object> GetLatestReleasedVersion()
        {
            Dictionary<string, object> result;
            Debugger.WriteLine(
                LogLevel.Info,
                $"Getting info about the latest version from {MaidFiddler.RELEASES_LATEST_REQUEST_URL}");
            HttpWebRequest releasesRequest =
                    (HttpWebRequest) WebRequest.Create(MaidFiddler.RELEASES_LATEST_REQUEST_URL);
            releasesRequest.UserAgent = "Maid Fiddler Update Checker";
            releasesRequest.Accept = "application/json";
            HttpWebResponse wr = (HttpWebResponse) releasesRequest.GetResponse();
            Debugger.WriteLine(LogLevel.Info, "Got a response!");
            Debugger.WriteLine(LogLevel.Info, $"Response code: {wr.StatusCode}");
            if (!wr.ContentType.StartsWith("application/json"))
            {
                Debugger.WriteLine(
                    LogLevel.Error,
                    $"Could not load version data! Content gotten: {wr.ContentType} Skipping version checking...");
                return null;
            }
            JsonReader jr = new JsonReader(wr.GetResponseStream());
            result = jr.Deserialize<Dictionary<string, object>>();
            wr.Close();
            return result;
        }

        public static void RunUpdateChecker()
        {
            Debugger.WriteLine(LogLevel.Info, "Checking for updates...");
            UpdateInfo = new UpdateInfo();
            Dictionary<string, object> versionInfo;
            try
            {
                versionInfo = GetLatestReleasedVersion();
                if (versionInfo == null)
                    throw new Exception("No version data downloaded!");
            }
            catch (WebException e)
            {
                Debugger.WriteLine(LogLevel.Error, $"Failed to get latest version info! Reason: {e}");
                Debugger.WriteLine(
                    LogLevel.Error,
                    $"Response: {new StreamReader(e.Response.GetResponseStream()).ReadToEnd()}");
                UpdatesChecked = true;
                return;
            }
            catch (Exception e)
            {
                Debugger.WriteLine(LogLevel.Error, $"Failed to get latest version info! Reason: {e}");
                UpdatesChecked = true;
                return;
            }

            Debugger.WriteLine(LogLevel.Info, "Got latest version info!");
            Debugger.WriteLine(LogLevel.Info, $"Current version: {MaidFiddler.VERSION_TAG}");
            Debugger.WriteLine(LogLevel.Info, $"Latest version: {versionInfo["tag_name"]}");

            Match current = versionPattern.Match(MaidFiddler.VERSION_TAG);
            Match latest = versionPattern.Match((string) versionInfo["tag_name"]);
            if (!latest.Success)
            {
                Debugger.WriteLine(
                    LogLevel.Error,
                    "Could not process version tag. Tag is probably older than Beta 0.11 Skipping version check.");
                UpdatesChecked = true;
                return;
            }

            bool isNewAvailable;
            try
            {
                string currentVersionPrefix = current.Groups["prefix"].Value;
                int currentMajor = int.Parse(current.Groups["major"].Value);
                int currentMinor = int.Parse(current.Groups["minor"].Value);
                int currentPatch = current.Groups["patch"].Success ? int.Parse(current.Groups["patch"].Value) : 0;

                string latestVersionPrefix = latest.Groups["prefix"].Value;
                int latestMajor = int.Parse(latest.Groups["major"].Value);
                int latestMinor = int.Parse(latest.Groups["minor"].Value);
                int latestPatch = latest.Groups["patch"].Success ? int.Parse(latest.Groups["patch"].Value) : 0;

                bool samePrefix = latestVersionPrefix == currentVersionPrefix;
                bool sameMajor = latestMajor == currentMajor;
                bool sameMinor = latestMinor == currentMinor;

                isNewAvailable = latestVersionPrefix == "v" && currentVersionPrefix == "Beta-" ||
                                 samePrefix && latestMajor > currentMajor ||
                                 samePrefix && sameMajor && latestMinor > currentMinor ||
                                 samePrefix && sameMajor && sameMinor && latestPatch > currentPatch;
            }
            catch (Exception e)
            {
                Debugger.WriteLine(LogLevel.Error, $"Failed to parse tool versions! Reason: {e}");
                UpdatesChecked = true;
                return;
            }

            if (!isNewAvailable)
            {
                UpdatesChecked = true;
                return;
            }
            UpdateInfo newUpdateInfo = new UpdateInfo
            {
                IsAvailable = true,
                Version = (string) versionInfo["name"],
                Changelog = ((string) versionInfo["body"]).Replace("*", string.Empty)
            };
            UpdateInfo = newUpdateInfo;
            UpdatesChecked = true;
        }

        public static bool CheckPatcherVersion()
        {
            string title, text;
            try
            {
                if (GameVersion < MaidFiddler.SUPPORTED_MIN_CM3D2_VERSION)
                {
                    title = Translation.IsTranslated("ERROR_OLD_GAME_VERSION_TITLE")
                        ? $"{Translation.GetTranslation("ERROR_OLD_GAME_VERSION_TITLE")} (ID: {ERROR_OLD_GAME_VERSION})"
                        : $"Unsupported game version (ID: {ERROR_OLD_GAME_VERSION})";
                    text = Translation.IsTranslated("ERROR_OLD_GAME_VERSION")
                        ? string.Format(
                            Translation.GetTranslation("ERROR_OLD_GAME_VERSION"),
                            MaidFiddler.VERSION,
                            $"{GameVersion / 100}.{GameVersion % 100}",
                            $"{MaidFiddler.SUPPORTED_MIN_CM3D2_VERSION / 100}.{MaidFiddler.SUPPORTED_MIN_CM3D2_VERSION % 100}")
                        : $"Maid Fiddler {MaidFiddler.VERSION} detects that you are running an unsupported version of CM3D2.\nYou are running CM3D2 version {GameVersion / 100}.{GameVersion % 100}.\nMaid Fiddler requires CM3D2 version {MaidFiddler.SUPPORTED_MIN_CM3D2_VERSION / 100}.{MaidFiddler.SUPPORTED_MIN_CM3D2_VERSION % 100} or higher to function.\nThe game may be unstable, until it is updated.";

                    MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return false;
                }
                MaidFiddlerPatchedAttribute[] attributes =
                        (MaidFiddlerPatchedAttribute[])
                        typeof(Maid).GetCustomAttributes(typeof(MaidFiddlerPatchedAttribute), false);
                if (attributes.Length != 1)
                {
                    title = Translation.IsTranslated("ERROR_UNPATCHED_TITLE")
                        ? $"{Translation.GetTranslation("ERROR_UNPATCHED_TITLE")} (ID: {ERROR_UNPATCHED})"
                        : $"Did you forget something? (ID: {ERROR_UNPATCHED})";
                    text = Translation.IsTranslated("ERROR_UNPATCHED")
                        ? string.Format(Translation.GetTranslation("ERROR_UNPATCHED"), MaidFiddler.VERSION)
                        : $"Maid Fiddler {MaidFiddler.VERSION} detects that Assembly-CSharp.dll hasn't been patched or is patched with an outdated version of Maid Fiddler Patcher.\nIn order to function, Maid Fiddler requires that the assembly is patched with ReiPatcher or Sybaris.\nThe game may be unstable, until it is patched.\n\nMake sure you have the latest patcher downloaded and re-patch the game.";

                    MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return false;
                }
                if (MaidFiddler.SUPPORTED_PATCH_MIN > attributes[0].PatchVersion ||
                    attributes[0].PatchVersion > MaidFiddler.SUPPORTED_PATCH_MAX)
                {
                    title = Translation.IsTranslated("ERROR_OLD_PATCH_TITLE")
                        ? $"{Translation.GetTranslation("ERROR_OLD_PATCH_TITLE")} (ID: {ERROR_OLD_PATCH})"
                        : $"Unsupported patch version (ID: {ERROR_OLD_PATCH})";

                    text = Translation.IsTranslated("ERROR_OLD_PATCH")
                        ? string.Format(
                            Translation.GetTranslation("ERROR_OLD_PATCH"),
                            MaidFiddler.VERSION,
                            attributes[0].PatchVersion,
                            MaidFiddler.SUPPORTED_PATCH_MIN,
                            MaidFiddler.SUPPORTED_PATCH_MAX)
                        : $"Maid Fiddler {MaidFiddler.VERSION} detected that Assembly-CSharp.dll is patched with an unsupported version of Maid Fiddler Patcher.\nThe game was patched with patcher version {attributes[0].PatchVersion}, but this version of Maid Fiddler supports only patcher versions {MaidFiddler.SUPPORTED_PATCH_MIN} through {MaidFiddler.SUPPORTED_PATCH_MAX}.\nThe game may not be functional until it is re-patched!\n\nMake sure you have the latest patcher downloaded and re-patch the game.";

                    MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            catch (Exception)
            {
                title = Translation.IsTranslated("ERROR_CHECK_FAILED_TITLE")
                    ? $"{Translation.GetTranslation("ERROR_CHECK_FAILED_TITLE")} (ID: {ERROR_CHECK_FAILED})"
                    : $"Failed to locate the patcher (ID: {ERROR_CHECK_FAILED})";
                text = Translation.IsTranslated("ERROR_CHECK_FAILED")
                    ? Translation.GetTranslation("ERROR_CHECK_FAILED")
                    : "Maid Fiddler failed to check that Assembly-CSharp.dll has been patched properly. Perhaps some other patch is causing the issue.\nMaid Fiddler can still be used, but it may cause errors.\n\nPlease re-patch the game and make sure no other plug-ins cause any interference.";

                MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return true;
            }
            return true;
        }

        public static bool RemoteCertificateValidationCallback(object sender,
                                                               X509Certificate certificate,
                                                               X509Chain chain,
                                                               SslPolicyErrors sslPolicyErrors)
        {
            bool pass = true;
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            foreach (X509ChainStatus t in
                chain.ChainStatus.Where(t => t.Status != X509ChainStatusFlags.RevocationStatusUnknown))
            {
                chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                bool chainIsValid = chain.Build((X509Certificate2) certificate);
                if (!chainIsValid)
                    pass = false;
            }
            return pass;
        }

        public static string GenerateFileName()
        {
            return Convert.ToBase64String(BitConverter.GetBytes(DateTime.Now.Ticks)).Replace('/', '$');
        }

        public static void ThrowErrorMessage(Exception e, string action, MaidFiddler plugin)
        {
            if (errorThrown)
                return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== Maid Fiddler DUMP ===").AppendLine();
            sb.AppendLine($"Game version: {GameVersion}");
            sb.AppendLine($"Mod version: {MaidFiddler.VERSION}");
            sb.AppendLine($"Info: {action}");
            sb.AppendLine($"Error message: {e}");
            if (e.InnerException != null)
                sb.Append($"Underlying exception: {e.InnerException}");

            bool dumpCreated;
            string filename = $"MaidFiddler_err_{GenerateFileName()}.txt";
            try
            {
                using (TextWriter tw = new StreamWriter(File.Create(filename)))
                {
                    tw.Write(sb.ToString());
                    dumpCreated = true;
                }
            }
            catch (Exception)
            {
                dumpCreated = false;
            }

            string dumpCreatedMsg =
                    $"A log named {filename} was created.\nPlease, send this log to the developer with the description of what you attempted to do";
            string dumpNotCreatedMsg =
                    $"Failed to create a dump message. Send a screenshot of the following stack trace to the developer:\n==START==\n{sb}\n==END==";

            string title = Translation.IsTranslated("ERROR_LOG_MESSAGE_TITLE")
                ? Translation.GetTranslation("ERROR_LOG_MESSAGE_TITLE")
                : "Oh noes!";
            string text = Translation.IsTranslated("ERROR_LOG_MESSAGE") &&
                          (dumpCreated && Translation.IsTranslated("ERROR_LOG_CREATED") ||
                           !dumpCreated && Translation.IsTranslated("ERROR_LOG_NOT_CREATED"))
                ? $"{Translation.GetTranslation("ERROR_LOG_MESSAGE")}\n{(dumpCreated ? string.Format(Translation.GetTranslation("ERROR_LOG_CREATED"), filename) : string.Format(Translation.GetTranslation("ERROR_LOG_NOT_CREATED"), sb))}"
                : $"Oh no! An error has occured in Maid Fiddler!\n{(dumpCreated ? dumpCreatedMsg : dumpNotCreatedMsg)}";


            MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

            MaidFiddlerGUI guiLoc = plugin.Gui;
            plugin.Gui = null;
            guiLoc?.Close(true);
            errorThrown = true;
        }
    }
}