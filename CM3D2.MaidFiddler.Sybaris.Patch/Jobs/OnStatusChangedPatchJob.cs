using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Sybaris.Patcher.PatchJob;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Sybaris.Patcher.Jobs
{
    public class OnStatusChangedPatchJob : PatchJobCollection
    {
        protected override MethodDefinition HookMethod { get; set; }

        protected override InjectFlags InjectFlags => InjectFlags.PassStringTag
                                                      | InjectFlags.PassFields
                                                      | InjectFlags.ModifyReturn;

        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }

        private InjectFlags FeaturePropensityInjectFlags => InjectFlags.PassStringTag | InjectFlags.PassFields;

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("MaidParam");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks")
                                     .GetMethod(nameof(MaidStatusChangeHooks.OnStatusChanged));

            MemberFields = new[] {TargetType.GetField("maid_")};
        }

        protected override void LoadJobs()
        {
            AddSet("Care");
            AddSet("Charm");
            AddSet("CurExcite");
            AddSet("CurMind");
            AddSet("CurReason");
            AddSet("Elegance");
            AddSet("Frustration");
            AddSet("Hentai");
            AddSet("Housi");
            AddSet("Hp");
            AddSet("Inyoku");
            AddSet("Likability");
            AddSet("Lovely");
            AddSet("MaidPoint");
            AddSet("Mind");
            AddSet("MValue");
            AddSet("OthersPlayCount");
            AddSet("PlayNumber");
            AddSet("Reason");
            AddSet("Reception");
            AddSet("StudyRate");
            AddSet("TeachRate");
            AddSet("YotogiPlayCount");
            AddSet("SexualBack");
            AddSet("SexualCuri");
            AddSet("SexualFront");
            AddSet("SexualMouth");
            AddSet("SexualNipple");
            AddSet("SexualThroat");
            AddSet("Sales");
            AddSet("TotalSales");
            AddSet("Evaluation");
            AddSet("TotalEvaluation");

            Set("Sexual", -1);
            Set("FirstName");
            Set("LastName");
            Set("FreeComment");
            Set("Personal");
            Set("ContractType");
            Set("MaidClassType");
            Set("Name");
            Set("YotogiClassType");
            Set("Condition");
            Set("ConditionSpecial");
            Set("InitSeikeiken");
            Set("CurHp");
            Set("NightWorkId");
            Set("NoonWorkId");
            Set("PopularRank");
            Set("Employment");
            Set("Marriage");
            Set("FirstNameCall");
            Set("Leader");
            Set("RentalMaid");
            Set("SeikeikenFront");
            Set("SeikeikenBack");
            Set("Seikeiken");
            Set("NewWife");

            Add("MaidClassExp", 0, typeof(int));
            Add("YotogiClassExp", 0, typeof(int));

            MethodWithTag("UpdateProfileComment", "Profile");

            SpecialSet("Feature",
                       -1,
                       FeaturePropensityInjectFlags,
                       "System.Collections.Generic.HashSet`1<param.Feature>");
            SpecialSet("Propensity",
                       -1,
                       FeaturePropensityInjectFlags,
                       "System.Collections.Generic.HashSet`1<param.Propensity>");
            SpecialClear("Feature", 0, FeaturePropensityInjectFlags);
            SpecialClear("Propensity", 0, FeaturePropensityInjectFlags);
        }

        private void SpecialClear(string name, int offset, InjectFlags customFlags)
        {
            MethodDefinition target = TargetType.GetMethod($"Clear{name}");
            if (target == null)
            {
                Logger.Log($"Method {TargetType.Name}.Clear{name} not found, skipping...");
                return;
            }

            PatchTargets.Add(new HookInjectJob(name,
                                               offset,
                                               target,
                                               HookMethod,
                                               customFlags,
                                               new int[0],
                                               MemberFields));
        }

        private void SpecialSet(string name, int offset, InjectFlags customFlags, params string[] parameters)
        {
            MethodDefinition target = TargetType.GetMethod($"Set{name}", parameters);
            if (target == null)
            {
                Logger.Log($"Method {TargetType.Name}.Set{name} not found, skipping...");
                return;
            }

            PatchTargets.Add(new HookInjectJob(name,
                                               offset,
                                               target,
                                               HookMethod,
                                               customFlags,
                                               new int[0],
                                               MemberFields));
        }
    }
}