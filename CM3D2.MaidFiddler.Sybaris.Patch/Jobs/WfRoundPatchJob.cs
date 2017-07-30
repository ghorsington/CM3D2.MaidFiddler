using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Sybaris.Patcher.PatchJob;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Sybaris.Patcher.Jobs
{
    public class WfRoundPatchJob : PatchJobCollection
    {
        protected override MethodDefinition HookMethod { get; set; }

        protected override InjectFlags InjectFlags => InjectFlags.ModifyReturn | InjectFlags.PassParametersVal;

        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }

        private MethodDefinition HookMethodLong { get; set; }

        private MethodDefinition HookMethodRoundInt { get; set; }

        private MethodDefinition HookMethodRoundLong { get; set; }

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("wf");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.ValueLimitHooks")
                                     .GetMethod(nameof(ValueLimitHooks.OnValueRound),
                                                typeof(int).MakeByRefType(),
                                                typeof(int));
            HookMethodLong = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.ValueLimitHooks")
                                         .GetMethod(nameof(ValueLimitHooks.OnValueRound),
                                                    typeof(long).MakeByRefType(),
                                                    typeof(long));

            HookMethodRoundInt = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.ValueLimitHooks")
                                             .GetMethod(nameof(ValueLimitHooks.OnValueRound),
                                                        typeof(int).MakeByRefType(),
                                                        typeof(int),
                                                        typeof(int),
                                                        typeof(int));
            HookMethodRoundLong = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.ValueLimitHooks")
                                              .GetMethod(nameof(ValueLimitHooks.OnValueRound),
                                                         typeof(long).MakeByRefType(),
                                                         typeof(long),
                                                         typeof(long),
                                                         typeof(long));

            MemberFields = new FieldDefinition[0];
        }

        protected override void LoadJobs()
        {
            CustomMethod("NumRound2", HookMethod);
            CustomMethod("NumRound3", HookMethod);
            CustomMethod("NumRound4", HookMethod, 0, typeof(int));

            CustomMethod("NumRound4", HookMethodLong, 0, typeof(long));
            CustomMethod("NumRound6", HookMethodLong);

            CustomMethod("RoundMinMax", HookMethodRoundInt, 0, typeof(int), typeof(int), typeof(int));

            CustomMethod("RoundMinMax", HookMethodRoundLong, 0, typeof(long), typeof(long), typeof(long));
        }
    }
}