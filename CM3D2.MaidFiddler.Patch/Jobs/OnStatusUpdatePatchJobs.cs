using System;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Patch.PatchJob;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Patch.Jobs
{
    public class OnStatusUpdatePatchJobs : PatchJobCollection
    {
        protected override MethodDefinition HookMethod { get; set; }
        protected override InjectFlags InjectFlags => 0;
        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("MaidParam");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks")
                                     .GetMethod(nameof(MaidStatusChangeHooks.OnStatusUpdate));

            MemberFields = new[] {TargetType.GetField("maid_")};
        }

        protected override void LoadJobs()
        {
            SetEnumBool("Propensity");
            SetEnumBool("Feature");
        }

        protected void SetEnumBool(string name)
        {
            MethodDefinition target = TargetType.GetMethod("Set" + name);
            if (target == null)
            {
                Console.WriteLine($"Method {TargetType.Name}.Set{name} not found, skipping...");
                return;
            }

            PatchTargets.Add(new EnumBoolHookInjectJob(name, target, HookMethod, MemberFields));
        }
    }
}