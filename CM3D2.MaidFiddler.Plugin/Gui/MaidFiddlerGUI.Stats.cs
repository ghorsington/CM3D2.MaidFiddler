using System.Collections.Generic;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Utils;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        private Dictionary<int, MaidChangeType> maidBonusStatsTableDic;
        private Dictionary<int, MaidChangeType> maidEroTableDic;
        private Dictionary<int, MaidChangeType> maidParamsTableDic;
        private Dictionary<int, MaidChangeType> maidStatsTableDic;
        public Dictionary<MaidChangeType, DataGridViewRow> MaidParameters { get; set; }

        private void AddRow(MaidChangeType type,
                            DataGridView table,
                            IDictionary<int, MaidChangeType> dic,
                            bool addLock = true)
        {
            string key = EnumHelper.GetName(type);
            int index = addLock ? table.Rows.Add(key, 0, false) : table.Rows.Add(key, 0);
            dic.Add(index, type);
            Translation.AddTranslationAction(key, s => table.Rows[index].Cells[0].Value = s);
            MaidParameters.Add(type, table.Rows[index]);
        }

        private void InitMaidStatsTab()
        {
            Debugger.Assert(() =>
            {
                MaidParameters = new Dictionary<MaidChangeType, DataGridViewRow>();
                Translation.AddTranslatableControl(tabPage_stats);

                // Maid params
                Translation.AddTranslatableControl(groupBox_params);
                maidParamsTableDic = new Dictionary<int, MaidChangeType>();
                foreach (DataGridViewColumn column in dataGridView_params.Columns)
                {
                    Translation.AddTranslationAction(column.HeaderText, s => column.HeaderText = s);
                }
                dataGridView_params.CellValueChanged += OnCellValueChanged;
                dataGridView_params.CellContentClick += OnCellContentClick;
                AddRow(MaidChangeType.CurExcite, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.CurMind, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.CurReason, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.CurHp, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.Mind, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.Reason, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.Hp, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.Care, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.Charm, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.Elegance, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.Hentai, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.Housi, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.Inyoku, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.Likability, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.MValue, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.Reception, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.Lovely, dataGridView_params, maidParamsTableDic);
                AddRow(MaidChangeType.Frustration, dataGridView_params, maidParamsTableDic);
                dataGridView_params.Height = dataGridView_params.ColumnHeadersHeight
                                             + dataGridView_params.Rows[0].Height*dataGridView_params.RowCount;

                // Maid bonus params
                Translation.AddTranslatableControl(groupBox_maid_params_bonus);
                maidBonusStatsTableDic = new Dictionary<int, MaidChangeType>();
                foreach (DataGridViewColumn column in dataGridView_maid_params_bonus.Columns)
                {
                    Translation.AddTranslationAction(column.HeaderText, s => column.HeaderText = s);
                }
                dataGridView_maid_params_bonus.CellValueChanged += OnCellValueChanged;
                AddRow(MaidChangeType.BonusCare, dataGridView_maid_params_bonus, maidBonusStatsTableDic, false);
                AddRow(MaidChangeType.BonusCharm, dataGridView_maid_params_bonus, maidBonusStatsTableDic, false);
                AddRow(MaidChangeType.BonusElegance, dataGridView_maid_params_bonus, maidBonusStatsTableDic, false);
                AddRow(MaidChangeType.BonusHentai, dataGridView_maid_params_bonus, maidBonusStatsTableDic, false);
                AddRow(MaidChangeType.BonusHousi, dataGridView_maid_params_bonus, maidBonusStatsTableDic, false);
                AddRow(MaidChangeType.BonusHp, dataGridView_maid_params_bonus, maidBonusStatsTableDic, false);
                AddRow(MaidChangeType.BonusInyoku, dataGridView_maid_params_bonus, maidBonusStatsTableDic, false);
                AddRow(MaidChangeType.BonusLovely, dataGridView_maid_params_bonus, maidBonusStatsTableDic, false);
                AddRow(MaidChangeType.BonusMind, dataGridView_maid_params_bonus, maidBonusStatsTableDic, false);
                AddRow(MaidChangeType.BonusMValue, dataGridView_maid_params_bonus, maidBonusStatsTableDic, false);
                AddRow(MaidChangeType.BonusReception, dataGridView_maid_params_bonus, maidBonusStatsTableDic, false);
                AddRow(MaidChangeType.BonusTeachRate, dataGridView_maid_params_bonus, maidBonusStatsTableDic, false);
                dataGridView_maid_params_bonus.Height = dataGridView_maid_params_bonus.ColumnHeadersHeight
                                                        + dataGridView_maid_params_bonus.Rows[0].Height
                                                        *dataGridView_maid_params_bonus.RowCount;

                // Maid ero zones
                Translation.AddTranslatableControl(groupBox_ero_zones);
                maidEroTableDic = new Dictionary<int, MaidChangeType>();
                foreach (DataGridViewColumn column in dataGridView_ero_zones.Columns)
                {
                    Translation.AddTranslationAction(column.HeaderText, s => column.HeaderText = s);
                }
                dataGridView_ero_zones.CellValueChanged += OnCellValueChanged;
                dataGridView_ero_zones.CellContentClick += OnCellContentClick;
                AddRow(MaidChangeType.SexualBack, dataGridView_ero_zones, maidEroTableDic);
                AddRow(MaidChangeType.SexualCuri, dataGridView_ero_zones, maidEroTableDic);
                AddRow(MaidChangeType.SexualFront, dataGridView_ero_zones, maidEroTableDic);
                AddRow(MaidChangeType.SexualMouth, dataGridView_ero_zones, maidEroTableDic);
                AddRow(MaidChangeType.SexualNipple, dataGridView_ero_zones, maidEroTableDic);
                AddRow(MaidChangeType.SexualThroat, dataGridView_ero_zones, maidEroTableDic);
                dataGridView_ero_zones.Height = dataGridView_ero_zones.ColumnHeadersHeight
                                                + dataGridView_ero_zones.Rows[0].Height*dataGridView_ero_zones.RowCount;


                // Maid stats
                Translation.AddTranslatableControl(groupBox_statistics);
                maidStatsTableDic = new Dictionary<int, MaidChangeType>();
                foreach (DataGridViewColumn column in dataGridView_statistics.Columns)
                {
                    Translation.AddTranslationAction(column.HeaderText, s => column.HeaderText = s);
                }
                dataGridView_statistics.CellValueChanged += OnCellValueChanged;
                dataGridView_statistics.CellContentClick += OnCellContentClick;
                AddRow(MaidChangeType.MaidPoint, dataGridView_statistics, maidStatsTableDic);
                AddRow(MaidChangeType.OthersPlayCount, dataGridView_statistics, maidStatsTableDic);
                AddRow(MaidChangeType.PlayNumber, dataGridView_statistics, maidStatsTableDic);
                AddRow(MaidChangeType.StudyRate, dataGridView_statistics, maidStatsTableDic);
                AddRow(MaidChangeType.TeachRate, dataGridView_statistics, maidStatsTableDic);
                AddRow(MaidChangeType.YotogiPlayCount, dataGridView_statistics, maidStatsTableDic);
                AddRow(MaidChangeType.Sales, dataGridView_statistics, maidStatsTableDic);
                AddRow(MaidChangeType.TotalSales, dataGridView_statistics, maidStatsTableDic);
                AddRow(MaidChangeType.Evaluation, dataGridView_statistics, maidStatsTableDic);
                AddRow(MaidChangeType.TotalEvaluation, dataGridView_statistics, maidStatsTableDic);
                AddRow(MaidChangeType.PopularRank, dataGridView_statistics, maidStatsTableDic);
                dataGridView_statistics.Height = dataGridView_statistics.ColumnHeadersHeight
                                                 + dataGridView_statistics.Rows[0].Height
                                                 *dataGridView_statistics.RowCount;
            }, "Failed to load maid stats tab");
        }

        private void OnCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (clearingTables) return;
            if (e.ColumnIndex != PARAMS_COLUMN_LOCK) return;
            DataGridView table = (DataGridView) sender;

            MaidInfo maid = SelectedMaid;
            if (maid == null) return;

            MaidChangeType? type = null;
            if (table == dataGridView_params) type = maidParamsTableDic[e.RowIndex];
            else if (table == dataGridView_ero_zones) type = maidEroTableDic[e.RowIndex];
            else if (table == dataGridView_statistics) type = maidStatsTableDic[e.RowIndex];
            if (type == null) return;

            bool val = !((bool) table[e.ColumnIndex, e.RowIndex].Value);
            if (val) maid.Lock(type.Value);
            else maid.Unlock(type.Value);
        }

        private void OnCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (clearingTables || e.ColumnIndex == PARAMS_COLUMN_LOCK) return;
            DataGridView table = (DataGridView) sender;
            MaidInfo maid = SelectedMaid;
            if (SelectedMaid == null) return;

            MaidChangeType? type = null;
            if (table == dataGridView_params) type = maidParamsTableDic[e.RowIndex];
            else if (table == dataGridView_ero_zones) type = maidEroTableDic[e.RowIndex];
            else if (table == dataGridView_maid_params_bonus) type = maidBonusStatsTableDic[e.RowIndex];
            else if (table == dataGridView_statistics) type = maidStatsTableDic[e.RowIndex];
            if (type == null) return;

            if (valueUpdate[type.Value])
            {
                valueUpdate[type.Value] = false;
                return;
            }

            object val = table[e.ColumnIndex, e.RowIndex].Value;

            if (!(val is int) && !(val is long))
            {
                maid.UpdateField(type.Value);
                return;
            }

            if (maid.IsHardLocked(type.Value))
            {
                Debugger.WriteLine(LogLevel.Info,
                    $"Value {EnumHelper.GetName(type.Value)} is locked! Unlocking temporarily...");
                maid.UnlockTemp(type.Value);
            }

            maid.SetValue(type.Value, val);
        }
    }
}