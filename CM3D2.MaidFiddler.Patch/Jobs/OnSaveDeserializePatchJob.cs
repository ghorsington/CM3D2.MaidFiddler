using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Patch.PatchJob;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Patch.Jobs
{
    public class OnSaveDeserializePatchJob : PatchJobCollection
    {
        protected override MethodDefinition HookMethod { get; set; }

        protected override InjectFlags InjectFlags => InjectFlags.PassParametersVal;

        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("GameMain");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.FiddlerHooks")
                                     .GetMethod(nameof(FiddlerHooks.OnSaveDeserialize));

            MemberFields = new FieldDefinition[0];
        }

        protected override void LoadJobs()
        {
            Method("Deserialize", -1);
        }
    }
}