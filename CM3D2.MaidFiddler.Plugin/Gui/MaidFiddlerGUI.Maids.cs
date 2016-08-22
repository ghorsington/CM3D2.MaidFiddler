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

        private MaidComparer comparer;
        private int currentQueue;
        private SortedList<Maid, MaidInfo> loadedMaids;
        private Dictionary<string, Image> maidThumbnails;
        private Dictionary<MaidChangeType, Action>[] valueUpdateQueue;

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
            comparer = new MaidComparer(Plugin);
            loadedMaids = new SortedList<Maid, MaidInfo>(comparer);
            maidThumbnails = new Dictionary<string, Image>();
            valueUpdateQueue = new Dictionary<MaidChangeType, Action>[2];
            valueUpdateQueue[0] = new Dictionary<MaidChangeType, Action>();
            valueUpdateQueue[1] = new Dictionary<MaidChangeType, Action>();
        }

        private bool IsMaidLoaded(Maid maid)
        {
            return loadedMaids.ContainsKey(maid);
        }

        public void ReloadMaids()
        {
            InvokeAsync((UpdateInternal) ReloadMaids, GameMain.Instance.CharacterMgr.GetStockMaidList().ToList());
        }

        public void UnloadMaids()
        {
            InvokeAsync((Action) _UnloadMaids);
        }

        private void _UnloadMaids()
        {
            Debugger.Assert(() =>
            {
                Debugger.WriteLine(LogLevel.Info, "Unloading maids!");
                Debugger.WriteLine(LogLevel.Info, $"Loaded maids: {loadedMaids.Count}");
                loadedMaids.Clear();
                currentQueue = 0;
                valueUpdateQueue[0].Clear();
                valueUpdateQueue[1].Clear();
                if (maidThumbnails.Count > 0) maidThumbnails.ForEach(thumb => thumb.Value.Dispose());
                maidThumbnails.Clear();
                UpdateList();
            }, "Failed to unload maids");
        }

        private void ReloadMaids(List<Maid> maids)
        {
            Debugger.Assert(() =>
            {
                Debugger.WriteLine(LogLevel.Info, "Reloading maids!");
                Debugger.WriteLine(LogLevel.Info, $"Maids: {maids.Count}");
                Debugger.WriteLine(LogLevel.Info, $"Loaded maids: {loadedMaids.Count}");
                loadedMaids.Clear();
                currentQueue = 0;
                valueUpdateQueue[0].Clear();
                valueUpdateQueue[1].Clear();
                if (maidThumbnails.Count > 0) maidThumbnails.ForEach(thumb => thumb.Value.Dispose());
                maidThumbnails.Clear();
                loadedMaids = new SortedList<Maid, MaidInfo>(maids.ToDictionary(m => m, m => new MaidInfo(m, this)),
                    comparer);
                loadedMaids.ForEach(m =>
                {
                    Texture2D thumb = m.Value.Maid.GetThumIcon();
                    if (thumb == null) return;
                    using (MemoryStream stream = new MemoryStream(thumb.EncodeToPNG()))
                    {
                        Debugger.WriteLine("Loading PNG of size: " + stream.Length);
                        maidThumbnails.Add(m.Key.Param.status.guid, Image.FromStream(stream));
                    }
                });

                UpdateList();
            }, "Failed to reload all maids");
        }

        public void UpdateMaids()
        {
            InvokeAsync((UpdateInternal) UpdateMaids, GameMain.Instance.CharacterMgr.GetStockMaidList().ToList());
        }

        private void UpdateMaids(List<Maid> newMaids)
        {
            Debugger.Assert(() =>
            {
                if (newMaids.Count != loadedMaids.Count) goto update;

                newMaids.Sort((m1, m2) => comparer.Compare(m1, m2));
                if (newMaids.SequenceEqual(loadedMaids.Values.Select(m => m.Maid), comparer)) return;

                update:
#if DEBUG
                Stopwatch sw = Stopwatch.StartNew();
#endif
                Debugger.WriteLine(LogLevel.Info, "Updating maid list!");
                Debugger.WriteLine(LogLevel.Info, $" New count:  {newMaids.Count}, Loaded count: {loadedMaids.Count}");
                loadedMaids = new SortedList<Maid, MaidInfo>(loadedMaids.Where(m =>
                {
                    bool result = newMaids.Contains(m.Key);
                    if (result) newMaids.Remove(m.Key);
                    else
                    {
                        if (SelectedMaid != null && m.Value.Maid == SelectedMaid.Maid)
                        {
                            currentQueue = 0;
                            valueUpdateQueue[0].Clear();
                            valueUpdateQueue[1].Clear();
                        }
                        if (maidThumbnails.ContainsKey(m.Key.Param.status.guid)) maidThumbnails[m.Key.Param.status.guid].Dispose();
                        maidThumbnails.Remove(m.Key.Param.status.guid);
                    }
                    return result;
                }).ToList().Union(newMaids.Select(m =>
                {
                    Debugger.WriteLine(LogLevel.Info, "Adding new maid info.");
                    MaidInfo info = new MaidInfo(m, this);
                    Debugger.WriteLine(LogLevel.Info, "Loading thumbnail");
                    Texture2D thumb = m.GetThumIcon();
                    if (thumb == null) return new KeyValuePair<Maid, MaidInfo>(m, info);
                    using (MemoryStream stream = new MemoryStream(thumb.EncodeToPNG()))
                    {
                        Debugger.WriteLine("Loading PNG of size: " + stream.Length);
                        maidThumbnails.Add(m.Param.status.guid, Image.FromStream(stream));
                    }
                    return new KeyValuePair<Maid, MaidInfo>(m, info);
                })).ToDictionary(m => m.Key, m => m.Value), comparer);


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
            }, "Failed to update maid list");
        }

        public void UpdateSelectedMaidValues()
        {
            if (InvokeRequired)
            {
                InvokeAsync((Action) UpdateSelectedMaidValues);
                return;
            }
            MaidChangeType cType = 0;
            Debugger.Assert(() =>
            {
                int processingQueue = currentQueue;
                currentQueue = 1 - currentQueue;
                if (valueUpdateQueue[processingQueue].Count <= 0) return;
                Debugger.WriteLine(LogLevel.Info, $"Updating values (Queue {processingQueue})...");
                foreach (KeyValuePair<MaidChangeType, Action> type in valueUpdateQueue[processingQueue])
                {
                    cType = type.Key;
                    type.Value();
                }
                valueUpdateQueue[processingQueue].Clear();
            }, $"Failed to update scheduled maid value. Type: {cType}");
        }

        private class MaidComparer : IEqualityComparer<Maid>, IComparer<Maid>
        {
            private readonly MaidFiddler plugin;

            public MaidComparer(MaidFiddler plugin)
            {
                this.plugin = plugin;
            }

            public int Compare(Maid x, Maid y)
            {
                int result = 0;
                return plugin.MaidCompareMethods.Any(method => (result = method(x, y)) != 0)
                           ? result : plugin.MaidCompareID(x, y);
            }

            public bool Equals(Maid x, Maid y)
            {
                return x.Param.status.Equals(y.Param.status);
            }

            public int GetHashCode(Maid obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}