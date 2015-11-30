using System;

namespace CM3D2.MaidFiddler.Hook
{
    [AttributeUsage(AttributeTargets.All)]
    public class MaidFiddlerPatchedAttribute : Attribute
    {
        public uint PatchVersion;

        public MaidFiddlerPatchedAttribute(uint version)
        {
            PatchVersion = version;
        }
    }
}