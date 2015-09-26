using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Utils;
using param;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        private Dictionary<Control, MaidChangeType> uiControls;
        private Dictionary<MaidChangeType, bool> valueUpdate;

        private void InitMaidInfoTab()
        {
            Debugger.Assert(
            () =>
            {
                valueUpdate = EnumHelper.MaidChangeTypes.ToDictionary(e => e, e => false);
                uiControls = new Dictionary<Control, MaidChangeType>();

                GetFieldText(tabPage_info);
                GetFieldText(label_sex_experience);
                GetFieldText(label_curr_class);

                InitField(label_first_name, textBox_first_name, MaidChangeType.FirstName);
                InitField(label_last_name, textBox_last_name, MaidChangeType.LastName);
                InitField(null, checkBox_is_fist_name_call, MaidChangeType.FirstNameCall);

                InitField(label_personality, comboBox_personality, MaidChangeType.Personal);
                for (Personal e = Personal.Pure; e < Personal.Max; e++)
                    comboBox_personality.Items.Add(GetFieldText(EnumHelper.GetName(e)));

                InitField(label_contract_type, comboBox_contract_type, MaidChangeType.ContractType);
                for (ContractType e = ContractType.Nurture; e < ContractType.Max; e++)
                    comboBox_contract_type.Items.Add(GetFieldText(EnumHelper.GetName(e)));

                InitField(null, checkBox_leader, MaidChangeType.Leader);
                InitField(label_condition, comboBox_condition, MaidChangeType.Condition);
                for (Condition e = Condition.Null; e < Condition.Max; e++)
                    comboBox_condition.Items.Add(GetFieldText(EnumHelper.GetName(e)));

                InitField(label_condition_special, comboBox_condition_special, MaidChangeType.ConditionSpecial);
                for (ConditionSpecial e = ConditionSpecial.Null; e <= ConditionSpecial.Osioki; e++)
                    comboBox_condition_special.Items.Add(GetFieldText(EnumHelper.GetName(e)));

                InitField(label_employment_day, textBox_employment_day, MaidChangeType.Employment);
                InitField(label_init_seikeiken, comboBox_init_seikeiken, MaidChangeType.InitSeikeiken);
                for (Seikeiken e = Seikeiken.No_No; e < Seikeiken.Max; e++)
                    comboBox_init_seikeiken.Items.Add(GetFieldText(EnumHelper.GetName(e)));

                InitField(label_seikeiken, comboBox_seikeiken, MaidChangeType.Seikeiken);
                for (Seikeiken e = Seikeiken.No_No; e < Seikeiken.Max; e++)
                    comboBox_seikeiken.Items.Add(GetFieldText(EnumHelper.GetName(e)));

                InitField(label_curr_maid_class, comboBox_current_maid_class, MaidChangeType.MaidClassType);
                for (MaidClassType e = MaidClassType.Novice; e < MaidClassType.EnabledMAX; e++)
                    comboBox_current_maid_class.Items.Add(GetFieldText($"Maid_{EnumHelper.GetName(e)}"));

                InitField(label_curr_yotogi_class, comboBox_current_yotogi_class, MaidChangeType.YotogiClassType);
                for (YotogiClassType e = YotogiClassType.Debut; e < YotogiClassType.EnabledMAX; e++)
                    comboBox_current_yotogi_class.Items.Add(GetFieldText($"Yotogi_{EnumHelper.GetName(e)}"));

                InitField(label_profile, textBox_profile, MaidChangeType.Profile);
                InitField(label_free_comment, textBox_free_comment, MaidChangeType.FreeComment);
            },
            "Failed to initialize maid info tab");
        }
    }
}