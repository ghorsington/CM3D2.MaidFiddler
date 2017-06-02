using System;

namespace CM3D2.MaidFiddler.Plugin.Net.RPC
{
    public class RpcException : Exception
    {
        public RpcErrorCode ErrorCode { get; }

        public RpcException(RpcErrorCode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}