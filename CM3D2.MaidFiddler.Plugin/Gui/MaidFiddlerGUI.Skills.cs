using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Utils;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        private Dictionary<int, int> rowToSkillID;
        private Dictionary<int, int> skillIDToRow;
        private bool updateSkillTable;

        private void InitYotogiSkillTab()
        {
            Debugger.Assert(
                () =>
                {
                    Translation.AddTranslatableControl(tabPage_skills);

                    rowToSkillID = new Dictionary<int, int>();
                    skillIDToRow = new Dictionary<int, int>();
                    foreach (DataGridViewColumn column in dataGridView_skill_data.Columns)
                        Translation.AddTranslationAction(column.HeaderText, s => column.HeaderText = s);
                    foreach (KeyValuePair<int, Yotogi.SkillData> dataDic in Yotogi.skill_data_list.SelectMany(e => e))
                    {
                        string key = dataDic.Value.name;
                        int row = dataGridView_skill_data.Rows.Add(false, key, 0, 0, (uint) 0);
                        Translation.AddTranslationAction(key,
                                                         s => dataGridView_skill_data.Rows[row].Cells[1].Value = s);
                        rowToSkillID.Add(row, dataDic.Key);
                        skillIDToRow.Add(dataDic.Key, row);
                    }
                    dataGridView_skill_data.CellContentClick += OnSkillCellContentClick;
                    dataGridView_skill_data.CellValueChanged += OnSkillCellValueChanged;
                    dataGridView_skill_data.Height = dataGridView_skill_data.ColumnHeadersHeight +
                                                     dataGridView_skill_data.Rows[0].Height *
                                                     dataGridView_skill_data.RowCount;
                },
                "Failed to initalize yotogi skill tab");
        }

        private void OnSkillCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (clearingTables || e.ColumnIndex != TABLE_COLUMN_HAS)
                return;
            UpdateSkillCell<bool>(e.ColumnIndex, e.RowIndex);
        }

        private void OnSkillCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (clearingTables)
                return;
            if (e.ColumnIndex == SKILL_COLUMN_PLAY_COUNT)
                UpdateSkillCell<uint>(e.ColumnIndex, e.RowIndex);
            else
                UpdateSkillCell<int>(e.ColumnIndex, e.RowIndex);
        }

        private void UpdateSkillCell<T>(int col, int row)
        {
            MaidInfo maid = SelectedMaid;
            if (maid == null)
                return;

            object val = dataGridView_skill_data[col, row].Value;

            if (val is bool)
                val = !(bool) val;

            int skillID = rowToSkillID[row];

            if (!updateSkillTable)
                if (val is T || col == SKILL_COLUMN_PLAY_COUNT && val is uint)
                    maid.SetSkillValue(skillID, col, val);
                else
                    maid.UpdateSkillData(skillID);
            updateSkillTable = false;
        }
    }
}