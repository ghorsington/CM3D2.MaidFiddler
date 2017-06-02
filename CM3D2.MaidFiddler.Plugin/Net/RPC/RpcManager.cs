using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CM3D2.MaidFiddler.Plugin.Utils;
using JsonFx.Json;

namespace CM3D2.MaidFiddler.Plugin.Net.RPC
{
    public class MessageParseFailEventArgs : EventArgs
    {
        public Exception Exception { get; internal set; }
    }

    public class RpcManager
    {
        public RpcManager()
        {
            Methods = new Dictionary<string, Method>();
            InstanceProviders = new Dictionary<Type, IInstanceProvider>();
        }

        public delegate void OnMessageParseFail(MessageParseFailEventArgs args);

        private Dictionary<string, Method> Methods { get; }
        private Dictionary<Type, IInstanceProvider> InstanceProviders { get; }

        public event OnMessageParseFail MessageParsingFailed;

        public void ParseMessageAsync(string message)
        {
            ParseMessageWorker worker = ParseMessageWork;

            worker.BeginInvoke(message, null, null);
        }

        public void CollectMethods(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsInterface)
                    continue;
                var rpcMethods = type.GetMethods(BindingFlags.Public)
                                     .Select(m => new
                                     {
                                         method = m,
                                         attr = m.GetCustomAttributes(typeof(RpcCallAttribute), true)
                                     })
                                     .Where(m => m.attr.Length > 0);
                foreach (var rpcMethod in rpcMethods)
                {
                    if (rpcMethod.method.IsAbstract)
                        continue;
                    RpcCallAttribute attribute = (RpcCallAttribute) rpcMethod.attr[0];

                    if (rpcMethod.method.IsStatic)
                        Methods.Add(string.IsNullOrEmpty(attribute.CallName)
                                        ? rpcMethod.method.Name
                                        : attribute.CallName,
                                    new Method(rpcMethod.method));
                    else
                    {
                        Type declaringType = rpcMethod.method.DeclaringType;
                        if (!InstanceProviders.TryGetValue(declaringType, out IInstanceProvider provider))
                            throw new Exception($"No instance provider found for {declaringType.Name}");
                        Methods.Add(
                            $"{declaringType.Name}.{(string.IsNullOrEmpty(attribute.CallName) ? rpcMethod.method.Name : attribute.CallName)}",
                            new InstanceMethod(rpcMethod.method, provider));
                    }
                }
            }
        }

        public void RegisterInstanceProvider<T>(IInstanceProvider provider)
        {
            InstanceProviders.Add(typeof(T), provider);
        }

        public Method GetMethod(string method)
        {
            return Methods.TryGetValue(method, out Method result) ? result : null;
        }

        private void ParseMessageWork(string message)
        {
            Debugger.WriteLine(LogLevel.Info, "Attempting to deserialize the message...");
            JsonReader reader = new JsonReader(message);
            try
            {
                RpcData result = reader.Deserialize<RpcData>();

                if (!result.ValidVersion)
                    throw new RpcException(RpcErrorCode.ParseError - 1,
                                           $"This JSON-RPC implementation only supports version {RpcData.JsonRpcVersion}.");
            }
            catch (Exception e)
            {
                MessageParseFailEventArgs args = new MessageParseFailEventArgs
                {
                    Exception = e
                };
                MessageParsingFailed?.BeginInvoke(args, null, null);
            }
        }

        private delegate void ParseMessageWorker(string message);
    }
}