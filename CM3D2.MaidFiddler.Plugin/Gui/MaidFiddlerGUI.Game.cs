using System.Collections.Generic;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Utils;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        private Dictionary<int, PlayerChangeType> gameValuesDic;
        private Dictionary<Control, PlayerChangeType> uiControlsPlayer;
        private Dictionary<PlayerChangeType, bool> valueUpdatePlayer;
        public Dictionary<PlayerChangeType, DataGridViewRow> PlayerParameters { get; set; }

        private void AddRow(PlayerChangeType type,
                            DataGridView table,
                            IDictionary<int, PlayerChangeType> dic,
                            bool addLock = true)
        {
            string key = EnumHelper.GetName(type);
            int index = addLock ? table.Rows.Add(key, 0, false) : table.Rows.Add(key, 0);
            dic.Add(index, type);
            Translation.AddTranslationAction(key, s => table.Rows[index].Cells[0].Value = s);
            PlayerParameters.Add(type, table.Rows[index]);
        }

        private void InitGameTab()
        {
            Debugger.Assert(
                () =>
                {
                    PlayerParameters = new Dictionary<PlayerChangeType, DataGridViewRow>();
                    uiControlsPlayer = new Dictionary<Control, PlayerChangeType>();
                    valueUpdatePlayer = EnumHelper.GetValues<PlayerChangeType>().ToDictionary(e => e, e => false);

                    Translation.AddTranslatableControl(tabPage_player);
                    Translation.AddTranslatableControl(groupBox_game_params_gen);
                    InitField(label_player_name, textBox_player_name, PlayerChangeType.Name);
                    InitField(label_scenario_phase, comboBox_scenario_phase, PlayerChangeType.ScenarioPhase);
                    for (int i = 0; i < comboBox_scenario_phase.Items.Count; i++)
                    {
                        int i1 = i;
                        Translation.AddTranslationAction(
                            (string) comboBox_scenario_phase.Items[i],
                            s => comboBox_scenario_phase.Items[i1] = s);
                    }

                    // Game parameters
                    Translation.AddTranslatableControl(groupBox_game_params_adv);
                    gameValuesDic = new Dictionary<int, PlayerChangeType>();
                    foreach (DataGridViewColumn column in dataGridView_game_params.Columns)
                        Translation.AddTranslationAction(column.HeaderText, s => column.HeaderText = s);
                    AddRow(PlayerChangeType.Money, dataGridView_game_params, gameValuesDic);
                    AddRow(PlayerChangeType.ShopUseMoney, dataGridView_game_params, gameValuesDic);
                    AddRow(PlayerChangeType.SalonLoan, dataGridView_game_params, gameValuesDic);
                    AddRow(PlayerChangeType.InitSalonLoan, dataGridView_game_params, gameValuesDic);
                    AddRow(PlayerChangeType.Days, dataGridView_game_params, gameValuesDic);
                    AddRow(PlayerChangeType.PhaseDays, dataGridView_game_params, gameValuesDic);
                    AddRow(PlayerChangeType.BaseMaidPoints, dataGridView_game_params, gameValuesDic);
                    AddRow(PlayerChangeType.SalonClean, dataGridView_game_params, gameValuesDic);
                    AddRow(PlayerChangeType.SalonBeautiful, dataGridView_game_params, gameValuesDic);
                    AddRow(PlayerChangeType.SalonEvaluation, dataGridView_game_params, gameValuesDic);
                    AddRow(PlayerChangeType.SalonGrade, dataGridView_game_params, gameValuesDic);
                    AddRow(PlayerChangeType.BestSalonGrade, dataGridView_game_params, gameValuesDic);
                    dataGridView_game_params.CellValueChanged += OnGameTabCellValueChanged;
                    dataGridView_game_params.CellContentClick += OnGameTabCellContentClick;
                    dataGridView_game_params.Height = dataGridView_game_params.ColumnHeadersHeight +
                                                      dataGridView_game_params.Rows[0].Height *
                                                      dataGridView_game_params.RowCount;
                },
                "Failed to initalize game tab");
        }

        private void OnGameTabCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (clearingTables || e.ColumnIndex != PARAMS_COLUMN_LOCK)
                return;
            DataGridView table = (DataGridView) sender;

            PlayerChangeType type = gameValuesDic[e.RowIndex];

            bool val = !(bool) table[e.ColumnIndex, e.RowIndex].Value;
            if (val)
                Player.Lock(type);
            else
                Player.Unlock(type);
        }

        private void OnGameTabCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (clearingTables || e.ColumnIndex == PARAMS_COLUMN_LOCK)
                return;
            DataGridView table = (DataGridView) sender;

            PlayerChangeType type = gameValuesDic[e.RowIndex];

            if (valueUpdatePlayer[type])
            {
                valueUpdatePlayer[type] = false;
                return;
            }

            object val = table[e.ColumnIndex, e.RowIndex].Value;

            if (!(val is int) && !(val is long))
            {
                Player.UpdateField(type);
                return;
            }

            bool wasLocked = Player.IsLocked(type);
            if (wasLocked)
            {
                Debugger.WriteLine(LogLevel.Info, $"Value {EnumHelper.GetName(type)} is locked! Unlocking...");
                Player.Unlock(type);
            }

            Player.SetValue(type, val);

            if (wasLocked)
            {
                Debugger.WriteLine(LogLevel.Info, $"Returning lock to {EnumHelper.GetName(type)}...");
                Player.Lock(type);
            }
        }
    }
}