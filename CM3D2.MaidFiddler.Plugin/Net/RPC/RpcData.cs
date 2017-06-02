using System.Reflection;
using JsonFx.Json;

namespace CM3D2.MaidFiddler.Plugin.Net.RPC
{
    public class RpcData
    {
        public const string JsonRpcVersion = "2.0";

        [JsonName("jsonrpc")]
        public string Version { get; set; } = null;

        [JsonName("method")]
        public string Method { get; set; } = null;

        [JsonName("params")]
        public object Parameters { get; set; } = null;

        [JsonName("id")]
        public string Id { get; set; } = null;

        [JsonName("result")]
        public object Result { get; set; } = null;

        [JsonName("error")]
        public RpcErrorData Error { get; set; }

        public bool ValidVersion => Version != null && Version == JsonRpcVersion;

        public bool TryAsRequest(RpcManager manager)
        {
            if (Method == null)
                return false;

            Method method = manager.GetMethod(Method);
            if (method == null)
                return false;

            return false;
        }
    }
}