using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Utils;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    internal partial class AboutGUI : Form
    {
        public AboutGUI()
        {
            InitializeComponent();
            Text = Translation.GetTranslation(Text);
            Translation.GetTranslation(labelVersion);
            Translation.GetTranslation(labelContributors);
            Translation.GetTranslation(labelPlugins);
            Translation.GetTranslation(okButton);
            label_contributors.Text = MaidFiddler.CONTRIBUTORS;
            label_version.Text = $"{MaidFiddler.VERSION} (CM3D2 Version {FiddlerUtils.GameVersion})";
            labelProductName.Text += $"\n{MaidFiddler.PROJECT_PAGE}";

            PluginData.Type[] plugins =
                    EnumHelper.GetValues<PluginData.Type>().TakeWhile(GameUty.CheckPackFlag).ToArray();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < plugins.Length; i++)
            {
                sb.Append(EnumHelper.GetName(plugins[i]));
                if (i < plugins.Length - 1)
                    sb.Append(", ");
                if ((i + 1) % 3 == 0)
                    sb.Append("\n");
            }
            Debugger.WriteLine($"Installed plugins: {sb}");
            textBox_plugins.Text = sb.ToString();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}