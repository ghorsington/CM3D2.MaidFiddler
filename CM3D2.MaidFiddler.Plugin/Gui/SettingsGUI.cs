using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Utils;
using UnityEngine;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class SettingsGUI : Form
    {
        private readonly MaidFiddler plugin;
        private string originalKeyCombo;

        public SettingsGUI(MaidFiddler plugin)
        {
            this.plugin = plugin;
            InitializeComponent();
            LoadLabels();

            LoadComponents();
        }

        private void LoadComponents()
        {
            AutoValidate = AutoValidate.EnablePreventFocusChange;
            textBox_key.Text = originalKeyCombo = EnumHelper.EnumsToString(plugin.CFGStartGUIKey, '+');
            textBox_key.Validating += ValidateKeyCombo;

            comboBox_name_style.SelectedIndex = plugin.CFGUseJapaneseNameStyle ? 1 : 0;
            checkBox_open_on_startup.Checked = plugin.CFGOpenOnStartup;

            List<MaidOrderStyle> orderStyles = EnumHelper.GetValues<MaidOrderStyle>().ToList();
            List<MaidOrderStyle> selectedOrderStyles = plugin.CFGOrderStyle;
            foreach (MaidOrderStyle orderStyle in selectedOrderStyles)
            {
                listBox_order_selected.Items.Add(new MaidOrderStyleData {OrderStyle = orderStyle});
            }
            listBox_order_selected.DisplayMember = "DisplayName";
            listBox_order_selected.SelectedIndexChanged += OnSelectedOrderStyleSelected;

            IEnumerable<MaidOrderStyle> avalableOrderStyles = orderStyles.Except(selectedOrderStyles);
            foreach (MaidOrderStyle orderStyle in avalableOrderStyles)
            {
                listBox_order_available.Items.Add(new MaidOrderStyleData {OrderStyle = orderStyle});
            }
            listBox_order_available.DisplayMember = "DisplayName";
            listBox_order_available.SelectedIndexChanged += OnAvailableOrderStyleSelected;

            comboBox_order_direction.SelectedIndex = (((int) plugin.CFGOrderDirection) + 1) / 2;

            button_order_add.Click += OnOrderStyleAdd;
            button_order_remove.Click += OnOrderStyleRemove;
            button_order_up.Click += MoveUpSelectedOrderStyle;
            button_order_down.Click += MoveDownSelectedOrderStyle;
        }

        private void ValidateKeyCombo(object sender, CancelEventArgs e)
        {
            if (EnumHelper.ParseEnums<KeyCode>(textBox_key.Text, '+').Count != 0)
                return;
            e.Cancel = true;
            textBox_key.Text = originalKeyCombo;
            textBox_key.Select(0, originalKeyCombo.Length);
            SystemSounds.Hand.Play();
        }

        private void MoveUpSelectedOrderStyle(object sender, EventArgs eventArgs)
        {
            if (listBox_order_selected.SelectedIndex <= 0)
                return;
            int selectedIndex = listBox_order_selected.SelectedIndex;
            MaidOrderStyleData orderStyle = (MaidOrderStyleData) listBox_order_selected.Items[selectedIndex];
            listBox_order_selected.Items.RemoveAt(selectedIndex);
            listBox_order_selected.Items.Insert(selectedIndex - 1, orderStyle);
            listBox_order_selected.SelectedIndex = selectedIndex - 1;
        }

        private void MoveDownSelectedOrderStyle(object sender, EventArgs eventArgs)
        {
            if (listBox_order_selected.SelectedIndex == -1
                || listBox_order_selected.SelectedIndex >= listBox_order_selected.Items.Count - 1)
                return;
            int selectedIndex = listBox_order_selected.SelectedIndex;
            MaidOrderStyleData orderStyle = (MaidOrderStyleData) listBox_order_selected.Items[selectedIndex];
            listBox_order_selected.Items.RemoveAt(selectedIndex);
            listBox_order_selected.Items.Insert(selectedIndex + 1, orderStyle);
            listBox_order_selected.SelectedIndex = selectedIndex + 1;
        }

        private void OnOrderStyleRemove(object sender, EventArgs eventArgs)
        {
            if (listBox_order_selected.SelectedIndex == -1)
                return;
            MaidOrderStyleData orderStyle =
            (MaidOrderStyleData) listBox_order_selected.Items[listBox_order_selected.SelectedIndex];
            listBox_order_selected.Items.RemoveAt(listBox_order_selected.SelectedIndex);
            listBox_order_available.Items.Add(orderStyle);
            if (listBox_order_selected.Items.Count <= 1)
                button_order_remove.Enabled = false;
        }

        private void OnOrderStyleAdd(object sender, EventArgs eventArgs)
        {
            if (listBox_order_available.SelectedIndex == -1)
                return;
            MaidOrderStyleData orderStyle =
            (MaidOrderStyleData) listBox_order_available.Items[listBox_order_available.SelectedIndex];
            listBox_order_available.Items.RemoveAt(listBox_order_available.SelectedIndex);
            listBox_order_selected.Items.Add(orderStyle);
        }

        private void OnAvailableOrderStyleSelected(object sender, EventArgs eventArgs)
        {
            bool enable = listBox_order_available.SelectedIndex != -1;
            button_order_add.Enabled = enable;
        }

        private void OnSelectedOrderStyleSelected(object sender, EventArgs eventArgs)
        {
            bool enable = listBox_order_selected.SelectedIndex != -1;
            button_order_remove.Enabled = enable && listBox_order_selected.Items.Count > 1;
            button_order_up.Enabled = listBox_order_selected.SelectedIndex > 0;
            button_order_down.Enabled = enable
                                        && listBox_order_selected.SelectedIndex < listBox_order_selected.Items.Count - 1;
        }

        private void LoadLabels()
        {
            Text = Translation.GetTranslation(Text);

            Translation.GetTranslation(groupBox_key);
            Translation.GetTranslation(label_key_description);
            Translation.GetTranslation(label_key);

            Translation.GetTranslation(groupBox_gui);
            Translation.GetTranslation(label_name_style);
            for (int i = 0; i < comboBox_name_style.Items.Count; i++)
            {
                comboBox_name_style.Items[i] = Translation.GetTranslation((string) comboBox_name_style.Items[i]);
            }
            Translation.GetTranslation(checkBox_open_on_startup);

            Translation.GetTranslation(groupBox_order);
            Translation.GetTranslation(label_order_description);
            Translation.GetTranslation(label_order_available);
            Translation.GetTranslation(label_order_selected);
            Translation.GetTranslation(label_order_direction);
            for (int i = 0; i < comboBox_order_direction.Items.Count; i++)
            {
                comboBox_order_direction.Items[i] =
                Translation.GetTranslation((string) comboBox_order_direction.Items[i]);
            }

            Translation.GetTranslation(button_apply);
            Translation.GetTranslation(button_cancel);
        }

        private void OnApplyClicked(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            plugin.CFGStartGUIKey = EnumHelper.ParseEnums<KeyCode>(textBox_key.Text, '+');
            plugin.CFGUseJapaneseNameStyle = comboBox_name_style.SelectedIndex == 1;
            plugin.CFGOrderStyle =
            listBox_order_selected.Items.Cast<MaidOrderStyleData>().Select(data => data.OrderStyle).ToList();
            plugin.CFGOrderDirection = (MaidOrderDirection) (comboBox_order_direction.SelectedIndex * 2 - 1);

            Close();
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void OnOpenOnStartupCheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox) sender;
            plugin.CFGOpenOnStartup = cb.Checked;
        }

        private struct MaidOrderStyleData
        {
            public MaidOrderStyle OrderStyle;
            public string DisplayName => Translation.GetTranslation($"OrderStyle_{OrderStyle}");
        }
    }
}