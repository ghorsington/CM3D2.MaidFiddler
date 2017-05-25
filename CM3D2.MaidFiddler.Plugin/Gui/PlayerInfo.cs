using System;
using System.Collections.Generic;
using CM3D2.MaidFiddler.Plugin.Utils;
using param;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        public class PlayerInfo
        {
            private readonly MaidFiddlerGUI gui;
            private readonly Dictionary<PlayerChangeType, bool> valueLocks;
            private Dictionary<PlayerChangeType, Action<int>> setMethodInt;
            private Dictionary<PlayerChangeType, Action<long>> setMethodLong;
            private Dictionary<PlayerChangeType, Action<string>> setMethodString;
            private Dictionary<PlayerChangeType, Action<PlayerChangeType>> updateMethods;

            public PlayerInfo(MaidFiddlerGUI gui)
            {
                this.gui = gui;
                valueLocks = new Dictionary<PlayerChangeType, bool>();
                ((PlayerChangeType[]) Enum.GetValues(typeof(PlayerChangeType))).ForEach(t => valueLocks.Add(t, false));
                InitFunctions();
            }

            public PlayerParam Player => GameMain.Instance.CharacterMgr.GetPlayerParam();

            public bool IsLocked(PlayerChangeType type)
            {
                Debugger.WriteLine(
                    LogLevel.Info,
                    $"Attempted to change value {EnumHelper.GetName(type)}. Locked: {valueLocks[type]}");
                return valueLocks[type];
            }

            public void Lock(PlayerChangeType type)
            {
                if (valueLocks.ContainsKey(type))
                    valueLocks[type] = true;
            }

            public void Unlock(PlayerChangeType type)
            {
                if (valueLocks.ContainsKey(type))
                    valueLocks[type] = false;
            }

            public void SetValue(PlayerChangeType type, object value)
            {
                Action<int> setValInt;
                Action<long> setValLong;
                Action<string> setValString;
                if (setMethodInt.TryGetValue(type, out setValInt))
                {
                    int val;
                    string s = value as string;
#pragma warning disable 642
                    if (s != null && int.TryParse(s, out val))
                        ;
#pragma warning restore 642
                    else if (value is int)
                        val = (int) value;
                    else
                        return;

                    setValInt(val);
                }
                else if (setMethodLong.TryGetValue(type, out setValLong))
                {
                    long val;
                    string s = value as string;
#pragma warning disable 642
                    if (s != null && !long.TryParse(s, out val))
                        ;
#pragma warning restore 642
                    else if (value is int || value is long)
                        val = (long) value;
                    else
                        return;

                    setValLong(val);
                }
                else if (value is string && setMethodString.TryGetValue(type, out setValString))
                    setValString((string) value);
            }

            public void UpdateAll()
            {
                try
                {
                    foreach (KeyValuePair<PlayerChangeType, Action<PlayerChangeType>> updateMethod in updateMethods)
                    {
                        gui.valueUpdatePlayer[updateMethod.Key] = true;
                        updateMethod.Value(updateMethod.Key);
                        gui.valueUpdatePlayer[updateMethod.Key] = false;
                    }
                }
                catch (Exception e)
                {
                    FiddlerUtils.ThrowErrorMessage(e, "Failed to update player values", gui.Plugin);
                }
            }

            public void UpdateField(PlayerChangeType type)
            {
                Debugger.WriteLine(LogLevel.Info, $"Updating value {type}!");
                Action<PlayerChangeType> updateVal;
                if (!updateMethods.TryGetValue(type, out updateVal))
                    return;
                gui.valueUpdatePlayer[type] = true;
                updateVal(type);
                gui.valueUpdatePlayer[type] = false;
            }

            private void InitFunctions()
            {
                setMethodInt = new Dictionary<PlayerChangeType, Action<int>>
                {
                    {PlayerChangeType.Days, SetDays},
                    {PlayerChangeType.PhaseDays, SetPhaseDays},
                    {PlayerChangeType.SalonBeautiful, SetSalonBeautiful},
                    {PlayerChangeType.SalonClean, SetSalonClean},
                    {PlayerChangeType.SalonEvaluation, SetSalonEvaluation},
                    {PlayerChangeType.BestSalonGrade, SetBestSalonGrade},
                    {PlayerChangeType.SalonGrade, SetSalonGrade},
                    {PlayerChangeType.ScenarioPhase, SetScenarioPhase},
                    {PlayerChangeType.BaseMaidPoints, SetBaseMaidPoints}
                };

                setMethodLong = new Dictionary<PlayerChangeType, Action<long>>
                {
                    {PlayerChangeType.Money, SetMoney},
                    {PlayerChangeType.SalonLoan, SetSalonLoan},
                    {PlayerChangeType.ShopUseMoney, SetShopUseMoney},
                    {PlayerChangeType.InitSalonLoan, SetInitSalonLoan}
                };

                setMethodString = new Dictionary<PlayerChangeType, Action<string>>
                {
                    {PlayerChangeType.Name, SetName}
                };

                updateMethods = new Dictionary<PlayerChangeType, Action<PlayerChangeType>>
                {
                    {PlayerChangeType.ScenarioPhase, UpdateScenatioPhase},
                    {PlayerChangeType.Name, UpdateName}
                };

                for (PlayerChangeType e = PlayerChangeType.Days; e < PlayerChangeType.InitSalonLoan; e++)
                {
                    if (e == PlayerChangeType.ScenarioPhase)
                        continue;

                    updateMethods.Add(e, UpdateTableValue);
                }
            }

            private void SetBaseMaidPoints(int obj)
            {
                Debugger.WriteLine($"Setting init maid points to {obj}");
                Status.kInitMaidPoint = obj;
            }

            private void SetBestSalonGrade(int obj)
            {
                Player.SetBestSalonGrade(obj);
            }

            private void SetDays(int obj)
            {
                Player.SetDays(obj);
            }

            private void SetInitSalonLoan(long obj)
            {
                Player.SetInitSalonLoan(obj);
            }

            private void SetMoney(long obj)
            {
                Player.SetMoney(obj);
            }

            private void SetName(string obj)
            {
                if (gui.removeValueLimit)
                {
                    Player.status_.player_name = obj;
                    UpdateField(PlayerChangeType.Name);
                }
                else
                    Player.SetName(obj);
            }

            private void SetPhaseDays(int obj)
            {
                Player.SetPhaseDays(obj);
            }

            private void SetSalonBeautiful(int obj)
            {
                Player.SetSalonBeautiful(obj);
            }

            private void SetSalonClean(int obj)
            {
                Player.SetSalonClean(obj);
            }

            private void SetSalonEvaluation(int obj)
            {
                Player.SetSalonEvaluation(obj);
            }

            private void SetSalonGrade(int obj)
            {
                Player.SetSalonGrade(obj);
            }

            private void SetSalonLoan(long obj)
            {
                Player.SetSalonLoan(obj);
            }

            private void SetScenarioPhase(int obj)
            {
                Player.SetScenarioPhase(obj);
            }

            private void SetShopUseMoney(long obj)
            {
                Player.SetShopUseMoney(obj);
            }

            private void UpdateTableValue(PlayerChangeType type)
            {
                object value;
                switch (type)
                {
                    case PlayerChangeType.Days:
                        value = Player.status.days;
                        break;
                    case PlayerChangeType.PhaseDays:
                        value = Player.status.phase_days;
                        break;
                    case PlayerChangeType.SalonBeautiful:
                        value = Player.status.salon_beautiful;
                        break;
                    case PlayerChangeType.SalonClean:
                        value = Player.status.salon_clean;
                        break;
                    case PlayerChangeType.SalonEvaluation:
                        value = Player.status.salon_evaluation;
                        break;
                    case PlayerChangeType.Money:
                        value = Player.status.money;
                        break;
                    case PlayerChangeType.SalonLoan:
                        value = Player.status.salon_loan;
                        break;
                    case PlayerChangeType.ShopUseMoney:
                        value = Player.status.shop_use_money;
                        break;
                    case PlayerChangeType.BestSalonGrade:
                        value = Player.status.best_salon_grade;
                        break;
                    case PlayerChangeType.SalonGrade:
                        value = Player.status.current_salon_grade;
                        break;
                    case PlayerChangeType.InitSalonLoan:
                        value = Player.status.init_salon_loan;
                        break;
                    default:
                        value = "ERROR";
                        break;
                }
                Debugger.Assert(
                    () => { gui.PlayerParameters[type].Cells[PARAMS_COLUMN_VALUE].Value = value; },
                    $"Failed to update player parameter {EnumHelper.GetName(type)}!");
            }

            private void UpdateName(PlayerChangeType _)
            {
                gui.textBox_player_name.Text = Player.status.player_name;
            }

            private void UpdateScenatioPhase(PlayerChangeType _)
            {
                gui.comboBox_scenario_phase.SelectedIndex = Player.status.scenario_phase;
            }
        }
    }
}