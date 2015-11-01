using System.Windows.Forms;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Utils;
using param;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        private bool updateMaidClassField, updateYotogiClassField;

        private void InitClassesTab()
        {
            Debugger.Assert(
            () =>
            {
                GetFieldText(tabPage_classes);

                // Maid classes
                GetFieldText(groupBox_maid_classes);
                foreach (DataGridViewColumn column in dataGridView_maid_classes.Columns)
                {
                    column.HeaderText = GetFieldText(column.HeaderText);
                }
                for (MaidClassType e = 0; e < EnumHelper.MaxMaidClassType; e++)
                    dataGridView_maid_classes.Rows.Add(
                    false,
                    GetFieldText($"Maid_{EnumHelper.GetName(e)}"),
                    0,
                    0);
                dataGridView_maid_classes.CellValueChanged += OnClassTabCellValueChanged;
                dataGridView_maid_classes.CellContentClick += OnClassTabCellContentClick;
                dataGridView_maid_classes.Height = dataGridView_maid_classes.ColumnHeadersHeight
                                                   + dataGridView_maid_classes.Rows[0].Height
                                                   * dataGridView_maid_classes.RowCount;

                // Yotogi classes
                GetFieldText(groupBox_yotogi_classes);
                foreach (DataGridViewColumn column in dataGridView_yotogi_classes.Columns)
                {
                    column.HeaderText = GetFieldText(column.HeaderText);
                }
                for (YotogiClassType e = 0; e < EnumHelper.MaxYotogiClass; e++)
                    dataGridView_yotogi_classes.Rows.Add(false, GetFieldText($"Yotogi_{EnumHelper.GetName(e)}"), 0, 0);
                dataGridView_yotogi_classes.CellValueChanged += OnClassTabCellValueChanged;
                dataGridView_yotogi_classes.CellContentClick += OnClassTabCellContentClick;
                dataGridView_yotogi_classes.Height = dataGridView_yotogi_classes.ColumnHeadersHeight
                                                     + dataGridView_yotogi_classes.Rows[0].Height
                                                     * dataGridView_yotogi_classes.RowCount;
            },
            "Failed to load maid classes tab");
        }

        private void OnClassTabCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (clearingTables || e.ColumnIndex != TABLE_COLUMN_HAS)
                return;
            DataGridView table = (DataGridView) sender;
            UpdateMaid_YotogiClassValue<bool>(table, e.ColumnIndex, e.RowIndex);
        }

        private void OnClassTabCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (clearingTables)
                return;
            DataGridView table = (DataGridView) sender;
            UpdateMaid_YotogiClassValue<int>(table, e.ColumnIndex, e.RowIndex);
        }

        private void UpdateMaid_YotogiClassValue<T>(DataGridView table, int col, int row)
        {
            MaidInfo maid = SelectedMaid;
            if (maid == null)
                return;

            object val = table[col, row].Value;

            if (val is bool)
                val = !((bool) val);

            if (table == dataGridView_maid_classes)
            {
                if (!updateMaidClassField)
                {
                    MaidClassType type = (MaidClassType) row;
                    if (val is T)
                        maid.SetValue(type, col, val);
                    else
                        maid.UpdateField(MaidChangeType.MaidClassType, (int) type);
                }
                updateMaidClassField = false;
            }
            else if (table == dataGridView_yotogi_classes)
            {
                if (!updateYotogiClassField)
                {
                    YotogiClassType type = (YotogiClassType) row;
                    if (val is T)
                        maid.SetValue(type, col, val);
                    else
                        maid.UpdateField(MaidChangeType.YotogiClassType, (int) type);
                }
                updateYotogiClassField = false;
            }
        }
    }
}