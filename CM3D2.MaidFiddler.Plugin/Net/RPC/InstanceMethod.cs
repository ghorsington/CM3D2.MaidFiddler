using System.Reflection;

namespace CM3D2.MaidFiddler.Plugin.Net.RPC
{
    public class InstanceMethod : Method
    {
        public InstanceMethod(MethodInfo methodInfo, IInstanceProvider instanceProvider) : base(methodInfo)
        {
            InstanceProvider = instanceProvider;
        }

        public IInstanceProvider InstanceProvider { get; }

        public override void Invoke(params object[] parameters)
        {
        }
    }
}