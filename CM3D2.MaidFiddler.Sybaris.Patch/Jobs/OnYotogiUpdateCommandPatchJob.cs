using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Sybaris.Patcher.PatchJob;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Sybaris.Patcher.Jobs
{
    public class OnYotogiUpdateCommandPatchJob : PatchJobCollection
    {
        protected override MethodDefinition HookMethod { get; set; }
        protected override InjectFlags InjectFlags => InjectFlags.PassFields;
        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("YotogiPlayManager");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks")
                                     .GetMethod(nameof(MaidStatusChangeHooks.OnUpdateCommand));

            MemberFields = new[]
            {
                TargetType.GetField("player_state_"),
                TargetType.GetField("valid_command_dic_"),
                TargetType.GetField("command_factory_")
            };
        }

        protected override void LoadJobs()
        {
            Method("UpdateCommand", -1);
        }
    }
}