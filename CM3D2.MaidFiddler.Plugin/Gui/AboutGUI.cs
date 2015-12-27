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
            Text = Translation.GetFieldText(Text);
            Translation.GetFieldText(labelVersion);
            Translation.GetFieldText(labelContributors);
            Translation.GetFieldText(okButton);
            label_contributors.Text = MaidFiddler.CONTRIBUTORS;
            label_version.Text = $"{MaidFiddler.VERSION} (CM3D2 Version {Misc.GAME_VERSION})";
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}