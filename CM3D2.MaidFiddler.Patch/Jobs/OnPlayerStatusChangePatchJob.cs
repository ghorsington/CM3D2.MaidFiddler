using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Patch.PatchJob;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Patch.Jobs
{
    public class OnPlayerStatusChangePatchJob : PatchJobCollection
    {
        protected override MethodDefinition HookMethod { get; set; }
        protected override InjectFlags InjectFlags => InjectFlags.PassStringTag | InjectFlags.ModifyReturn;
        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("PlayerParam");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.PlayerStatusChangeHooks")
                                     .GetMethod(nameof(PlayerStatusChangeHooks.OnPlayerStatChanged));

            MemberFields = new FieldDefinition[0];
        }

        protected override void LoadJobs()
        {
            AddSet("Days");
            AddSet("PhaseDays");
            AddSet("SalonBeautiful");
            AddSet("SalonClean");
            AddSet("SalonEvaluation");
            AddSet("Money");
            AddSet("SalonLoan");
            AddSet("ShopUseMoney");

            Set("BestSalonGrade");
            Set("SalonGrade");
            Set("ScenarioPhase");
            Set("InitSalonLoan");
            Set("Name");
        }
    }
}