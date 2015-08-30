using System;
using CM3D2.MaidFiddler.Hook;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public static class EnumHelper
    {
        public static readonly MaidChangeType[] MaidChangeTypes;

        static EnumHelper()
        {
            MaidChangeTypes = (MaidChangeType[]) Enum.GetValues(typeof (MaidChangeType));
        }

        public static string GetName<T>(T value)
        {
            return Enum.GetName(typeof (T), value);
        }

        public static T[] GetValues<T>()
        {
            return (T[]) Enum.GetValues(typeof (T));
        }
    }
}