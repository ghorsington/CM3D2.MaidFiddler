using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Utils;
using param;
using Schedule;
using Status = param_player.Status;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        private bool allYotogiCommandsVisible;
        private bool forceAllScenesEnabled;
        private bool removeValueLimit;
        private bool vipAlwaysVisible;
        private bool yotogiAllSkillsVisible;
        private bool yotogiSkillsVisible;

        private void ToggleAllScenesVisible(object sender, EventArgs e)
        {
            if (FiddlerUtils.GameVersion < 119)
            {
                string title = Translation.IsTranslated("FEATURE_UNSUPPORTED_TITLE")
                                   ? Translation.GetTranslation("FEATURE_UNSUPPORTED_TITLE")
                                   : "This feature is unsupported";
                string text = Translation.IsTranslated("FEATURE_UNSUPPORTED")
                                  ? Translation.GetTranslation("FEATURE_UNSUPPORTED")
                                  : "This feature is unsupported in this version of CM3D2.\nUpdate your game to use this feature.";
                MessageBox.Show(text, title, MessageBoxButtons.OK);
                return;
            }
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            forceAllScenesEnabled = !forceAllScenesEnabled;
            item.Checked = forceAllScenesEnabled;
        }

        private void CleanDebt(object sender, EventArgs e)
        {
            Player.Player.SetSalonLoan(0);
        }

        private void LockAllValues(object sender, EventArgs e)
        {
            MaidInfo maid = SelectedMaid;
            maid.SetAllLock(true);
        }

        private void ResetVip(object sender, EventArgs e)
        {
            foreach (var workState in Player.Player.status_.night_works_state_dic)
                workState.Value.finish = false;
        }

        private void SetClassIsHave(Maid maid, string classDataFieldName, int classID, bool value)
        {
            FieldInfo classDataField = maid.Param.status_.GetType().GetField(classDataFieldName);
            object classData =
                classDataField.FieldType.GetMethod("GetValue", new[] {typeof(int)})
                              .Invoke(classDataField.GetValue(maid.Param.status_), new[] {(object) classID});
            classData.GetType().GetField("is_have").SetValue(classData, value);
        }

        private void SetClassLevel(Maid maid, string classDataFieldName, int classID, int level)
        {
            FieldInfo classDataField = maid.Param.status_.GetType().GetField(classDataFieldName);
            object classData =
                classDataField.FieldType.GetMethod("GetValue", new[] {typeof(int)})
                              .Invoke(classDataField.GetValue(maid.Param.status_), new[] {(object) classID});
            ((SimpleExperienceSystem) classData.GetType().GetField("exp_system").GetValue(classData)).SetLevel(level);
        }

        private void SetClassLevel(object sender, EventArgs e)
        {
            Debugger.WriteLine(LogLevel.Info, "Prompting class level set.");
            uint v;
            TextDialog td = new TextDialog(Translation.GetTranslation("GUI_CLASS_LVL_TITLE"),
                                           Translation.GetTranslation("GUI_CLASS_LVL_PROMPT"), "0",
                                           s => uint.TryParse(s, out v) && (v <= 10), Translation.GetTranslation("OK"),
                                           Translation.GetTranslation("CANCEL"))
            {
                StartPosition = FormStartPosition.CenterParent
            };
            DialogResult dr = td.ShowDialog(this);
            Debugger.WriteLine(LogLevel.Info, $"Prompt result: {EnumHelper.GetName(dr)}, {td.Input}");

            if (dr != DialogResult.OK)
                return;
            v = uint.Parse(td.Input);
            int val = (int) v;
            td.Dispose();

            MaidInfo selected = SelectedMaid;
            Maid maid = selected.Maid;

            for (int maidClass = 0; maidClass < EnumHelper.MaxMaidClass; maidClass++)
            {
                SetClassIsHave(maid, "maid_class_data", maidClass, true);
                SetClassLevel(maid, "maid_class_data", maidClass, val);
            }
            selected.UpdateMaidClasses();

            foreach (int yotogiClass in EnumHelper.EnabledYotogiClasses)
            {
                SetClassIsHave(maid, "yotogi_class_data", yotogiClass, true);
                SetClassLevel(maid, "yotogi_class_data", yotogiClass, val);
            }

            selected.UpdateYotogiClasses();
        }

        private void SetForceEnableAll(object sender, EventArgs e)
        {
            MaidInfo maid = SelectedMaid;
            Debugger.Assert(() =>
                            {
                                foreach (var noonWork in ScheduleCSVData.NoonWorkData)
                                    maid.SetWorkValue(noonWork.Value.id, TABLE_COLUMN_HAS, true);
                                foreach (var nightWork in ScheduleCSVData.NightWorkData)
                                {
                                    maid.SetNightWorkValue(nightWork.Value.id, true);
                                    maid.UpdateNightWorkValue(nightWork.Value.id);
                                }
                            }, "Failed to force all work enabled");
        }

        private void SetForceDisableAll(object sender, EventArgs e)
        {
            MaidInfo maid = SelectedMaid;
            Debugger.Assert(() =>
                            {
                                foreach (var noonWork in ScheduleCSVData.NoonWorkData)
                                    maid.SetWorkValue(noonWork.Value.id, TABLE_COLUMN_HAS, false);
                                foreach (var nightWork in ScheduleCSVData.NightWorkData)
                                {
                                    maid.SetNightWorkValue(nightWork.Value.id, false);
                                    maid.UpdateNightWorkValue(nightWork.Value.id);
                                }
                            }, "Failed to force all work disabled");
        }

        private void SetMaxAll(object sender, EventArgs e)
        {
            SetMaxStats(sender, e);
            SetMaxEroZones(sender, e);
            SetMaxYotogiLevel(sender, e);
            SetMaxWorkPlayCount(sender, e);
            SetMaxFeature(sender, e);
            SetMaxPropensity(sender, e);
            SetMaxClassLevel(sender, e);
        }

        private void SetMaxClassLevel(object sender, EventArgs e)
        {
            MaidInfo selected = SelectedMaid;
            Maid maid = selected.Maid;

            for (int maidClass = 0; maidClass < EnumHelper.MaxMaidClass; maidClass++)
            {
                SetClassIsHave(maid, "maid_class_data", maidClass, true);
                SetClassLevel(maid, "maid_class_data", maidClass, 10);
            }
            selected.UpdateMaidClasses();

            foreach (int yotogiClass in EnumHelper.EnabledYotogiClasses)
            {
                SetClassIsHave(maid, "yotogi_class_data", yotogiClass, true);
                SetClassLevel(maid, "yotogi_class_data", yotogiClass, 10);
            }
            selected.UpdateYotogiClasses();
        }

        private void SetMaxCredits(object sender, EventArgs e)
        {
            Player.Player.SetMoney(9999999999L);
        }

        private void SetMaxEroZones(object sender, EventArgs e)
        {
            MaidParam maidParam = SelectedMaid.Maid.Param;
            maidParam.SetSexualMouth(1000);
            maidParam.SetSexualCuri(1000);
            maidParam.SetSexualNipple(1000);
            maidParam.SetSexualThroat(1000);
        }

        private void SetMaxFeature(object sender, EventArgs e)
        {
            MaidParam maidParam = SelectedMaid.Maid.Param;

            for (Feature i = Feature.Null + 1; i < EnumHelper.MaxFeature; i++)
                maidParam.SetFeature(i, true);
        }

        private void SetMaxPropensity(object sender, EventArgs e)
        {
            MaidParam maidParam = SelectedMaid.Maid.Param;

            for (Propensity i = Propensity.Null + 1; i < EnumHelper.MaxPropensity; i++)
                maidParam.SetPropensity(i, true);
        }

        private void SetMaxSalonGrade(object sender, EventArgs e)
        {
            Player.Player.SetSalonBeautiful(999);
            Player.Player.SetSalonClean(999);
            Player.Player.SetSalonEvaluation(999);
            Player.Player.SetSalonGrade(5);
            Player.Player.SetBestSalonGrade(5);
        }

        private void SetMaxStats(object sender, EventArgs e)
        {
            MaidParam maidParam = SelectedMaid.Maid.Param;

            maidParam.SetCare(9999);
            maidParam.SetCharm(9999);
            maidParam.SetElegance(9999);
            maidParam.SetEvaluation(999999L);
            maidParam.SetFrustration(0);
            maidParam.SetHentai(9999);
            maidParam.SetHousi(9999);
            maidParam.SetHp(999);
            maidParam.SetInyoku(9999);
            maidParam.SetLikability(999);
            maidParam.SetLovely(9999);
            maidParam.SetMValue(9999);
            maidParam.SetMaidPoint(999);
            maidParam.SetPlayNumber(9999);
            maidParam.SetMind(9999);
            maidParam.SetReason(9999);
            maidParam.SetReception(9999);
            maidParam.SetPopularRank(99);
            maidParam.SetSales(9999999999L);
            maidParam.SetCurHp(999);
            maidParam.SetCurMind(999);
            maidParam.SetCurReason(999);
        }

        private void SetMaxWorkPlayCount(object sender, EventArgs e)
        {
            MaidInfo maid = SelectedMaid;
            foreach (var noonWork in ScheduleCSVData.NoonWorkData)
            {
                maid.Maid.Param.SetNewGetWork(noonWork.Value.id);
                maid.SetWorkValue(noonWork.Value.id, TABLE_COLUMN_TOTAL_XP, 999U);
            }
        }

        private void SetMaxYotogiLevel(object sender, EventArgs e)
        {
            Debugger.Assert(() =>
            {
                MaidInfo maid = SelectedMaid;
                foreach (var dataDic in Yotogi.skill_data_list.SelectMany(s => s))
                {
                    maid.Maid.Param.SetNewGetSkill(dataDic.Value.id);
                    maid.Maid.Param.AddSkillExp(dataDic.Value.id, 10000);
                    maid.Maid.Param.status_.skill_data[dataDic.Value.id].play_count = 1;
                    maid.UpdateSkillData(dataDic.Value.id);
                }
            }, "Failed to set all yotogi levels to max");
        }

        private void SetUnlockMaxAllMaids(object sender, EventArgs e)
        {
            foreach (var maid in loadedMaids)
            {
                MaidInfo maidInfo = maid.Value;
                Maid currentMaid = maid.Key;
                MaidParam maidParam = currentMaid.Param;
                Debugger.WriteLine(LogLevel.Info,
                                   $"Setting all to max for {currentMaid.Param.status.first_name} {currentMaid.Param.status.last_name}");

                for (int maidClass = 0; maidClass < EnumHelper.MaxMaidClass; maidClass++)
                {
                    SetClassIsHave(currentMaid, "maid_class_data", maidClass, true);
                    SetClassLevel(currentMaid, "maid_class_data", maidClass, 10);
                }
                maidInfo.UpdateMaidClasses();

                foreach (int yotogiClass in EnumHelper.EnabledYotogiClasses)
                {
                    SetClassIsHave(currentMaid, "yotogi_class_data", yotogiClass, true);
                    SetClassLevel(currentMaid, "yotogi_class_data", yotogiClass, 10);
                }
                maidInfo.UpdateYotogiClasses();

                maidParam.SetSexualMouth(1000);
                maidParam.SetSexualCuri(1000);
                maidParam.SetSexualNipple(1000);
                maidParam.SetSexualThroat(1000);

                for (Feature i = Feature.Null + 1; i < EnumHelper.MaxFeature; i++)
                    maidParam.SetFeature(i, true);

                for (Propensity i = Propensity.Null + 1; i < EnumHelper.MaxPropensity; i++)
                    maidParam.SetPropensity(i, true);

                maidParam.SetCare(9999);
                maidParam.SetCharm(9999);
                maidParam.SetElegance(9999);
                maidParam.SetEvaluation(999999L);
                maidParam.SetFrustration(0);
                maidParam.SetHentai(9999);
                maidParam.SetHousi(9999);
                maidParam.SetHp(999);
                maidParam.SetInyoku(9999);
                maidParam.SetLikability(999);
                maidParam.SetLovely(9999);
                maidParam.SetMValue(9999);
                maidParam.SetMaidPoint(999);
                maidParam.SetPlayNumber(9999);
                maidParam.SetMind(9999);
                maidParam.SetReason(9999);
                maidParam.SetReception(9999);
                maidParam.SetPopularRank(99);
                maidParam.SetSales(9999999999L);
                maidParam.SetCurHp(999);
                maidParam.SetCurMind(999);
                maidParam.SetCurReason(999);

                foreach (var noonWork in ScheduleCSVData.NoonWorkData)
                {
                    maidParam.SetNewGetWork(noonWork.Value.id);
                    maidInfo.SetWorkValue(noonWork.Value.id, TABLE_COLUMN_TOTAL_XP, 999U);
                }

                foreach (var dataDic in Yotogi.skill_data_list.SelectMany(s => s))
                {
                    maidParam.SetNewGetSkill(dataDic.Value.id);
                    maidParam.AddSkillExp(dataDic.Value.id, 10000);
                    maidParam.status_.skill_data[dataDic.Value.id].play_count = 1;
                    maidInfo.UpdateSkillData(dataDic.Value.id);
                }

                foreach (var dataDic in Yotogi.skill_data_list.SelectMany(s => s))
                {
                    maidParam.SetNewGetSkill(dataDic.Value.id);
                    maidInfo.UpdateHasSkill(dataDic.Value.id);
                }
            }
        }

        private void SetYotogiUsedTimes(object sender, EventArgs e)
        {
            uint v;
            TextDialog td = new TextDialog(Translation.GetTranslation("GUI_YOTOGI_TIMES_TITLE"),
                                           Translation.GetTranslation("GUI_YOTOGI_TIMES_PROMPT"), "0",
                                           s => uint.TryParse(s, out v), Translation.GetTranslation("OK"),
                                           Translation.GetTranslation("CANCEL"))
            {
                StartPosition = FormStartPosition.CenterParent
            };
            DialogResult dr = td.ShowDialog(this);
            Debugger.WriteLine(LogLevel.Info, $"Prompt result: {EnumHelper.GetName(dr)}, {td.Input}");

            if (dr != DialogResult.OK)
                return;
            v = uint.Parse(td.Input);
            td.Dispose();

            MaidInfo maid = SelectedMaid;

            foreach (var skill in
                Yotogi.skill_data_list.SelectMany(ee => ee).Where(ss => maid.Maid.Param.status.IsGetSkill(ss.Key)))
            {
                maid.Maid.Param.status_.skill_data[skill.Key].play_count = v;
                maid.UpdateSkillData(skill.Value.id);
            }
        }

        private void ToggleValueLimit(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            removeValueLimit = !removeValueLimit;
            item.Checked = removeValueLimit;
        }

        private void ToggleVIPVisible(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            vipAlwaysVisible = !vipAlwaysVisible;
            item.Checked = vipAlwaysVisible;
        }

        private void ToggleYotogiCommandEnable(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            allYotogiCommandsVisible = !allYotogiCommandsVisible;
            item.Checked = allYotogiCommandsVisible;
        }

        private void ToggleYotogiSkillsVisibleBasic(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            yotogiSkillsVisible = !yotogiSkillsVisible;
            item.Checked = yotogiSkillsVisible;
            if (!yotogiAllSkillsVisible)
                return;
            yotogiAllSkillsVisible = false;
            menu_item_all_yotogi_vis.Checked = false;
        }

        private void ToggleYotogiSkillsVisible(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            yotogiAllSkillsVisible = !yotogiAllSkillsVisible;
            item.Checked = yotogiAllSkillsVisible;
            if (!yotogiSkillsVisible)
                return;
            yotogiSkillsVisible = false;
            menu_item_all_yotogi_vis_basic.Checked = false;
        }

        private void UnlockAll(object sender, EventArgs e)
        {
            UnlockAllSkills(sender, e);
            UnlockAllMaidClasses(sender, e);
            UnlockAllYotogiClasses(sender, e);
        }

        private void UnlockAllItems(object sender, EventArgs e)
        {
            var names = Player.Player.status_.have_item_list.Keys.ToArray();
            for (int i = 0; i < Player.Player.status_.have_item_list.Keys.Count; i++)
                Player.Player.AddHaveItem(names[i]);
            foreach (var shopItem in Shop.item_data_dic)
                Player.Player.SetShopLineup(shopItem.Value.id, Status.ShopItemStatus.Purchased);
        }

        private void UnlockAllMaidClasses(object sender, EventArgs e)
        {
            MaidInfo maid = SelectedMaid;
            for (int i = 0; i < EnumHelper.MaxMaidClass; i++)
                maid.SetMaidClassValue(i, TABLE_COLUMN_HAS, true);
        }

        private void UnlockAllSkills(object sender, EventArgs e)
        {
            MaidInfo maid = SelectedMaid;
            foreach (var dataDic in Yotogi.skill_data_list.SelectMany(s => s))
            {
                maid.Maid.Param.SetNewGetSkill(dataDic.Value.id);
                maid.UpdateHasSkill(dataDic.Value.id);
            }
        }

        private void UnlockAllTrophies(object sender, EventArgs e)
        {
            foreach (var data in Trophy.trophy_list)
                Player.Player.AddHaveTrophy(data.Key);
        }

        private void UnlockAllValues(object sender, EventArgs e)
        {
            MaidInfo maid = SelectedMaid;
            maid.SetAllLock(false);
        }

        private void UnlockAllYotogiClasses(object sender, EventArgs e)
        {
            MaidInfo maid = SelectedMaid;
            foreach (int yotogiClass in EnumHelper.EnabledYotogiClasses)
                maid.SetYotogiClassValue(yotogiClass, TABLE_COLUMN_HAS, true);
        }
    }
}