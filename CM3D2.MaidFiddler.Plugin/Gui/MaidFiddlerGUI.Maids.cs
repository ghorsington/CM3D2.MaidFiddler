using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Utils;
using UnityEngine;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        private SortedList<string, MaidInfo> loadedMaids;
        private Dictionary<string, Image> maidThumbnails;
        private Dictionary<MaidChangeType, Action> valueUpdateQueue;

        public MaidInfo SelectedMaid
            =>
            listBox1.Items.Count == 0 || listBox1.SelectedIndex == -1
            ? null : (MaidInfo) listBox1.Items[listBox1.SelectedIndex];

        private MaidInfo GetMaidInfo(Maid maid)
        {
            return loadedMaids[maid.Param.status.guid];
        }

        private void InitMaids()
        {
            loadedMaids = new SortedList<string, MaidInfo>();
            maidThumbnails = new Dictionary<string, Image>();
            valueUpdateQueue = new Dictionary<MaidChangeType, Action>();
        }

        private bool IsMaidLoaded(Maid maid)
        {
            return loadedMaids.ContainsKey(maid.Param.status.guid);
        }

        public void ReloadMaids()
        {
            InvokeAsync((UpdateInternal) ReloadMaids, GameMain.Instance.CharacterMgr.GetStockMaidList());
        }

        private void ReloadMaids(List<Maid> maids)
        {
            Debugger.WriteLine(LogLevel.Info, "Reloading maids!");
            Debugger.WriteLine(LogLevel.Info, $"Maids: {maids.Count}");
            Debugger.WriteLine(LogLevel.Info, $"Loaded maids: {loadedMaids.Count}");
            loadedMaids.Clear();
            valueUpdateQueue.Clear();
            if (maidThumbnails.Count > 0)
                maidThumbnails.ForEach(thumb => thumb.Value.Dispose());
            maidThumbnails.Clear();
            loadedMaids =
            new SortedList<string, MaidInfo>(maids.ToDictionary(m => m.Param.status.guid, m => new MaidInfo(m, this)));
            loadedMaids.ForEach(
            m =>
            {
                Texture2D thumb = m.Value.Maid.GetThumIcon();
                if (thumb == null)
                    return;
                using (MemoryStream stream = new MemoryStream(thumb.EncodeToPNG()))
                {
                    Debugger.WriteLine("Loading PNG of size: " + stream.Length);
                    maidThumbnails.Add(m.Key, Image.FromStream(stream));
                }
            });

            UpdateList();
        }

        public void UpdateMaids()
        {
            InvokeAsync((UpdateInternal) UpdateMaids, GameMain.Instance.CharacterMgr.GetStockMaidList());
        }

        private void UpdateMaids(List<Maid> newMaids)
        {
            if (newMaids.Count != loadedMaids.Count)
                goto update;

            newMaids.Sort((m1, m2) => string.CompareOrdinal(m1.Param.status.guid, m2.Param.status.guid));
            if (newMaids.SequenceEqual(loadedMaids.Values.Select(m => m.Maid), new MaidComparer()))
                return;

            update:
            Debugger.WriteLine(LogLevel.Info, "Updating maid list!");
            Dictionary<string, Maid> newMaidList = newMaids.ToDictionary(m => m.Param.status.guid, m => m);
            Debugger.WriteLine(LogLevel.Info, $" New count:  {newMaids.Count}, Loaded count: {loadedMaids.Count}");
            loadedMaids = new SortedList<string, MaidInfo>(
            loadedMaids.Where(
            m =>
            {
                bool result = newMaidList.ContainsKey(m.Key);
                if (result)
                    newMaidList.Remove(m.Key);
                else
                {
                    if (SelectedMaid != null && m.Value.Maid == SelectedMaid.Maid)
                        valueUpdateQueue.Clear();
                    if (maidThumbnails.ContainsKey(m.Key))
                        maidThumbnails[m.Key].Dispose();
                    maidThumbnails.Remove(m.Key);
                }
                return result;
            }).ToList().Union(
            newMaidList.ToDictionary(
            m => m.Key,
            m =>
            {
                Debugger.WriteLine(LogLevel.Info, "Adding new maid info.");
                MaidInfo info = new MaidInfo(m.Value, this);
                Debugger.WriteLine(LogLevel.Info, "Loading thumbnail");
                Texture2D thumb = m.Value.GetThumIcon();
                if (thumb == null)
                    return info;
                using (MemoryStream stream = new MemoryStream(thumb.EncodeToPNG()))
                {
                    Debugger.WriteLine("Loading PNG of size: " + stream.Length);
                    maidThumbnails.Add(m.Key, Image.FromStream(stream));
                }
                return info;
            })).ToDictionary(m => m.Key, m => m.Value));


            Debugger.WriteLine(LogLevel.Info, $"New loaded maids count: {loadedMaids.Count}");
#if DEBUG
            newMaidList.ForEach(
            m => Debugger.WriteLine($"Added {m.Value.Param.status.first_name} {m.Value.Param.status.last_name}"));
            Debugger.WriteLine();
#endif
            Debugger.WriteLine("Updating list.");
            UpdateList();
            Debugger.WriteLine("Finished updating list.");
        }

        public void UpdateSelectedMaidValues()
        {
            if (InvokeRequired)
            {
                InvokeAsync((Action) UpdateSelectedMaidValues);
                return;
            }

            if (valueUpdateQueue.Count <= 0)
                return;
            Debugger.WriteLine(LogLevel.Info, "Updating values...");
            foreach (KeyValuePair<MaidChangeType, Action> type in valueUpdateQueue)
            {
                type.Value();
            }
            valueUpdateQueue.Clear();
        }

        private class MaidComparer : IEqualityComparer<Maid>
        {
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