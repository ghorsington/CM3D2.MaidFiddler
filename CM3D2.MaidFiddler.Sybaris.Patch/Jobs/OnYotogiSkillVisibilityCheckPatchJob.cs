using System.Linq;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Sybaris.Patcher.PatchJob;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Sybaris.Patcher.Jobs
{
    public class OnYotogiSkillVisibilityCheckPatchJob : PatchJobCollection
    {
        private const string Prefix = "IsExec";

        protected override MethodDefinition HookMethod { get; set; }

        protected override InjectFlags InjectFlags => InjectFlags.PassStringTag | InjectFlags.ModifyReturn;

        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("Yotogi")
                                     .NestedTypes.FirstOrDefault(t => t.Name == "SkillData");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks")
                                     .GetMethod(nameof(MaidStatusChangeHooks.OnYotogiSkillVisibilityCheck));

            MemberFields = new FieldDefinition[0];
        }

        protected override void LoadJobs()
        {
            MethodWithPrefix(Prefix, "Maid");
            MethodWithPrefix(Prefix, "Stage");
        }
    }
}