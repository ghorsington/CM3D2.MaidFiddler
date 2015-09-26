using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Utils;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        private const string TEXT_FILE_NAME = @"MaidFiddler\labels.txt";
        private Image defaultThumb;
        private Dictionary<string, string> translationDictionary;

        private void GetFieldText(Control c)
        {
            if (translationDictionary.ContainsKey(c.Text))
                c.Text = translationDictionary[c.Text];
        }

        private string GetFieldText(string id)
        {
            return translationDictionary.ContainsKey(id) ? translationDictionary[id] : id;
        }

        private void InitText()
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

        private void InitThumbnail()
        {
            Debugger.Assert(
            () =>
            {
                Debugger.WriteLine("Loading generic maid thumbnail...");
                string thumbPath = Path.Combine(MaidFiddler.DATA_PATH, @"MaidFiddler\Images\no_thumbnail.png");
                Debugger.WriteLine($"Path: {thumbPath}");
                try
                {
                    defaultThumb = File.Exists(thumbPath) ? Image.FromFile(thumbPath) : null;
                }
                catch (FileNotFoundException)
                {
                    Debugger.WriteLine(LogLevel.Error, "Could not find the default thumbnail!");
                    defaultThumb = null;
                }
            },
            "Failed to load the generic maid thumbnail");
        }
    }
}