using System;

namespace CM3D2.MaidFiddler.Plugin.Net.RPC.Data
{
    public class RpcResult : RpcObject
    {
        public RpcResult(string version, object id, object result) : base(version)
        {
            Id = id;
            Result = result;
        }

        public object Id { get; }

        public object Result { get; }

        public override void Process(RpcManager manager)
        {
            throw new NotImplementedException();
        }

        public static RpcObject ToRpcResult(RpcRawData data)
        {
            return new RpcResult(data.Version, data.Id, data.Result);
        }
    }
}