using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Hook;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI : Form
    {
        private bool initialized;

        public MaidFiddlerGUI()
        {
            InitializeComponent();
            
            Player = new PlayerInfo(this);
            removeValueLimit = false;
            InitText();
            InitMenuText();
            InitThumbnail();
            InitMaidInfoTab();
            InitMaidStatsTab();
            InitClassesTab();
            InitWorkTab();
            InitYotogiSkillTab();
            InitMiscTab();
            InitGameTab();
            ControlsEnabled = false;
            Player.UpdateAll();

            InitMaids();

            playerValueUpdateQueue = new Dictionary<PlayerChangeType, Action>();

            FormClosing += OnFormClosing;
            VisibleChanged += OnVisibleChanged;

            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.DrawItem += DrawListBox;
            listBox1.SelectedValueChanged += OnSelectedValueChanged;

            InitHookCallbacks();
        }

        private void DrawListBox(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (listBox1.Items.Count == 0)
                return;
            MaidInfo m = listBox1.Items[e.Index] as MaidInfo;
            if (m == null)
                return;

            Image maidThumb = maidThumbnails.ContainsKey(m.Maid.Param.status.guid)
                              ? maidThumbnails[m.Maid.Param.status.guid] : null;

            if (maidThumb == null && defaultThumb == null)
                e.Graphics.FillRectangle(Brushes.BlueViolet, e.Bounds.X, e.Bounds.Y, e.Bounds.Height, e.Bounds.Height);
            else
            {
                e.Graphics.DrawImage(
                maidThumb ?? defaultThumb,
                e.Bounds.X,
                e.Bounds.Y,
                e.Bounds.Height,
                e.Bounds.Height);
            }
            e.Graphics.DrawString(
            $"{m.Maid.Param.status.first_name} {m.Maid.Param.status.last_name}",
            e.Font,
            Brushes.Black,
            e.Bounds.X + e.Bounds.Height + 5,
            e.Bounds.Y + (e.Bounds.Height - e.Font.Height) / 2,
            StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }

        private void InitMenuText()
        {
            foreach (ToolStripDropDownItem item in menuStrip1.Items)
            {
                LoadMenuText(item);
                item.Text = GetFieldText(item.Text);
            }
        }

        private void LoadMenuText(ToolStripDropDownItem item)
        {
            foreach (ToolStripItem toolStripItem in item.DropDownItems)
            {
                ToolStripDropDownItem downItem = toolStripItem as ToolStripDropDownItem;
                if (downItem != null)
                    LoadMenuText(downItem);
                toolStripItem.Text = GetFieldText(toolStripItem.Text);
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs formClosingEventArgs)
        {
            Debugger.WriteLine($"Closing GUI. Destroying the GUI: {destroyGUI}");
            formClosingEventArgs.Cancel = !destroyGUI;
            if (!destroyGUI)
                Hide();
            //else
            //{
            //    Console.WriteLine("Cleaning up...");
            //    valueUpdateQueue.Clear();
            //    for (int i = 0;
            //         i < valueUpdate.Count;
            //         i++)
            //    {
            //        valueUpdate[(ChangeType) i] = true;
            //    }
            //    listBox1.ClearSelected();
            //    listBox1.Items.Clear();
            //    loadedMaids.Clear();
            //    maidInfos.Clear();
            //    if (maidThumbnails.Count > 0) maidThumbnails.ForEach(m => m.Value.Dispose());
            //    maidThumbnails.Clear();
            //}
        }

        private void OnSelectedValueChanged(object sender, EventArgs e)
        {
            Debugger.WriteLine("Changed selected maid!");
            valueUpdateQueue.Clear();
            MaidInfo maid = SelectedMaid;
            if (maid == null)
            {
                Debugger.WriteLine(LogLevel.Error, "MAID IS NULL");
                ClearAllFields();
                ControlsEnabled = false;
                return;
            }
            Debugger.WriteLine(
            LogLevel.Info,
            $"New maid: {maid.Maid.Param.status.first_name} {maid.Maid.Param.status.last_name}");
            ControlsEnabled = true;
            maid.UpdateAll();
        }

        private void OnVisibleChanged(object sender, EventArgs eventArgs)
        {
            if (!initialized && IsHandleCreated)
            {
                Debugger.WriteLine(LogLevel.Info, "No handle! Creating one...");
                CreateHandle();
                initialized = true;
            }
            if (Visible)
                UpdateMaids(GameMain.Instance.CharacterMgr.GetStockMaidList());
        }

        private void UpdateList()
        {
            listBox1.ClearSelected();
            ClearAllFields();
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            if (loadedMaids.Count > 0)
                listBox1.Items.AddRange(loadedMaids.Select(m => m.Value as object).ToArray());
            listBox1.EndUpdate();
            listBox1.Invalidate();
        }

        private delegate void UpdateInternal(List<Maid> newMaids);
    }
}