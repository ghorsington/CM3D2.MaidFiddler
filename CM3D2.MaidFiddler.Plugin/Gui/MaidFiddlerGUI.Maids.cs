using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Utils;
using UnityEngine;
using Debugger = CM3D2.MaidFiddler.Plugin.Utils.Debugger;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        public delegate int MaidCompareMethod(Maid x, Maid y);

        private const string NAME_FORMAT = "{0}{1}";
        private readonly MaidComparer comparer = new MaidComparer();
        private SortedList<Maid, MaidInfo> loadedMaids;
        private Dictionary<string, Image> maidThumbnails;
        private Dictionary<MaidChangeType, Action> valueUpdateQueue;

        public MaidInfo SelectedMaid
            =>
            listBox1.Items.Count == 0 || listBox1.SelectedIndex == -1
            ? null : (MaidInfo) listBox1.Items[listBox1.SelectedIndex];

        private MaidInfo GetMaidInfo(Maid maid)
        {
            return loadedMaids[maid];
        }

        private void InitMaids()
        {
            loadedMaids = new SortedList<Maid, MaidInfo>(comparer);
            maidThumbnails = new Dictionary<string, Image>();
            valueUpdateQueue = new Dictionary<MaidChangeType, Action>();
        }

        private bool IsMaidLoaded(Maid maid)
        {
            return loadedMaids.ContainsKey(maid);
        }

        public static int MaidCompareEmployedDay(Maid x, Maid y)
        {
            return ComputeOrder(x.Param.status.employment_day, y.Param.status.employment_day);
        }

        public static int MaidCompareCreateTime(Maid x, Maid y)
        {
            return ComputeOrder(x.Param.status.create_time_num, y.Param.status.create_time_num);
        }

        private static int ComputeOrder<T>(T x, T y) where T : IComparable<T>
        {
            return MaidFiddler.MaidOrderDirection * x.CompareTo(y);
        }

        public static int MaidCompareFirstLastName(Maid x, Maid y)
        {
            return MaidFiddler.MaidOrderDirection
                   * string.CompareOrdinal(
                   string.Format(
                   NAME_FORMAT,
                   x.Param.status.first_name.ToUpperInvariant(),
                   x.Param.status.last_name.ToUpperInvariant()),
                   string.Format(
                   NAME_FORMAT,
                   y.Param.status.first_name.ToUpperInvariant(),
                   y.Param.status.last_name.ToUpperInvariant()));
        }

        public static int MaidCompareID(Maid x, Maid y)
        {
            return MaidFiddler.MaidOrderDirection * string.CompareOrdinal(x.Param.status.guid, y.Param.status.guid);
        }

        public static int MaidCompareLastFirstName(Maid x, Maid y)
        {
            return MaidFiddler.MaidOrderDirection
                   * string.CompareOrdinal(
                   string.Format(
                   NAME_FORMAT,
                   x.Param.status.last_name.ToUpperInvariant(),
                   x.Param.status.first_name.ToUpperInvariant()),
                   string.Format(
                   NAME_FORMAT,
                   y.Param.status.last_name.ToUpperInvariant(),
                   y.Param.status.first_name.ToUpperInvariant()));
        }

        public void ReloadMaids()
        {
            InvokeAsync((UpdateInternal) ReloadMaids, GameMain.Instance.CharacterMgr.GetStockMaidList());
        }

        private void ReloadMaids(List<Maid> maids)
        {
            Debugger.Assert(
            () =>
            {
                Debugger.WriteLine(LogLevel.Info, "Reloading maids!");
                Debugger.WriteLine(LogLevel.Info, $"Maids: {maids.Count}");
                Debugger.WriteLine(LogLevel.Info, $"Loaded maids: {loadedMaids.Count}");
                loadedMaids.Clear();
                valueUpdateQueue.Clear();
                if (maidThumbnails.Count > 0)
                    maidThumbnails.ForEach(thumb => thumb.Value.Dispose());
                maidThumbnails.Clear();
                loadedMaids = new SortedList<Maid, MaidInfo>(
                maids.ToDictionary(m => m, m => new MaidInfo(m, this)),
                comparer);
                loadedMaids.ForEach(
                m =>
                {
                    Texture2D thumb = m.Value.Maid.GetThumIcon();
                    if (thumb == null)
                        return;
                    using (MemoryStream stream = new MemoryStream(thumb.EncodeToPNG()))
                    {
                        Debugger.WriteLine("Loading PNG of size: " + stream.Length);
                        maidThumbnails.Add(m.Key.Param.status.guid, Image.FromStream(stream));
                    }
                });

                UpdateList();
            },
            "Failed to reload all maids");
        }

        public void UpdateMaids()
        {
            InvokeAsync((UpdateInternal) UpdateMaids, GameMain.Instance.CharacterMgr.GetStockMaidList().ToList());
        }

        private void UpdateMaids(List<Maid> newMaids)
        {
            Debugger.Assert(
            () =>
            {
                if (newMaids.Count != loadedMaids.Count)
                    goto update;

                newMaids.Sort((m1, m2) => comparer.Compare(m1, m2));
                if (newMaids.SequenceEqual(loadedMaids.Values.Select(m => m.Maid), comparer))
                    return;

                update:
#if DEBUG
                Stopwatch sw = Stopwatch.StartNew();
#endif
                Debugger.WriteLine(LogLevel.Info, "Updating maid list!");
                Debugger.WriteLine(LogLevel.Info, $" New count:  {newMaids.Count}, Loaded count: {loadedMaids.Count}");
                loadedMaids = new SortedList<Maid, MaidInfo>(
                loadedMaids.Where(
                m =>
                {
                    bool result = newMaids.Contains(m.Key);
                    if (result)
                        newMaids.Remove(m.Key);
                    else
                    {
                        if (SelectedMaid != null && m.Value.Maid == SelectedMaid.Maid)
                            valueUpdateQueue.Clear();
                        if (maidThumbnails.ContainsKey(m.Key.Param.status.guid))
                            maidThumbnails[m.Key.Param.status.guid].Dispose();
                        maidThumbnails.Remove(m.Key.Param.status.guid);
                    }
                    return result;
                }).ToList().Union(
                newMaids.Select(
                m =>
                {
                    Debugger.WriteLine(LogLevel.Info, "Adding new maid info.");
                    MaidInfo info = new MaidInfo(m, this);
                    Debugger.WriteLine(LogLevel.Info, "Loading thumbnail");
                    Texture2D thumb = m.GetThumIcon();
                    if (thumb == null)
                        return new KeyValuePair<Maid, MaidInfo>(m, info);
                    using (MemoryStream stream = new MemoryStream(thumb.EncodeToPNG()))
                    {
                        Debugger.WriteLine("Loading PNG of size: " + stream.Length);
                        maidThumbnails.Add(m.Param.status.guid, Image.FromStream(stream));
                    }
                    return new KeyValuePair<Maid, MaidInfo>(m, info);
                })).ToDictionary(m => m.Key, m => m.Value),
                comparer);


                Debugger.WriteLine(LogLevel.Info, $"New loaded maids count: {loadedMaids.Count}");
#if DEBUG
                sw.Stop();
                Debugger.WriteLine(LogLevel.Info, $"Updated maid list in {sw.Elapsed.TotalMilliseconds} ms");

                newMaids.ForEach(
                m => Debugger.WriteLine($"Added {m.Param.status.first_name} {m.Param.status.last_name}"));
                Debugger.WriteLine();
#endif
                Debugger.WriteLine("Updating list.");
                UpdateList();
                Debugger.WriteLine("Finished updating list.");
            },
            "Failed to update maid list");
        }

        public void UpdateSelectedMaidValues()
        {
            if (InvokeRequired)
            {
                InvokeAsync((Action) UpdateSelectedMaidValues);
                return;
            }
            MaidChangeType cType = 0;
            Debugger.Assert(
            () =>
            {
                if (valueUpdateQueue.Count <= 0)
                    return;
                Debugger.WriteLine(LogLevel.Info, "Updating values...");
                foreach (KeyValuePair<MaidChangeType, Action> type in valueUpdateQueue)
                {
                    cType = type.Key;
                    type.Value();
                }
                valueUpdateQueue.Clear();
            },
            $"Failed to update scheduled maid value. Type: {cType}");
        }

        private class MaidComparer : IEqualityComparer<Maid>, IComparer<Maid>
        {
            public int Compare(Maid x, Maid y)
            {
                int result = 0;
                return MaidFiddler.MaidCompareMethods.Any(method => (result = method(x, y)) != 0)
                       ? result : MaidCompareID(x, y);
            }

            public bool Equals(Maid x, Maid y)
            {
                return x.Param.status.guid.Equals(y.Param.status.guid);
            }

            public int GetHashCode(Maid obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}