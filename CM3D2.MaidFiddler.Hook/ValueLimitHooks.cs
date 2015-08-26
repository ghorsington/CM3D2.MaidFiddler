using System;

namespace CM3D2.MaidFiddler.Hook
{
    public class ValueLimitEventArgs : EventArgs
    {
        public bool RemoveLimit { get; set; }
    }

    public static class ValueLimitHooks
    {
        public delegate void ValueLimitHandle(ValueLimitEventArgs args);

        public static bool OnValueRound(out int result, int num)
        {
            ValueLimitEventArgs args = new ValueLimitEventArgs {RemoveLimit = false};
            ToggleValueLimit?.Invoke(args);
            result = num;
            return args.RemoveLimit;
        }

        public static bool OnValueRound(out long result, long num)
        {
            ValueLimitEventArgs args = new ValueLimitEventArgs {RemoveLimit = false};
            ToggleValueLimit?.Invoke(args);
            result = num;
            return args.RemoveLimit;
        }

        public static bool OnValueRound(out int result, int num, int min, int max)
        {
            ValueLimitEventArgs args = new ValueLimitEventArgs {RemoveLimit = false};
            ToggleValueLimit?.Invoke(args);
            result = num;
            return args.RemoveLimit;
        }

        public static bool OnValueRound(out long result, long num, long min, long max)
        {
            ValueLimitEventArgs args = new ValueLimitEventArgs {RemoveLimit = false};
            ToggleValueLimit?.Invoke(args);
            result = num;
            return args.RemoveLimit;
        }

        public static event ValueLimitHandle ToggleValueLimit;
    }
}