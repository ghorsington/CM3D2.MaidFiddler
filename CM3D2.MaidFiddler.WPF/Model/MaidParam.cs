using System.Collections.Generic;

namespace CM3D2.MaidFiddler.WPF.Model
{
    public class MaidParam
    {
        public static int KInitMaidPoint { get; set; }

        public string GuidId { get; set; }

        public string CreateTime { get; set; }

        public ulong CreateTimeNum { get; set; }

        public int EmploymentDay { get; set; }

        public int MaidPoint { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Profile { get; set; }

        public string FreeComment { get; set; }

        public int InitSeikeiken { get; set; }

        public int Seikeiken { get; set; }

        public int Personal { get; set; }

        public int ContractType { get; set; }

        public MaidClassData<int>[] MaidClassDataA { get; set; }

        public int CurrentMaidClass { get; set; }

        public MaidClassData<int>[] YotogiClassData { get; set; }

        public int CurrentYotogiClass { get; set; }

        public HashSet<int> Feature { get; set; }

        public HashSet<int> Propensity { get; set; }

        public int Condition { get; set; }

        public int ConditionSpecial { get; set; }

        public int YotogiPlayCount { get; set; }

        public int OthersPlayCount { get; set; }

        public int Likability { get; set; }

        public int StudyRate { get; set; }

        public int CurExcite { get; set; }

        public int CurHp { get; set; }

        public int Hp { get; set; }

        public int CurMind { get; set; }

        public int Mind { get; set; }

        public int CurReason { get; set; }

        public int Reason { get; set; }

        public int Reception { get; set; }

        public int Care { get; set; }

        public int Lovely { get; set; }

        public int Inyoku { get; set; }

        public int Elegance { get; set; }

        public int MValue { get; set; }

        public int Charm { get; set; }

        public int Hentai { get; set; }

        public int Housi { get; set; }

        public int TeachRate { get; set; }

        public int Sexual { get; set; }

        public int PlayNumber { get; set; }

        public int Frustration { get; set; }

        public int PopularRank { get; set; }

        public long Evaluation { get; set; }

        public long TotalEvaluation { get; set; }

        public long Sales { get; set; }

        public long TotalSales { get; set; }

        public bool IsFirstNameCall { get; set; }

        public bool IsRentalMaid { get; set; }

        public int NoonWorkId { get; set; }

        public int NightWorkId { get; set; }

        public Dictionary<int, SkillData> SkillDataDic { get; set; }

        public Dictionary<int, WorkData> WorkDataDic { get; set; }

        public Dictionary<string, int> GenericFlag { get; set; }

        public Dictionary<string, string> PartsDic { get; set; }

        public bool Employment { get; set; }

        public bool Leader { get; set; }

        public int SexualMouth { get; set; }

        public int SexualThroat { get; set; }

        public int SexualNipple { get; set; }

        public int SexualFront { get; set; }

        public int SexualBack { get; set; }

        public int SexualCuri { get; set; }

        public int BonusHp { get; set; }

        public int BonusMind { get; set; }

        public int BonusReception { get; set; }

        public int BonusCare { get; set; }

        public int BonusLovely { get; set; }

        public int BonusInyoku { get; set; }

        public int BonusElegance { get; set; }

        public int BonusMValue { get; set; }

        public int BonusCharm { get; set; }

        public int BonusHentai { get; set; }

        public int BonusHousi { get; set; }

        public int BonusTeachRate { get; set; }

        public bool IsMarriage { get; set; }

        public class SkillData
        {
            public int id;

            public uint PlayCount { get; set; }

            public int Level { get; set; }

            public int CurExp { get; set; }

            public int NextExp { get; set; }
        }

        public class WorkData
        {
            public int id;

            public uint PlayCount { get; set; }

            public int Level { get; set; }
        }

        public class MaidClassData<T>
        {
            public T type;

            public bool IsHave { get; set; }

            public int Level { get; set; }

            public int CurExp { get; set; }

            public int NextExp { get; set; }
        }
    }
}