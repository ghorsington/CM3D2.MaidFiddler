using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Utils;
using Debugger = CM3D2.MaidFiddler.Plugin.Utils.Debugger;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class TranslationSelectionGUI : Form
    {
        private readonly MaidFiddler plugin;

        public TranslationSelectionGUI(MaidFiddler plugin)
        {
            this.plugin = plugin;
            InitializeComponent();
            Text = Translation.GetTranslation(Text);
            Translation.GetTranslation(label_prompt);
            Translation.GetTranslation(label_lang_name);
            Translation.GetTranslation(label_lang_version);
            Translation.GetTranslation(label_lang_author);
            Translation.GetTranslation(button_download_github);
            Translation.GetTranslation(button_download_url);
            Translation.GetTranslation(button_apply);
            Translation.GetTranslation(button_cancel);
            Translation.GetTranslation(button_open_translation_folder);
            listBox_translations.DisplayMember = "DisplayName";
            listBox_translations.SelectedValueChanged += OnLanguageSelected;
            LoadTranslations(Translation.CurrentTranslationFile);
        }

        private void OnLanguageSelected(object sender, EventArgs e)
        {
            if (listBox_translations.SelectedIndex == -1) return;
            TranslationData translationData = (TranslationData) listBox_translations.SelectedItem;
            label_lang_val.Text = translationData.Language != string.Empty
                                      ? translationData.Language : Translation.GetTranslation("UNKNOWN");
            label_version_val.Text = translationData.Version != string.Empty
                                         ? translationData.Version : Translation.GetTranslation("UNKNOWN");
            label_author_val.Text = translationData.Author != string.Empty
                                        ? translationData.Author : Translation.GetTranslation("UNKNOWN");
        }

        private void LoadTranslations(string selectedLanguageFile)
        {
            listBox_translations.Items.Clear();
            Debugger.WriteLine(LogLevel.Info,
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

                    if (line == null || line.Trim() == string.Empty) continue;

                    Match match = Translation.TagPattern.Match(line);
                    if (!match.Success) continue;

                    translationData.FileName = Path.GetFileNameWithoutExtension(filePath);
                    translationData.Language = match.Groups["lang"].Value;
                    translationData.Version = match.Groups["ver"].Value;
                    translationData.Author = match.Groups["auth"].Value;

                    Debugger.WriteLine(LogLevel.Info,
                        $"Found language: File={translationData.FileName}, Lang={translationData.Language}");

                    int i = listBox_translations.Items.Add(translationData);
                    if (translationData.FileName == selectedLanguageFile) selected = i;
                }
            }

            if (selected != -1) listBox_translations.SelectedIndex = selected;
        }

        private void OnApplyTranslation(object sender, EventArgs e)
        {
            TranslationData? translationData = listBox_translations.SelectedIndex != -1
                                                   ? (TranslationData?) listBox_translations.SelectedItem : null;
            string langFileName;
            if (translationData.HasValue
                && (Translation.CurrentTranslationFile != (langFileName = translationData.Value.FileName)
                    || Translation.CurrentTranslationFile == langFileName
                    && (Translation.CurrentTranslationVersion == string.Empty
                        || Translation.CurrentTranslationVersion != translationData.Value.Version)))
            {
                Translation.LoadTranslation(langFileName);
                plugin.CFGSelectedDefaultLanguage = langFileName;
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
            Debugger.WriteLine(LogLevel.Info, $"Opening translation folder at {translationsPath}");
            if (!Directory.Exists(translationsPath))
            {
                Debugger.WriteLine(LogLevel.Warning, "No translation folder found. Creating one...");
                Directory.CreateDirectory(translationsPath);
            }
            Process.Start(new ProcessStartInfo {FileName = translationsPath, UseShellExecute = true, Verb = "open"});
        }

        private void OpenTranslationDownloadGithub(object sender, EventArgs e)
        {
            string list = string.Empty;
            LoadingBarGUI loadingBarGui = new LoadingBarGUI(Translation.GetTranslation("LOADING"),
                Translation.GetTranslation("TL_LIST_LOADING"), true, g =>
                {
                    HttpWebRequest webRequest =
                        (HttpWebRequest)
                        WebRequest.Create($"{MaidFiddler.RESOURCE_URL}/Resources/Translations/translation_list.txt");
                    g.Timer.Start();
                    Debugger.WriteLine(LogLevel.Info, "Getting translation list...");
                    webRequest.BeginGetResponse(ar =>
                    {
                        try
                        {
                            HttpWebResponse response = (HttpWebResponse) webRequest.EndGetResponse(ar);
                            Debugger.WriteLine(LogLevel.Info, "Got response!");
                            Debugger.WriteLine(LogLevel.Info, $"Response: {response.StatusCode}");
                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                MessageBox.Show("Failed to retreive translation list: List not found.", "Boop!",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            Stream s = response.GetResponseStream();
                            Debugger.WriteLine(LogLevel.Info, "Reading response");
                            StringBuilder sb = new StringBuilder();
                            byte[] responseBuffer = new byte[1024];
                            int read;
                            do
                            {
                                read = s.Read(responseBuffer, 0, responseBuffer.Length);
                                sb.Append(Encoding.UTF8.GetString(responseBuffer, 0, read));
                            } while (read > 0);
                            list = sb.ToString();
                            g.DialogResult = DialogResult.OK;
                        }
                        catch (WebException we)
                        {
                            MessageBox.Show($"Failed to retreive translation list.\nResponse: {we.Message}", "Boop!",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            g.DialogResult = DialogResult.Abort;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Unknown error occurred.\nInfo: {ex.ToString()}", "Boop!",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            g.DialogResult = DialogResult.Abort;
                        }
                        finally
                        {
                            g.Timer.Stop();
                            g.Close();
                        }
                    }, null);
                });
            DialogResult result = loadingBarGui.ShowDialog(this);
            loadingBarGui.Dispose();
            if (result != DialogResult.OK) return;
            GithubTranslationsGUI tlGui = new GithubTranslationsGUI(list.Remove(0, 1));
            tlGui.ShowDialog(this);
            tlGui.Dispose();
            LoadTranslations(Translation.CurrentTranslationFile);
        }

        private void OpenTranslationDownloadUrl(object sender, EventArgs e)
        {
            Uri uri = null;
            TextDialog tdURL = new TextDialog(Translation.GetTranslation("TL_URL_TITLE"),
                Translation.GetTranslation("TL_URL_TEXT"), string.Empty, s =>
                {
                    try
                    {
                        uri = new Uri(s);
                        return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }, Translation.GetTranslation("OK"), Translation.GetTranslation("CANCEL"));
            DialogResult promptDialog = tdURL.ShowDialog(this);
            tdURL.Dispose();

            if (promptDialog != DialogResult.OK) return;

            string translationsPath = Path.Combine(MaidFiddler.DATA_PATH, Translation.TRANSLATIONS_PATH);
            LoadingBarGUI loadingBarGui = new LoadingBarGUI(Translation.GetTranslation("LOADING"),
                Translation.GetTranslation("TL_URL_DOWNLOADING"), true, g =>
                {
                    HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(uri);
                    g.Timer.Start();
                    Debugger.WriteLine(LogLevel.Info, "Getting translation...");
                    webRequest.BeginGetResponse(ar =>
                    {
                        try
                        {
                            HttpWebResponse response = (HttpWebResponse) webRequest.EndGetResponse(ar);
                            Debugger.WriteLine(LogLevel.Info, "Got response!");
                            Debugger.WriteLine(LogLevel.Info, $"Response: {response.StatusCode}");
                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                MessageBox.Show("Failed to download the translation: File not found", "Boop!",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                g.DialogResult = DialogResult.Abort;
                                g.Timer.Stop();
                                g.Close();
                                return;
                            }
                            Stream s = response.GetResponseStream();
                            Debugger.WriteLine(LogLevel.Info, "Reading response");
                            StringBuilder sb = new StringBuilder();
                            byte[] responseBuffer = new byte[1024];
                            int read;
                            do
                            {
                                read = s.Read(responseBuffer, 0, responseBuffer.Length);
                                sb.Append(Encoding.UTF8.GetString(responseBuffer, 0, read));
                            } while (read > 0);

                            using (TextReader tr = new StringReader(sb.ToString()))
                            {
                                if (!Translation.TagPattern.Match(tr.ReadLine()).Success)
                                {
                                    Debugger.WriteLine(LogLevel.Error, "Failed to parse the translation: No tag found!");
                                    MessageBox.Show(
                                        "The file does not contain the translation tag and thus cannot be recognised as a Maid Fiddler translation file.\nThe file has not been saved.",
                                        "Boop!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    g.DialogResult = DialogResult.Abort;
                                    g.Timer.Stop();
                                    g.Close();
                                    return;
                                }
                            }

                            string tlFileName = Path.GetFileNameWithoutExtension(uri.AbsolutePath);
                            Debugger.WriteLine($"File name: {tlFileName}");

                            if (tlFileName == string.Empty
                                || File.Exists(Path.Combine(translationsPath, $"{tlFileName}.txt")))
                            {
                                char[] invalidChars = Path.GetInvalidFileNameChars();
                                TextDialog tdFileName =
                                    new TextDialog(Translation.GetTranslation("TL_NAME_CHANGE_TITLE"),
                                        Translation.GetTranslation("TL_NAME_CHANGE"), tlFileName,
                                        s1 => !s1.Any(c => invalidChars.Contains(c)), Translation.GetTranslation("OK"),
                                        Translation.GetTranslation("CANCEL"));
                                if (tdFileName.ShowDialog(g) == DialogResult.OK) tlFileName = tdFileName.Input;
                                if (tlFileName == string.Empty) tlFileName = FiddlerUtils.GenerateFileName();

                                tdFileName.Dispose();
                            }

                            string path = Path.Combine(translationsPath, $"{tlFileName}.txt");
                            Debugger.WriteLine($"Writing translation to {path}");

                            using (TextWriter tw = File.CreateText(path)) tw.Write(sb.ToString());
                            g.DialogResult = DialogResult.OK;
                        }
                        catch (WebException we)
                        {
                            MessageBox.Show($"Failed to retreive translation.\nResponse: {we.Message}", "Boop!",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            g.DialogResult = DialogResult.Abort;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Unknown error occurred.\nInfo: {ex.ToString()}", "Boop!",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            g.DialogResult = DialogResult.Abort;
                        }
                        finally
                        {
                            g.Timer.Stop();
                            g.Close();
                        }
                    }, null);
                });
            DialogResult result = loadingBarGui.ShowDialog(this);
            loadingBarGui.Dispose();
            if (result != DialogResult.OK) return;
            MessageBox.Show(Translation.GetTranslation("TL_DOWNLOAD_DONE"),
                Translation.GetTranslation("TL_DOWNLOAD_DONE_TITLE"), MessageBoxButtons.OK);
            LoadTranslations(Translation.CurrentTranslationFile);
        }

        private struct TranslationData
        {
            public string FileName, Language, Version, Author;
            public string DisplayName => Language == string.Empty ? FileName : Language;
        }
    }
}