using System;

namespace CM3D2.MaidFiddler.Plugin
{
    public static class EnumHelper
    {
        public static string GetName<T>(T value)
        {
            return Enum.GetName(typeof (T), value);
        }
    }
}