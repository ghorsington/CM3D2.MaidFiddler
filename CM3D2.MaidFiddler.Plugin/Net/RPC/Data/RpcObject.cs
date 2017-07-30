namespace CM3D2.MaidFiddler.Plugin.Net.RPC.Data
{
    public abstract class RpcObject
    {
        protected RpcObject(string version)
        {
            Version = version;
        }

        public string Version { get; }

        public abstract void Process(RpcManager manager);
    }
}