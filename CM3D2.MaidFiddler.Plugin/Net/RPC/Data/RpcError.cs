using System;

namespace CM3D2.MaidFiddler.Plugin.Net.RPC.Data
{
    public class RpcError : RpcObject
    {
        public RpcError(string version, object id, RpcErrorData errorData) : base(version)
        {
            ErrorData = errorData;
            Id = id;
        }

        public RpcErrorData ErrorData { get; }

        public object Id { get; }

        public static RpcObject FromRawData(RpcRawData data)
        {
            return new RpcError(data.Version, data.Id, data.Error);
        }

        public override void Process(RpcManager manager)
        {
            throw new NotImplementedException();
        }
    }
}