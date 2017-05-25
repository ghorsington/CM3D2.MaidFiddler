using System;
using System.Globalization;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public static class StringUtils
    {
        public static T Parse<T>(this string val, T @default) where T : IConvertible
        {
            if (val == string.Empty)
                return @default;

            T result;
            try
            {
                result = (T) Convert.ChangeType(val, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return @default;
            }

            return result;
        }

        public static string SetIfNullOrEmpty(this string that, string val)
        {
            return that == null || that.Trim() == string.Empty ? val : that;
        }
    }
}