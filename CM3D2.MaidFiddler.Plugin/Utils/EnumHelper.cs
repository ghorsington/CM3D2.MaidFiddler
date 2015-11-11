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
        public static readonly Personal[] Personalities;
        public static readonly Condition[] Conditions;
        public static readonly ContractType[] ContractTypes;
        public static readonly ContractType MaxContractType;
        public static readonly Condition MaxCondition;
        public static readonly Personal MaxPersonality;
        public static readonly YotogiClassType MaxYotogiClass;
        public static readonly MaidClassType MaxMaidClass;

        static EnumHelper()
        {
            ContractTypes = GetValues<ContractType>();
            Conditions = GetValues<Condition>();
            Personalities = GetValues<Personal>();
            MaidChangeTypes = GetValues<MaidChangeType>();
            MaidClasses = GetValues<MaidClassType>();
            YotogiClasses = GetValues<YotogiClassType>();

            MaxMaidClass = MaidClasses[MaidClasses.Length - 2];
            MaxYotogiClass = YotogiClasses[YotogiClasses.Length - 2];
            MaxPersonality = Personalities[Personalities.Length - 1];
            MaxCondition = Conditions[Conditions.Length - 1];
            MaxContractType = ContractTypes[ContractTypes.Length - 1];
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