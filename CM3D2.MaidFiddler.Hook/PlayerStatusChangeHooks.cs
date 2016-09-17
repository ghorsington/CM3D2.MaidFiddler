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
        public bool Block { get; set; }
        public PlayerChangeType Tag { get; internal set; }
    }

    public static class PlayerStatusChangeHooks
    {
        public static event EventHandler<PlayerValueChangeEventArgs> PlayerValueChanged;

        public static bool OnPlayerStatChanged(int tag)
        {
            PlayerValueChangeEventArgs args = new PlayerValueChangeEventArgs
            {
                Tag = (PlayerChangeType) tag,
                Block = false
            };
            PlayerValueChanged?.Invoke(null, args);
            return args.Block;
        }
    }
}