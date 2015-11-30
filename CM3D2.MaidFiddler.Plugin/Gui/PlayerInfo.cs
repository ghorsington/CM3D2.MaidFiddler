using System;
using System.Collections.Generic;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Utils;
using param;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        public class PlayerInfo
        {
            private readonly MaidFiddlerGUI gui;
            private Dictionary<PlayerChangeType, Action<int>> setMethodInt;
            private Dictionary<PlayerChangeType, Action<long>> setMethodLong;
            private Dictionary<PlayerChangeType, Action<string>> setMethodString;
            private Dictionary<PlayerChangeType, Action> updateMethods;

            public PlayerInfo(MaidFiddlerGUI gui)
            {
                this.gui = gui;

                InitFunctions();
            }

            public PlayerParam Player => GameMain.Instance.CharacterMgr.GetPlayerParam();

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

                setMethodString = new Dictionary<PlayerChangeType, Action<string>> {{PlayerChangeType.Name, SetName}};

                updateMethods = new Dictionary<PlayerChangeType, Action>
                {
                    {PlayerChangeType.Days, UpdateDays},
                    {PlayerChangeType.PhaseDays, UpdatePhaseDays},
                    {PlayerChangeType.SalonBeautiful, UpdateSalonBeautiful},
                    {PlayerChangeType.SalonClean, UpdateSalonClean},
                    {PlayerChangeType.SalonEvaluation, UpdateSalonEvaluation},
                    {PlayerChangeType.Money, UpdateMoney},
                    {PlayerChangeType.SalonLoan, UpdateSalonLoan},
                    {PlayerChangeType.ShopUseMoney, UpdateShopUseMoney},
                    {PlayerChangeType.BestSalonGrade, UpdateBestSalonGrade},
                    {PlayerChangeType.SalonGrade, UpdateSalonGrade},
                    {PlayerChangeType.ScenarioPhase, UpdateScenatioPhase},
                    {PlayerChangeType.InitSalonLoan, UpdateInitSalonLoad},
                    {PlayerChangeType.Name, UpdateName},
                    {PlayerChangeType.BaseMaidPoints, UpdateBaseMaidPoints}
                };
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
                    if (s == null || !long.TryParse(s, out val))
                        return;

                    setValLong(val);
                }
                else if (value is string && setMethodString.TryGetValue(type, out setValString))
                    setValString((string) value);

                gui.valueUpdatePlayer[type] = true;
                UpdateField(type);
            }

            public void UpdateAll()
            {
                try
                {
                    foreach (KeyValuePair<PlayerChangeType, Action> updateMethod in updateMethods)
                    {
                        gui.valueUpdatePlayer[updateMethod.Key] = true;
                        updateMethod.Value();
                        gui.valueUpdatePlayer[updateMethod.Key] = false;
                    }
                }
                catch (Exception e)
                {
                    FiddlerUtils.ThrowErrorMessage(e, "Failed to update player values");
                }
            }

            private void UpdateBaseMaidPoints()
            {
                Debugger.WriteLine($"Updating init maid points");
                gui.textBox_maid_points_base.Text = Status.kInitMaidPoint.ToString();
            }

            private void UpdateBestSalonGrade()
            {
                gui.textBox_best_salon_grade.Text = Player.status.best_salon_grade.ToString();
            }

            private void UpdateDays()
            {
                gui.textBox_days.Text = Player.status.days.ToString();
            }

            public void UpdateField(PlayerChangeType type)
            {
                Action updateVal;
                if (!updateMethods.TryGetValue(type, out updateVal))
                    return;
                gui.valueUpdatePlayer[type] = true;
                updateVal();
                gui.valueUpdatePlayer[type] = false;
            }

            private void UpdateInitSalonLoad()
            {
                gui.textBox_init_salon_loan.Text = Player.status.init_salon_loan.ToString();
            }

            private void UpdateMoney()
            {
                gui.textBox_money.Text = Player.status.money.ToString();
            }

            private void UpdateName()
            {
                gui.textBox_player_name.Text = Player.status.player_name;
            }

            private void UpdatePhaseDays()
            {
                gui.textBox_phase_days.Text = Player.status.phase_days.ToString();
            }

            private void UpdateSalonBeautiful()
            {
                gui.textBox_salon_beautiful.Text = Player.status.salon_beautiful.ToString();
            }

            private void UpdateSalonClean()
            {
                gui.textBox_salon_clean.Text = Player.status.salon_clean.ToString();
            }

            private void UpdateSalonEvaluation()
            {
                gui.textBox_salon_evaluation.Text = Player.status.salon_evaluation.ToString();
            }

            private void UpdateSalonGrade()
            {
                gui.textBox_current_salon_grade.Text = Player.status.current_salon_grade.ToString();
            }

            private void UpdateSalonLoan()
            {
                gui.textBox_salon_loan.Text = Player.status.salon_loan.ToString();
            }

            private void UpdateScenatioPhase()
            {
                gui.comboBox_scenario_phase.SelectedIndex = Player.status.scenario_phase;
            }

            private void UpdateShopUseMoney()
            {
                gui.textBox_shop_use_money.Text = Player.status.shop_use_money.ToString();
            }
        }
    }
}