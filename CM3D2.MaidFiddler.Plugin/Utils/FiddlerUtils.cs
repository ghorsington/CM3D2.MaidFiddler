using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Gui;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public static class FiddlerUtils
    {
        private static bool errorThrown;

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
                    $"Maid Fiddler {MaidFiddler.VERSION} detects that the assembly hasn't been patched or is patched with an outdated version of Maid Fiddler patcher.\nIn order to function Maid Fiddler requires that the assembly is patched with ReiPatcher or Sybaris.\n\nMake sure you have the latest patcher downloaded and re-patch the game.",
                    "Did you forget something?",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
                    return false;
                }
                if (MaidFiddler.SUPPORTED_PATCH_MIN > attributes[0].PatchVersion
                    || attributes[0].PatchVersion > MaidFiddler.SUPPORTED_PATCH_MAX)
                {
                    MessageBox.Show(
                    $"Maid Fiddler {MaidFiddler.VERSION} detected that the game is patched with an unsupported version of Maid Fiddler Patcher.\nThe game was patched with patcher version {attributes[0].PatchVersion}, but this version of Maid Fiddler supports only patcher versions {MaidFiddler.SUPPORTED_PATCH_MIN} through {MaidFiddler.SUPPORTED_PATCH_MAX}.\n\nMake sure you have the latest patcher downloaded and re-patch the game.",
                    "Unsupported patch version",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                "Maid Fiddler failed to check that the game has been patched properly. Perhaps some other patch is causing in issue.\nMaid Fiddler can still be used, but it may cause errors.\n\nPlease re-patch the game and make sure no other plug-ins cause any interference.",
                "Failed to locate the patcher",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
                return true;
            }
            return true;
        }

        public static void ThrowErrorMessage(Exception e, string action)
        {
            if (errorThrown)
                return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== Maid Fiddler DUMP ===").AppendLine();
            sb.AppendLine($"Game version: {Misc.GAME_VERSION}");
            sb.AppendLine($"Mod version: {MaidFiddler.VERSION}");
            sb.AppendLine($"Info: {action}");
            sb.AppendLine($"Error message: {e}");
            if (e.InnerException != null)
                sb.Append($"Underlying exception: {e.InnerException}");

            bool dumpCreated;
            string filename = $"MaidFiddler_err_{DateTime.Now.Ticks}.txt";
            try
            {
                using (FileStream fs = File.Create(filename))
                {
                    using (TextWriter tw = new StreamWriter(fs))
                    {
                        tw.Write(sb.ToString());
                        dumpCreated = true;
                    }
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

            MessageBox.Show(
            $"Oh no! An error has occured in Maid Fiddler!\n{(dumpCreated ? dumpCreatedMsg : dumpNotCreatedMsg)}",
            "Oh noes!",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);

            MaidFiddlerGUI guiLoc = MaidFiddler.Gui;
            MaidFiddler.Gui = null;
            guiLoc?.Close(true);
            errorThrown = true;
        }
    }
}