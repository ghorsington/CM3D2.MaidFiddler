using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace CM3D2.MaidFiddler.WPF.Translations
{
    public class TranslationManager
    {
        private static TranslationManager instance;

        public static readonly Regex TagPattern =
                new Regex("#MAIDFIDDLER_TRANSLATION \"(?<lang>.*)\" \"(?<ver>.*)\" \"(?<auth>.*)\"");

        private Dictionary<string, string> translationDictionary;

        private TranslationManager()
        {
            translationDictionary = new Dictionary<string, string>();
            CurrentTranslationFile = null;
            CurrentTranslationVersion = string.Empty;
        }

        public static TranslationManager Instance => instance ?? (instance = new TranslationManager());

        public static string CurrentTranslationFile { get; private set; }
        public static string CurrentTranslationVersion { get; private set; }

        public event EventHandler LanguageChanged;

        public object Translate(object key)
        {
            string translation;
            return translationDictionary.TryGetValue((string) key, out translation)
                ? translation
                : $"{key}".Replace("_", "__");
        }

        public void LoadTranslation(string filename)
        {
            string filePath = $"{filename}.txt";
            string version = string.Empty;
            if (!File.Exists(filePath))
                filename = string.Empty;
            else
            {
                Dictionary<string, string> newTranslationDictionary = new Dictionary<string, string>();
                using (TextReader reader = File.OpenText(filePath))
                {
                    string line = reader.ReadLine();
                    if (line != null)
                    {
                        Match match = TagPattern.Match(line);
                        if (match.Success)
                            version = match.Groups["ver"].Value;
                    }
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line == string.Empty || line.StartsWith(";"))
                            continue;

                        string[] parts = line.Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length != 2)
                            continue;

                        string text = parts[1].Replace(@"\n", "\n").Replace(@"\r", "\r");

                        if (text.Trim() == string.Empty)
                            continue;

                        if (!newTranslationDictionary.ContainsKey(parts[0]))
                            newTranslationDictionary.Add(parts[0], text);
                        else
                            newTranslationDictionary[parts[0]] = text;
                    }
                }
                translationDictionary.Clear();
                translationDictionary = newTranslationDictionary;
            }
            CurrentTranslationFile = filename;
            CurrentTranslationVersion = version;
            OnLanguageChanged();
        }

        private void OnLanguageChanged()
        {
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class LanguageChangedEventManager : WeakEventManager
    {
        private LanguageChangedEventManager()
        {
        }

        private static LanguageChangedEventManager CurrentManager
        {
            get
            {
                Type managerType = typeof(LanguageChangedEventManager);
                LanguageChangedEventManager manager = (LanguageChangedEventManager) GetCurrentManager(managerType);

                if (manager != null)
                    return manager;
                manager = new LanguageChangedEventManager();
                SetCurrentManager(managerType, manager);

                return manager;
            }
        }

        public static void AddHandler(TranslationManager source, EventHandler handler)
        {
            CurrentManager.ProtectedAddHandler(source, handler);
        }

        public static void RemoveHandler(TranslationManager source, EventHandler handler)
        {
            CurrentManager.ProtectedRemoveHandler(source, handler);
        }

        public static void AddListener(TranslationManager source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }

        public static void RemoveListener(TranslationManager source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        protected override void StartListening(object source)
        {
            TranslationManager manager = (TranslationManager) source;
            manager.LanguageChanged += DeliverEvent;
        }

        protected override void StopListening(object source)
        {
            TranslationManager manager = (TranslationManager) source;
            manager.LanguageChanged -= DeliverEvent;
        }
    }
}