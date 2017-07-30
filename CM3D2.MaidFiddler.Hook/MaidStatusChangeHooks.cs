using System;
using System.Collections.Generic;
using Schedule;

namespace CM3D2.MaidFiddler.Hook
{
    public class HookEventArgs : EventArgs
    {
        public Maid CallerMaid { get; internal set; }
        public string Tag { get; internal set; }
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
        public bool ForceEnabled { get; set; }
        public int ID { get; internal set; }
    }

    public class PostProcessEventArgs : EventArgs
    {
        public ScheduleScene ScheduleScene { get; internal set; }
        public int SlotID { get; internal set; }
        public string WorkType { get; internal set; }
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

    public class NightWorkVisibleEventArgs : EventArgs
    {
        public bool Force { get; set; }
        public bool Visible { get; set; }
        public int WorkID { get; internal set; }
    }

    public class YotogiSkillVisibleEventArgs : EventArgs
    {
        public bool ForceVisible { get; set; }
        public string Type { get; internal set; }
    }

    public class PostProcessSceneEventArgs : EventArgs
    {
        public bool ForceEnabled { get; set; }
    }

    public static class MaidStatusChangeHooks
    {
        public static event EventHandler<WorkEventArgs> CheckWorkEnabled;
        public static event EventHandler<HookEventArgs> ClassUpdated;
        public static event EventHandler<CommandUpdateEventArgs> CommandUpdate;

        public static event EventHandler<UpdateFeaturePropensityEventArgs> FeaturePropensityUpdated;

        public static event EventHandler<StatusChangedEventArgs> NewProperty;

        public static event EventHandler<NightWorkVisibleEventArgs> NightWorkVisibilityCheck;

        public static event EventHandler<PostProcessSceneEventArgs> ProcessFreeModeScene;

        public static event EventHandler<PostProcessEventArgs> ProcessWorkData;

        public static event EventHandler<StatusChangedEventArgs> PropertyRemoved;

        public static event EventHandler<StatusEventArgs> StatusChanged;

        public static event EventHandler<StatusChangedEventArgs> StatusChangedID;

        public static event EventHandler<StatusUpdateEventArgs> StatusUpdated;
        public static event EventHandler<ThumbnailEventArgs> ThumbnailChanged;

        public static event EventHandler<YotogiSkillVisibleEventArgs> YotogiSkillVisibilityCheck;

        public static bool CheckNightWorkVisibility(out bool result, int workID)
        {
            NightWorkVisibleEventArgs args = new NightWorkVisibleEventArgs
            {
                Visible = false,
                Force = false,
                WorkID = workID
            };
            NightWorkVisibilityCheck?.Invoke(null, args);
            result = args.Visible;
            return args.Force;
        }

        public static void OnClassTypeUpdate(string tag, ref Maid currentMaid)
        {
            HookEventArgs args = new HookEventArgs
            {
                CallerMaid = currentMaid,
                Tag = tag
            };
            ClassUpdated?.Invoke(null, args);
        }

        public static void OnFeaturePropensityUpdated(ref Maid maid, bool updateFeature, bool updatePropensity)
        {
            UpdateFeaturePropensityEventArgs args = new UpdateFeaturePropensityEventArgs
            {
                CallerMaid = maid,
                UpdateFeature = updateFeature,
                UpdatePropensity = updatePropensity
            };
            FeaturePropensityUpdated?.Invoke(null, args);
        }


        public static void OnNewPropertyGet(string tag, ref Maid currentMaid, int id)
        {
            StatusChangedEventArgs args = new StatusChangedEventArgs
            {
                Tag = tag,
                CallerMaid = currentMaid,
                BlockAssignment = false,
                ID = id,
                Value = -1
            };
            NewProperty?.Invoke(null, args);
        }

        public static void OnPropertyRemoved(string tag, ref Maid currentMaid, int id)
        {
            StatusChangedEventArgs args = new StatusChangedEventArgs
            {
                Tag = tag,
                CallerMaid = currentMaid,
                ID = id,
                Value = -1,
                BlockAssignment = false
            };
            PropertyRemoved?.Invoke(null, args);
        }

        public static bool OnStatusChanged(string tag, ref Maid currentMaid)
        {
            StatusEventArgs args = new StatusEventArgs
            {
                Tag = tag,
                CallerMaid = currentMaid,
                BlockAssignment = false
            };

            StatusChanged?.Invoke(null, args);

            return args.BlockAssignment;
        }


        public static bool OnStatusChangedID(string tag, ref Maid currentMaid, int id)
        {
            StatusChangedEventArgs args = new StatusChangedEventArgs
            {
                Tag = tag,
                CallerMaid = currentMaid,
                ID = id,
                Value = -1,
                BlockAssignment = false
            };
            StatusChangedID?.Invoke(null, args);
            return args.BlockAssignment;
        }


        public static void OnStatusUpdate(string tag, ref Maid currentMaid, int enumVal, bool value)
        {
            StatusUpdateEventArgs args = new StatusUpdateEventArgs
            {
                Tag = tag,
                CallerMaid = currentMaid,
                EnumVal = enumVal,
                Value = value
            };
            StatusUpdated?.Invoke(null, args);
        }

        public static void OnThumbnailChanged(Maid maid)
        {
            ThumbnailEventArgs args = new ThumbnailEventArgs
            {
                Maid = maid
            };
            ThumbnailChanged?.Invoke(null, args);
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
            CommandUpdate?.Invoke(null, args);
        }

        public static bool OnWorkEnableCheck(string tag, out bool result, int workID, Maid maid)
        {
            WorkEventArgs args = new WorkEventArgs
            {
                Tag = tag,
                CallerMaid = maid,
                ForceEnabled = false,
                ID = workID
            };
            CheckWorkEnabled?.Invoke(null, args);
            result = args.ForceEnabled;
            return args.ForceEnabled;
        }

        public static bool OnYotogiSkillVisibilityCheck(string tag, out bool ret)
        {
            YotogiSkillVisibleEventArgs args = new YotogiSkillVisibleEventArgs
            {
                Type = tag,
                ForceVisible = false
            };
            YotogiSkillVisibilityCheck?.Invoke(null, args);
            ret = true;
            return args.ForceVisible;
        }

        public static void PostProcessFreeModeScene(ref bool enabled)
        {
            PostProcessSceneEventArgs args = new PostProcessSceneEventArgs
            {
                ForceEnabled = enabled
            };
            ProcessFreeModeScene?.Invoke(null, args);
            enabled = args.ForceEnabled;
        }

        public static void ReloadWorkData(string tag, ref ScheduleScene scheduleScene, int slotNo)
        {
            PostProcessEventArgs args = new PostProcessEventArgs
            {
                ScheduleScene = scheduleScene,
                SlotID = slotNo,
                WorkType = tag
            };
            ProcessWorkData?.Invoke(null, args);
        }
    }
}