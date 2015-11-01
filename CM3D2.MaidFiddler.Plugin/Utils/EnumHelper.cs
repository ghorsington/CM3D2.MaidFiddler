using System;
using CM3D2.MaidFiddler.Hook;
using param;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public static class EnumHelper
    {
        public static readonly MaidChangeType[] MaidChangeTypes;
        public static readonly MaidClassType[] MaidClasses;
        public static readonly YotogiClassType[] YotogiClasses;

        public static YotogiClassType MaxYotogiClass;
        public static MaidClassType MaxMaidClassType;

        static EnumHelper()
        {
            MaidChangeTypes = (MaidChangeType[]) Enum.GetValues(typeof (MaidChangeType));
            MaidClasses = (MaidClassType[]) Enum.GetValues(typeof (MaidClassType));
            YotogiClasses = (YotogiClassType[]) Enum.GetValues(typeof (YotogiClassType));

            MaxMaidClassType = MaidClasses[MaidClasses.Length - 2];
            MaxYotogiClass = YotogiClasses[YotogiClasses.Length - 2];
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