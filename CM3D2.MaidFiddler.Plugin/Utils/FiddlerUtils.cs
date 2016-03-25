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
        private const int ERROR_UNPATCHED = 0;
        private const int ERROR_OLD_PATCH = 1;
        private const int ERROR_CHECK_FAILED_TITLE = 2;
        private static bool errorThrown;
        public static int GameVersion => (int) typeof (Misc).GetField(nameof(Misc.GAME_VERSION)).GetValue(null);

        public static bool CheckPatcherVersion()
        {
            try
            {
                MaidFiddlerPatchedAttribute[] attributes =
                (MaidFiddlerPatchedAttribute[])
                typeof (Maid).GetCustomAttributes(typeof (MaidFiddlerPatchedAttribute), false);
                if (attributes.Length != 1)
                {
                    MessageBox.Show(
                    string.Format(Translation.GetTranslation("ERROR_UNPATCHED"), MaidFiddler.VERSION),
                    $"{Translation.GetTranslation("ERROR_UNPATCHED_TITLE")} (ID: {ERROR_UNPATCHED})",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
                    return false;
                }
                if (MaidFiddler.SUPPORTED_PATCH_MIN > attributes[0].PatchVersion
                    || attributes[0].PatchVersion > MaidFiddler.SUPPORTED_PATCH_MAX)
                {
                    MessageBox.Show(
                    string.Format(
                    Translation.GetTranslation("ERROR_OLD_PATCH"),
                    MaidFiddler.VERSION,
                    attributes[0].PatchVersion,
                    MaidFiddler.SUPPORTED_PATCH_MIN,
                    MaidFiddler.SUPPORTED_PATCH_MAX),
                    $"{Translation.GetTranslation("ERROR_OLD_PATCH_TITLE")} (ID: {ERROR_OLD_PATCH})",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                Translation.GetTranslation("ERROR_CHECK_FAILED"),
                $"{Translation.GetTranslation("ERROR_CHECK_FAILED_TITLE")} (ID: {ERROR_CHECK_FAILED_TITLE})",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
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
            string filename =
            $"MaidFiddler_err_{Convert.ToBase64String(BitConverter.GetBytes(DateTime.Now.Ticks)).Replace('/', '$')}.txt";
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

            MessageBox.Show(
            $"{Translation.GetTranslation("ERROR_LOG_MESSAGE")}\n{(dumpCreated ? string.Format(Translation.GetTranslation("ERROR_LOG_CREATED"), filename) : string.Format(Translation.GetTranslation("ERROR_LOG_NOT_CREATED"), sb))}",
            Translation.GetTranslation("ERROR_LOG_MESSAGE_TITLE"),
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);

            MaidFiddlerGUI guiLoc = plugin.Gui;
            plugin.Gui = null;
            guiLoc?.Close(true);
            errorThrown = true;
        }
    }
}