namespace CM3D2.MaidFiddler.Plugin.Gui
{
    partial class TranslationSelectionGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.listBox_translations = new System.Windows.Forms.ListBox();
            this.label_prompt = new System.Windows.Forms.Label();
            this.label_lang_name = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label_lang_version = new System.Windows.Forms.Label();
            this.label_lang_author = new System.Windows.Forms.Label();
            this.label_lang_val = new System.Windows.Forms.Label();
            this.label_version_val = new System.Windows.Forms.Label();
            this.label_author_val = new System.Windows.Forms.Label();
            this.button_apply = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_open_translation_folder = new System.Windows.Forms.Button();
            this.button_download_url = new System.Windows.Forms.Button();
            this.button_download_github = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox_translations
            // 
            this.listBox_translations.FormattingEnabled = true;
            this.listBox_translations.Location = new System.Drawing.Point(13, 52);
            this.listBox_translations.Name = "listBox_translations";
            this.listBox_translations.Size = new System.Drawing.Size(244, 199);
            this.listBox_translations.TabIndex = 0;
            // 
            // label_prompt
            // 
            this.label_prompt.Location = new System.Drawing.Point(12, 9);
            this.label_prompt.Name = "label_prompt";
            this.label_prompt.Size = new System.Drawing.Size(306, 40);
            this.label_prompt.TabIndex = 7;
            this.label_prompt.Text = "GUI_TRANS_SELECT_PROMPT";
            // 
            // label_lang_name
            // 
            this.label_lang_name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_lang_name.AutoSize = true;
            this.label_lang_name.Location = new System.Drawing.Point(22, 0);
            this.label_lang_name.Name = "label_lang_name";
            this.label_lang_name.Size = new System.Drawing.Size(81, 13);
            this.label_lang_name.TabIndex = 8;
            this.label_lang_name.Text = "GUI_TS_LANG";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label_lang_name, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_lang_version, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_lang_author, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_lang_val, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_version_val, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_author_val, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(263, 52);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(209, 136);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // label_lang_version
            // 
            this.label_lang_version.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_lang_version.AutoSize = true;
            this.label_lang_version.Location = new System.Drawing.Point(3, 13);
            this.label_lang_version.Name = "label_lang_version";
            this.label_lang_version.Size = new System.Drawing.Size(100, 13);
            this.label_lang_version.TabIndex = 9;
            this.label_lang_version.Text = "GUI_TS_VERSION";
            // 
            // label_lang_author
            // 
            this.label_lang_author.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_lang_author.AutoSize = true;
            this.label_lang_author.Location = new System.Drawing.Point(5, 26);
            this.label_lang_author.Name = "label_lang_author";
            this.label_lang_author.Size = new System.Drawing.Size(98, 110);
            this.label_lang_author.TabIndex = 10;
            this.label_lang_author.Text = "GUI_TS_AUTHOR";
            // 
            // label_lang_val
            // 
            this.label_lang_val.AutoSize = true;
            this.label_lang_val.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_lang_val.Location = new System.Drawing.Point(109, 0);
            this.label_lang_val.Name = "label_lang_val";
            this.label_lang_val.Size = new System.Drawing.Size(97, 13);
            this.label_lang_val.TabIndex = 11;
            this.label_lang_val.Text = "LANGUAGE";
            // 
            // label_version_val
            // 
            this.label_version_val.AutoSize = true;
            this.label_version_val.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_version_val.Location = new System.Drawing.Point(109, 13);
            this.label_version_val.Name = "label_version_val";
            this.label_version_val.Size = new System.Drawing.Size(97, 13);
            this.label_version_val.TabIndex = 12;
            this.label_version_val.Text = "VERSION";
            // 
            // label_author_val
            // 
            this.label_author_val.AutoSize = true;
            this.label_author_val.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_author_val.Location = new System.Drawing.Point(109, 26);
            this.label_author_val.Name = "label_author_val";
            this.label_author_val.Size = new System.Drawing.Size(97, 110);
            this.label_author_val.TabIndex = 13;
            this.label_author_val.Text = "AUTHOR";
            // 
            // button_apply
            // 
            this.button_apply.Location = new System.Drawing.Point(316, 256);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(75, 23);
            this.button_apply.TabIndex = 10;
            this.button_apply.Text = "APPLY";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.OnApplyTranslation);
            // 
            // button_cancel
            // 
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(397, 256);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 11;
            this.button_cancel.Text = "CANCEL";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.OnCancelClicked);
            // 
            // button_open_translation_folder
            // 
            this.button_open_translation_folder.Location = new System.Drawing.Point(316, 228);
            this.button_open_translation_folder.Name = "button_open_translation_folder";
            this.button_open_translation_folder.Size = new System.Drawing.Size(156, 23);
            this.button_open_translation_folder.TabIndex = 12;
            this.button_open_translation_folder.Text = "GUI_TS_OPEN_TS_FOLDER";
            this.button_open_translation_folder.UseVisualStyleBackColor = true;
            this.button_open_translation_folder.Click += new System.EventHandler(this.OpenTranslationsFolder);
            // 
            // button_download_url
            // 
            this.button_download_url.Location = new System.Drawing.Point(147, 257);
            this.button_download_url.Name = "button_download_url";
            this.button_download_url.Size = new System.Drawing.Size(110, 23);
            this.button_download_url.TabIndex = 13;
            this.button_download_url.Text = "GUI_TS_DL_URL";
            this.button_download_url.UseVisualStyleBackColor = true;
            this.button_download_url.Click += new System.EventHandler(this.OpenTranslationDownloadUrl);
            // 
            // button_download_github
            // 
            this.button_download_github.Location = new System.Drawing.Point(12, 257);
            this.button_download_github.Name = "button_download_github";
            this.button_download_github.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button_download_github.Size = new System.Drawing.Size(129, 23);
            this.button_download_github.TabIndex = 14;
            this.button_download_github.Text = "GUI_TS_DL_GITHUB";
            this.button_download_github.UseVisualStyleBackColor = true;
            this.button_download_github.Click += new System.EventHandler(this.OpenTranslationDownloadGithub);
            // 
            // TranslationSelectionGUI
            // 
            this.AcceptButton = this.button_apply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_cancel;
            this.ClientSize = new System.Drawing.Size(484, 291);
            this.Controls.Add(this.button_download_github);
            this.Controls.Add(this.button_download_url);
            this.Controls.Add(this.button_open_translation_folder);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_apply);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label_prompt);
            this.Controls.Add(this.listBox_translations);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TranslationSelectionGUI";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GUI_TRANS_SELECT_TITLE";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_translations;
        private System.Windows.Forms.Label label_prompt;
        private System.Windows.Forms.Label label_lang_name;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label_lang_version;
        private System.Windows.Forms.Label label_lang_author;
        private System.Windows.Forms.Button button_apply;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label_lang_val;
        private System.Windows.Forms.Label label_version_val;
        private System.Windows.Forms.Label label_author_val;
        private System.Windows.Forms.Button button_open_translation_folder;
        private System.Windows.Forms.Button button_download_url;
        private System.Windows.Forms.Button button_download_github;
    }
}