using System;
using CM3D2.MaidFiddler.Hook;
using Mono.Cecil;
using Mono.Cecil.Cil;
using param;
using ReiPatcher;
using ReiPatcher.Patch;
using ReiPatcherPlus;

namespace CM3D2.MaidFiddler.Patch
{
    public class MaidFiddlerPatcher : PatchBase
    {
        private const string TAG = "CM3D2_MAID_FIDDLER";
        private AssemblyDefinition FiddlerAssembly;
        public override string Name => "MaidFiddler Patcher";
        public override string Version => GetType().Assembly.GetName().Version.ToString();

        public override bool CanPatch(PatcherArguments args)
        {
            return args.Assembly.Name.Name == "Assembly-CSharp" && !this.HasAttribute(args.Assembly, TAG);
        }

        public override void Patch(PatcherArguments args)
        {
            TypeDefinition gameMainType = args.Assembly.MainModule.GetType("GameMain");
            TypeDefinition maidParam = args.Assembly.MainModule.GetType("MaidParam");
            TypeDefinition maidType = args.Assembly.MainModule.GetType("Maid");
            TypeDefinition scheduleAPI = args.Assembly.MainModule.GetType("Schedule.ScheduleAPI");
            TypeDefinition daytimeTaskCtrl = args.Assembly.MainModule.GetType("DaytimeTaskCtrl");
            TypeDefinition nightTaskCtrl = args.Assembly.MainModule.GetType("NightTaskCtrl");
            TypeDefinition playerParam = args.Assembly.MainModule.GetType("PlayerParam");
            TypeDefinition yotogiPlayMgr = args.Assembly.MainModule.GetType("YotogiPlayManager");
            TypeDefinition wf = args.Assembly.MainModule.GetType("wf");

            TypeDefinition hookType = FiddlerAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.FiddlerHooks");
            TypeDefinition maidHooks = FiddlerAssembly.MainModule.GetType(
            "CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks");
            TypeDefinition playerHooks =
            FiddlerAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.PlayerStatusChangeHooks");
            TypeDefinition valueLimitHooks = FiddlerAssembly.MainModule.GetType(
            "CM3D2.MaidFiddler.Hook.ValueLimitHooks");

            MethodHook hook = this.GetHookMethod(
            gameMainType.GetMethod("Deserialize"),
            hookType,
            "OnSaveDeserialize",
            MethodFeatures.PassMethodParametersByValue);
            this.AttachMethod(hook, -1);

            // Maid hooks
            MethodDefinition statusChangeHook = maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnStatusChanged));
            MethodDefinition propertyGetHook = maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnNewPropertyGet));
            MethodDefinition statusChangeIDHook1 = maidHooks.GetMethod(
            nameof(MaidStatusChangeHooks.OnStatusChangedID),
            Parameter.FromType<int>(),
            Parameter.FromTypeRef(typeof (Maid)),
            Parameter.FromType<int>());
            MethodDefinition statusChangeIDHook2 = maidHooks.GetMethod(
            nameof(MaidStatusChangeHooks.OnStatusChangedID),
            Parameter.FromType<int>(),
            Parameter.FromTypeRef(typeof (Maid)),
            Parameter.FromType<int>(),
            Parameter.FromType<int>());
            MethodDefinition propertyRemovedHook = maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnPropertyRemoved));
            MethodDefinition statusUpdateHook = maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnStatusUpdate));
            MethodDefinition maidYotogiUpdateHook =
            maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnMaidClassAndYotogiUpdate));
            MethodDefinition classUpdateHook = maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnClassTypeUpdate));
            MethodDefinition skillExpAddedHook = maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnSkillExpAdded));
            MethodDefinition thumbnailChangedHook = maidHooks.GetMethod(
            nameof(MaidStatusChangeHooks.OnThumbnailChanged));
            MethodDefinition noonWorkEnableCheckHook =
            maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnNoonWorkEnableCheck));
            MethodDefinition nightWorkEnableCheckHook =
            maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnNightWorkEnableCheck));
            MethodDefinition reloadNoonWorkDataHook =
            maidHooks.GetMethod(nameof(MaidStatusChangeHooks.ReloadNoonWorkData));
            MethodDefinition reloadNightWorkDataHook =
            maidHooks.GetMethod(nameof(MaidStatusChangeHooks.ReloadNightWorkData));
            MethodDefinition featurePropensityUpdatedHook =
            maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnFeaturePropensityUpdated));
            MethodDefinition nightWorkVisCheckHook =
            maidHooks.GetMethod(nameof(MaidStatusChangeHooks.CheckNightWorkVisibility));

            MethodDefinition onValueRoundInt1 = valueLimitHooks.GetMethod(
            nameof(ValueLimitHooks.OnValueRound),
            Parameter.FromTypeRef(typeof (int)),
            Parameter.FromType<int>());
            MethodDefinition onValueRoundLong1 = valueLimitHooks.GetMethod(
            nameof(ValueLimitHooks.OnValueRound),
            Parameter.FromTypeRef(typeof (long)),
            Parameter.FromType<long>());
            MethodDefinition onValueRoundInt3 = valueLimitHooks.GetMethod(
            nameof(ValueLimitHooks.OnValueRound),
            Parameter.FromTypeRef(typeof (int)),
            Parameter.FromType<int>(),
            Parameter.FromType<int>(),
            Parameter.FromType<int>());
            MethodDefinition onValueRoundLong3 = valueLimitHooks.GetMethod(
            nameof(ValueLimitHooks.OnValueRound),
            Parameter.FromTypeRef(typeof (long)),
            Parameter.FromType<long>(),
            Parameter.FromType<long>(),
            Parameter.FromType<long>());


            // Player hooks
            MethodDefinition playerStatChangeHook =
            playerHooks.GetMethod(nameof(PlayerStatusChangeHooks.OnPlayerStatChanged));

            MethodFeatures features1 = MethodFeatures.PassCustomTag | MethodFeatures.PassMemberReferences
                                       | MethodFeatures.PassReturn;
            MethodFeatures features2 = features1 | MethodFeatures.PassMethodParametersByValue;
            MethodFeatures features3 = MethodFeatures.PassMemberReferences | MethodFeatures.PassCustomTag;


            string[] typeNames = Enum.GetNames(typeof (MaidChangeType));

            Console.WriteLine("Pathichg basic Add/Set methods:");

            for (int i = (int) MaidChangeType.Care; i <= (int) MaidChangeType.TotalEvaluation; i++)
            {
                WritePreviousLine("Add" + typeNames[i]);

                this.AttachMethod(
                maidParam.GetMethod("Add" + typeNames[i]),
                statusChangeHook,
                0,
                i,
                features1,
                InsertDirection.Before,
                maidParam.GetField("maid_"));

                WritePreviousLine("Set" + typeNames[i]);

                this.AttachMethod(
                maidParam.GetMethod("Set" + typeNames[i]),
                statusChangeHook,
                0,
                i,
                features1,
                InsertDirection.Before,
                maidParam.GetField("maid_"));
            }

            for (int i = (int) MaidChangeType.FirstName; i <= (int) MaidChangeType.Seikeiken; i++)
            {
                WritePreviousLine("Set" + typeNames[i]);

                this.AttachMethod(
                maidParam.GetMethod("Set" + typeNames[i]),
                statusChangeHook,
                0,
                i,
                features1,
                InsertDirection.Before,
                maidParam.GetField("maid_"));
            }

            for (int i = (int) MaidChangeType.MaidClassExp; i <= (int) MaidChangeType.YotogiClassExp; i++)
            {
                WritePreviousLine("Add" + typeNames[i]);

                this.AttachMethod(
                maidParam.GetMethod("Add" + typeNames[i], Parameter.FromType<int>()),
                statusChangeHook,
                0,
                i,
                features1,
                InsertDirection.Before,
                maidParam.GetField("maid_"));
            }


            for (int i = (int) MaidChangeType.SkillPlayCount; i <= (int) MaidChangeType.WorkPlayCount; i++)
            {
                WritePreviousLine("Add" + typeNames[i]);

                this.AttachMethod(
                maidParam.GetMethod("Add" + typeNames[i]),
                statusChangeIDHook1,
                0,
                i,
                features2,
                InsertDirection.Before,
                maidParam.GetField("maid_"));
            }

            WritePreviousLine("UpdateProfileComment");

            this.AttachMethod(
            maidParam.GetMethod("UpdateProfileComment"),
            statusChangeHook,
            0,
            (int) MaidChangeType.Profile,
            features1,
            InsertDirection.Before,
            maidParam.GetField("maid_"));

            WritePreviousLine("Add" + typeNames[(int) MaidChangeType.SkillExp]);

            this.AttachMethod(
            maidParam.GetMethod("Add" + typeNames[(int) MaidChangeType.SkillExp]),
            skillExpAddedHook,
            0,
            0,
            MethodFeatures.PassMemberReferences | MethodFeatures.PassMethodParametersByValue,
            InsertDirection.Before,
            maidParam.GetField("maid_"));

            WritePreviousLine("Set" + typeNames[(int) MaidChangeType.WorkLevel]);

            this.AttachMethod(
            maidParam.GetMethod("Set" + typeNames[(int) MaidChangeType.WorkLevel]),
            statusChangeIDHook2,
            0,
            (int) MaidChangeType.WorkLevel,
            features2,
            InsertDirection.Before,
            maidParam.GetField("maid_"));

            WritePreviousLine("SetPropensity");

            PatchFuncEnumBool(MaidChangeType.Propensity, maidParam.GetMethod("SetPropensity"), statusUpdateHook);

            WritePreviousLine("SetFeature");

            PatchFuncEnumBool(MaidChangeType.Feature, maidParam.GetMethod("SetFeature"), statusUpdateHook);

            RPPLogger.VERBOSE_LEVEL = RPPLogger.VerboseLevel.Patcher;


            for (int i = (int) MaidChangeType.NewGetSkill; i <= (int) MaidChangeType.NewGetWork; i++)
            {
                WritePreviousLine("Set" + typeNames[i]);

                this.AttachMethod(
                maidParam.GetMethod("Set" + typeNames[i]),
                propertyGetHook,
                0,
                i,
                features3 | MethodFeatures.PassMethodParametersByValue,
                InsertDirection.Before,
                maidParam.GetField("maid_"));
            }

            for (int i = (int) MaidChangeType.Skill; i <= (int) MaidChangeType.Work; i++)
            {
                WritePreviousLine("Remove" + typeNames[i]);

                this.AttachMethod(
                maidParam.GetMethod("Remove" + typeNames[i]),
                propertyRemovedHook,
                0,
                i,
                features3 | MethodFeatures.PassMethodParametersByValue,
                InsertDirection.Before,
                maidParam.GetField("maid_"));
            }

            WritePreviousLine("UpdatetAcquisitionMaidClassType");

            this.AttachMethod(
            maidParam.GetMethod("UpdatetAcquisitionMaidClassType"),
            classUpdateHook,
            0,
            (int) MaidChangeType.MaidClassType,
            features3,
            InsertDirection.Before,
            maidParam.GetField("maid_"));

            WritePreviousLine("UpdatetAcquisitionYotogiClassType");

            this.AttachMethod(
            maidParam.GetMethod("UpdatetAcquisitionYotogiClassType"),
            classUpdateHook,
            0,
            (int) MaidChangeType.YotogiClassType,
            features3,
            InsertDirection.Before,
            maidParam.GetField("maid_"));

            WritePreviousLine("UpdateMaidClassAndYotogiClassStatus");

            this.AttachMethod(
            maidParam.GetMethod("UpdateMaidClassAndYotogiClassStatus"),
            maidYotogiUpdateHook,
            0,
            0,
            MethodFeatures.PassMemberReferences,
            InsertDirection.Before,
            maidParam.GetField("maid_"));

            WritePreviousLine("AddMaidClassExp");

            PatchFuncEnum(
            MaidChangeType.MaidClassType,
            maidParam.GetMethod("AddMaidClassExp", Parameter.FromType<MaidClassType>(), Parameter.FromType<int>()),
            statusChangeIDHook1);

            WritePreviousLine("AddYotogiClassExp");

            PatchFuncEnum(
            MaidChangeType.YotogiClassType,
            maidParam.GetMethod("AddYotogiClassExp", Parameter.FromType<YotogiClassType>(), Parameter.FromType<int>()),
            statusChangeIDHook1);

            WritePreviousLine("ThumShot");

            this.AttachMethod(
            maidType.GetMethod("ThumShot"),
            thumbnailChangedHook,
            -1,
            0,
            MethodFeatures.PassTargetType);

            WritePreviousLine("EnableNoonWork");

            this.AttachMethod(
            scheduleAPI.GetMethod("EnableNoonWork"),
            noonWorkEnableCheckHook,
            0,
            0,
            MethodFeatures.PassReturn | MethodFeatures.PassMethodParametersByValue);

            WritePreviousLine("EnableNightWork");

            this.AttachMethod(
            scheduleAPI.GetMethod("EnableNightWork"),
            nightWorkEnableCheckHook,
            0,
            0,
            MethodFeatures.PassReturn | MethodFeatures.PassMethodParametersByValue);

            WritePreviousLine("DaytimeTaskCtrl.LoadData");

            MethodHook mh = MethodHook.FromMethodDefinition(
            daytimeTaskCtrl.GetMethod("LoadData"),
            reloadNoonWorkDataHook,
            MethodFeatures.PassMemberReferences | MethodFeatures.PassMethodParametersByValue,
            null,
            daytimeTaskCtrl.GetField("m_scheduleApi"));

            this.AttachMethod(mh, 5);

            WritePreviousLine("NightTaskCtrl.LoadData");

            MethodHook mh2 = MethodHook.FromMethodDefinition(
            nightTaskCtrl.GetMethod("LoadData"),
            reloadNightWorkDataHook,
            MethodFeatures.PassMemberReferences | MethodFeatures.PassMethodParametersByValue,
            null,
            nightTaskCtrl.GetField("m_scheduleApi"));

            this.AttachMethod(mh2, 5);

            WritePreviousLine("VisibleNightWork");

            this.AttachMethod(
            scheduleAPI.GetMethod("VisibleNightWork"),
            nightWorkVisCheckHook,
            0,
            0,
            MethodFeatures.PassReturn | MethodFeatures.PassMethodParametersByValue);

            WritePreviousLine("UpdateFeatureAndPropensity");

            this.AttachMethod(
            maidParam.GetMethod("UpdateFeatureAndPropensity"),
            featurePropensityUpdatedHook,
            -1,
            0,
            MethodFeatures.PassMemberReferences | MethodFeatures.PassMethodParametersByValue,
            InsertDirection.Before,
            maidParam.GetField("maid_"));

            for (PlayerChangeType e = PlayerChangeType.Days; e <= PlayerChangeType.ShopUseMoney; e++)
            {
                string addMethod = $"Add{Enum.GetName(typeof (PlayerChangeType), e)}";
                string setMethod = $"Set{Enum.GetName(typeof (PlayerChangeType), e)}";
                WritePreviousLine(addMethod);

                this.AttachMethod(
                playerParam.GetMethod(addMethod),
                playerStatChangeHook,
                0,
                (int) e,
                MethodFeatures.PassCustomTag);

                WritePreviousLine(setMethod);

                this.AttachMethod(
                playerParam.GetMethod(setMethod),
                playerStatChangeHook,
                0,
                (int) e,
                MethodFeatures.PassCustomTag);
            }

            for (PlayerChangeType e = PlayerChangeType.BestSalonGrade; e <= PlayerChangeType.Name; e++)
            {
                string setMethod = $"Set{Enum.GetName(typeof (PlayerChangeType), e)}";

                WritePreviousLine(setMethod);

                this.AttachMethod(
                playerParam.GetMethod(setMethod),
                playerStatChangeHook,
                0,
                (int) e,
                MethodFeatures.PassCustomTag);
            }

            WritePreviousLine("UpdateCommand");

            this.AttachMethod(
            yotogiPlayMgr.GetMethod("UpdateCommand"),
            maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnUpdateCommand)),
            -1,
            0,
            MethodFeatures.PassMemberReferences,
            InsertDirection.Before,
            yotogiPlayMgr.GetField("player_state_"),
            yotogiPlayMgr.GetField("valid_command_dic_"),
            yotogiPlayMgr.GetField("command_factory_"));

            WritePreviousLine("NumRound2");

            this.AttachMethod(
            wf.GetMethod("NumRound2"),
            onValueRoundInt1,
            0,
            0,
            MethodFeatures.PassReturn | MethodFeatures.PassMethodParametersByValue);

            WritePreviousLine("NumRound3");

            this.AttachMethod(
            wf.GetMethod("NumRound3"),
            onValueRoundInt1,
            0,
            0,
            MethodFeatures.PassReturn | MethodFeatures.PassMethodParametersByValue);

            WritePreviousLine("NumRound4(int)");

            this.AttachMethod(
            wf.GetMethod("NumRound4", Parameter.FromType<int>()),
            onValueRoundInt1,
            0,
            0,
            MethodFeatures.PassReturn | MethodFeatures.PassMethodParametersByValue);

            WritePreviousLine("NumRound4(long)");

            this.AttachMethod(
            wf.GetMethod("NumRound4", Parameter.FromType<long>()),
            onValueRoundLong1,
            0,
            0,
            MethodFeatures.PassReturn | MethodFeatures.PassMethodParametersByValue);

            WritePreviousLine("NumRound6");

            this.AttachMethod(
            wf.GetMethod("NumRound6"),
            onValueRoundLong1,
            0,
            0,
            MethodFeatures.PassReturn | MethodFeatures.PassMethodParametersByValue);

            WritePreviousLine("RoundMinMax(int)");

            this.AttachMethod(
            wf.GetMethod("RoundMinMax", Parameter.FromType<int>(), Parameter.FromType<int>(), Parameter.FromType<int>()),
            onValueRoundInt3,
            0,
            0,
            MethodFeatures.PassReturn | MethodFeatures.PassMethodParametersByValue);

            WritePreviousLine("RoundMinMax(long)");

            this.AttachMethod(
            wf.GetMethod(
            "RoundMinMax",
            Parameter.FromType<long>(),
            Parameter.FromType<long>(),
            Parameter.FromType<long>()),
            onValueRoundLong3,
            0,
            0,
            MethodFeatures.PassReturn | MethodFeatures.PassMethodParametersByValue);

            Console.WriteLine("Done. Patching class members:\n");
            WritePreviousLine("MaidParam.status_");
            this.ChangeAccess(maidParam, "status_");

            WritePreviousLine("MaidParam.status_");
            this.ChangeAccess(playerParam, "status_");


            SetPatchedAttribute(args.Assembly, TAG);
            Console.WriteLine("\nPatching complete.");
        }

        private void PatchFuncEnum(MaidChangeType tag, MethodDefinition target, MethodDefinition hook)
        {
            MethodReference hookRef = target.Module.Import(hook);
            FieldReference maidFieldRef = target.Module.Import(target.DeclaringType.GetField("maid_"));

            Instruction start = target.Body.Instructions[0];
            ILProcessor il = target.Body.GetILProcessor();

            il.InsertBefore(start, il.Create(OpCodes.Ldc_I4, (int) tag));
            il.InsertBefore(start, il.Create(OpCodes.Ldarg_0));
            il.InsertBefore(start, il.Create(OpCodes.Ldflda, maidFieldRef));
            il.InsertBefore(start, il.Create(OpCodes.Ldarg_1));
            il.InsertBefore(start, il.Create(OpCodes.Call, hookRef));
        }

        private void PatchFuncEnumBool(MaidChangeType tag, MethodDefinition target, MethodDefinition hook)
        {
            MethodReference hookRef = target.Module.Import(hook);
            FieldReference maidFieldRef = target.Module.Import(target.DeclaringType.GetField("maid_"));

            Instruction start = target.Body.Instructions[0];
            ILProcessor il = target.Body.GetILProcessor();

            il.InsertBefore(start, il.Create(OpCodes.Ldc_I4, (int) tag));
            il.InsertBefore(start, il.Create(OpCodes.Ldarg_0));
            il.InsertBefore(start, il.Create(OpCodes.Ldflda, maidFieldRef));
            il.InsertBefore(start, il.Create(OpCodes.Ldarg_1));
            il.InsertBefore(start, il.Create(OpCodes.Ldarg_2));
            il.InsertBefore(start, il.Create(OpCodes.Call, hookRef));
        }

        public override void PrePatch()
        {
            Console.WriteLine("Requesting assembly");
            RPConfig.RequestAssembly("Assembly-CSharp.dll");
            Console.WriteLine("Loading assembly");
            FiddlerAssembly = this.LoadAssembly("CM3D2.MaidFiddler.Hook.dll");
        }

        private void WritePreviousLine(string msg)
        {
            int currentTop = Console.CursorTop - 1;
            Console.SetCursorPosition(0, currentTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentTop);
            Console.WriteLine(msg);
        }
    }
}