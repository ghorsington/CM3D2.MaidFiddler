using System;

namespace CM3D2.MaidFiddler.Hook
{
    public enum PlayerChangeType
    {
        // Add/Set (int)
        Days,
        PhaseDays,
        SalonBeautiful,
        SalonClean,
        SalonEvaluation,
        // Add/set (long)
        Money,
        SalonLoan,
        ShopUseMoney,

        // Set (int)
        BestSalonGrade,
        SalonGrade,
        ScenarioPhase,

        // Set (long)
        InitSalonLoan,

        // Set (string)
        Name,

        BaseMaidPoints
    }

    public class PlayerValueChangeEventArgs : EventArgs
    {
        public PlayerChangeType Tag { get; internal set; }
    }

    public static class PlayerStatusChangeHooks
    {
        public static event EventHandler<PlayerValueChangeEventArgs> PlayerValueChanged;

        public static void OnPlayerStatChanged(int tag)
        {
            PlayerValueChangeEventArgs args = new PlayerValueChangeEventArgs {Tag = (PlayerChangeType) tag};
            PlayerValueChanged?.Invoke(null, args);
        }
    }
}