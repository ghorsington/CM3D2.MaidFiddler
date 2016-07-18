using System;

namespace CM3D2.MaidFiddler.Hook
{
    [AttributeUsage(AttributeTargets.All)]
    public class MaidFiddlerPatcherAttribute : Attribute
    {
        public uint PatcherType;

        public MaidFiddlerPatcherAttribute(uint type)
        {
            PatcherType = type;
        }
    }

    public enum PatcherType
    {
        ReiPatcher,
        Sybaris
    }
}