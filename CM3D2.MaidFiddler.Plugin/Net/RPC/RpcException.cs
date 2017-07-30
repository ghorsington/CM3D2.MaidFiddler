using System;
using CM3D2.MaidFiddler.Plugin.Net.RPC.Data;

namespace CM3D2.MaidFiddler.Plugin.Net.RPC
{
    public class RpcException : Exception
    {
        public RpcException(RpcErrorCode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public RpcErrorCode ErrorCode { get; }
    }
}