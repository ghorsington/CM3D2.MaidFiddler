using JsonFx.Json;

namespace CM3D2.MaidFiddler.Plugin.Net.RPC
{
    public class RpcErrorData
    {
        [JsonName("code")]
        public int Code { get; set; } = 0;

        [JsonName("messsage")]
        public string Message { get; set; } = null;

        [JsonName("data")]
        public object Data { get; set; } = null;
    }
}