using System;
using System.IO;
using System.Windows.Forms;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public class ErrorLog
    {
        public static void ThrowErrorMessage(Exception e)
        {
            bool dumpCreated;
            string filename = $"MaidFiddler_err_{DateTime.Now.Ticks}.txt";
            try
            {
                using (FileStream fs = File.Create(filename))
                {
                    using (TextWriter tw = new StreamWriter(fs))
                    {
                        tw.WriteLine("Maid Fiddler DUMP\n");
                        tw.WriteLine($"Error type: {e.GetType()}. Message. {e.Message}");
                        tw.WriteLine("Stack trace:");
                        tw.WriteLine(e.StackTrace);
                        dumpCreated = true;
                    }
                }
            }
            catch (Exception)
            {
                dumpCreated = false;
            }

            string dumpCreatedMsg =
            $"A log was named {filename} was created.\nPlease, send this log to the developer with the description of what you attempted to do";
            string dumpNotCreatedMsg =
            $"Failed to create a dump message. Send a screenshot of the following stack trace to the developer:\n==START==\n{e.GetType()}:{e.Message}\n{e.StackTrace}\n==END==";

            MessageBox.Show(
            $"Oh no! Maid Fiddler has crashed!\n{(dumpCreated ? dumpCreatedMsg : dumpNotCreatedMsg)}",
            "Oh noes!",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
        }
    }
}