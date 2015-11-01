using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Gui;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public static class ErrorLog
    {
        private static bool errorThrown;

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