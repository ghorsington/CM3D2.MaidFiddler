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

                Translation.AddTranslatableControl(tabPage_info);
                Translation.AddTranslatableControl(label_sex_experience);
                Translation.AddTranslatableControl(label_curr_class);

                InitField(label_first_name, textBox_first_name, MaidChangeType.FirstName);
                InitField(label_last_name, textBox_last_name, MaidChangeType.LastName);
                InitField(null, checkBox_is_fist_name_call, MaidChangeType.FirstNameCall);
                InitField(null, checkBox_is_marriage, MaidChangeType.Marriage);

                InitField(label_personality, comboBox_personality, MaidChangeType.Personal);
                string key;
                for (Personal e = 0; e < EnumHelper.MaxPersonality; e++)
                {
                    key = EnumHelper.GetName(e);
                    int i = comboBox_personality.Items.Add(key);
                    Translation.AddTranslationAction(key, s => comboBox_personality.Items[i] = s);
                }

                InitField(label_contract_type, comboBox_contract_type, MaidChangeType.ContractType);
                for (ContractType e = 0; e < EnumHelper.MaxContractType; e++)
                {
                    key = EnumHelper.GetName(e);
                    int i = comboBox_contract_type.Items.Add(key);
                    Translation.AddTranslationAction(key, s => comboBox_contract_type.Items[i] = s);
                }

                InitField(null, checkBox_leader, MaidChangeType.Leader);
                InitField(null, checkBox_rental, MaidChangeType.RentalMaid);
                InitField(label_condition, comboBox_condition, MaidChangeType.Condition);
                for (Condition e = 0; e < EnumHelper.MaxCondition; e++)
                {
                    key = EnumHelper.GetName(e);
                    int i = comboBox_condition.Items.Add(key);
                    Translation.AddTranslationAction(key, s => comboBox_condition.Items[i] = s);
                }

                InitField(label_condition_special, comboBox_condition_special, MaidChangeType.ConditionSpecial);
                for (ConditionSpecial e = ConditionSpecial.Null; e <= ConditionSpecial.Osioki; e++)
                {
                    key = EnumHelper.GetName(e);
                    int i = comboBox_condition_special.Items.Add(key);
                    Translation.AddTranslationAction(key, s => comboBox_condition_special.Items[i] = s);
                }

                InitField(label_employment_day, textBox_employment_day, MaidChangeType.Employment);
                InitField(label_init_seikeiken, comboBox_init_seikeiken, MaidChangeType.InitSeikeiken);
                for (Seikeiken e = Seikeiken.No_No; e < Seikeiken.Max; e++)
                {
                    key = EnumHelper.GetName(e);
                    int i = comboBox_init_seikeiken.Items.Add(key);
                    Translation.AddTranslationAction(key, s => comboBox_init_seikeiken.Items[i] = s);
                }

                InitField(label_seikeiken, comboBox_seikeiken, MaidChangeType.Seikeiken);
                for (Seikeiken e = Seikeiken.No_No; e < Seikeiken.Max; e++)
                {
                    key = EnumHelper.GetName(e);
                    int i = comboBox_seikeiken.Items.Add(key);
                    Translation.AddTranslationAction(key, s => comboBox_seikeiken.Items[i] = s);
                }

                InitField(label_curr_maid_class, comboBox_current_maid_class, MaidChangeType.MaidClassType);
                for (MaidClassType e = 0; e < EnumHelper.MaxMaidClass; e++)
                {
                    key = $"Maid_{EnumHelper.GetName(e)}";
                    int i = comboBox_current_maid_class.Items.Add(key);
                    Translation.AddTranslationAction(key, s => comboBox_current_maid_class.Items[i] = s);
                }

                InitField(label_curr_yotogi_class, comboBox_current_yotogi_class, MaidChangeType.YotogiClassType);
                for (YotogiClassType e = 0; e < EnumHelper.MaxYotogiClass; e++)
                {
                    key = $"Yotogi_{EnumHelper.GetName(e)}";
                    int i = comboBox_current_yotogi_class.Items.Add(key);
                    Translation.AddTranslationAction(key, s => comboBox_current_yotogi_class.Items[i] = s);
                }

                InitField(label_profile, textBox_profile, MaidChangeType.Profile);
                InitField(label_free_comment, textBox_free_comment, MaidChangeType.FreeComment);

                if (FiddlerUtils.GameVersion < 121 || !FiddlerUtils.PlusPackInstalled)
                    checkBox_rental.Hide();

                if (FiddlerUtils.GameVersion < 133 || !FiddlerUtils.PlusPack2Installed)
                    checkBox_is_marriage.Hide();
            },
            "Failed to initialize maid info tab");
        }
    }
}