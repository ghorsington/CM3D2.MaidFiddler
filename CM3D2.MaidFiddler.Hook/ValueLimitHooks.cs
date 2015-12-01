using System;

namespace CM3D2.MaidFiddler.Hook
{
    public class ValueLimitEventArgs : EventArgs
    {
        public bool RemoveLimit { get; set; }
    }

    public static class ValueLimitHooks
    {
        public static event EventHandler<ValueLimitEventArgs> ToggleValueLimit;

        public static bool OnValueRound(out int result, int num)
        {
            ValueLimitEventArgs args = new ValueLimitEventArgs {RemoveLimit = false};
            ToggleValueLimit?.Invoke(null, args);
            result = num;
            return args.RemoveLimit;
        }

        public static bool OnValueRound(out long result, long num)
        {
            ValueLimitEventArgs args = new ValueLimitEventArgs {RemoveLimit = false};
            ToggleValueLimit?.Invoke(null, args);
            result = num;
            return args.RemoveLimit;
        }

        public static bool OnValueRound(out int result, int num, int min, int max)
        {
            ValueLimitEventArgs args = new ValueLimitEventArgs {RemoveLimit = false};
            ToggleValueLimit?.Invoke(null, args);
            result = num;
            return args.RemoveLimit;
        }

        public static bool OnValueRound(out long result, long num, long min, long max)
        {
            ValueLimitEventArgs args = new ValueLimitEventArgs {RemoveLimit = false};
            ToggleValueLimit?.Invoke(null, args);
            result = num;
            return args.RemoveLimit;
        }
    }
}