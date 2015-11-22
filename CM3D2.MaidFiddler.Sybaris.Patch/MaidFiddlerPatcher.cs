using System;
using System.IO;
using System.Reflection;
using CM3D2.MaidFiddler.Hook;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Sybaris.Patcher
{
    public static class MaidFiddlerPatcher
    {
        public static readonly string[] TargetAssemblyNames = {"Assembly-CSharp.dll"};

        public static void Patch(AssemblyDefinition assembly)
        {
            AssemblyDefinition hookAssembly =
            AssemblyLoader.LoadAssembly(
            Path.Combine(
            Path.GetDirectoryName(Assembly.GetAssembly(typeof (MaidFiddlerPatcher)).Location),
            @"..\Plugins\Managed\CM3D2.MaidFiddler.Hook.dll"));

            TypeDefinition gameMainType = assembly.MainModule.GetType("GameMain");
            TypeDefinition maidParam = assembly.MainModule.GetType("MaidParam");
            TypeDefinition maidType = assembly.MainModule.GetType("Maid");
            TypeDefinition scheduleAPI = assembly.MainModule.GetType("Schedule.ScheduleAPI");
            TypeDefinition daytimeTaskCtrl = assembly.MainModule.GetType("DaytimeTaskCtrl");
            TypeDefinition nightTaskCtrl = assembly.MainModule.GetType("NightTaskCtrl");
            TypeDefinition playerParam = assembly.MainModule.GetType("PlayerParam");
            TypeDefinition yotogiPlayMgr = assembly.MainModule.GetType("YotogiPlayManager");
            TypeDefinition wf = assembly.MainModule.GetType("wf");
            TypeDefinition status = assembly.MainModule.GetType("param.Status");

            TypeDefinition hookType = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.FiddlerHooks");
            TypeDefinition maidHooks = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks");
            TypeDefinition playerHooks =
            hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.PlayerStatusChangeHooks");
            TypeDefinition valueLimitHooks = hookAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.ValueLimitHooks");

            gameMainType.GetMethod("Deserialize")
                        .GetInjector(hookType, "OnSaveDeserialize", InjectFlags.PassParametersVal)
                        .Inject(-1);

            // Maid hooks
            MethodDefinition statusChangeHook = maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnStatusChanged));
            MethodDefinition propertyGetHook = maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnNewPropertyGet));
            MethodDefinition statusChangeIDHook1 = maidHooks.GetMethod(
            nameof(MaidStatusChangeHooks.OnStatusChangedID),
            "System.Int32",
            "Maid&",
            "System.Int32");
            MethodDefinition statusChangeIDHook2 = maidHooks.GetMethod(
            nameof(MaidStatusChangeHooks.OnStatusChangedID),
            "System.Int32",
            "Maid&",
            "System.Int32",
            "System.Int32");
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
            typeof (int).MakeByRefType(),
            typeof (int));
            MethodDefinition onValueRoundLong1 = valueLimitHooks.GetMethod(
            nameof(ValueLimitHooks.OnValueRound),
            typeof (long).MakeByRefType(),
            typeof (long));
            MethodDefinition onValueRoundInt3 = valueLimitHooks.GetMethod(
            nameof(ValueLimitHooks.OnValueRound),
            typeof (int).MakeByRefType(),
            typeof (int),
            typeof (int),
            typeof (int));
            MethodDefinition onValueRoundLong3 = valueLimitHooks.GetMethod(
            nameof(ValueLimitHooks.OnValueRound),
            typeof (long).MakeByRefType(),
            typeof (long),
            typeof (long),
            typeof (long));


            // Player hooks
            MethodDefinition playerStatChangeHook =
            playerHooks.GetMethod(nameof(PlayerStatusChangeHooks.OnPlayerStatChanged));

            const InjectFlags features1 = InjectFlags.PassTag | InjectFlags.PassFields | InjectFlags.ModifyReturn;
            const InjectFlags features2 = features1 | InjectFlags.PassParametersVal;
            const InjectFlags features3 = InjectFlags.PassFields | InjectFlags.PassTag;


            string[] typeNames = Enum.GetNames(typeof (MaidChangeType));

            for (int i = (int) MaidChangeType.Care; i <= (int) MaidChangeType.TotalEvaluation; i++)
            {
                maidParam.GetMethod($"Add{typeNames[i]}")
                         .InjectWith(statusChangeHook, 0, i, features1, typeFields: new[] {maidParam.GetField("maid_")});

                maidParam.GetMethod($"Set{typeNames[i]}")
                         .InjectWith(statusChangeHook, 0, i, features1, typeFields: new[] {maidParam.GetField("maid_")});
            }

            for (int i = (int) MaidChangeType.FirstName; i <= (int) MaidChangeType.Seikeiken; i++)
            {
                maidParam.GetMethod($"Set{typeNames[i]}")
                         .InjectWith(statusChangeHook, 0, i, features1, typeFields: new[] {maidParam.GetField("maid_")});
            }

            for (int i = (int) MaidChangeType.MaidClassExp; i <= (int) MaidChangeType.YotogiClassExp; i++)
            {
                maidParam.GetMethod($"Add{typeNames[i]}", typeof (int))
                         .InjectWith(statusChangeHook, 0, i, features1, typeFields: new[] {maidParam.GetField("maid_")});
            }

            for (int i = (int) MaidChangeType.SkillPlayCount; i <= (int) MaidChangeType.WorkPlayCount; i++)
            {
                maidParam.GetMethod($"Add{typeNames[i]}")
                         .InjectWith(
                         statusChangeIDHook1,
                         0,
                         i,
                         features2,
                         typeFields: new[] {maidParam.GetField("maid_")});
            }

            maidParam.GetMethod("UpdateProfileComment")
                     .InjectWith(
                     statusChangeHook,
                     0,
                     (int) MaidChangeType.Profile,
                     features1,
                     typeFields: new[] {maidParam.GetField("maid_")});

            maidParam.GetMethod($"Add{typeNames[(int) MaidChangeType.SkillExp]}")
                     .InjectWith(
                     skillExpAddedHook,
                     0,
                     0,
                     InjectFlags.PassFields | InjectFlags.PassParametersVal,
                     typeFields: new[] {maidParam.GetField("maid_")});

            maidParam.GetMethod($"Set{typeNames[(int) MaidChangeType.WorkLevel]}")
                     .InjectWith(
                     statusChangeIDHook2,
                     0,
                     (int) MaidChangeType.WorkLevel,
                     features2,
                     typeFields: new[] {maidParam.GetField("maid_")});

            PatchFuncEnumBool(MaidChangeType.Propensity, maidParam.GetMethod("SetPropensity"), statusUpdateHook);

            PatchFuncEnumBool(MaidChangeType.Feature, maidParam.GetMethod("SetFeature"), statusUpdateHook);

            for (int i = (int) MaidChangeType.NewGetSkill; i <= (int) MaidChangeType.NewGetWork; i++)
            {
                maidParam.GetMethod($"Set{typeNames[i]}")
                         .InjectWith(
                         propertyGetHook,
                         0,
                         i,
                         features3 | InjectFlags.PassParametersVal,
                         typeFields: new[] {maidParam.GetField("maid_")});
            }

            for (int i = (int) MaidChangeType.Skill; i <= (int) MaidChangeType.Work; i++)
            {
                maidParam.GetMethod($"Remove{typeNames[i]}")
                         .InjectWith(
                         propertyRemovedHook,
                         0,
                         i,
                         features3 | InjectFlags.PassParametersVal,
                         typeFields: new[] {maidParam.GetField("maid_")});
            }

            maidParam.GetMethod("UpdatetAcquisitionMaidClassType")
                     .InjectWith(
                     classUpdateHook,
                     0,
                     (int) MaidChangeType.MaidClassType,
                     features3,
                     typeFields: new[] {maidParam.GetField("maid_")});

            maidParam.GetMethod("UpdatetAcquisitionYotogiClassType")
                     .InjectWith(
                     classUpdateHook,
                     0,
                     (int) MaidChangeType.YotogiClassType,
                     features3,
                     typeFields: new[] {maidParam.GetField("maid_")});

            maidParam.GetMethod("UpdateMaidClassAndYotogiClassStatus")
                     .InjectWith(
                     maidYotogiUpdateHook,
                     0,
                     0,
                     InjectFlags.PassFields,
                     typeFields: new[] {maidParam.GetField("maid_")});

            PatchFuncEnum(
            MaidChangeType.MaidClassType,
            maidParam.GetMethod("AddMaidClassExp", "param.MaidClassType", "System.Int32"),
            statusChangeIDHook1);

            PatchFuncEnum(
            MaidChangeType.YotogiClassType,
            maidParam.GetMethod("AddYotogiClassExp", "param.YotogiClassType", "System.Int32"),
            statusChangeIDHook1);

            maidType.GetMethod("ThumShot").InjectWith(thumbnailChangedHook, -1, 0, InjectFlags.PassInvokingInstance);

            scheduleAPI.GetMethod("EnableNoonWork")
                       .InjectWith(
                       noonWorkEnableCheckHook,
                       0,
                       0,
                       InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            scheduleAPI.GetMethod("EnableNightWork")
                       .InjectWith(
                       nightWorkEnableCheckHook,
                       0,
                       0,
                       InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            daytimeTaskCtrl.GetMethod("LoadData")
                           .InjectWith(
                           reloadNoonWorkDataHook,
                           5,
                           flags: InjectFlags.PassFields | InjectFlags.PassParametersVal,
                           typeFields: new[] {daytimeTaskCtrl.GetField("m_scheduleApi")});

            nightTaskCtrl.GetMethod("LoadData")
                         .InjectWith(
                         reloadNightWorkDataHook,
                         5,
                         flags: InjectFlags.PassFields | InjectFlags.PassParametersVal,
                         typeFields: new[] {nightTaskCtrl.GetField("m_scheduleApi")});

            scheduleAPI.GetMethod("VisibleNightWork")
                       .InjectWith(
                       nightWorkVisCheckHook,
                       0,
                       0,
                       InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            maidParam.GetMethod("UpdateFeatureAndPropensity")
                     .InjectWith(
                     featurePropensityUpdatedHook,
                     -1,
                     0,
                     InjectFlags.PassFields | InjectFlags.PassParametersVal,
                     typeFields: new[] {maidParam.GetField("maid_")});

            for (PlayerChangeType e = PlayerChangeType.Days; e <= PlayerChangeType.ShopUseMoney; e++)
            {
                string addMethod = $"Add{Enum.GetName(typeof (PlayerChangeType), e)}";
                string setMethod = $"Set{Enum.GetName(typeof (PlayerChangeType), e)}";
                playerParam.GetMethod(addMethod).InjectWith(playerStatChangeHook, 0, (int) e, InjectFlags.PassTag);

                playerParam.GetMethod(setMethod).InjectWith(playerStatChangeHook, 0, (int) e, InjectFlags.PassTag);
            }

            for (PlayerChangeType e = PlayerChangeType.BestSalonGrade; e <= PlayerChangeType.Name; e++)
            {
                string setMethod = $"Set{Enum.GetName(typeof (PlayerChangeType), e)}";

                playerParam.GetMethod(setMethod).InjectWith(playerStatChangeHook, 0, (int) e, InjectFlags.PassTag);
            }

            yotogiPlayMgr.GetMethod("UpdateCommand")
                         .InjectWith(
                         maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnUpdateCommand)),
                         -1,
                         0,
                         InjectFlags.PassFields,
                         typeFields:
                         new[]
                         {
                             yotogiPlayMgr.GetField("player_state_"),
                             yotogiPlayMgr.GetField("valid_command_dic_"),
                             yotogiPlayMgr.GetField("command_factory_")
                         });

            wf.GetMethod("NumRound2")
              .InjectWith(onValueRoundInt1, 0, 0, InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            wf.GetMethod("NumRound3")
              .InjectWith(onValueRoundInt1, 0, 0, InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            wf.GetMethod("NumRound4", typeof (int))
              .InjectWith(onValueRoundInt1, 0, 0, InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            wf.GetMethod("NumRound4", typeof (long))
              .InjectWith(onValueRoundLong1, 0, 0, InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            wf.GetMethod("NumRound6")
              .InjectWith(onValueRoundLong1, 0, 0, InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            wf.GetMethod("RoundMinMax", typeof (int), typeof (int), typeof (int))
              .InjectWith(onValueRoundInt3, 0, 0, InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            wf.GetMethod("RoundMinMax", typeof (long), typeof (long), typeof (long))
              .InjectWith(onValueRoundLong3, 0, 0, InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            maidParam.ChangeAccess("status_");

            playerParam.ChangeAccess("status_");

            status.ChangeAccess("kInitMaidPoint");
        }

        private static void PatchFuncEnum(MaidChangeType tag, MethodDefinition target, MethodDefinition hook)
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

        private static void PatchFuncEnumBool(MaidChangeType tag, MethodDefinition target, MethodDefinition hook)
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
    }
}