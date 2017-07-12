namespace CM3D2.MaidFiddler.Plugin.Gui
{
    partial class AboutGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_version = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.labelContributors = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelProductName = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label_licence = new System.Windows.Forms.Label();
            this.labelPlugins = new System.Windows.Forms.Label();
            this.label_contributors = new System.Windows.Forms.Label();
            this.textBox_plugins = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_version
            // 
            this.label_version.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_version.AutoSize = true;
            this.label_version.Location = new System.Drawing.Point(176, 33);
            this.label_version.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.label_version.MaximumSize = new System.Drawing.Size(0, 17);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(42, 13);
            this.label_version.TabIndex = 25;
            this.label_version.Text = "Version";
            this.label_version.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(301, 167);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 24;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // labelContributors
            // 
            this.labelContributors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelContributors.AutoSize = true;
            this.labelContributors.Location = new System.Drawing.Point(6, 106);
            this.labelContributors.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelContributors.MaximumSize = new System.Drawing.Size(0, 17);
            this.labelContributors.Name = "labelContributors";
            this.labelContributors.Size = new System.Drawing.Size(161, 13);
            this.labelContributors.TabIndex = 21;
            this.labelContributors.Text = "GUI_ABOUT_CONTRIBUTORS";
            this.labelContributors.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelVersion
            // 
            this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(44, 33);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelVersion.MaximumSize = new System.Drawing.Size(0, 17);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(123, 13);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "GUI_ABOUT_VERSION";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelProductName
            // 
            this.labelProductName.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelProductName, 2);
            this.labelProductName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductName.Location = new System.Drawing.Point(6, 0);
            this.labelProductName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelProductName.MaximumSize = new System.Drawing.Size(0, 17);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(370, 13);
            this.labelProductName.TabIndex = 19;
            this.labelProductName.Text = "Maid Fiddler - An in-game editor for CM3D2";
            this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelProductName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelVersion, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.okButton, 1, 6);
            this.tableLayoutPanel.Controls.Add(this.label_licence, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.labelContributors, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.labelPlugins, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.label_contributors, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.label_version, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.textBox_plugins, 1, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(379, 193);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // label_licence
            // 
            this.label_licence.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.label_licence, 2);
            this.label_licence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_licence.Location = new System.Drawing.Point(3, 119);
            this.label_licence.Name = "label_licence";
            this.label_licence.Size = new System.Drawing.Size(373, 26);
            this.label_licence.TabIndex = 26;
            this.label_licence.Text = "Copyright (c) 2015 Geoffrey \"denikson\" Horsington\r\nLicensed under the MIT licence" +
    "";
            this.label_licence.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPlugins
            // 
            this.labelPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPlugins.AutoSize = true;
            this.labelPlugins.Location = new System.Drawing.Point(45, 46);
            this.labelPlugins.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.labelPlugins.MaximumSize = new System.Drawing.Size(0, 17);
            this.labelPlugins.Name = "labelPlugins";
            this.labelPlugins.Size = new System.Drawing.Size(122, 17);
            this.labelPlugins.TabIndex = 28;
            this.labelPlugins.Text = "GUI_ABOUT_PLUGINS";
            this.labelPlugins.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_contributors
            // 
            this.label_contributors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_contributors.AutoSize = true;
            this.label_contributors.Location = new System.Drawing.Point(173, 106);
            this.label_contributors.Name = "label_contributors";
            this.label_contributors.Size = new System.Drawing.Size(63, 13);
            this.label_contributors.TabIndex = 27;
            this.label_contributors.Text = "Contributors";
            // 
            // textBox_plugins
            // 
            this.textBox_plugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_plugins.Location = new System.Drawing.Point(173, 49);
            this.textBox_plugins.Multiline = true;
            this.textBox_plugins.Name = "textBox_plugins";
            this.textBox_plugins.ReadOnly = true;
            this.textBox_plugins.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_plugins.Size = new System.Drawing.Size(203, 54);
            this.textBox_plugins.TabIndex = 30;
            this.textBox_plugins.Text = "Plugins";
            // 
            // AboutGUI
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(397, 211);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutGUI";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GUI_ABOUT_TITLE";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_version;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label labelContributors;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label label_licence;
        private System.Windows.Forms.Label label_contributors;
        private System.Windows.Forms.Label labelPlugins;
        private System.Windows.Forms.TextBox textBox_plugins;
    }
}
