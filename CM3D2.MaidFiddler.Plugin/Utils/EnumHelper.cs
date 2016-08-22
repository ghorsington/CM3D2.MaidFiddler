using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CM3D2.MaidFiddler.Hook;
using param;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public static class EnumHelper
    {
        public static readonly Propensity[] Propensities;
        public static readonly Feature[] Features;
        public static readonly MaidChangeType[] MaidChangeTypes;
        public static readonly MaidClassType[] MaidClasses;
        public static readonly YotogiClassType[] YotogiClasses;
        public static readonly Personal[] Personalities;
        public static readonly Condition[] Conditions;
        public static readonly ContractType[] ContractTypes;
        public static readonly ContractType MaxContractType;
        public static readonly Condition MaxCondition;
        public static readonly Personal MaxPersonality;
        public static readonly YotogiClassType MaxYotogiClass;
        public static readonly MaidClassType MaxMaidClass;
        public static readonly Propensity MaxPropensity;
        public static readonly Feature MaxFeature;

        static EnumHelper()
        {
            ContractTypes = GetValues<ContractType>();
            Conditions = GetValues<Condition>();
            Personalities = GetValues<Personal>();
            MaidChangeTypes = GetValues<MaidChangeType>();
            MaidClasses = GetValues<MaidClassType>();
            YotogiClasses = GetValues<YotogiClassType>();
            Propensities = GetValues<Propensity>();
            Features = GetValues<Feature>();

            MaxMaidClass = MaidClasses[MaidClasses.Length - 2];
            MaxYotogiClass = YotogiClasses[YotogiClasses.Length - 2];
            MaxPersonality = Personalities[Personalities.Length - 1];
            MaxCondition = Conditions[Conditions.Length - 1];
            MaxContractType = ContractTypes[ContractTypes.Length - 1];
            MaxPropensity = Propensities[Propensities.Length - 1];
            MaxFeature = Features[Features.Length - 1];
        }

        public static string GetName<T>(T value)
        {
            return Enum.GetName(typeof (T), value);
        }

        public static T[] GetValues<T>()
        {
            return (T[]) Enum.GetValues(typeof (T));
        }

        public static bool TryParse<T>(string val, out T result, bool ignoreCase = false)
        {
            try
            {
                result = (T) Enum.Parse(typeof (T), val, ignoreCase);
            }
            catch (Exception)
            {
                result = default(T);
                return false;
            }
            return true;
        }

        public static string EnumsToString<T>(IList<T> keys, char separator)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < keys.Count; i++)
            {
                sb.Append(GetName(keys[i]));
                if (i != keys.Count - 1) sb.Append(separator);
            }

            return sb.ToString();
        }

        public static List<T> ParseEnums<T>(string value, char separator)
        {
            List<T> result = new List<T>();

            string[] values = value.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length == 0) return new List<T>();
            try
            {
                foreach (T val in
                    values.Select(keyCode => (T) Enum.Parse(typeof (T), keyCode, true))
                          .Where(kc => !result.Contains(kc)))
                {
                    result.Add(val);
                }
            }
            catch (Exception)
            {
                return new List<T>();
            }

            return result;
        }
    }
}