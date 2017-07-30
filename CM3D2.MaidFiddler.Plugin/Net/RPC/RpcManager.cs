using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using CM3D2.MaidFiddler.Plugin.Net.RPC.Data;
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
        public delegate void OnMessageParseFail(MessageParseFailEventArgs args);

        private Queue<RpcObject> incomingObjectQueue;

        private bool isRunningMessageProcessor;
        private readonly Thread messageParserThread;

        private readonly Queue<string> messageQueue;
        private readonly Semaphore unprocessedMessages;

        public RpcManager()
        {
            Methods = new Dictionary<string, Method>();
            InstanceProviders = new Dictionary<Type, IInstanceProvider>();

            messageQueue = new Queue<string>();
            incomingObjectQueue = new Queue<RpcObject>();
            unprocessedMessages = new Semaphore(0, int.MaxValue);
            messageParserThread = new Thread(ParseMessages);
        }

        private Dictionary<Type, IInstanceProvider> InstanceProviders { get; }

        private Dictionary<string, Method> Methods { get; }

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
                    if (rpcMethod.method.ContainsGenericParameters && rpcMethod.method.GetParameters().Length == 0)
                        continue;
                    RpcCallAttribute attribute = (RpcCallAttribute) rpcMethod.attr[0];

                    // TODO: Handle overloads and identically named methods
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
                        Methods
                                .Add($"{declaringType.Name}.{(string.IsNullOrEmpty(attribute.CallName) ? rpcMethod.method.Name : attribute.CallName)}",
                                     new InstanceMethod(rpcMethod.method, provider));
                    }
                }
            }
        }

        public Method GetMethod(string method)
        {
            return Methods.TryGetValue(method, out Method result) ? result : null;
        }

        public void ParseMessage(string message)
        {
            lock (messageQueue)
            {
                messageQueue.Enqueue(message);
            }
            unprocessedMessages.Release();
        }

        public void RegisterInstanceProvider<T>(IInstanceProvider provider)
        {
            InstanceProviders.Add(typeof(T), provider);
        }

        public void Start()
        {
            if (isRunningMessageProcessor)
                return;
            isRunningMessageProcessor = true;
            messageParserThread.Start();
        }

        public void Stop()
        {
            if (!isRunningMessageProcessor)
                return;
            isRunningMessageProcessor = false;
            messageParserThread.Abort();
        }

        private void ParseMessages()
        {
            while (unprocessedMessages.WaitOne())
            {
                string message;
                lock (messageQueue)
                {
                    message = messageQueue.Dequeue();
                }
                Debugger.WriteLine(LogLevel.Info, "Attempting to deserialize the message...");
                JsonReader reader = new JsonReader(message);
                try
                {
                    RpcRawData result = reader.Deserialize<RpcRawData>();

                    Debugger.WriteLine(LogLevel.Info, result.ToString());
                    if (!result.ValidVersion)
                        throw new RpcException(RpcErrorCode.ParseError - 1,
                                               $"This JSON-RPC implementation only supports version {RpcRawData.JsonRpcVersion}.");
                }
                catch (Exception e)
                {
                    Debugger.WriteLine(LogLevel.Info, $"Failed to parse message because: {e}");
                }
            }
        }
    }
}