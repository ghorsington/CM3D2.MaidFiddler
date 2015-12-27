using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Utils;
using Debugger = CM3D2.MaidFiddler.Plugin.Utils.Debugger;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class TranslationSelectionGUI : Form
    {
        private readonly MaidFiddler plugin;

        private readonly Regex tagPattern =
        new Regex("#MAIDFIDDLER_TRANSLATION \"(?<lang>.*)\" \"(?<ver>.*)\" \"(?<auth>.*)\"");

        public TranslationSelectionGUI(MaidFiddler plugin)
        {
            this.plugin = plugin;
            InitializeComponent();
            Text = Translation.GetFieldText(Text);
            Translation.GetFieldText(label_prompt);
            Translation.GetFieldText(label_lang_name);
            Translation.GetFieldText(label_lang_version);
            Translation.GetFieldText(label_lang_author);
            Translation.GetFieldText(button_apply);
            Translation.GetFieldText(button_cancel);
            Translation.GetFieldText(button_open_translation_folder);
            listBox_translations.DisplayMember = "DisplayName";
            listBox_translations.SelectedValueChanged += OnLanguageSelected;
            LoadTranslations(Translation.CurrentTranslationFile);
        }

        private void OnLanguageSelected(object sender, EventArgs e)
        {
            if (listBox_translations.SelectedIndex == -1)
                return;
            TranslationData translationData = (TranslationData) listBox_translations.SelectedItem;
            label_lang_val.Text = translationData.Language != string.Empty
                                  ? translationData.Language : Translation.GetFieldText("UNKNOWN");
            label_version_val.Text = translationData.Version != string.Empty
                                     ? translationData.Version : Translation.GetFieldText("UNKNOWN");
            label_author_val.Text = translationData.Author != string.Empty
                                    ? translationData.Author : Translation.GetFieldText("UNKNOWN");
        }

        private void LoadTranslations(string selectedLanguageFile)
        {
            Debugger.WriteLine(
            LogLevel.Info,
            $"Loading translation files. Selected language file: {selectedLanguageFile}");
            string translationsPath = Path.Combine(MaidFiddler.DATA_PATH, Translation.TRANSLATIONS_PATH);
            if (!Directory.Exists(translationsPath))
            {
                Debugger.WriteLine(LogLevel.Warning, "No translation folder found. Creating one...");
                Directory.CreateDirectory(translationsPath);
                return;
            }

            string[] files = Directory.GetFiles(translationsPath, "*.txt");
            int selected = -1;
            foreach (string filePath in files)
            {
                using (StreamReader sr = File.OpenText(filePath))
                {
                    TranslationData translationData = new TranslationData();
                    string line = sr.ReadLine();

                    if (line == null || line.Trim() == string.Empty)
                        continue;

                    Match match = tagPattern.Match(line);
                    if (!match.Success)
                        continue;

                    translationData.FileName = Path.GetFileNameWithoutExtension(filePath);
                    translationData.Language = match.Groups["lang"].Value;
                    translationData.Version = match.Groups["ver"].Value;
                    translationData.Author = match.Groups["auth"].Value;

                    Debugger.WriteLine(
                    LogLevel.Info,
                    $"Found language: File={translationData.FileName}, Lang={translationData.Language}");

                    int i = listBox_translations.Items.Add(translationData);
                    if (translationData.FileName == selectedLanguageFile)
                        selected = i;
                }
            }

            if (selected != -1)
                listBox_translations.SelectedIndex = selected;
        }

        private void OnApplyTranslation(object sender, EventArgs e)
        {
            string langFileName;
            if (listBox_translations.SelectedIndex != -1
                && Translation.CurrentTranslationFile
                != (langFileName = ((TranslationData) listBox_translations.SelectedItem).FileName))
            {
                Translation.LoadTranslation(langFileName);
                plugin.SelectedDefaultLanguage = langFileName;
            }
            Close();
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void OpenTranslationsFolder(object sender, EventArgs e)
        {
            string translationsPath = Path.Combine(MaidFiddler.DATA_PATH, Translation.TRANSLATIONS_PATH);
            if (!Directory.Exists(translationsPath))
            {
                Debugger.WriteLine(LogLevel.Warning, "No translation folder found. Creating one...");
                Directory.CreateDirectory(translationsPath);
            }
            Process.Start(translationsPath);
        }

        private struct TranslationData
        {
            public string FileName, Language, Version, Author;
            public string DisplayName => Language == string.Empty ? FileName : Language;
        }
    }
}