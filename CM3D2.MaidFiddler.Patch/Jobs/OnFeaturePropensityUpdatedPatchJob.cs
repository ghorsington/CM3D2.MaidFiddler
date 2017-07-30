using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Patch.PatchJob;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Patch.Jobs
{
    public class OnFeaturePropensityUpdatedPatchJob : PatchJobCollection
    {
        protected override MethodDefinition HookMethod { get; set; }

        protected override InjectFlags InjectFlags => InjectFlags.PassFields | InjectFlags.PassParametersVal;

        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("MaidParam");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks")
                                     .GetMethod(nameof(MaidStatusChangeHooks.OnFeaturePropensityUpdated));

            MemberFields = new[] {TargetType.GetField("maid_")};
        }

        protected override void LoadJobs()
        {
            Method("UpdateFeatureAndPropensity", -1);
        }
    }
}