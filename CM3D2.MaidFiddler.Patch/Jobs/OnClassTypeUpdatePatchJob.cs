using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Patch.PatchJob;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Patch.Jobs
{
    public class OnClassTypeUpdatePatchJob : PatchJobCollection
    {
        private const string Prefix = "UpdatetAcquisition";

        protected override MethodDefinition HookMethod { get; set; }

        protected override InjectFlags InjectFlags => InjectFlags.PassFields | InjectFlags.PassStringTag;

        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("MaidParam");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks")
                                     .GetMethod(nameof(MaidStatusChangeHooks.OnClassTypeUpdate));

            MemberFields = new[] {TargetType.GetField("maid_")};
        }

        protected override void LoadJobs()
        {
            MethodWithPrefix(Prefix, "MaidClassType");
            MethodWithPrefix(Prefix, "YotogiClassType");
            MethodWithTag("UpdateMaidClassAndYotogiClassStatus", "MaidAndYotogiClassType");
        }
    }
}