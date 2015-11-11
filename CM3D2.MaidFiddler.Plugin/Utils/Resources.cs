using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public static class Resources
    {
        private const string TEXT_FILE_NAME = @"MaidFiddler\labels.txt";
        private static Dictionary<string, string> translationDictionary;

        public static Image DefaultThumbnail { get; private set; } 

        static Resources()
        {
            InitText();
            InitThumbnail();
        }

        public static void GetFieldText(Control c)
        {
            string result;
            if (translationDictionary.TryGetValue(c.Text, out result))
                c.Text = result;
        }

        public static string GetFieldText(string id)
        {
            string result;
            return translationDictionary.TryGetValue(id, out result) ? result : id;
        }

        private static void InitText()
        {
            Debugger.Assert(
            () =>
            {
                Debugger.WriteLine("Loading texts");
                translationDictionary = new Dictionary<string, string>();
                string filePath = Path.Combine(MaidFiddler.DATA_PATH, TEXT_FILE_NAME);
                Debugger.WriteLine(LogLevel.Info, $"File path: {filePath}");
                if (File.Exists(filePath))
                {
                    using (TextReader reader = File.OpenText(filePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            line = line.Trim();
                            if (line == string.Empty || line.StartsWith(";"))
                                continue;

                            string[] parts = line.Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length != 2)
                                continue;

                            translationDictionary.Add(parts[0], parts[1]);
                        }
                    }
                }
                Debugger.WriteLine(LogLevel.Info, "Texts loaded");
            },
            "Texts loaded");
        }

        public static void InitThumbnail()
        {
            Debugger.Assert(
            () =>
            {
                Debugger.WriteLine("Loading generic maid thumbnail...");
                string thumbPath = Path.Combine(MaidFiddler.DATA_PATH, @"MaidFiddler\Images\no_thumbnail.png");
                Debugger.WriteLine($"Path: {thumbPath}");
                try
                {
                    DefaultThumbnail = File.Exists(thumbPath) ? Image.FromFile(thumbPath) : null;
                }
                catch (FileNotFoundException)
                {
                    Debugger.WriteLine(LogLevel.Error, "Could not find the default thumbnail!");
                    DefaultThumbnail = null;
                }
            },
            "Failed to load the generic maid thumbnail");
        }
    }
}