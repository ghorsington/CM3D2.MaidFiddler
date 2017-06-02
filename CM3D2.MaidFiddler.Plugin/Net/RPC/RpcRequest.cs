namespace CM3D2.MaidFiddler.Plugin.Net.RPC
{
    public class RpcRequest
    {
        public string Method { get; }

        public object[] Parameters { get; }

        public string ID { get; }
    }
}