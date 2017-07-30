using System.Linq;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Sybaris.Patcher.PatchJob;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Sybaris.Patcher.Jobs
{
    public class AddClassExpPatchJob : PatchJobCollection
    {
        protected override MethodDefinition HookMethod { get; set; }
        protected override InjectFlags InjectFlags => 0;
        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("MaidParam");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks")
                                     .GetMethod(nameof(MaidStatusChangeHooks.OnStatusChangedID));

            MemberFields = new[] {TargetType.GetField("maid_")};
        }

        protected void AddEnum(string name, int paramCount)
        {
            MethodDefinition target = TargetType.GetMethods("Add" + name)
                                                .FirstOrDefault(m => m.Parameters.Count == paramCount);
            if (target == null)
            {
                Logger.Log($"Method {TargetType.Name}.Add{name} not found, skipping...");
                return;
            }

            PatchTargets.Add(new EnumHookInjectJob(name, target, HookMethod, MemberFields));
        }

        protected override void LoadJobs()
        {
            AddEnum("MaidClassExp", 2);
            AddEnum("YotogiClassExp", 2);
        }
    }
}