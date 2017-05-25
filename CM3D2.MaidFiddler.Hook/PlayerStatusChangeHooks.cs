using System;

namespace CM3D2.MaidFiddler.Hook
{
    public class PlayerValueChangeEventArgs : EventArgs
    {
        public bool Block { get; set; }
        public string Tag { get; internal set; }
    }

    public static class PlayerStatusChangeHooks
    {
        public static event EventHandler<PlayerValueChangeEventArgs> PlayerValueChanged;

        public static bool OnPlayerStatChanged(string tag)
        {
            PlayerValueChangeEventArgs args = new PlayerValueChangeEventArgs
            {
                Tag = tag,
                Block = false
            };
            PlayerValueChanged?.Invoke(null, args);
            return args.Block;
        }
    }
}