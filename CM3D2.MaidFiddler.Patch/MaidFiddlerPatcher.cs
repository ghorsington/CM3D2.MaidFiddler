using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CM3D2.MaidFiddler.Hook;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Inject;
using param;
using ReiPatcher;
using ReiPatcher.Patch;

namespace CM3D2.MaidFiddler.Patch
{
    public class MaidFiddlerPatcher : PatchBase
    {
        private const string TAG = "CM3D2_MAID_FIDDLER";
        private AssemblyDefinition FiddlerAssembly;
        public override string Name => "MaidFiddler Patcher";
        public override string Version => "1.2.2.0";

        public override bool CanPatch(PatcherArguments args)
        {
            return args.Assembly.Name.Name == "Assembly-CSharp" && !HasAttribute(args.Assembly, TAG);
        }

        private bool HasAttribute(AssemblyDefinition assembly, string tag)
        {
            return GetPatchedAttributes(assembly).Any(ass => ass.Info == tag);
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
            TypeDefinition status = args.Assembly.MainModule.GetType("param.Status");
            TypeDefinition skillData =
            args.Assembly.MainModule.GetType("Yotogi").NestedTypes.FirstOrDefault(t => t.Name == "SkillData");
            TypeDefinition freeModeItemEveryday = args.Assembly.MainModule.GetType("FreeModeItemEveryday");
            TypeDefinition freeModeItemVip = args.Assembly.MainModule.GetType("FreeModeItemVip");

            TypeDefinition hookType = FiddlerAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.FiddlerHooks");
            TypeDefinition maidHooks = FiddlerAssembly.MainModule.GetType(
            "CM3D2.MaidFiddler.Hook.MaidStatusChangeHooks");
            TypeDefinition playerHooks =
            FiddlerAssembly.MainModule.GetType("CM3D2.MaidFiddler.Hook.PlayerStatusChangeHooks");
            TypeDefinition valueLimitHooks = FiddlerAssembly.MainModule.GetType(
            "CM3D2.MaidFiddler.Hook.ValueLimitHooks");

            gameMainType.GetMethod("Deserialize")
                        .GetInjector(hookType, "OnSaveDeserialize", InjectFlags.PassParametersVal)
                        .Inject(-1);

            // Maid hooks
            MethodDefinition statusChangeHook = maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnStatusChanged));
            MethodDefinition statusChangeCallbackHook =
            maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnStatusChangedCallback));
            MethodDefinition propertyGetHook = maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnNewPropertyGet));
            MethodDefinition statusChangeIDHook1 = maidHooks.GetMethod(
            nameof(MaidStatusChangeHooks.OnStatusChangedID),
            typeof (int),
            typeof (Maid).MakeByRefType(),
            typeof (int));
            MethodDefinition statusChangeIDHook2 = maidHooks.GetMethod(
            nameof(MaidStatusChangeHooks.OnStatusChangedID),
            typeof (int),
            typeof (Maid).MakeByRefType(),
            typeof (int),
            typeof (int));
            MethodDefinition propertyRemovedHook = maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnPropertyRemoved));
            MethodDefinition statusUpdateHook = maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnStatusUpdate));
            MethodDefinition maidYotogiUpdateHook =
            maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnMaidClassAndYotogiUpdate));
            MethodDefinition classUpdateHook = maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnClassTypeUpdate));
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
            MethodDefinition yotogiSkillVisCheckHook =
            maidHooks.GetMethod(nameof(MaidStatusChangeHooks.OnYotogiSkillVisibilityCheck));
            MethodDefinition postProcessFreeModeSceneHook =
            maidHooks.GetMethod(nameof(MaidStatusChangeHooks.PostProcessFreeModeScene));

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

            Console.WriteLine("Patching basic Add/Set methods:");

            for (int i = (int) MaidChangeType.Care; i <= (int) MaidChangeType.TotalEvaluation; i++)
            {
                WritePreviousLine($"Add{typeNames[i]}");
                maidParam.GetMethod($"Add{typeNames[i]}")
                         .InjectWith(statusChangeHook, 0, i, features1, typeFields: new[] {maidParam.GetField("maid_")});

                WritePreviousLine($"Set{typeNames[i]}");
                maidParam.GetMethod($"Set{typeNames[i]}")
                         .InjectWith(statusChangeHook, 0, i, features1, typeFields: new[] {maidParam.GetField("maid_")});
            }

            MethodDefinition setSexual = maidParam.GetMethod("SetSexual");
            if (setSexual != null)
            {
                WritePreviousLine("SetSexual");
                maidParam.GetMethod("SetSexual")
                         .InjectWith(
                         statusChangeHook,
                         -1,
                         (int) MaidChangeType.Sexual,
                         features1,
                         typeFields: new[] {maidParam.GetField("maid_")});
            }


            for (int i = (int) MaidChangeType.FirstName; i <= (int) MaidChangeType.Seikeiken; i++)
            {
                WritePreviousLine($"Set{typeNames[i]}");
                MethodDefinition setDefinition = maidParam.GetMethod($"Set{typeNames[i]}");
                if (setDefinition == null)
                {
                    Console.WriteLine("Method not found (probably older version of the game). Skipping...");
                    continue;
                }
                setDefinition.InjectWith(
                statusChangeHook,
                0,
                i,
                features1,
                typeFields: new[] {maidParam.GetField("maid_")});
            }

            for (int i = (int) MaidChangeType.MaidClassExp; i <= (int) MaidChangeType.YotogiClassExp; i++)
            {
                WritePreviousLine($"Add{typeNames[i]}");
                maidParam.GetMethod($"Add{typeNames[i]}", typeof (int))
                         .InjectWith(statusChangeHook, 0, i, features1, typeFields: new[] {maidParam.GetField("maid_")});
            }

            for (int i = (int) MaidChangeType.SkillPlayCount; i <= (int) MaidChangeType.WorkPlayCount; i++)
            {
                WritePreviousLine($"Add{typeNames[i]}");
                maidParam.GetMethod($"Add{typeNames[i]}")
                         .InjectWith(
                         statusChangeIDHook1,
                         0,
                         i,
                         features2,
                         typeFields: new[] {maidParam.GetField("maid_")});
            }

            WritePreviousLine("UpdateProfileComment");
            maidParam.GetMethod("UpdateProfileComment")
                     .InjectWith(
                     statusChangeHook,
                     0,
                     (int) MaidChangeType.Profile,
                     features1,
                     typeFields: new[] {maidParam.GetField("maid_")});

            WritePreviousLine($"Add{typeNames[(int) MaidChangeType.SkillExp]}");
            maidParam.GetMethod($"Add{typeNames[(int) MaidChangeType.SkillExp]}")
                     .InjectWith(
                     statusChangeIDHook2,
                     0,
                     (int) MaidChangeType.SkillExp,
                     InjectFlags.PassFields | InjectFlags.PassTag | InjectFlags.PassParametersVal,
                     typeFields: new[] {maidParam.GetField("maid_")});

            WritePreviousLine($"Set{typeNames[(int) MaidChangeType.WorkLevel]}");
            maidParam.GetMethod($"Set{typeNames[(int) MaidChangeType.WorkLevel]}")
                     .InjectWith(
                     statusChangeIDHook2,
                     0,
                     (int) MaidChangeType.WorkLevel,
                     features2,
                     typeFields: new[] {maidParam.GetField("maid_")});

            WritePreviousLine("SetPropensity");
            PatchFuncEnumBool(MaidChangeType.Propensity, maidParam.GetMethod("SetPropensity"), statusUpdateHook);

            WritePreviousLine("SetFeature");
            PatchFuncEnumBool(MaidChangeType.Feature, maidParam.GetMethod("SetFeature"), statusUpdateHook);

            for (int i = (int) MaidChangeType.NewGetSkill; i <= (int) MaidChangeType.NewGetWork; i++)
            {
                WritePreviousLine($"Set{typeNames[i]}");
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
                WritePreviousLine($"Remove{typeNames[i]}");
                maidParam.GetMethod($"Remove{typeNames[i]}")
                         .InjectWith(
                         propertyRemovedHook,
                         0,
                         i,
                         features3 | InjectFlags.PassParametersVal,
                         typeFields: new[] {maidParam.GetField("maid_")});
            }

            WritePreviousLine("UpdatetAcquisitionMaidClassType");
            maidParam.GetMethod("UpdatetAcquisitionMaidClassType")
                     .InjectWith(
                     classUpdateHook,
                     0,
                     (int) MaidChangeType.MaidClassType,
                     features3,
                     typeFields: new[] {maidParam.GetField("maid_")});

            WritePreviousLine("UpdatetAcquisitionYotogiClassType");
            maidParam.GetMethod("UpdatetAcquisitionYotogiClassType")
                     .InjectWith(
                     classUpdateHook,
                     0,
                     (int) MaidChangeType.YotogiClassType,
                     features3,
                     typeFields: new[] {maidParam.GetField("maid_")});

            WritePreviousLine("UpdateMaidClassAndYotogiClassStatus");
            maidParam.GetMethod("UpdateMaidClassAndYotogiClassStatus")
                     .InjectWith(
                     maidYotogiUpdateHook,
                     0,
                     0,
                     InjectFlags.PassFields,
                     typeFields: new[] {maidParam.GetField("maid_")});

            WritePreviousLine("AddMaidClassExp");
            PatchFuncEnum(
            MaidChangeType.MaidClassType,
            maidParam.GetMethod("AddMaidClassExp", typeof (MaidClassType), typeof (int)),
            statusChangeIDHook1);

            WritePreviousLine("AddYotogiClassExp");
            PatchFuncEnum(
            MaidChangeType.YotogiClassType,
            maidParam.GetMethod("AddYotogiClassExp", typeof (YotogiClassType), typeof (int)),
            statusChangeIDHook1);

            WritePreviousLine("ThumShot");
            maidType.GetMethod("ThumShot").InjectWith(thumbnailChangedHook, -1, 0, InjectFlags.PassInvokingInstance);

            MethodDefinition setThumIcon = maidType.GetMethod("SetThumIcon");
            if (setThumIcon != null)
            {
                WritePreviousLine("SetThumIcon");
                setThumIcon.InjectWith(thumbnailChangedHook, -1, 0, InjectFlags.PassInvokingInstance);
            }
            WritePreviousLine("EnableNoonWork");
            scheduleAPI.GetMethod("EnableNoonWork")
                       .InjectWith(
                       noonWorkEnableCheckHook,
                       0,
                       0,
                       InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            WritePreviousLine("EnableNightWork");
            scheduleAPI.GetMethod("EnableNightWork")
                       .InjectWith(
                       nightWorkEnableCheckHook,
                       0,
                       0,
                       InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            WritePreviousLine("DaytimeTaskCtrl.LoadData");
            daytimeTaskCtrl.GetMethod("LoadData")
                           .InjectWith(
                           reloadNoonWorkDataHook,
                           5,
                           flags: InjectFlags.PassFields | InjectFlags.PassParametersVal,
                           typeFields: new[] {daytimeTaskCtrl.GetField("m_scheduleApi")});

            WritePreviousLine("NightTaskCtrl.LoadData");
            nightTaskCtrl.GetMethod("LoadData")
                         .InjectWith(
                         reloadNightWorkDataHook,
                         5,
                         flags: InjectFlags.PassFields | InjectFlags.PassParametersVal,
                         typeFields: new[] {nightTaskCtrl.GetField("m_scheduleApi")});

            WritePreviousLine("VisibleNightWork");
            scheduleAPI.GetMethod("VisibleNightWork")
                       .InjectWith(
                       nightWorkVisCheckHook,
                       0,
                       0,
                       InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);


            WritePreviousLine("UpdateFeatureAndPropensity");
            maidParam.GetMethod("UpdateFeatureAndPropensity")
                     .InjectWith(
                     featurePropensityUpdatedHook,
                     -1,
                     0,
                     InjectFlags.PassFields | InjectFlags.PassParametersVal,
                     typeFields: new[] {maidParam.GetField("maid_")});

            WritePreviousLine("SetFeature(HashSet)");
            maidParam.GetMethod("SetFeature", typeof (HashSet<>).MakeGenericType(typeof (Feature)))
                     .InjectWith(
                     statusChangeCallbackHook,
                     -1,
                     (int) MaidChangeType.FeatureHash,
                     InjectFlags.PassTag | InjectFlags.PassFields,
                     typeFields: new[] {maidParam.GetField("maid_")});

            WritePreviousLine("SetPropensity(HashSet)");
            maidParam.GetMethod("SetPropensity", typeof (HashSet<>).MakeGenericType(typeof (Propensity)))
                     .InjectWith(
                     statusChangeCallbackHook,
                     -1,
                     (int) MaidChangeType.PropensityHash,
                     InjectFlags.PassTag | InjectFlags.PassFields,
                     typeFields: new[] {maidParam.GetField("maid_")});

            for (PlayerChangeType e = PlayerChangeType.Days; e <= PlayerChangeType.ShopUseMoney; e++)
            {
                string addMethod = $"Add{Enum.GetName(typeof (PlayerChangeType), e)}";
                string setMethod = $"Set{Enum.GetName(typeof (PlayerChangeType), e)}";
                WritePreviousLine(addMethod);
                playerParam.GetMethod(addMethod).InjectWith(playerStatChangeHook, 0, (int) e, InjectFlags.PassTag);

                WritePreviousLine(setMethod);
                playerParam.GetMethod(setMethod).InjectWith(playerStatChangeHook, 0, (int) e, InjectFlags.PassTag);
            }

            for (PlayerChangeType e = PlayerChangeType.BestSalonGrade; e <= PlayerChangeType.Name; e++)
            {
                string setMethod = $"Set{Enum.GetName(typeof (PlayerChangeType), e)}";

                WritePreviousLine(setMethod);
                playerParam.GetMethod(setMethod).InjectWith(playerStatChangeHook, 0, (int) e, InjectFlags.PassTag);
            }

            WritePreviousLine("UpdateCommand");
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

            WritePreviousLine("IsExecMaid");
            skillData.GetMethod("IsExecMaid")
                     .InjectWith(yotogiSkillVisCheckHook, 0, 0, InjectFlags.PassTag | InjectFlags.ModifyReturn);

            WritePreviousLine("IsExecStage");
            skillData.GetMethod("IsExecStage")
                     .InjectWith(yotogiSkillVisCheckHook, 0, 1, InjectFlags.PassTag | InjectFlags.ModifyReturn);

            WritePreviousLine("NumRound2");
            wf.GetMethod("NumRound2")
              .InjectWith(onValueRoundInt1, 0, 0, InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            WritePreviousLine("NumRound3");
            wf.GetMethod("NumRound3")
              .InjectWith(onValueRoundInt1, 0, 0, InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            WritePreviousLine("NumRound4(int)");
            wf.GetMethod("NumRound4", typeof (int))
              .InjectWith(onValueRoundInt1, 0, 0, InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            WritePreviousLine("NumRound4(long)");
            wf.GetMethod("NumRound4", typeof (long))
              .InjectWith(onValueRoundLong1, 0, 0, InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            WritePreviousLine("NumRound6");
            wf.GetMethod("NumRound6")
              .InjectWith(onValueRoundLong1, 0, 0, InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            WritePreviousLine("RoundMinMax(int)");
            wf.GetMethod("RoundMinMax", typeof (int), typeof (int), typeof (int))
              .InjectWith(onValueRoundInt3, 0, 0, InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            WritePreviousLine("RoundMinMax(long)");
            wf.GetMethod("RoundMinMax", typeof (long), typeof (long), typeof (long))
              .InjectWith(onValueRoundLong3, 0, 0, InjectFlags.ModifyReturn | InjectFlags.PassParametersVal);

            if (freeModeItemEveryday != null)
            {
                WritePreviousLine("FreeModeItemEveryday.ctor");
                freeModeItemEveryday.GetMethod(".ctor")
                                    .InjectWith(
                                    postProcessFreeModeSceneHook,
                                    -1,
                                    0,
                                    InjectFlags.PassFields,
                                    typeFields: new[] {freeModeItemEveryday.GetField("is_enabled_")});
            }

            if (freeModeItemVip != null)
            {
                WritePreviousLine("FreeModeItemVip.ctor");
                freeModeItemVip.GetMethod(".ctor")
                               .InjectWith(
                               postProcessFreeModeSceneHook,
                               -1,
                               0,
                               InjectFlags.PassFields,
                               typeFields: new[] {freeModeItemVip.GetField("is_enabled_")});
            }

            Console.WriteLine("Done. Patching class members:\n");
            WritePreviousLine("MaidParam.status_");
            maidParam.ChangeAccess("status_");

            WritePreviousLine("MaidParam.status_");
            playerParam.ChangeAccess("status_");

            WritePreviousLine("param.Status.kInitMaidPoint");
            status.ChangeAccess("kInitMaidPoint");

            SetPatchedAttribute(args.Assembly, TAG);
            SetCustomPatchedAttribute(args.Assembly);
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
            FiddlerAssembly = AssemblyLoader.LoadAssembly(Path.Combine(AssembliesDir, "CM3D2.MaidFiddler.Hook.dll"));
        }

        private void SetCustomPatchedAttribute(AssemblyDefinition ass)
        {
            CustomAttribute attr =
            new CustomAttribute(
            ass.MainModule.Import(typeof (MaidFiddlerPatchedAttribute).GetConstructor(new[] {typeof (uint)})));
            attr.ConstructorArguments.Add(
            new CustomAttributeArgument(ass.MainModule.Import(typeof (uint)), uint.Parse(Version.Replace(".", ""))));

            CustomAttribute attr2 =
            new CustomAttribute(
            ass.MainModule.Import(typeof (MaidFiddlerPatcherAttribute).GetConstructor(new[] {typeof (uint)})));
            attr2.ConstructorArguments.Add(
            new CustomAttributeArgument(ass.MainModule.Import(typeof (uint)), (uint) PatcherType.ReiPatcher));

            ass.MainModule.GetType("Maid").CustomAttributes.Add(attr);
            ass.MainModule.GetType("Maid").CustomAttributes.Add(attr2);
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