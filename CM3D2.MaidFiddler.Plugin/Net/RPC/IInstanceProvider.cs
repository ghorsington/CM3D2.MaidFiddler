namespace CM3D2.MaidFiddler.Plugin.Net.RPC
{
    public interface IInstanceProvider
    {
        object GetInstance(string instanceIdentifier);
    }
}