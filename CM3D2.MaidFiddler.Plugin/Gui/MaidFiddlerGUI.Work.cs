using System.Collections.Generic;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Hook;
using Schedule;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        private Dictionary<int, int> nightWorkIDToRow;
        private Dictionary<int, int> noonWorkIDToRow;
        private Dictionary<int, int> rowToNightWorkID;
        private Dictionary<int, int> rowToNoonWorkID;
        //public bool updateNightWorkTable;
        private bool updateWorkTable;

        private void InitWorkTab()
        {
            GetFieldText(tabPage_work);
            GetFieldText(groupBox_maid_current_work);

            // Noon
            GetFieldText(groupBox_noon_work);
            InitField(label_work_noon, comboBox_work_noon, MaidChangeType.NoonWorkId);
            rowToNoonWorkID = new Dictionary<int, int>();
            noonWorkIDToRow = new Dictionary<int, int>();
            foreach (DataGridViewColumn column in dataGridView_noon_work_data.Columns)
            {
                column.HeaderText = GetFieldText(column.HeaderText);
            }
            foreach (KeyValuePair<int, ScheduleCSVData.NoonWork> noonWork in ScheduleCSVData.NoonWorkData)
            {
                string name = GetFieldText(noonWork.Value.name);
                int index = dataGridView_noon_work_data.Rows.Add(false, name, 0, (uint) 0);
                comboBox_work_noon.Items.Add(name);
                rowToNoonWorkID.Add(index, noonWork.Key);
                noonWorkIDToRow.Add(noonWork.Key, index);
            }
            dataGridView_noon_work_data.CellContentClick += OnWorkCellContentClick;
            dataGridView_noon_work_data.CellValueChanged += OnWorkCellValueChanged;
            dataGridView_noon_work_data.Height = dataGridView_noon_work_data.ColumnHeadersHeight
                                                 + dataGridView_noon_work_data.Rows[0].Height
                                                 * dataGridView_noon_work_data.RowCount;

            // Night
            GetFieldText(groupBox_night_work);
            InitField(label_work_night, comboBox_work_night, MaidChangeType.NightWorkId);
            nightWorkIDToRow = new Dictionary<int, int>();
            rowToNightWorkID = new Dictionary<int, int>();
            foreach (DataGridViewColumn column in dataGridView_night_work.Columns)
            {
                column.HeaderText = GetFieldText(column.HeaderText);
            }
            foreach (KeyValuePair<int, ScheduleCSVData.NightWork> nightWork in ScheduleCSVData.NightWorkData)
            {
                string name = GetFieldText(nightWork.Value.name);
                int index = dataGridView_night_work.Rows.Add(false, name);
                comboBox_work_night.Items.Add(GetFieldText(nightWork.Value.name));
                nightWorkIDToRow.Add(nightWork.Value.id, index);
                rowToNightWorkID.Add(index, nightWork.Value.id);
            }
            dataGridView_night_work.CellContentClick += OnNightWorkCellContentClick;
            dataGridView_night_work.Height = dataGridView_night_work.ColumnHeadersHeight
                                             + dataGridView_night_work.Rows[0].Height * dataGridView_night_work.RowCount;
        }

        private void OnNightWorkCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (clearingTables || e.ColumnIndex != TABLE_COLUMN_HAS)
                return;

            MaidInfo maid = SelectedMaid;
            if (maid == null)
                return;

            bool val = (bool) dataGridView_night_work[e.ColumnIndex, e.RowIndex].Value;
            int workID = rowToNightWorkID[e.RowIndex];
            maid.SetNightWorkValue(workID, !val);
            maid.UpdateNightWorkValue(workID);
        }

        private void OnWorkCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (clearingTables || e.ColumnIndex != TABLE_COLUMN_HAS)
                return;
            UpdateWorkCell<bool>(e.ColumnIndex, e.RowIndex);
        }

        private void OnWorkCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (clearingTables)
                return;
            if (e.ColumnIndex == TABLE_COLUMN_TOTAL_XP)
                UpdateWorkCell<uint>(e.ColumnIndex, e.RowIndex);
            else
                UpdateWorkCell<int>(e.ColumnIndex, e.RowIndex);
        }

        private void UpdateWorkCell<T>(int col, int row)
        {
            MaidInfo maid = SelectedMaid;
            if (maid == null)
                return;

            object val = dataGridView_noon_work_data[col, row].Value;

            if (val is bool)
                val = !(bool) val;

            int workID = rowToNoonWorkID[row];

            if (!updateWorkTable)
            {
                if (val is T)
                    maid.SetWorkValue(workID, col, val);
                else
                    maid.UpdateWorkData(workID);
            }
            updateWorkTable = false;
        }
    }
}