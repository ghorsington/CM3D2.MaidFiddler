using System.Reflection;

namespace CM3D2.MaidFiddler.Plugin.Net.RPC
{
    public class Method
    {
        public MethodInfo MethodInfo { get; }

        public Method(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;
        }

        public virtual void Invoke(params object[] parameters)
        {

        }
    }
}