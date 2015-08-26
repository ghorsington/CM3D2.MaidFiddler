using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using param;
using Schedule;
using Status = param_player.Status;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        private bool allYotogiCommandsVisible;
        private bool removeValueLimit;

        private void CleanDebt(object sender, EventArgs e)
        {
            Player.Player.SetSalonLoan(0);
        }

        private void ResetVip(object sender, EventArgs e)
        {
            foreach (KeyValuePair<int, Status.NightWorkState> workState in Player.Player.status_.night_works_state_dic)
            {
                workState.Value.finish = false;
            }
        }

        private void SetForceEnableAll(object sender, EventArgs e)
        {
            MaidInfo maid = SelectedMaid;
            foreach (KeyValuePair<int, ScheduleCSVData.NoonWork> noonWork in ScheduleCSVData.NoonWorkData)
            {
                maid.SetWorkValue(noonWork.Value.id, TABLE_COLUMN_HAS, true);
            }
            foreach (KeyValuePair<int, ScheduleCSVData.NightWork> nightWork in ScheduleCSVData.NightWorkData)
            {
                maid.SetNightWorkValue(nightWork.Value.id, true);
                maid.UpdateNightWorkValue(nightWork.Value.id);
            }
        }

        private void SetMaxAll(object sender, EventArgs e)
        {
            SetMaxStats(sender, e);
            SetMaxEroZones(sender, e);
            SetMaxYotogiLevel(sender, e);
            SetMaxWorkPlayCount(sender, e);
            SetMaxFeature(sender, e);
            SetMaxPropensity(sender, e);
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

            for (Feature i = Feature.Null + 1; i < Feature.Max; i++)
            {
                maidParam.SetFeature(i, true);
            }
        }

        private void SetMaxPropensity(object sender, EventArgs e)
        {
            MaidParam maidParam = SelectedMaid.Maid.Param;

            for (Propensity i = Propensity.Null + 1; i < Propensity.Max; i++)
            {
                maidParam.SetPropensity(i, true);
            }
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
            maidParam.SetCurHp(999);
            maidParam.SetCurMind(999);
            maidParam.SetCurReason(999);
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
        }

        private void SetMaxWorkPlayCount(object sender, EventArgs e)
        {
            MaidInfo maid = SelectedMaid;
            foreach (KeyValuePair<int, ScheduleCSVData.NoonWork> noonWork in ScheduleCSVData.NoonWorkData)
            {
                maid.Maid.Param.SetNewGetWork(noonWork.Value.id);
                maid.SetWorkValue(noonWork.Value.id, TABLE_COLUMN_TOTAL_XP, 999U);
            }
        }

        private void SetMaxYotogiLevel(object sender, EventArgs e)
        {
            MaidInfo maid = SelectedMaid;
            foreach (KeyValuePair<int, Yotogi.SkillData> dataDic in Yotogi.skill_data_list.SelectMany(s => s))
            {
                maid.Maid.Param.SetNewGetSkill(dataDic.Value.id);
                maid.Maid.Param.AddSkillExp(dataDic.Value.id, 10000);
                maid.UpdateSkillData(dataDic.Value.id);
            }
        }

        private void ToggleValueLimit(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            removeValueLimit = !removeValueLimit;
            item.Checked = removeValueLimit;
        }

        private void ToggleYotogiCommandEnable(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            allYotogiCommandsVisible = !allYotogiCommandsVisible;
            item.Checked = allYotogiCommandsVisible;
        }

        private void UnlockAll(object sender, EventArgs e)
        {
            UnlockAllSkills(sender, e);
            UnlockAllMaidClasses(sender, e);
            UnlockAllYotogiClasses(sender, e);
        }

        private void UnlockAllItems(object sender, EventArgs e)
        {
            string[] names = Player.Player.status_.have_item_list.Keys.ToArray();
            for (int i = 0; i < Player.Player.status_.have_item_list.Keys.Count; i++)
            {
                Player.Player.AddHaveItem(names[i]);
            }
        }

        private void UnlockAllMaidClasses(object sender, EventArgs e)
        {
            MaidInfo maid = SelectedMaid;
            for (MaidClassType i = 0; i < MaidClassType.EnabledMAX; i++)
            {
                maid.SetValue(i, TABLE_COLUMN_HAS, true);
            }
        }

        private void UnlockAllSkills(object sender, EventArgs e)
        {
            MaidInfo maid = SelectedMaid;
            foreach (KeyValuePair<int, Yotogi.SkillData> dataDic in Yotogi.skill_data_list.SelectMany(s => s))
            {
                maid.Maid.Param.SetNewGetSkill(dataDic.Value.id);
                maid.UpdateHasSkill(dataDic.Value.id);
            }
        }

        private void UnlockAllTrophies(object sender, EventArgs e)
        {
            foreach (KeyValuePair<int, Trophy.Data> data in Trophy.trophy_list)
            {
                Player.Player.AddHaveTrophy(data.Key);
            }
        }

        private void UnlockAllYotogiClasses(object sender, EventArgs e)
        {
            MaidInfo maid = SelectedMaid;
            for (YotogiClassType i = 0; i < YotogiClassType.EnabledMAX; i++)
            {
                maid.SetValue(i, TABLE_COLUMN_HAS, true);
            }
        }
    }
}