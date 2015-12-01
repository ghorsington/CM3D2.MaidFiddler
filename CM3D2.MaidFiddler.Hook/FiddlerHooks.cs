using System;

namespace CM3D2.MaidFiddler.Hook
{
    public static class FiddlerHooks
    {
        public static event Action<int> SaveLoadedEvent;

        public static void OnSaveDeserialize(int saveNo)
        {
            SaveLoadedEvent?.Invoke(saveNo);
        }
    }
}