using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Utils;
using param;
using Schedule;
using Debugger = CM3D2.MaidFiddler.Plugin.Utils.Debugger;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        // Full skill data found at Yotogi.skill_data_list
        // Full work data at ScheduleCSVData.NoonWorkData and ScheduleCSVData.NightWorkData
        public class MaidInfo
        {
            private readonly MaidFiddlerGUI gui;
            private Dictionary<int, bool> forceUpdateNightWorks;
            private Dictionary<int, bool> forceUpdateNoonWorks;
            private Dictionary<MaidChangeType, Action<MaidChangeType>> maidParamsUpdaters;
            private Dictionary<MaidChangeType, Action<bool>> setFunctionsBool;
            private Dictionary<MaidChangeType, Action<int>> setFunctionsInt;
            private Dictionary<MaidChangeType, Action<long>> setFunctionsLong;
            private Dictionary<MaidChangeType, Action<string>> setFunctionsString;
            private Dictionary<MaidChangeType, Action<int, int>> updateFunctionsID;

            public MaidInfo(Maid maid, MaidFiddlerGUI gui)
            {
                Debugger.WriteLine("Creating new MaidInfo");
                this.gui = gui;
                forceUpdateNightWorks = new Dictionary<int, bool>();
                Maid = maid;
                ValueLocks = new Dictionary<MaidChangeType, bool>();
                EnumHelper.MaidChangeTypes.ForEach(t => ValueLocks.Add(t, false));
                TempUnlocks = new Dictionary<MaidChangeType, bool>();
                EnumHelper.MaidChangeTypes.ForEach(t => TempUnlocks.Add(t, false));
#if DEBUG
                Debugger.WriteLine(LogLevel.Info, "Loading functions");
                Stopwatch watch = new Stopwatch();
                watch.Start();
#endif
                InitFunctions();
#if DEBUG
                watch.Stop();
                Debugger.WriteLine(
                LogLevel.Info,
                $"Functions loaded. Time taken: {watch.Elapsed.TotalMilliseconds} ms.");
#endif
            }

            public Maid Maid { get; }
            private Dictionary<MaidChangeType, bool> TempUnlocks { get; }
            private Dictionary<MaidChangeType, Action> UpdateFunctions { get; set; }
            private Dictionary<MaidChangeType, bool> ValueLocks { get; }

            private void InitFunctions()
            {
                setFunctionsInt = new Dictionary<MaidChangeType, Action<int>>
                {
                    {MaidChangeType.Personal, SetPersonal},
                    {MaidChangeType.ContractType, SetContractType},
                    {MaidChangeType.Condition, SetCondition},
                    {MaidChangeType.ConditionSpecial, SetConditionSpecial},
                    {MaidChangeType.Employment, SetEmploymentDay},
                    {MaidChangeType.InitSeikeiken, SetInitSeikeiken},
                    {MaidChangeType.Seikeiken, SetSeikeiken},
                    {MaidChangeType.MaidClassType, SetMaidClassType},
                    {MaidChangeType.YotogiClassType, SetYotogiClassType},
                    {MaidChangeType.Care, Maid.Param.SetCare},
                    {MaidChangeType.Charm, Maid.Param.SetCharm},
                    {MaidChangeType.CurExcite, Maid.Param.SetCurExcite},
                    {MaidChangeType.CurMind, Maid.Param.SetCurMind},
                    {MaidChangeType.CurReason, Maid.Param.SetCurReason},
                    {MaidChangeType.Elegance, Maid.Param.SetElegance},
                    {MaidChangeType.Frustration, Maid.Param.SetFrustration},
                    {MaidChangeType.Hentai, Maid.Param.SetHentai},
                    {MaidChangeType.Housi, Maid.Param.SetHousi},
                    {MaidChangeType.Hp, Maid.Param.SetHp},
                    {MaidChangeType.Inyoku, Maid.Param.SetInyoku},
                    {MaidChangeType.Likability, Maid.Param.SetLikability},
                    {MaidChangeType.Lovely, Maid.Param.SetLovely},
                    {MaidChangeType.MaidPoint, Maid.Param.SetMaidPoint},
                    {MaidChangeType.Mind, Maid.Param.SetMind},
                    {MaidChangeType.MValue, Maid.Param.SetMValue},
                    {MaidChangeType.OthersPlayCount, Maid.Param.SetOthersPlayCount},
                    {MaidChangeType.PlayNumber, Maid.Param.SetPlayNumber},
                    {MaidChangeType.Reason, Maid.Param.SetReason},
                    {MaidChangeType.Reception, Maid.Param.SetReception},
                    {MaidChangeType.StudyRate, Maid.Param.SetStudyRate},
                    {MaidChangeType.TeachRate, Maid.Param.SetTeachRate},
                    {MaidChangeType.YotogiPlayCount, Maid.Param.SetYotogiPlayCount},
                    {MaidChangeType.SexualBack, SetSexualBack},
                    {MaidChangeType.SexualCuri, Maid.Param.SetSexualCuri},
                    {MaidChangeType.SexualFront, SetSexualFront},
                    {MaidChangeType.SexualMouth, Maid.Param.SetSexualMouth},
                    {MaidChangeType.SexualNipple, Maid.Param.SetSexualNipple},
                    {MaidChangeType.SexualThroat, Maid.Param.SetSexualThroat},
                    {MaidChangeType.CurHp, Maid.Param.SetCurHp},
                    {MaidChangeType.PopularRank, Maid.Param.SetPopularRank},
                    {MaidChangeType.NightWorkId, SetNightWorkId},
                    {MaidChangeType.NoonWorkId, SetNoonWorkId},
                    {MaidChangeType.BonusCare, SetBonusCare},
                    {MaidChangeType.BonusCharm, SetBonusCharm},
                    {MaidChangeType.BonusElegance, SetBonusElegance},
                    {MaidChangeType.BonusHentai, SetBonusHentai},
                    {MaidChangeType.BonusHousi, SetBonusHousi},
                    {MaidChangeType.BonusHp, SetBonusHp},
                    {MaidChangeType.BonusInyoku, SetBonusInyoku},
                    {MaidChangeType.BonusLovely, SetBonusLovely},
                    {MaidChangeType.BonusMind, SetBonusMind},
                    {MaidChangeType.BonusMValue, SetBonusMValue},
                    {MaidChangeType.BonusReception, SetBonusReception},
                    {MaidChangeType.BonusTeachRate, SetBonusTeachRate}
                };

                setFunctionsString = new Dictionary<MaidChangeType, Action<string>>
                {
                    {MaidChangeType.FirstName, SetFirstName},
                    {MaidChangeType.LastName, SetLastName},
                    {MaidChangeType.FreeComment, SetFreeComment},
                    {MaidChangeType.Profile, SetProfile}
                };

                setFunctionsBool = new Dictionary<MaidChangeType, Action<bool>>
                {
                    {MaidChangeType.FirstNameCall, SetFirstNameCall},
                    {MaidChangeType.Leader, SetLeader}
                };

                setFunctionsLong = new Dictionary<MaidChangeType, Action<long>>
                {
                    {MaidChangeType.Sales, Maid.Param.SetSales},
                    {MaidChangeType.TotalSales, Maid.Param.SetTotalSales},
                    {MaidChangeType.Evaluation, Maid.Param.SetEvaluation},
                    {MaidChangeType.TotalEvaluation, Maid.Param.SetTotalEvaluation}
                };

                UpdateFunctions = new Dictionary<MaidChangeType, Action>
                {
                    {MaidChangeType.Personal, UpdatePersonal},
                    {MaidChangeType.ContractType, UpdateContractType},
                    {MaidChangeType.Condition, UpdateCondition},
                    {MaidChangeType.ConditionSpecial, UpdateConditionSpecial},
                    {MaidChangeType.Employment, UpdateEmploymentDay},
                    {MaidChangeType.InitSeikeiken, UpdateInitSeikeiken},
                    {MaidChangeType.Seikeiken, UpdateSeikeiken},
                    {MaidChangeType.MaidClassType, UpdateMaidClassType},
                    {MaidChangeType.YotogiClassType, UpdateYotogiClassType},
                    {MaidChangeType.FirstName, UpdateFirstName},
                    {MaidChangeType.LastName, UpdateLastName},
                    {MaidChangeType.Leader, UpdateLeader},
                    {MaidChangeType.FirstNameCall, UpdateFirstNameCall},
                    {MaidChangeType.FreeComment, UpdateFreeComment},
                    {MaidChangeType.Profile, UpdateProfile},
                    {MaidChangeType.MaidClassExp, UpdateCurrentMaidClass},
                    {MaidChangeType.YotogiClassExp, UpdateCurrentYotogiClass},
                    {MaidChangeType.NightWorkId, UpdateCurrentNightWorkId},
                    {MaidChangeType.NoonWorkId, UpdateCurrentNoonWorkId}
                };

                updateFunctionsID = new Dictionary<MaidChangeType, Action<int, int>>
                {
                    {MaidChangeType.MaidClassType, UpdateMaidClass},
                    {MaidChangeType.YotogiClassType, UpdateYotogiClass},
                    {MaidChangeType.WorkLevel, UpdateWorkLevel},
                    {MaidChangeType.WorkPlayCount, UpdateWorkPlayCount},
                    {MaidChangeType.SkillPlayCount, UpdateSkillPlayCount},
                    {MaidChangeType.SkillExp, UpdateSkillExp}
                };

                maidParamsUpdaters = new Dictionary<MaidChangeType, Action<MaidChangeType>>();
                for (MaidChangeType e = MaidChangeType.Care; e <= MaidChangeType.TotalEvaluation; e++)
                {
                    maidParamsUpdaters.Add(e, UpdateMaidParam);
                }
                maidParamsUpdaters.Add(MaidChangeType.CurHp, UpdateMaidParam);
                maidParamsUpdaters.Add(MaidChangeType.PopularRank, UpdateMaidParam);
                for (MaidChangeType e = MaidChangeType.BonusCare; e <= MaidChangeType.BonusTeachRate; e++)
                {
                    maidParamsUpdaters.Add(e, UpdateMaidParam);
                }


                forceUpdateNoonWorks = new Dictionary<int, bool>();
                foreach (KeyValuePair<int, ScheduleCSVData.NoonWork> noonWork in ScheduleCSVData.NoonWorkData)
                    forceUpdateNoonWorks.Add(noonWork.Key, false);

                forceUpdateNightWorks = new Dictionary<int, bool>();
                foreach (KeyValuePair<int, ScheduleCSVData.NightWork> nightWork in ScheduleCSVData.NightWorkData)
                    forceUpdateNightWorks.Add(nightWork.Key, false);
            }

            public bool IsHardLocked(MaidChangeType type)
            {
                return ValueLocks[type];
            }

            public bool IsLocked(MaidChangeType type)
            {
                bool result = ValueLocks[type] && !TempUnlocks[type];
                Debugger.WriteLine(
                $"Attempting to change the value of {EnumHelper.GetName(type)}. Is locked: {ValueLocks[type]}. Is temp unlock: {TempUnlocks[type]}.");
                TempUnlocks[type] = false;
                return result;
            }

            public bool IsNightWorkForceEnabled(int id)
            {
                return forceUpdateNightWorks[id];
            }

            public bool IsNoonWorkForceEnabled(int id)
            {
                return forceUpdateNoonWorks[id];
            }

            public void Lock(MaidChangeType type)
            {
                if (ValueLocks.ContainsKey(type))
                    ValueLocks[type] = true;
            }

            public void SetAllLock(bool state)
            {
                Debugger.Assert(
                () =>
                {
                    foreach (KeyValuePair<MaidChangeType, Action<MaidChangeType>> maidParamsUpdater in
                    maidParamsUpdaters.Where(maidParamsUpdater => maidParamsUpdater.Key <= MaidChangeType.Profile))
                    {
                        ValueLocks[maidParamsUpdater.Key] = state;
                        gui.MaidParameters[maidParamsUpdater.Key].Cells[PARAMS_COLUMN_LOCK].Value =
                        ValueLocks[maidParamsUpdater.Key];
                    }
                },
                "Failed to set lock state to all parameters");
            }

            public void Unlock(MaidChangeType type)
            {
                if (ValueLocks.ContainsKey(type))
                    ValueLocks[type] = false;
            }

            public void UnlockTemp(MaidChangeType type)
            {
                if (TempUnlocks.ContainsKey(type))
                    TempUnlocks[type] = true;
            }

            #region Setters

            private void SetBonusCare(int val)
            {
                Maid.Param.status_.maid_class_bonus_status.care = val;
            }

            private void SetBonusCharm(int val)
            {
                Maid.Param.status_.maid_class_bonus_status.charm = val;
            }

            private void SetBonusElegance(int val)
            {
                Maid.Param.status_.maid_class_bonus_status.elegance = val;
            }

            private void SetBonusHentai(int val)
            {
                Maid.Param.status_.maid_class_bonus_status.hentai = val;
            }

            private void SetBonusHousi(int val)
            {
                Maid.Param.status_.maid_class_bonus_status.housi = val;
            }

            private void SetBonusHp(int val)
            {
                Maid.Param.status_.maid_class_bonus_status.hp = val;
            }

            private void SetBonusInyoku(int val)
            {
                Maid.Param.status_.maid_class_bonus_status.inyoku = val;
            }

            private void SetBonusLovely(int val)
            {
                Maid.Param.status_.maid_class_bonus_status.lovely = val;
            }

            private void SetBonusMind(int val)
            {
                Maid.Param.status_.maid_class_bonus_status.mind = val;
            }

            private void SetBonusMValue(int val)
            {
                Maid.Param.status_.maid_class_bonus_status.m_value = val;
            }

            private void SetBonusReception(int val)
            {
                Maid.Param.status_.maid_class_bonus_status.reception = val;
            }

            private void SetBonusTeachRate(int val)
            {
                Maid.Param.status_.maid_class_bonus_status.teach_rate = val;
            }

            private void SetNightWorkId(int index)
            {
                Maid.Param.SetNightWorkId(gui.rowToNightWorkID[index]);
            }

            public void SetNightWorkValue(int workId, bool val)
            {
                forceUpdateNightWorks[workId] = val;
                UpdateNightWorkValue(workId);
            }

            private void SetNoonWorkId(int index)
            {
                Maid.Param.SetNoonWorkId(gui.rowToNoonWorkID[index]);
            }

            public void SetSkillValue(int skillID, int column, object val)
            {
                Debugger.Assert(
                () =>
                {
                    Status.SkillData skillData;
                    switch (column)
                    {
                        case TABLE_COLUMN_HAS:
                            bool value = (bool) val;
                            if (value)
                                Maid.Param.SetNewGetSkill(skillID);
                            else
                                Maid.Param.RemoveSkill(skillID);
                            break;
                        case TABLE_COLUMN_LEVEL:
                            if (Maid.Param.status_.skill_data.TryGetValue(skillID, out skillData))
                                skillData.exp_system.SetLevel((int) val);
                            break;
                        case TABLE_COLUMN_TOTAL_XP:
                            if (Maid.Param.status_.skill_data.TryGetValue(skillID, out skillData))
                                skillData.exp_system.SetTotalExp((int) val);
                            break;
                        case SKILL_COLUMN_PLAY_COUNT:
                            if (Maid.Param.status_.skill_data.TryGetValue(skillID, out skillData))
                                skillData.play_count = (uint) val;
                            break;
                    }
                    gui.updateSkillTable = true;
                    UpdateSkillData(skillID);
                },
                $"Failed to set skill value for ID {skillID} to ({column}, {val}).");
            }

            public void SetValue(MaidChangeType type, object val)
            {
                Debugger.Assert(
                () =>
                {
                    Debugger.WriteLine($"Setting {EnumHelper.GetName(type)} to {val}");
                    Action<int> setValInt;
                    Action<long> setValLong;
                    Action<string> setValString;
                    Action<bool> setValBool;
                    if (setFunctionsInt.TryGetValue(type, out setValInt))
                    {
                        int v;
                        string str = val as string;
#pragma warning disable 642
                        if (str != null && int.TryParse(str, out v))
                            ;
#pragma warning restore 642
                        else if (val is int)
                            v = (int) val;
                        else
                            return;

                        setValInt(v);
                    }
                    else if (setFunctionsLong.TryGetValue(type, out setValLong))
                    {
                        long v;
                        string str = val as string;
                        if (str == null || !long.TryParse(str, out v))
                            return;

                        setValLong(v);
                    }
                    else if (val is string && setFunctionsString.TryGetValue(type, out setValString))
                        setValString((string) val);
                    else if (val is bool && setFunctionsBool.TryGetValue(type, out setValBool))
                        setValBool((bool) val);
                    else
                        Debugger.WriteLine(LogLevel.Error, $"No setter function found for {EnumHelper.GetName(type)}!");

                    //gui.valueUpdate[type] = true;
                    //UpdateField(type);
                },
                $"Failed to set value for type {EnumHelper.GetName(type)} to {val}");
            }

            public void SetValue(MaidClassType type, int col, object val)
            {
                Debugger.Assert(
                () =>
                {
                    switch (col)
                    {
                        case TABLE_COLUMN_HAS:
                            if (!(val is bool))
                                return;
                            Maid.Param.status_.maid_class_data[(int) type].is_have = (bool) val;
                            break;
                        case TABLE_COLUMN_LEVEL:
                            if (!(val is int))
                                return;
                            Maid.Param.status_.maid_class_data[(int) type].exp_system.SetLevel((int) val);
                            break;
                        case TABLE_COLUMN_TOTAL_XP:
                            if (!(val is int))
                                return;
                            Maid.Param.status_.maid_class_data[(int) type].exp_system.SetTotalExp((int) val);
                            break;
                    }
                    UpdateField(MaidChangeType.MaidClassType, (int) type);
                },
                $"Failed to set maid value of type {EnumHelper.GetName(type)} to {val}");
            }

            public void SetValue(YotogiClassType type, int col, object val)
            {
                Debugger.Assert(
                () =>
                {
                    switch (col)
                    {
                        case TABLE_COLUMN_HAS:
                            if (!(val is bool))
                                return;
                            Maid.Param.status_.yotogi_class_data[(int) type].is_have = (bool) val;
                            break;
                        case TABLE_COLUMN_LEVEL:
                            if (!(val is int))
                                return;
                            Maid.Param.status_.yotogi_class_data[(int) type].exp_system.SetLevel((int) val);
                            break;
                        case TABLE_COLUMN_TOTAL_XP:
                            if (!(val is int))
                                return;
                            Maid.Param.status_.yotogi_class_data[(int) type].exp_system.SetTotalExp((int) val);
                            break;
                    }
                    UpdateField(MaidChangeType.YotogiClassType, (int) type);
                },
                $"Failed to set yotogi class type {EnumHelper.GetName(type)} to {val}");
            }

            public void SetWorkValue(int workID, int column, object val)
            {
                Debugger.Assert(
                () =>
                {
                    Status.WorkData workData;
                    switch (column)
                    {
                        case TABLE_COLUMN_HAS:
                            bool value = (bool) val;
                            forceUpdateNoonWorks[workID] = value;
                            break;
                        case TABLE_COLUMN_LEVEL:
                            if (Maid.Param.status_.work_data.TryGetValue(workID, out workData))
                                workData.level = (int) val;
                            break;
                        case TABLE_COLUMN_TOTAL_XP:
                            if (Maid.Param.status_.work_data.TryGetValue(workID, out workData))
                                workData.play_count = (uint) val;
                            break;
                    }
                    UpdateWorkData(workID);
                },
                $"Failed to set work value for {workID} to {val}");
            }

            private void SetCondition(int val)
            {
                Maid.Param.SetCondition((Condition) val);
            }

            private void SetConditionSpecial(int val)
            {
                Maid.Param.SetConditionSpecial((ConditionSpecial) val);
            }

            private void SetContractType(int val)
            {
                Maid.Param.SetContractType((ContractType) val);
            }

            private void SetEmploymentDay(int val)
            {
                Maid.Param.SetEmployment(true);
                Maid.Param.status_.employment_day = val;
            }

            private void SetFirstName(string obj)
            {
                gui.valueUpdate[MaidChangeType.FirstName] = false;
                if (gui.removeValueLimit)
                {
                    Maid.Param.status_.first_name = obj;
                    UpdateField(MaidChangeType.FirstName);
                }
                else
                    Maid.Param.SetFirstName(obj);
            }

            private void SetFirstNameCall(bool obj)
            {
                gui.valueUpdate[MaidChangeType.FirstNameCall] = false;
                Maid.Param.SetFirstNameCall(obj);
            }

            private void SetFreeComment(string obj)
            {
                gui.valueUpdate[MaidChangeType.FreeComment] = false;
                if (gui.removeValueLimit)
                {
                    Maid.Param.status_.free_comment = obj;
                    UpdateField(MaidChangeType.FreeComment);
                }
                else
                    Maid.Param.SetFreeComment(obj);
            }

            private void SetInitSeikeiken(int val)
            {
                Maid.Param.SetInitSeikeiken((Seikeiken) val);
            }

            private void SetLastName(string obj)
            {
                gui.valueUpdate[MaidChangeType.LastName] = false;
                if (gui.removeValueLimit)
                {
                    Maid.Param.status_.last_name = obj;
                    UpdateField(MaidChangeType.LastName);
                }
                else
                    Maid.Param.SetLastName(obj);
            }

            private void SetLeader(bool obj)
            {
                gui.valueUpdate[MaidChangeType.Leader] = false;
                Maid.Param.SetLeader(obj);
            }

            private void SetMaidClassType(int val)
            {
                Maid.Param.SetMaidClassType((MaidClassType) val);
            }

            private void SetPersonal(int val)
            {
                Maid.Param.SetPersonal((Personal) val);
            }

            private void SetProfile(string obj)
            {
                Maid.Param.status_.profile = gui.textBox_profile.Text;
            }

            private void SetSeikeiken(int val)
            {
                Maid.Param.SetSeikeiken((Seikeiken) val);
            }

            private void SetSexualBack(int obj)
            {
                Maid.Param.status_.sexual.back = obj;
            }

            private void SetSexualFront(int obj)
            {
                Maid.Param.status_.sexual.front = obj;
            }

            private void SetYotogiClassType(int val)
            {
                Maid.Param.SetYotogiClassType((YotogiClassType) val);
            }

            #endregion

            #region Updaters

            public void UpdateAll()
            {
                string action = "Updating maid value (unknown source). Update type: {0}";
                MaidChangeType cType = 0;
                try
                {
                    action = "Updating normal maid function {0}";
                    foreach (KeyValuePair<MaidChangeType, Action> updateFunction in UpdateFunctions)
                    {
                        cType = updateFunction.Key;
                        gui.valueUpdate[updateFunction.Key] = true;
                        updateFunction.Value();
                        gui.valueUpdate[updateFunction.Key] = false;
                    }
                    action = "Updating maid parameter {0}";
                    foreach (
                    KeyValuePair<MaidChangeType, Action<MaidChangeType>> maidParamsUpdater in maidParamsUpdaters)
                    {
                        cType = maidParamsUpdater.Key;
                        maidParamsUpdater.Value(maidParamsUpdater.Key);
                        if (maidParamsUpdater.Key <= MaidChangeType.Profile)
                        {
                            gui.MaidParameters[maidParamsUpdater.Key].Cells[PARAMS_COLUMN_LOCK].Value =
                            ValueLocks[maidParamsUpdater.Key];
                        }
                    }
                    action = "Updating maid noon work";
                    foreach (KeyValuePair<int, ScheduleCSVData.NoonWork> noonWork in ScheduleCSVData.NoonWorkData)
                    {
                        UpdateWorkData(noonWork.Value.id);
                    }
                    action = "Updating maid night work";
                    foreach (KeyValuePair<int, ScheduleCSVData.NightWork> nightWork in ScheduleCSVData.NightWorkData)
                    {
                        UpdateNightWorkValue(nightWork.Value.id);
                    }
                    action = "Updating maid yotogi skill";
                    foreach (KeyValuePair<int, Yotogi.SkillData> skill in Yotogi.skill_data_list.SelectMany(e => e))
                    {
                        UpdateSkillData(skill.Value.id);
                    }
                    action = "Updating maid features";
                    for (Feature e = Feature.Null + 1; e < Feature.Max; e++)
                        UpdateMiscStatus(MaidChangeType.Feature, (int) e);
                    action = "Updating maid propensity";
                    for (Propensity e = Propensity.Null + 1; e < Propensity.Max; e++)
                        UpdateMiscStatus(MaidChangeType.Propensity, (int) e);
                    UpdateMaidClasses();
                    UpdateYotogiClasses();
                }
                catch (Exception e)
                {
                    ErrorLog.ThrowErrorMessage(
                    e,
                    $"Failed to update maid value for maid {Maid.Param.status.first_name} {Maid.Param.status.last_name}. Reason: {string.Format(action, EnumHelper.GetName(cType))}");
                }
            }

            private void UpdateCurrentNightWorkId()
            {
                gui.comboBox_work_night.SelectedIndex = Maid.Param.status.night_work_id == 0
                                                        ? -1 : gui.nightWorkIDToRow[Maid.Param.status.night_work_id];
            }

            private void UpdateCurrentNoonWorkId()
            {
                gui.comboBox_work_noon.SelectedIndex = Maid.Param.status.noon_work_id == 0
                                                       ? -1 : gui.noonWorkIDToRow[Maid.Param.status.noon_work_id];
            }

            public void UpdateField(MaidChangeType type, int id, int val = -1)
            {
                Debugger.Assert(
                () =>
                {
                    Debugger.WriteLine($"Updating value {EnumHelper.GetName(type)}");
                    gui.valueUpdate[type] = true;
                    Action<int, int> updateValID;
                    if (updateFunctionsID.TryGetValue(type, out updateValID))
                        updateValID(id, val);
                    gui.valueUpdate[type] = false;
                },
                $"Failed to update maid field for type {EnumHelper.GetName(type)}");
            }

            public void UpdateField(MaidChangeType type)
            {
                Debugger.Assert(
                () =>
                {
                    Debugger.WriteLine($"Updating value {EnumHelper.GetName(type)}");
                    gui.valueUpdate[type] = true;
                    Action updateVal;
                    Action<MaidChangeType> updateParam;
                    if (UpdateFunctions.TryGetValue(type, out updateVal))
                        updateVal();
                    else if (maidParamsUpdaters.TryGetValue(type, out updateParam))
                        updateParam(type);
                    else
                        Debugger.WriteLine(LogLevel.Error, $"No update function found for {EnumHelper.GetName(type)}!");
                    gui.valueUpdate[type] = false;
                },
                $"Failed to update maid field for type {EnumHelper.GetName(type)}");
            }

            public void UpdateHasSkill(int skillID)
            {
                Debugger.Assert(
                () =>
                {
                    gui.dataGridView_skill_data[TABLE_COLUMN_HAS, gui.skillIDToRow[skillID]].Value =
                    Maid.Param.status.IsGetSkill(skillID);
                },
                $"Failed to update skill ID {skillID}");
            }

            public void UpdateHasWork(int workID)
            {
                Debugger.Assert(
                () =>
                {
                    gui.dataGridView_noon_work_data[TABLE_COLUMN_HAS, gui.noonWorkIDToRow[workID]].Value =
                    Maid.Param.status.IsGetWork(workID);
                },
                $"Failed to update work ID {workID}");
            }

            public void UpdateMaidClasses()
            {
                Debugger.Assert(
                () =>
                {
                    for (MaidClassType e = 0; e < EnumHelper.MaxMaidClass; e++)
                        UpdateMaidClass((int) e, 0);
                    //UpdateField(MaidChangeType.MaidClassType, (int) e);
                },
                "Failed to update maid class type");
            }

            public void UpdateMiscStatus(MaidChangeType tag, int enumVal, bool val)
            {
                Debugger.Assert(
                () =>
                {
                    switch (tag)
                    {
                        case MaidChangeType.Feature:
                            gui.updateFeature = true;
                            gui.checkedListBox_feature.SetItemChecked(enumVal - 1, val);
                            gui.updateFeature = false;
                            break;
                        case MaidChangeType.Propensity:
                            gui.updatePropensity = true;
                            gui.checkedListBox_propensity.SetItemChecked(enumVal - 1, val);
                            gui.updatePropensity = false;
                            break;
                    }
                },
                $"Failed to update misc status of type {EnumHelper.GetName(tag)}. Attempted to update option {enumVal - 1} to {val}.");
            }

            public void UpdateMiscStatus(MaidChangeType tag, int enumVal)
            {
                Debugger.Assert(
                () =>
                {
                    bool newVal;
                    switch (tag)
                    {
                        case MaidChangeType.Feature:
                            newVal = Maid.Param.status_.feature.Contains((Feature) (enumVal));
                            if (gui.checkedListBox_feature.GetItemChecked(enumVal - 1) != newVal)
                            {
                                gui.updateFeature = true;
                                gui.checkedListBox_feature.SetItemChecked(enumVal - 1, newVal);
                                gui.updateFeature = false;
                            }
                            break;
                        case MaidChangeType.Propensity:
                            newVal = Maid.Param.status_.propensity.Contains((Propensity) (enumVal));
                            if (gui.checkedListBox_propensity.GetItemChecked(enumVal - 1) != newVal)
                            {
                                gui.updatePropensity = true;
                                gui.checkedListBox_propensity.SetItemChecked(enumVal - 1, newVal);
                                gui.updatePropensity = false;
                            }
                            break;
                    }
                },
                $"Failed to update misc status of type {EnumHelper.GetName(tag)}. Attempted to update option check {enumVal - 1} ");
            }

            public void UpdateNightWorkValue(int workID)
            {
                Debugger.Assert(
                () =>
                {
                    int row = gui.nightWorkIDToRow[workID];
                    gui.updateNightWorkTable = true;
                    gui.dataGridView_night_work[TABLE_COLUMN_HAS, row].Value = forceUpdateNightWorks[workID];
                    gui.updateNightWorkTable = false;
                },
                $"Failed to update night work force value for {workID}");
            }

            public void UpdateSkillData(int skillID)
            {
                Debugger.Assert(
                () =>
                {
                    int rowIndex = gui.skillIDToRow[skillID];
                    DataGridViewRow row = gui.dataGridView_skill_data.Rows[rowIndex];

                    Status.SkillData skillData;
                    if (Maid.Param.status_.skill_data.TryGetValue(skillID, out skillData))
                    {
                        gui.updateSkillTable = true;
                        row.Cells[TABLE_COLUMN_HAS].Value = true;
                        gui.updateSkillTable = true;
                        row.Cells[TABLE_COLUMN_LEVEL].Value = skillData.exp_system.GetCurrentLevel();
                        gui.updateSkillTable = true;
                        row.Cells[TABLE_COLUMN_TOTAL_XP].Value = skillData.exp_system.GetTotalExp();
                        gui.updateSkillTable = true;
                        row.Cells[SKILL_COLUMN_PLAY_COUNT].Value = skillData.play_count;
                    }
                    else
                    {
                        gui.updateSkillTable = true;
                        row.Cells[TABLE_COLUMN_HAS].Value = false;
                        gui.updateSkillTable = true;
                        row.Cells[TABLE_COLUMN_LEVEL].Value = 0;
                        gui.updateSkillTable = true;
                        row.Cells[TABLE_COLUMN_TOTAL_XP].Value = 0;
                        gui.updateSkillTable = true;
                        row.Cells[SKILL_COLUMN_PLAY_COUNT].Value = 0;
                    }
                    gui.updateSkillTable = false;
                },
                $"Failed to update skill data for skill ID {skillID}");
            }

            private void UpdateSkillExp(int skillID, int val)
            {
                Debugger.Assert(
                () =>
                {
                    if (!Maid.Param.status.IsGetSkill(skillID))
                        return;
                    gui.updateSkillTable = true;
                    gui.dataGridView_skill_data[TABLE_COLUMN_LEVEL, gui.skillIDToRow[skillID]].Value =
                    Maid.Param.status_.skill_data[skillID].exp_system.GetCurrentLevel();

                    gui.updateSkillTable = true;
                    gui.dataGridView_skill_data[TABLE_COLUMN_TOTAL_XP, gui.skillIDToRow[skillID]].Value =
                    Maid.Param.status_.skill_data[skillID].exp_system.GetTotalExp();
                    gui.updateSkillTable = false;
                },
                $"Failed to update skill exp for skill ID {skillID}");
            }

            private void UpdateSkillPlayCount(int skillID, int _)
            {
                Debugger.Assert(
                () =>
                {
                    if (!Maid.Param.status.IsGetSkill(skillID))
                        return;
                    gui.updateSkillTable = true;
                    gui.dataGridView_skill_data[SKILL_COLUMN_PLAY_COUNT, gui.skillIDToRow[skillID]].Value =
                    Maid.Param.status_.skill_data[skillID].play_count;
                    gui.updateSkillTable = false;
                },
                $"Failed ti update skill play count for {skillID}");
            }

            public void UpdateWorkData(int workId)
            {
                Debugger.Assert(
                () =>
                {
                    int rowIndex = gui.noonWorkIDToRow[workId];
                    DataGridViewRow row = gui.dataGridView_noon_work_data.Rows[rowIndex];

                    gui.updateWorkTable = true;
                    row.Cells[TABLE_COLUMN_HAS].Value = forceUpdateNoonWorks[workId];
                    Status.WorkData workData;
                    if (Maid.Param.status_.work_data.TryGetValue(workId, out workData))
                    {
                        gui.updateWorkTable = true;
                        row.Cells[TABLE_COLUMN_LEVEL].Value = Maid.Param.status_.work_data[workId].level;
                        gui.updateWorkTable = true;
                        row.Cells[TABLE_COLUMN_TOTAL_XP].Value = Maid.Param.status_.work_data[workId].play_count;
                    }
                    else
                    {
                        gui.updateWorkTable = true;
                        row.Cells[TABLE_COLUMN_LEVEL].Value = 0;
                        gui.updateWorkTable = true;
                        row.Cells[TABLE_COLUMN_TOTAL_XP].Value = 0;
                    }
                    gui.updateWorkTable = false;
                },
                $"Failed to update work data for work ID {workId}");
            }

            private void UpdateWorkLevel(int workID, int _)
            {
                if (!Maid.Param.status.IsGetWork(workID))
                    return;
                gui.updateWorkTable = true;
                gui.dataGridView_noon_work_data[TABLE_COLUMN_LEVEL, gui.noonWorkIDToRow[workID]].Value =
                Maid.Param.status_.work_data[workID].level;
                gui.updateWorkTable = false;
            }

            private void UpdateWorkPlayCount(int workID, int _)
            {
                if (!Maid.Param.status.IsGetWork(workID))
                    return;
                gui.updateWorkTable = true;
                gui.dataGridView_noon_work_data[TABLE_COLUMN_TOTAL_XP, gui.noonWorkIDToRow[workID]].Value =
                Maid.Param.status_.work_data[workID].play_count;
                gui.updateWorkTable = false;
            }

            public void UpdateYotogiClasses()
            {
                Debugger.Assert(
                () =>
                {
                    for (YotogiClassType e = 0; e < EnumHelper.MaxYotogiClass; e++)
                        UpdateYotogiClass((int) e, 0);
                    //UpdateField(MaidChangeType.YotogiClassType, (int) e);
                },
                "Failed to update maid yotogi class");
            }

            public void UpdateMaidBonusValues()
            {
                Debugger.Assert(
                () =>
                {
                    for (MaidChangeType e = MaidChangeType.BonusCare; e <= MaidChangeType.BonusTeachRate; e++)
                        UpdateMaidParam(e);
                },
                "Failed to update maid bonus parameter value");
            }

            private void UpdateCurrentYotogiClass()
            {
                UpdateYotogiClass((int) Maid.Param.status.current_yotogi_class, -1);
            }

            private void UpdateCurrentMaidClass()
            {
                UpdateMaidClass((int) Maid.Param.status.current_maid_class, -1);
            }

            private void UpdateYotogiClass(int id, int _)
            {
                Debugger.Assert(
                () =>
                {
                    YotogiClassType yotogiClass = (YotogiClassType) id;
                    Debugger.WriteLine($"Updating yotogi class type {EnumHelper.GetName(yotogiClass)}");
                    if (yotogiClass >= EnumHelper.MaxYotogiClass)
                        return;

                    gui.updateYotogiClassField = true;
                    gui.dataGridView_yotogi_classes[TABLE_COLUMN_HAS, (int) yotogiClass].Value =
                    Maid.Param.status_.yotogi_class_data[(int) yotogiClass].is_have;

                    gui.updateYotogiClassField = true;
                    gui.dataGridView_yotogi_classes[TABLE_COLUMN_LEVEL, (int) yotogiClass].Value =
                    Maid.Param.status_.yotogi_class_data[(int) yotogiClass].exp_system.GetCurrentLevel();

                    gui.updateYotogiClassField = true;
                    gui.dataGridView_yotogi_classes[TABLE_COLUMN_TOTAL_XP, (int) yotogiClass].Value =
                    Maid.Param.status_.yotogi_class_data[(int) yotogiClass].exp_system.GetCurrentExp();
                    gui.updateYotogiClassField = false;
                },
                $"Failed to update yotogi class data for class ID {id}");
            }

            private void UpdateMaidClass(int id, int _)
            {
                Debugger.Assert(
                () =>
                {
                    MaidClassType maidClass = (MaidClassType) id;
                    Debugger.WriteLine($"Updating maid class type {EnumHelper.GetName(maidClass)}");
                    if (maidClass >= EnumHelper.MaxMaidClass)
                        return;

                    gui.updateMaidClassField = true;
                    gui.dataGridView_maid_classes[TABLE_COLUMN_HAS, (int) maidClass].Value =
                    Maid.Param.status_.maid_class_data[(int) maidClass].is_have;

                    gui.updateMaidClassField = true;
                    gui.dataGridView_maid_classes[TABLE_COLUMN_LEVEL, (int) maidClass].Value =
                    Maid.Param.status_.maid_class_data[(int) maidClass].exp_system.GetCurrentLevel();

                    gui.updateMaidClassField = true;
                    gui.dataGridView_maid_classes[TABLE_COLUMN_TOTAL_XP, (int) maidClass].Value =
                    Maid.Param.status_.maid_class_data[(int) maidClass].exp_system.GetCurrentExp();
                    gui.updateMaidClassField = false;
                },
                $"Failed to update maid class data for class ID {id}");
            }

            private void UpdateMaidParam(MaidChangeType type)
            {
                gui.valueUpdate[type] = true;
                object val;
                Status maidStatus = Maid.Param.status_;
                switch (type)
                {
                    case MaidChangeType.Care:
                        val = maidStatus.care;
                        break;
                    case MaidChangeType.Charm:
                        val = maidStatus.charm;
                        break;
                    case MaidChangeType.CurExcite:
                        val = maidStatus.cur_excite;
                        break;
                    case MaidChangeType.CurMind:
                        val = maidStatus.cur_mind;
                        break;
                    case MaidChangeType.CurReason:
                        val = maidStatus.cur_reason;
                        break;
                    case MaidChangeType.Elegance:
                        val = maidStatus.elegance;
                        break;
                    case MaidChangeType.Frustration:
                        val = maidStatus.frustration;
                        break;
                    case MaidChangeType.Hentai:
                        val = maidStatus.hentai;
                        break;
                    case MaidChangeType.Housi:
                        val = maidStatus.housi;
                        break;
                    case MaidChangeType.Hp:
                        val = maidStatus.hp;
                        break;
                    case MaidChangeType.Inyoku:
                        val = maidStatus.inyoku;
                        break;
                    case MaidChangeType.Likability:
                        val = maidStatus.likability;
                        break;
                    case MaidChangeType.Lovely:
                        val = maidStatus.lovely;
                        break;
                    case MaidChangeType.MaidPoint:
                        val = maidStatus.maid_point;
                        break;
                    case MaidChangeType.Mind:
                        val = maidStatus.mind;
                        break;
                    case MaidChangeType.MValue:
                        val = maidStatus.m_value;
                        break;
                    case MaidChangeType.OthersPlayCount:
                        val = maidStatus.others_play_count;
                        break;
                    case MaidChangeType.PlayNumber:
                        val = maidStatus.play_number;
                        break;
                    case MaidChangeType.Reason:
                        val = maidStatus.reason;
                        break;
                    case MaidChangeType.Reception:
                        val = maidStatus.reception;
                        break;
                    case MaidChangeType.StudyRate:
                        val = maidStatus.study_rate;
                        break;
                    case MaidChangeType.TeachRate:
                        val = maidStatus.teach_rate;
                        break;
                    case MaidChangeType.YotogiPlayCount:
                        val = maidStatus.yotogi_play_count;
                        break;
                    case MaidChangeType.SexualBack:
                        val = maidStatus.sexual.back;
                        break;
                    case MaidChangeType.SexualCuri:
                        val = maidStatus.sexual.curi;
                        break;
                    case MaidChangeType.SexualFront:
                        val = maidStatus.sexual.front;
                        break;
                    case MaidChangeType.SexualMouth:
                        val = maidStatus.sexual.mouth;
                        break;
                    case MaidChangeType.SexualNipple:
                        val = maidStatus.sexual.nipple;
                        break;
                    case MaidChangeType.SexualThroat:
                        val = maidStatus.sexual.throat;
                        break;
                    case MaidChangeType.Sales:
                        val = maidStatus.sales;
                        break;
                    case MaidChangeType.TotalSales:
                        val = maidStatus.total_sales;
                        break;
                    case MaidChangeType.Evaluation:
                        val = maidStatus.evaluation;
                        break;
                    case MaidChangeType.TotalEvaluation:
                        val = maidStatus.total_sales;
                        break;
                    case MaidChangeType.CurHp:
                        val = maidStatus.cur_hp;
                        break;
                    case MaidChangeType.PopularRank:
                        val = maidStatus.popular_rank;
                        break;
                    case MaidChangeType.BonusCare:
                        val = maidStatus.maid_class_bonus_status.care;
                        break;
                    case MaidChangeType.BonusCharm:
                        val = maidStatus.maid_class_bonus_status.charm;
                        break;
                    case MaidChangeType.BonusElegance:
                        val = maidStatus.maid_class_bonus_status.elegance;
                        break;
                    case MaidChangeType.BonusHentai:
                        val = maidStatus.maid_class_bonus_status.hentai;
                        break;
                    case MaidChangeType.BonusHousi:
                        val = maidStatus.maid_class_bonus_status.housi;
                        break;
                    case MaidChangeType.BonusHp:
                        val = maidStatus.maid_class_bonus_status.hp;
                        break;
                    case MaidChangeType.BonusInyoku:
                        val = maidStatus.maid_class_bonus_status.inyoku;
                        break;
                    case MaidChangeType.BonusLovely:
                        val = maidStatus.maid_class_bonus_status.lovely;
                        break;
                    case MaidChangeType.BonusMind:
                        val = maidStatus.maid_class_bonus_status.mind;
                        break;
                    case MaidChangeType.BonusMValue:
                        val = maidStatus.maid_class_bonus_status.m_value;
                        break;
                    case MaidChangeType.BonusReception:
                        val = maidStatus.maid_class_bonus_status.reception;
                        break;
                    case MaidChangeType.BonusTeachRate:
                        val = maidStatus.maid_class_bonus_status.teach_rate;
                        break;
                    default:
                        val = "ERROR";
                        break;
                }
                Debugger.Assert(
                () =>
                {
                    gui.MaidParameters[type].Cells[PARAMS_COLUMN_VALUE].Value = val;
                    gui.valueUpdate[type] = false;
                },
                $"Failed to update maid parameter {EnumHelper.GetName(type)} for maid {Maid.Param.status.first_name} {Maid.Param.status.last_name}");
            }

            private void UpdateCondition()
            {
                Debugger.WriteLine($"Setting condition to {EnumHelper.GetName(Maid.Param.status.condition)}");
                gui.comboBox_condition.SelectedIndex = (int) Maid.Param.status.condition;
            }

            private void UpdateConditionSpecial()
            {
                gui.comboBox_condition_special.SelectedIndex = (int) Maid.Param.status.condition_special;
            }

            private void UpdateContractType()
            {
                gui.comboBox_contract_type.SelectedIndex = (int) Maid.Param.status.contract_type;
            }

            private void UpdateEmploymentDay()
            {
                gui.textBox_employment_day.Text = Maid.Param.status.employment_day.ToString();
            }

            private void UpdateFirstName()
            {
                gui.valueUpdate[MaidChangeType.FirstName] = false;
                gui.textBox_first_name.Text = Maid.Param.status.first_name;
                gui.listBox1.Invalidate();
            }

            private void UpdateFirstNameCall()
            {
                gui.valueUpdate[MaidChangeType.FirstNameCall] = false;
                gui.checkBox_is_fist_name_call.Checked = Maid.Param.status.is_first_name_call;
            }

            private void UpdateFreeComment()
            {
                gui.valueUpdate[MaidChangeType.FreeComment] = false;
                gui.textBox_free_comment.Text = Maid.Param.status.free_comment;
            }

            private void UpdateInitSeikeiken()
            {
                gui.comboBox_init_seikeiken.SelectedIndex = (int) Maid.Param.status.init_seikeiken;
            }

            private void UpdateLastName()
            {
                gui.valueUpdate[MaidChangeType.LastName] = false;
                gui.textBox_last_name.Text = Maid.Param.status.last_name;
                gui.listBox1.Invalidate();
            }

            private void UpdateLeader()
            {
                gui.valueUpdate[MaidChangeType.Leader] = false;
                gui.checkBox_leader.Checked = Maid.Param.status.leader;
            }

            private void UpdateMaidClassType()
            {
                gui.comboBox_current_maid_class.SelectedIndex = (int) Maid.Param.status.current_maid_class;
            }

            private void UpdatePersonal()
            {
                gui.comboBox_personality.SelectedIndex = (int) Maid.Param.status.personal;
            }

            private void UpdateProfile()
            {
                gui.valueUpdate[MaidChangeType.Profile] = false;
                gui.textBox_profile.Text = Maid.Param.status.profile;
            }

            private void UpdateSeikeiken()
            {
                gui.comboBox_seikeiken.SelectedIndex = (int) Maid.Param.status.seikeiken;
            }

            private void UpdateYotogiClassType()
            {
                gui.comboBox_current_yotogi_class.SelectedIndex = (int) Maid.Param.status.current_yotogi_class;
            }

            #endregion
        }
    }
}