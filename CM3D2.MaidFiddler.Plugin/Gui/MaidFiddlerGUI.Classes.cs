using System.Windows.Forms;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Utils;

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
                Translation.AddTranslatableControl(tabPage_classes);

                // Maid classes
                Translation.AddTranslatableControl(groupBox_maid_classes);
                foreach (DataGridViewColumn column in dataGridView_maid_classes.Columns)
                {
                    Translation.AddTranslationAction(column.HeaderText, s => column.HeaderText = s);
                }
                for (int e = 0; e < EnumHelper.MaxMaidClass; e++)
                {
                    string key = $"Maid_{EnumHelper.GetMaidClassName(e)}";
                    int i = dataGridView_maid_classes.Rows.Add(false, key, 0, 0);
                    Translation.AddTranslationAction(key, s => dataGridView_maid_classes.Rows[i].Cells[1].Value = s);
                }
                dataGridView_maid_classes.CellValueChanged += OnClassTabCellValueChanged;
                dataGridView_maid_classes.CellContentClick += OnClassTabCellContentClick;
                dataGridView_maid_classes.Height = dataGridView_maid_classes.ColumnHeadersHeight
                                                   + dataGridView_maid_classes.Rows[0].Height
                                                   * dataGridView_maid_classes.RowCount;

                // Yotogi classes
                Translation.AddTranslatableControl(groupBox_yotogi_classes);
                foreach (DataGridViewColumn column in dataGridView_yotogi_classes.Columns)
                {
                    Translation.AddTranslationAction(column.HeaderText, s => column.HeaderText = s);
                }
                foreach (int yotogiClass in EnumHelper.EnabledYotogiClasses)
                {
                    string key = $"Yotogi_{EnumHelper.GetYotogiClassName(yotogiClass)}";
                    int i = dataGridView_yotogi_classes.Rows.Add(false, key, 0, 0);
                    Translation.AddTranslationAction(key, s => dataGridView_yotogi_classes.Rows[i].Cells[1].Value = s);
                }
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
                    if (val is T)
                        maid.SetMaidClassValue(row, col, val);
                    else
                        maid.UpdateField(MaidChangeType.MaidClassType, row);
                }
                updateMaidClassField = false;
            }
            else if (table == dataGridView_yotogi_classes)
            {
                if (!updateYotogiClassField)
                {
                    if (val is T)
                        maid.SetYotogiClassValue(EnumHelper.EnabledYotogiClasses[row], col, val);
                    else
                        maid.UpdateField(MaidChangeType.YotogiClassType, EnumHelper.EnabledYotogiClasses[row]);
                }
                updateYotogiClassField = false;
            }
        }
    }
}