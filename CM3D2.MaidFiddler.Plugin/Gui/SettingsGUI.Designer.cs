namespace CM3D2.MaidFiddler.Plugin.Gui
{
    partial class SettingsGUI
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
            this.groupBox_key = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label_key_description = new System.Windows.Forms.Label();
            this.textBox_key = new System.Windows.Forms.TextBox();
            this.label_key = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_apply = new System.Windows.Forms.Button();
            this.groupBox_gui = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label_name_style = new System.Windows.Forms.Label();
            this.comboBox_name_style = new System.Windows.Forms.ComboBox();
            this.groupBox_order = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.listBox_order_available = new System.Windows.Forms.ListBox();
            this.listBox_order_selected = new System.Windows.Forms.ListBox();
            this.button_order_add = new System.Windows.Forms.Button();
            this.button_order_remove = new System.Windows.Forms.Button();
            this.button_order_up = new System.Windows.Forms.Button();
            this.button_order_down = new System.Windows.Forms.Button();
            this.label_order_available = new System.Windows.Forms.Label();
            this.label_order_selected = new System.Windows.Forms.Label();
            this.label_order_description = new System.Windows.Forms.Label();
            this.label_order_direction = new System.Windows.Forms.Label();
            this.comboBox_order_direction = new System.Windows.Forms.ComboBox();
            this.groupBox_key.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox_gui.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox_order.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_key
            // 
            this.groupBox_key.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_key.Controls.Add(this.tableLayoutPanel1);
            this.groupBox_key.Location = new System.Drawing.Point(3, 3);
            this.groupBox_key.Name = "groupBox_key";
            this.groupBox_key.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox_key.Size = new System.Drawing.Size(478, 90);
            this.groupBox_key.TabIndex = 0;
            this.groupBox_key.TabStop = false;
            this.groupBox_key.Text = "GUI_SETTINGS_KEY";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label_key_description, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox_key, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_key, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(472, 71);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label_key_description
            // 
            this.label_key_description.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label_key_description, 2);
            this.label_key_description.Location = new System.Drawing.Point(3, 0);
            this.label_key_description.Name = "label_key_description";
            this.label_key_description.Size = new System.Drawing.Size(192, 13);
            this.label_key_description.TabIndex = 1;
            this.label_key_description.Text = "GUI_SETTINGS_KEY_DESCRIPTION";
            // 
            // textBox_key
            // 
            this.textBox_key.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_key.Location = new System.Drawing.Point(161, 16);
            this.textBox_key.Name = "textBox_key";
            this.textBox_key.Size = new System.Drawing.Size(308, 20);
            this.textBox_key.TabIndex = 0;
            // 
            // label_key
            // 
            this.label_key.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_key.AutoSize = true;
            this.label_key.Location = new System.Drawing.Point(3, 13);
            this.label_key.Name = "label_key";
            this.label_key.Size = new System.Drawing.Size(152, 13);
            this.label_key.TabIndex = 2;
            this.label_key.Text = "GUI_SETTINGS_KEY_LABEL";
            this.label_key.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.groupBox_key, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.groupBox_gui, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.groupBox_order, 0, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(484, 461);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.button_cancel);
            this.flowLayoutPanel1.Controls.Add(this.button_apply);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(319, 425);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(162, 27);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(84, 3);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 21);
            this.button_cancel.TabIndex = 4;
            this.button_cancel.Text = "CANCEL";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.OnCancelClicked);
            // 
            // button_apply
            // 
            this.button_apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_apply.Location = new System.Drawing.Point(3, 3);
            this.button_apply.Name = "button_apply";
            this.button_apply.Size = new System.Drawing.Size(75, 21);
            this.button_apply.TabIndex = 3;
            this.button_apply.Text = "APPLY";
            this.button_apply.UseVisualStyleBackColor = true;
            this.button_apply.Click += new System.EventHandler(this.OnApplyClicked);
            // 
            // groupBox_gui
            // 
            this.groupBox_gui.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_gui.Controls.Add(this.tableLayoutPanel2);
            this.groupBox_gui.Location = new System.Drawing.Point(3, 99);
            this.groupBox_gui.Name = "groupBox_gui";
            this.groupBox_gui.Size = new System.Drawing.Size(478, 47);
            this.groupBox_gui.TabIndex = 1;
            this.groupBox_gui.TabStop = false;
            this.groupBox_gui.Text = "GUI_SETTINGS_GUI";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoScroll = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label_name_style, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBox_name_style, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(472, 28);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label_name_style
            // 
            this.label_name_style.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_name_style.AutoSize = true;
            this.label_name_style.Location = new System.Drawing.Point(3, 0);
            this.label_name_style.Name = "label_name_style";
            this.label_name_style.Size = new System.Drawing.Size(163, 28);
            this.label_name_style.TabIndex = 0;
            this.label_name_style.Text = "GUI_SETTINGS_NAME_STYLE";
            this.label_name_style.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox_name_style
            // 
            this.comboBox_name_style.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_name_style.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_name_style.FormattingEnabled = true;
            this.comboBox_name_style.Items.AddRange(new object[] {
            "GUI_SETTINGS_NS_FIRST_LAST",
            "GUI_SETTINGS_NS_LAST_FIRST"});
            this.comboBox_name_style.Location = new System.Drawing.Point(172, 3);
            this.comboBox_name_style.Name = "comboBox_name_style";
            this.comboBox_name_style.Size = new System.Drawing.Size(297, 21);
            this.comboBox_name_style.TabIndex = 1;
            // 
            // groupBox_order
            // 
            this.groupBox_order.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_order.Controls.Add(this.tableLayoutPanel4);
            this.groupBox_order.Location = new System.Drawing.Point(3, 152);
            this.groupBox_order.Name = "groupBox_order";
            this.groupBox_order.Size = new System.Drawing.Size(478, 267);
            this.groupBox_order.TabIndex = 2;
            this.groupBox_order.TabStop = false;
            this.groupBox_order.Text = "GUI_SETTINGS_ORDER";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel4.Controls.Add(this.listBox_order_available, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.listBox_order_selected, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.button_order_add, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.button_order_remove, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.button_order_up, 3, 2);
            this.tableLayoutPanel4.Controls.Add(this.button_order_down, 3, 3);
            this.tableLayoutPanel4.Controls.Add(this.label_order_available, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label_order_selected, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.label_order_description, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label_order_direction, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.comboBox_order_direction, 2, 5);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 6;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(472, 248);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // listBox_order_available
            // 
            this.listBox_order_available.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_order_available.FormattingEnabled = true;
            this.listBox_order_available.Location = new System.Drawing.Point(3, 29);
            this.listBox_order_available.Name = "listBox_order_available";
            this.tableLayoutPanel4.SetRowSpan(this.listBox_order_available, 2);
            this.listBox_order_available.Size = new System.Drawing.Size(197, 184);
            this.listBox_order_available.TabIndex = 0;
            // 
            // listBox_order_selected
            // 
            this.listBox_order_selected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_order_selected.FormattingEnabled = true;
            this.listBox_order_selected.Location = new System.Drawing.Point(236, 29);
            this.listBox_order_selected.Name = "listBox_order_selected";
            this.tableLayoutPanel4.SetRowSpan(this.listBox_order_selected, 2);
            this.listBox_order_selected.Size = new System.Drawing.Size(197, 184);
            this.listBox_order_selected.TabIndex = 1;
            // 
            // button_order_add
            // 
            this.button_order_add.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_order_add.Enabled = false;
            this.button_order_add.Location = new System.Drawing.Point(206, 29);
            this.button_order_add.Name = "button_order_add";
            this.button_order_add.Size = new System.Drawing.Size(24, 23);
            this.button_order_add.TabIndex = 2;
            this.button_order_add.Text = "˃";
            this.button_order_add.UseVisualStyleBackColor = true;
            // 
            // button_order_remove
            // 
            this.button_order_remove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_order_remove.Enabled = false;
            this.button_order_remove.Location = new System.Drawing.Point(206, 59);
            this.button_order_remove.Name = "button_order_remove";
            this.button_order_remove.Size = new System.Drawing.Size(24, 23);
            this.button_order_remove.TabIndex = 3;
            this.button_order_remove.Text = "˂";
            this.button_order_remove.UseVisualStyleBackColor = true;
            // 
            // button_order_up
            // 
            this.button_order_up.Enabled = false;
            this.button_order_up.Location = new System.Drawing.Point(439, 29);
            this.button_order_up.Name = "button_order_up";
            this.button_order_up.Size = new System.Drawing.Size(24, 23);
            this.button_order_up.TabIndex = 4;
            this.button_order_up.Text = "˄";
            this.button_order_up.UseVisualStyleBackColor = true;
            // 
            // button_order_down
            // 
            this.button_order_down.Enabled = false;
            this.button_order_down.Location = new System.Drawing.Point(439, 59);
            this.button_order_down.Name = "button_order_down";
            this.button_order_down.Size = new System.Drawing.Size(24, 23);
            this.button_order_down.TabIndex = 5;
            this.button_order_down.Text = "˅";
            this.button_order_down.UseVisualStyleBackColor = true;
            // 
            // label_order_available
            // 
            this.label_order_available.AutoSize = true;
            this.label_order_available.Location = new System.Drawing.Point(3, 13);
            this.label_order_available.Name = "label_order_available";
            this.label_order_available.Size = new System.Drawing.Size(194, 13);
            this.label_order_available.TabIndex = 6;
            this.label_order_available.Text = "GUI_SETTINGS_ORDER_AVAILABLE";
            // 
            // label_order_selected
            // 
            this.label_order_selected.AutoSize = true;
            this.label_order_selected.Location = new System.Drawing.Point(236, 13);
            this.label_order_selected.Name = "label_order_selected";
            this.label_order_selected.Size = new System.Drawing.Size(193, 13);
            this.label_order_selected.TabIndex = 7;
            this.label_order_selected.Text = "GUI_SETTINGS_ORDER_SELECTED";
            // 
            // label_order_description
            // 
            this.label_order_description.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.label_order_description, 4);
            this.label_order_description.Location = new System.Drawing.Point(3, 0);
            this.label_order_description.Name = "label_order_description";
            this.label_order_description.Size = new System.Drawing.Size(210, 13);
            this.label_order_description.TabIndex = 8;
            this.label_order_description.Text = "GUI_SETTINGS_ORDER_DESCRIPTION";
            // 
            // label_order_direction
            // 
            this.label_order_direction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_order_direction.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.label_order_direction, 2);
            this.label_order_direction.Location = new System.Drawing.Point(34, 221);
            this.label_order_direction.Name = "label_order_direction";
            this.label_order_direction.Size = new System.Drawing.Size(196, 27);
            this.label_order_direction.TabIndex = 9;
            this.label_order_direction.Text = "GUI_SETTINGS_ORDER_DIRECTION";
            this.label_order_direction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox_order_direction
            // 
            this.comboBox_order_direction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_order_direction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_order_direction.FormattingEnabled = true;
            this.comboBox_order_direction.Items.AddRange(new object[] {
            "GUI_SETTINGS_DIR_DESC",
            "GUI_SETTINGS_DIR_ASC"});
            this.comboBox_order_direction.Location = new System.Drawing.Point(236, 224);
            this.comboBox_order_direction.Name = "comboBox_order_direction";
            this.comboBox_order_direction.Size = new System.Drawing.Size(197, 21);
            this.comboBox_order_direction.TabIndex = 10;
            // 
            // SettingsGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.tableLayoutPanel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsGUI";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GUI_SETTINGS_TITLE";
            this.groupBox_key.ResumeLayout(false);
            this.groupBox_key.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox_gui.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox_order.ResumeLayout(false);
            this.groupBox_order.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_key;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox_gui;
        private System.Windows.Forms.GroupBox groupBox_order;
        private System.Windows.Forms.Button button_apply;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ListBox listBox_order_available;
        private System.Windows.Forms.ListBox listBox_order_selected;
        private System.Windows.Forms.Button button_order_add;
        private System.Windows.Forms.Button button_order_remove;
        private System.Windows.Forms.Button button_order_up;
        private System.Windows.Forms.Button button_order_down;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label_name_style;
        private System.Windows.Forms.ComboBox comboBox_name_style;
        private System.Windows.Forms.Label label_order_available;
        private System.Windows.Forms.Label label_order_selected;
        private System.Windows.Forms.Label label_order_description;
        private System.Windows.Forms.Label label_order_direction;
        private System.Windows.Forms.ComboBox comboBox_order_direction;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox textBox_key;
        private System.Windows.Forms.Label label_key;
        private System.Windows.Forms.Label label_key_description;
    }
}