using JsonFx.Json;

namespace CM3D2.MaidFiddler.Plugin.Net.RPC.Data
{
    public class RpcRawData
    {
        public const string JsonRpcVersion = "2.0";

        [JsonName("error")]
        public RpcErrorData Error { get; set; } = null;

        [JsonName("id")]
        public string Id { get; set; } = null;

        [JsonName("method")]
        public string Method { get; set; } = null;

        [JsonName("params")]
        public object Parameters { get; set; } = null;

        [JsonName("result")]
        public object Result { get; set; } = null;

        public bool ValidVersion => Version != null && Version == JsonRpcVersion;

        [JsonName("jsonrpc")]
        public string Version { get; set; } = null;

        public RpcObject ToRpcObject(RpcManager manager)
        {
            if (!ValidVersion)
                throw new RpcException(RpcErrorCode.InvalidRequest,
                                       $"This implementation of JSON-RPC supports only version {JsonRpcVersion}");
            if (Error != null)
                return RpcError.FromRawData(this);
            if (Result != null)
                return RpcResult.ToRpcResult(this);
            if (Method != null)
                return RpcRequest.FromRawData(this, manager);
            throw new RpcException(RpcErrorCode.InvalidRequest, "Invalid request.");
        }

        public override string ToString()
        {
            return $"JSON-RPC {Version}: ID: \"{Id}\", Method: {Method}, Parameters: {Parameters}";
        }
    }

    public class RpcErrorData
    {
        [JsonName("code")]
        public int Code { get; set; } = 0;

        [JsonName("data")]
        public object Data { get; set; } = null;

        [JsonName("messsage")]
        public string Message { get; set; } = null;
    }
}