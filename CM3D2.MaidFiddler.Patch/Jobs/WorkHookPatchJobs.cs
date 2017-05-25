using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Patch.PatchJob;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Patch.Jobs
{
    public class ScheduleAPIPatchJobs : PatchJobCollection
    {
        private const string Prefix = "Enable";

        protected override MethodDefinition HookMethod { get; set; }

        protected override InjectFlags InjectFlags => InjectFlags.PassStringTag |
                                                      InjectFlags.ModifyReturn |
                                                      InjectFlags.PassParametersVal;

        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("Schedule.ScheduleAPI");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks")
                                     .GetMethod(nameof(MaidStatusChangeHooks.OnWorkEnableCheck));

            MemberFields = new FieldDefinition[0];
        }

        protected override void LoadJobs()
        {
            MethodWithPrefix(Prefix, "NoonWork");
            MethodWithPrefix(Prefix, "NightWork");
        }
    }

    public class CheckNightWorkVisibilityPatchJob : PatchJobCollection
    {
        protected override MethodDefinition HookMethod { get; set; }

        protected override InjectFlags InjectFlags => InjectFlags.ModifyReturn | InjectFlags.PassParametersVal;

        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("Schedule.ScheduleAPI");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks")
                                     .GetMethod(nameof(MaidStatusChangeHooks.CheckNightWorkVisibility));

            MemberFields = new FieldDefinition[0];
        }

        protected override void LoadJobs()
        {
            Method("VisibleNightWork");
        }
    }

    public class DaytimeTaskCtrlPatchJobs : PatchJobCollection
    {
        protected override MethodDefinition HookMethod { get; set; }

        protected override InjectFlags InjectFlags => InjectFlags.PassStringTag |
                                                      InjectFlags.PassFields |
                                                      InjectFlags.PassParametersVal;

        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("DaytimeTaskCtrl");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks")
                                     .GetMethod(nameof(MaidStatusChangeHooks.ReloadWorkData));

            MemberFields = new[] {TargetType.GetField("m_scheduleApi")};
        }

        protected override void LoadJobs()
        {
            MethodWithTag("LoadData", "NoonWork", 5);
        }
    }

    public class NightTaskCtrlPatchJobs : PatchJobCollection
    {
        protected override MethodDefinition HookMethod { get; set; }

        protected override InjectFlags InjectFlags => InjectFlags.PassStringTag |
                                                      InjectFlags.PassFields |
                                                      InjectFlags.PassParametersVal;

        protected override FieldDefinition[] MemberFields { get; set; }
        protected override TypeDefinition TargetType { get; set; }

        public override void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly)
        {
            TargetType = gameAssembly.MainModule.GetType("NightTaskCtrl");

            HookMethod = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks")
                                     .GetMethod(nameof(MaidStatusChangeHooks.ReloadWorkData));

            MemberFields = new[] {TargetType.GetField("m_scheduleApi")};
        }

        protected override void LoadJobs()
        {
            MethodWithTag("LoadData", "NightWork", 5);
        }
    }
}