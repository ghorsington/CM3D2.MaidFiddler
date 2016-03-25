using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Utils;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class GithubTranslationsGUI : Form
    {
        private readonly List<string> langCodes = new List<string>();
        private readonly string translationList;

        public GithubTranslationsGUI(string list)
        {
            InitializeComponent();

            translationList = list;
            Text = Translation.GetTranslation(Text);
            Translation.GetTranslation(label_text);
            foreach (DataGridViewColumn column in dataGridView_translations.Columns)
                column.HeaderText = Translation.GetTranslation(column.HeaderText);
            Translation.GetTranslation(button_download);
            Translation.GetTranslation(button_close);
            InitLanguageTable();
        }

        private void InitLanguageTable()
        {
            dataGridView_translations.Rows.Clear();
            langCodes.Clear();
            string translationsPath = Path.Combine(MaidFiddler.DATA_PATH, Translation.TRANSLATIONS_PATH);
            if (!Directory.Exists(translationsPath))
            {
                Debugger.WriteLine(LogLevel.Warning, "No translation folder found. Creating one...");
                Directory.CreateDirectory(translationsPath);
            }

            string[] files = Directory.GetFiles(translationsPath, "*.txt");
            List<LangData> installedLanguages = new List<LangData>();
            foreach (string filePath in files)
            {
                using (StreamReader sr = File.OpenText(filePath))
                {
                    string line = sr.ReadLine();

                    if (line == null || line.Trim() == string.Empty)
                        continue;

                    Match match = Translation.TagPattern.Match(line);
                    if (!match.Success)
                        continue;

                    LangData lang = new LangData
                    {
                        Name = Path.GetFileNameWithoutExtension(filePath),
                        Version = match.Groups["ver"].Value
                    };

                    installedLanguages.Add(lang);
                    Debugger.WriteLine(LogLevel.Info, $"Found language: File={lang.Name}, Version={lang.Version}");
                }
            }

            foreach (string[] langs in
            translationList.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)
                           .Select(s => s.Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries)))
            {
                LangData lang =
                installedLanguages.FirstOrDefault(
                data => string.Equals(data.Name, langs[0], StringComparison.InvariantCultureIgnoreCase));
                Debugger.WriteLine(LogLevel.Info, $"Lang name: {lang.Name}, Ver: {lang.Version}");
                string version = Equals(lang, default(LangData))
                                 ? Translation.GetTranslation("TL_NOT_INSTALLED") : lang.Version;
                dataGridView_translations.Rows.Add(langs[2], langs[1], langs[3], version);
                langCodes.Add(langs[0].ToUpperInvariant());
                Debugger.WriteLine(LogLevel.Info, $"Available language: Code={langs[0]}, Version={langs[3]}");
            }
        }

        private void CloseGui(object sender, EventArgs e) => Close();

        private void DownloadTranslations(object sender, EventArgs e)
        {
            if (dataGridView_translations.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                Translation.GetTranslation("TL_NO_TLS_SELECTED"),
                Translation.GetTranslation("TL_NO_TLS_SELECTED_TITLE"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
                return;
            }

            string translationsPath = Path.Combine(MaidFiddler.DATA_PATH, Translation.TRANSLATIONS_PATH);
            LoadingBarGUI loadingBarGui = new LoadingBarGUI(
            Translation.GetTranslation("LOADING"),
            $"{Translation.GetTranslation("TL_TRANSLATION_DOWNLOAD")}",
            true,
            g =>
            {
                g.Timer.Start();
                Thread downloaderThread = new Thread(
                () =>
                {
                    foreach (DataGridViewRow selectedRow in dataGridView_translations.SelectedRows)
                    {
                        string tlFileName = langCodes[selectedRow.Index];
                        g.TextLabel.Text =
                        $"{Translation.GetTranslation("TL_TRANSLATION_DOWNLOAD")} {selectedRow.Cells[0]}";
                        Debugger.WriteLine(LogLevel.Info, $"Downloading language ID {selectedRow.Index}");

                        try
                        {
                            HttpWebRequest webRequest =
                            (HttpWebRequest)
                            WebRequest.Create($"{MaidFiddler.RESOURCE_URL}/Resources/Translations/{tlFileName}.txt");

                            Debugger.WriteLine(
                            LogLevel.Info,
                            $"Getting translation file from {MaidFiddler.RESOURCE_URL}/Resources/Translations/{tlFileName}.txt");

                            HttpWebResponse response = (HttpWebResponse) webRequest.GetResponse();

                            Debugger.WriteLine(LogLevel.Info, "Got response!");
                            Debugger.WriteLine(LogLevel.Info, $"Response: {response.StatusCode}");

                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                MessageBox.Show(
                                "Failed to retreive translation: File not found.",
                                "Boop!",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                                g.DialogResult = DialogResult.Abort;
                                g.Timer.Stop();
                                g.Close();
                                return;
                            }

                            Stream s = response.GetResponseStream();
                            Debugger.WriteLine(LogLevel.Info, "Reading response");
                            byte[] responseBuffer = new byte[1024];
                            StringBuilder translationText = new StringBuilder();
                            int read;
                            do
                            {
                                read = s.Read(responseBuffer, 0, responseBuffer.Length);
                                translationText.Append(Encoding.UTF8.GetString(responseBuffer, 0, read));
                            } while (read > 0);

                            using (TextWriter tw = File.CreateText(Path.Combine(translationsPath, $"{tlFileName}.txt")))
                                tw.Write(translationText.ToString());
                        }
                        catch (WebException we)
                        {
                            MessageBox.Show(
                            $"Failed to retreive translation.\nResponse: {we.Message}",
                            "Boop!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                            g.DialogResult = DialogResult.Abort;
                            g.Timer.Stop();
                            g.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(
                            $"Could not download the translation.\nInfo: {ex}",
                            "Boop!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                            g.DialogResult = DialogResult.Abort;
                            g.Timer.Stop();
                            g.Close();
                        }
                    }
                    g.DialogResult = DialogResult.OK;
                    g.Timer.Stop();
                    g.Close();
                });
                downloaderThread.Start();
            });
            DialogResult result = loadingBarGui.ShowDialog(this);
            loadingBarGui.Dispose();
            if (result != DialogResult.OK)
                return;
            MessageBox.Show(
            Translation.GetTranslation("TL_DOWNLOAD_DONE"),
            Translation.GetTranslation("TL_DOWNLOAD_DONE_TITLE"),
            MessageBoxButtons.OK);

            InitLanguageTable();
        }

        private struct LangData
        {
            public string Name { get; set; }
            public string Version { get; set; }
        }
    }
}