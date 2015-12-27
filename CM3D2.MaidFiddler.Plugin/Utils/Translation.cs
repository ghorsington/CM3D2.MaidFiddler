using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public static class Translation
    {
        public const string TRANSLATIONS_PATH = @"MaidFiddler\Translations";
        private static readonly Dictionary<string, string> translationDictionary;
        private static readonly Dictionary<string, List<Action<string>>> translatableControlsDictionary;

        static Translation()
        {
            translatableControlsDictionary = new Dictionary<string, List<Action<string>>>();
            translationDictionary = new Dictionary<string, string>();
            CurrentTranslationFile = null;
        }

        public static string CurrentTranslationFile { get; private set; }

        public static void AddTranslatableControl(Control c)
        {
            AddTranslationAction(c.Text, s => c.Text = s);
        }

        public static void AddTranslationAction(string key, Action<string> translationAction)
        {
            List<Action<string>> actions;
            if (!translatableControlsDictionary.TryGetValue(key, out actions))
            {
                actions = new List<Action<string>>();
                translatableControlsDictionary.Add(key, actions);
            }
            actions.Add(translationAction);
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

        public static void LoadTranslation(string filename)
        {
            Debugger.Assert(
            () =>
            {
                Debugger.WriteLine($"Loading translation: {filename}");
                string filePath = Path.Combine(MaidFiddler.DATA_PATH, $@"{TRANSLATIONS_PATH}\{filename}.txt");
                Debugger.WriteLine(LogLevel.Info, $"File path: {filePath}");
                if (!File.Exists(filePath))
                {
                    Debugger.WriteLine(LogLevel.Error, "Failed to find such translation file.");
                    filename = string.Empty;
                }
                else
                {
                    Debugger.WriteLine(LogLevel.Info, "Loading translation labels.");
                    translationDictionary.Clear();
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

                            string text = parts[1].Replace(@"\n", "\n").Replace(@"\r", "\r");

                            translationDictionary.Add(parts[0], text);
                        }
                    }
                }
                Debugger.WriteLine(LogLevel.Info, "Texts loaded");
                CurrentTranslationFile = filename;
                ApplyTranslation();
            },
            "Texts loaded");
        }

        public static void ApplyTranslation()
        {
            Debugger.WriteLine(LogLevel.Info, "Applying translation");

            foreach (KeyValuePair<string, List<Action<string>>> translationItems in translatableControlsDictionary)
            {
                string translation;
                if (translationDictionary.TryGetValue(translationItems.Key, out translation))
                    translationItems.Value.ForEach(a => a.Invoke(translation));
                else
                    translationItems.Value.ForEach(a => a.Invoke(translationItems.Key));
            }
        }

        public static bool Exists(string name)
        {
            return File.Exists(Path.Combine(MaidFiddler.DATA_PATH, $@"{TRANSLATIONS_PATH}\{name}.txt"));
        }
    }
}