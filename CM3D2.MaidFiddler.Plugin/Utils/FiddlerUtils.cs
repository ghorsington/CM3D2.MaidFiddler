using System;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Gui;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public static class FiddlerUtils
    {
        private const string ERROR_UNPATCHED = "ERR_UNPATCHED";
        private const string ERROR_OLD_PATCH = "ERR_OLD_PATCH";
        private const string ERROR_CHECK_FAILED = "ERR_CHECK_FAILED";
        private static bool errorThrown;
        public static int GameVersion => (int) typeof (Misc).GetField(nameof(Misc.GAME_VERSION)).GetValue(null);
        public static bool PlusPack2Installed => GameUty.CheckPackFlag(PluginData.Type.PP002);
        public static bool PlusPackInstalled => GameUty.CheckPackFlag(PluginData.Type.PP001);

        public static bool CheckPatcherVersion()
        {
            string title, text;
            try
            {
                MaidFiddlerPatchedAttribute[] attributes =
                (MaidFiddlerPatchedAttribute[])
                typeof (Maid).GetCustomAttributes(typeof (MaidFiddlerPatchedAttribute), false);
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
                if (MaidFiddler.SUPPORTED_PATCH_MIN > attributes[0].PatchVersion
                    || attributes[0].PatchVersion > MaidFiddler.SUPPORTED_PATCH_MAX)
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
            foreach (
            X509ChainStatus t in chain.ChainStatus.Where(t => t.Status != X509ChainStatusFlags.RevocationStatusUnknown))
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
                           ? Translation.GetTranslation("ERROR_LOG_MESSAGE_TITLE") : "Oh noes!";
            string text = Translation.IsTranslated("ERROR_LOG_MESSAGE")
                          && (dumpCreated && Translation.IsTranslated("ERROR_LOG_CREATED")
                              || !dumpCreated && Translation.IsTranslated("ERROR_LOG_NOT_CREATED"))
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