using System;
using System.Collections.Generic;
using Schedule;

namespace CM3D2.MaidFiddler.Hook
{
    public enum MaidChangeType
    {
        // Get value ids
        MaidClassExp, // Also ADD
        YotogiClassExp, // Also ADD
        SkillPlayCount, // Add (id)
        WorkPlayCount, // Add (id)
        SkillExp, // Add (skill_id, add_val)
        WorkLevel, // Set (id, level)
        Propensity, // Set (target, val)
        Feature, // Set (target, val)
        // Set (return booleans)
        NewGetSkill,
        NewGetWork,

        // Add/Set
        Care,
        Charm,
        CurExcite,
        CurMind,
        CurReason,
        Elegance,
        Frustration,
        Hentai,
        Housi,
        Hp,
        Inyoku,
        Likability,
        Lovely,
        MaidPoint,
        Mind,
        MValue,
        OthersPlayCount,
        PlayNumber,
        Reason,
        Reception,
        StudyRate,
        TeachRate,
        YotogiPlayCount,
        // Sexual (add with limit/set)
        SexualBack,
        SexualCuri,
        SexualFront,
        SexualMouth,
        SexualNipple,
        SexualThroat,
        // Add/Set (long)
        Sales,
        TotalSales,
        Evaluation,
        TotalEvaluation,
        // Set (string)
        FirstName,
        LastName,
        FreeComment,
        // Only Set
        CurHp,
        NightWorkId,
        NoonWorkId,
        PopularRank,
        // Set (bool)
        Employment,
        FirstNameCall,
        Leader,
        RentalMaid,
        SeikeikenFront,
        SeikeikenBack,
        // Custom set
        Personal,
        ContractType,
        MaidClassType,
        Name,
        YotogiClassType,
        Condition,
        ConditionSpecial,
        InitSeikeiken,
        Seikeiken,
        //Remove
        Skill,
        Work,

        //Misc
        MaidAndYotogiClass,
        Profile,
        BonusCare,
        BonusCharm,
        BonusElegance,
        BonusHentai,
        BonusHousi,
        BonusHp,
        BonusInyoku,
        BonusLovely,
        BonusMind,
        BonusMValue,
        BonusReception,
        BonusTeachRate
    }

    public class HookEventArgs : EventArgs
    {
        public Maid CallerMaid { get; internal set; }
        public MaidChangeType Tag { get; internal set; }
    }

    public class StatusEventArgs : HookEventArgs
    {
        public bool BlockAssignment { get; set; }
    }

    public class StatusChangedEventArgs : StatusEventArgs
    {
        public int ID { get; internal set; }
        public int Value { get; internal set; }
    }

    public class StatusUpdateEventArgs : HookEventArgs
    {
        public int EnumVal { get; internal set; }
        public bool Value { get; internal set; }
    }

    public class ThumbnailEventArgs : EventArgs
    {
        public Maid Maid { get; internal set; }
    }

    public class WorkEventArgs : HookEventArgs
    {
        public bool CheckCalledTarget { get; internal set; }
        public bool ForceEnabled { get; set; }
        public int ID { get; internal set; }
    }

    public class PostProcessNoonEventArgs : EventArgs
    {
        public List<DaytimeTaskCtrl.DaytimeTaskButton> List { get; internal set; }
        public ScheduleScene ScheduleScene { get; internal set; }
        public int SlotID { get; internal set; }
    }

    public class PostProcessNightEventArgs : EventArgs
    {
        public List<NightTaskCtrl.NightTaskButton> List { get; internal set; }
        public ScheduleScene ScheduleScene { get; internal set; }
        public int SlotID { get; internal set; }
    }

    public class UpdateFeaturePropensityEventArgs : EventArgs
    {
        public Maid CallerMaid { get; internal set; }
        public bool UpdateFeature { get; internal set; }
        public bool UpdatePropensity { get; internal set; }
    }

    public class CommandUpdateEventArgs : EventArgs
    {
        public YotogiCommandFactory CommandFactory { get; internal set; }
        public Dictionary<YotogiPlay.PlayerState, Yotogi.SkillData.Command.Data[]> Commands { get; internal set; }
        public YotogiPlay.PlayerState PlayerState { get; internal set; }
    }

    public static class MaidStatusChangeHooks
    {
        public delegate void ClassUpdateHandle(HookEventArgs e);

        public delegate void NewPropertyGetHandle(StatusChangedEventArgs e);

        public delegate void PostProcessNightWorkDataHandle(PostProcessNightEventArgs e);

        public delegate void PostProcessNoonWorkDataHandle(PostProcessNoonEventArgs e);

        public delegate void PropertyRemovedHandle(StatusChangedEventArgs e);

        public delegate void SkillExpAddedHandle(StatusChangedEventArgs e);

        public delegate void StatusChangeHandle(StatusEventArgs e);

        public delegate void StatusChangeIDHandle(StatusChangedEventArgs e);

        public delegate void StatusUpdateHandle(StatusUpdateEventArgs e);

        public delegate void ThumbnailUpdateHandle(ThumbnailEventArgs e);

        public delegate void UpdateCommandHandle(CommandUpdateEventArgs e);

        public delegate void UpdateFeaturePropensityHandle(UpdateFeaturePropensityEventArgs e);

        public delegate void WorkEnableCheckHandle(WorkEventArgs e);

        public static event WorkEnableCheckHandle CheckWorkEnabled;
        public static event ClassUpdateHandle ClassUpdated;
        public static event UpdateCommandHandle CommandUpdate;
        public static event UpdateFeaturePropensityHandle FeaturePropensityUpdated;
        public static event NewPropertyGetHandle NewProperty;

        public static void OnClassTypeUpdate(int tag, ref Maid currentMaid)
        {
            HookEventArgs args = new HookEventArgs {CallerMaid = currentMaid, Tag = (MaidChangeType) tag};
            ClassUpdated?.Invoke(args);
        }

        public static void OnFeaturePropensityUpdated(ref Maid maid, bool updateFeature, bool updatePropensity)
        {
            UpdateFeaturePropensityEventArgs args = new UpdateFeaturePropensityEventArgs
            {
                CallerMaid = maid,
                UpdateFeature = updateFeature,
                UpdatePropensity = updatePropensity
            };
            FeaturePropensityUpdated?.Invoke(args);
        }

        public static void OnMaidClassAndYotogiUpdate(ref Maid currentMaid)
        {
            HookEventArgs args = new HookEventArgs {CallerMaid = currentMaid, Tag = MaidChangeType.MaidAndYotogiClass};
            ClassUpdated?.Invoke(args);
        }

        public static void OnNewPropertyGet(int tag, ref Maid currentMaid, int id)
        {
            StatusChangedEventArgs args = new StatusChangedEventArgs
            {
                Tag = (MaidChangeType) tag,
                CallerMaid = currentMaid,
                BlockAssignment = false,
                ID = id,
                Value = -1
            };
            NewProperty?.Invoke(args);
        }

        public static bool OnNightWorkEnableCheck(out bool result, int workID, Maid maid, bool calledTargetCheck)
        {
            WorkEventArgs args = new WorkEventArgs
            {
                Tag = MaidChangeType.NightWorkId,
                CallerMaid = maid,
                CheckCalledTarget = calledTargetCheck,
                ForceEnabled = false,
                ID = workID
            };
            CheckWorkEnabled?.Invoke(args);
            result = args.ForceEnabled;
            return args.ForceEnabled;
        }

        public static bool OnNoonWorkEnableCheck(out bool result, int workID, Maid maid)
        {
            WorkEventArgs args = new WorkEventArgs
            {
                Tag = MaidChangeType.NoonWorkId,
                CallerMaid = maid,
                CheckCalledTarget = false,
                ForceEnabled = false,
                ID = workID
            };
            CheckWorkEnabled?.Invoke(args);
            result = args.ForceEnabled;
            return args.ForceEnabled;
        }

        public static void OnPropertyRemoved(int tag, ref Maid currentMaid, int id)
        {
            StatusChangedEventArgs args = new StatusChangedEventArgs
            {
                Tag = (MaidChangeType) tag,
                CallerMaid = currentMaid,
                ID = id,
                Value = -1,
                BlockAssignment = false
            };
            PropertyRemoved?.Invoke(args);
        }

        public static void OnSkillExpAdded(ref Maid currentMaid, int id, int addval)
        {
            StatusChangedEventArgs args = new StatusChangedEventArgs
            {
                Tag = MaidChangeType.SkillExp,
                CallerMaid = currentMaid,
                ID = id,
                Value = addval
            };
            SkillExpAdded?.Invoke(args);
        }

        public static bool OnStatusChanged(int tag, ref Maid currentMaid)
        {
            StatusEventArgs args = new StatusEventArgs
            {
                Tag = (MaidChangeType) tag,
                CallerMaid = currentMaid,
                BlockAssignment = false
            };

            StatusChanged?.Invoke(args);

            return args.BlockAssignment;
        }

        public static bool OnStatusChangedID(int tag, ref Maid currentMaid, int id)
        {
            StatusChangedEventArgs args = new StatusChangedEventArgs
            {
                Tag = (MaidChangeType) tag,
                CallerMaid = currentMaid,
                ID = id,
                Value = -1,
                BlockAssignment = false
            };
            StatusChangedID?.Invoke(args);
            return args.BlockAssignment;
        }

        public static bool OnStatusChangedID(int tag, ref Maid currentMaid, int id, int val)
        {
            StatusChangedEventArgs args = new StatusChangedEventArgs
            {
                Tag = (MaidChangeType) tag,
                CallerMaid = currentMaid,
                ID = id,
                Value = val,
                BlockAssignment = false
            };
            StatusChangedID?.Invoke(args);
            return args.BlockAssignment;
        }

        public static void OnStatusUpdate(int tag, ref Maid currentMaid, int enumVal, bool value)
        {
            StatusUpdateEventArgs args = new StatusUpdateEventArgs
            {
                Tag = (MaidChangeType) tag,
                CallerMaid = currentMaid,
                EnumVal = enumVal,
                Value = value
            };
            StatusUpdated?.Invoke(args);
        }

        public static void OnThumbnailChanged(Maid maid)
        {
            ThumbnailEventArgs args = new ThumbnailEventArgs {Maid = maid};
            ThumbnailChanged?.Invoke(args);
        }

        public static void OnUpdateCommand(ref YotogiPlay.PlayerState playerState,
                                           ref Dictionary<YotogiPlay.PlayerState, Yotogi.SkillData.Command.Data[]>
                                           commandDictionary,
                                           ref YotogiCommandFactory commandFactory)
        {
            CommandUpdateEventArgs args = new CommandUpdateEventArgs
            {
                CommandFactory = commandFactory,
                Commands = commandDictionary,
                PlayerState = playerState
            };
            CommandUpdate?.Invoke(args);
        }

        public static void PostProcessNightWorkData(ref List<NightTaskCtrl.NightTaskButton> list,
                                                    ref ScheduleScene scheduleScene,
                                                    int slotNo)
        {
            PostProcessNightEventArgs args = new PostProcessNightEventArgs
            {
                List = list,
                ScheduleScene = scheduleScene,
                SlotID = slotNo
            };
            ProcessNightWorkData?.Invoke(args);
        }

        public static void PostProcessNoonWorkData(ref List<DaytimeTaskCtrl.DaytimeTaskButton> list,
                                                   ref ScheduleScene scheduleScene,
                                                   int slotNo)
        {
            PostProcessNoonEventArgs args = new PostProcessNoonEventArgs
            {
                List = list,
                ScheduleScene = scheduleScene,
                SlotID = slotNo
            };
            ProcessNoonWorkData?.Invoke(args);
        }

        public static event PostProcessNightWorkDataHandle ProcessNightWorkData;
        public static event PostProcessNoonWorkDataHandle ProcessNoonWorkData;
        public static event PropertyRemovedHandle PropertyRemoved;
        public static event SkillExpAddedHandle SkillExpAdded;
        public static event StatusChangeHandle StatusChanged;
        public static event StatusChangeIDHandle StatusChangedID;
        public static event StatusUpdateHandle StatusUpdated;
        public static event ThumbnailUpdateHandle ThumbnailChanged;
    }
}