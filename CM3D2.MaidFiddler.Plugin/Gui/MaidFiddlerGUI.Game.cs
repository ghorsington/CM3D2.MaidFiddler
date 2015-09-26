using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Utils;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        private Dictionary<Control, PlayerChangeType> uiControlsPlayer;
        private Dictionary<PlayerChangeType, bool> valueUpdatePlayer;

        private void InitGameTab()
        {
            Debugger.Assert(
            () =>
            {
                uiControlsPlayer = new Dictionary<Control, PlayerChangeType>();
                valueUpdatePlayer = EnumHelper.GetValues<PlayerChangeType>().ToDictionary(e => e, e => false);

                GetFieldText(tabPage_player);
                GetFieldText(label_salon_stats);

                InitField(label_player_name, textBox_player_name, PlayerChangeType.Name);
                InitField(label_money, textBox_money, PlayerChangeType.Money);
                InitField(label_shop_use_money, textBox_shop_use_money, PlayerChangeType.ShopUseMoney);
                InitField(label_salon_loan, textBox_salon_loan, PlayerChangeType.SalonLoan);
                InitField(label_init_salon_loan, textBox_init_salon_loan, PlayerChangeType.InitSalonLoan);
                InitField(label_phase_days, textBox_phase_days, PlayerChangeType.PhaseDays);
                InitField(label_scenario_phase, comboBox_scenario_phase, PlayerChangeType.ScenarioPhase);
                for (int i = 0; i < comboBox_scenario_phase.Items.Count; i++)
                {
                    comboBox_scenario_phase.Items[i] = GetFieldText((string) comboBox_scenario_phase.Items[i]);
                }

                InitField(label_days, textBox_days, PlayerChangeType.Days);
                InitField(label_salon_clean, textBox_salon_clean, PlayerChangeType.SalonClean);
                InitField(label_salon_beautiful, textBox_salon_beautiful, PlayerChangeType.SalonBeautiful);
                InitField(label_salon_evaluation, textBox_salon_evaluation, PlayerChangeType.SalonEvaluation);
                InitField(label_current_salon_grade, textBox_current_salon_grade, PlayerChangeType.SalonGrade);
                InitField(label_best_salon_grade, textBox_best_salon_grade, PlayerChangeType.BestSalonGrade);
                InitField(label_maid_points_base, textBox_maid_points_base, PlayerChangeType.BaseMaidPoints);
            },
            "Failed to initalize game tab");
        }
    }
}