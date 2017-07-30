using System;

namespace CM3D2.MaidFiddler.Plugin.Net.RPC
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RpcCallAttribute : Attribute
    {
        public RpcCallAttribute(string callName = "")
        {
            CallName = callName;
        }

        public string CallName { get; }
    }
}