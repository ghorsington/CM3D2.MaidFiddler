namespace CM3D2.MaidFiddler.Plugin.Gui
{
    partial class GithubTranslationsGUI
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
            this.label_text = new System.Windows.Forms.Label();
            this.dataGridView_translations = new System.Windows.Forms.DataGridView();
            this.TlName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TlNameEng = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TlVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TlCurrentVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_close = new System.Windows.Forms.Button();
            this.button_download = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_translations)).BeginInit();
            this.SuspendLayout();
            // 
            // label_text
            // 
            this.label_text.Location = new System.Drawing.Point(12, 9);
            this.label_text.Name = "label_text";
            this.label_text.Size = new System.Drawing.Size(400, 50);
            this.label_text.TabIndex = 0;
            this.label_text.Text = "TL_GITHUB_TEXT";
            // 
            // dataGridView_translations
            // 
            this.dataGridView_translations.AllowUserToAddRows = false;
            this.dataGridView_translations.AllowUserToDeleteRows = false;
            this.dataGridView_translations.AllowUserToResizeColumns = false;
            this.dataGridView_translations.AllowUserToResizeRows = false;
            this.dataGridView_translations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_translations.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_translations.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView_translations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_translations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TlName,
            this.TlNameEng,
            this.TlVersion,
            this.TlCurrentVersion});
            this.dataGridView_translations.Location = new System.Drawing.Point(12, 62);
            this.dataGridView_translations.MultiSelect = false;
            this.dataGridView_translations.Name = "dataGridView_translations";
            this.dataGridView_translations.ReadOnly = true;
            this.dataGridView_translations.RowHeadersVisible = false;
            this.dataGridView_translations.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView_translations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_translations.Size = new System.Drawing.Size(400, 158);
            this.dataGridView_translations.TabIndex = 1;
            // 
            // TlName
            // 
            this.TlName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TlName.FillWeight = 20F;
            this.TlName.HeaderText = "TL_NAME";
            this.TlName.Name = "TlName";
            this.TlName.ReadOnly = true;
            this.TlName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TlNameEng
            // 
            this.TlNameEng.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TlNameEng.FillWeight = 40F;
            this.TlNameEng.HeaderText = "TL_NAME_ENG";
            this.TlNameEng.Name = "TlNameEng";
            this.TlNameEng.ReadOnly = true;
            this.TlNameEng.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TlVersion
            // 
            this.TlVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TlVersion.FillWeight = 20F;
            this.TlVersion.HeaderText = "TL_VERSION";
            this.TlVersion.Name = "TlVersion";
            this.TlVersion.ReadOnly = true;
            this.TlVersion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TlCurrentVersion
            // 
            this.TlCurrentVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TlCurrentVersion.FillWeight = 30F;
            this.TlCurrentVersion.HeaderText = "TL_CURR_VERSION";
            this.TlCurrentVersion.Name = "TlCurrentVersion";
            this.TlCurrentVersion.ReadOnly = true;
            this.TlCurrentVersion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // button_close
            // 
            this.button_close.Location = new System.Drawing.Point(337, 226);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(75, 23);
            this.button_close.TabIndex = 2;
            this.button_close.Text = "CLOSE";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.CloseGui);
            // 
            // button_download
            // 
            this.button_download.Location = new System.Drawing.Point(12, 226);
            this.button_download.Name = "button_download";
            this.button_download.Size = new System.Drawing.Size(119, 23);
            this.button_download.TabIndex = 3;
            this.button_download.Text = "TL_DOWNLOAD";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.DownloadTranslations);
            // 
            // GithubTranslationsGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 261);
            this.Controls.Add(this.button_download);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.dataGridView_translations);
            this.Controls.Add(this.label_text);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GithubTranslationsGUI";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TL_GITHUB_TITLE";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_translations)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_text;
        private System.Windows.Forms.DataGridView dataGridView_translations;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Button button_download;
        private System.Windows.Forms.DataGridViewTextBoxColumn TlName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TlNameEng;
        private System.Windows.Forms.DataGridViewTextBoxColumn TlVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn TlCurrentVersion;
    }
}