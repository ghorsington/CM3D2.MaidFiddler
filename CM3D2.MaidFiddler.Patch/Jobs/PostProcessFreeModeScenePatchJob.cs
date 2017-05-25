using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Patch.PatchJob;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Patch.Jobs
{
    public class PostProcessFreeModeScenePatchJob : PatchJobCollection
    {
        protected override MethodDefinition HookMethod { get; set; }
        protected override InjectFlags InjectFlags => InjectFlags.PassFields;
        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }
        private FieldDefinition[] MemberFields2 { get; set; }
        private TypeDefinition TargetType2 { get; set; }

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("FreeModeItemEveryday");
            TargetType2 = gameAssembly.MainModule.GetType("FreeModeItemVip");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks")
                                     .GetMethod(nameof(MaidStatusChangeHooks.PostProcessFreeModeScene));

            MemberFields = new[] {TargetType.GetField("is_enabled_")};
            MemberFields2 = new[] {TargetType2.GetField("is_enabled_")};
        }

        protected override void LoadJobs()
        {
            Method(".ctor", -1);
            CustomMethod(".ctor", -1, TargetType2, HookMethod, InjectFlags, MemberFields2);
        }
    }
}