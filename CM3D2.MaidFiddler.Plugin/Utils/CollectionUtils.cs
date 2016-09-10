using System;
using System.Collections.Generic;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public static class CollectionUtils
    {
        public static void ForEach<T>(this IEnumerable<T> es, Action<T> action)
        {
            foreach (T e in es)
                action(e);
        }
    }
}