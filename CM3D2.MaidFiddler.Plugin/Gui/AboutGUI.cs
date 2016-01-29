using System;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Utils;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    partial class AboutGUI : Form
    {
        public AboutGUI()
        {
            InitializeComponent();
            Text = Translation.GetTranslation(Text);
            Translation.GetTranslation(labelVersion);
            Translation.GetTranslation(labelContributors);
            Translation.GetTranslation(okButton);
            label_contributors.Text = MaidFiddler.CONTRIBUTORS;
            label_version.Text = $"{MaidFiddler.VERSION} (CM3D2 Version {FiddlerUtils.GameVersion})";
            labelProductName.Text += $"\n{MaidFiddler.PROJECT_PAGE}";
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}