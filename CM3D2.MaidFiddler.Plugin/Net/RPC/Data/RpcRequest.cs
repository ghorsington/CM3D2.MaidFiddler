using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CM3D2.MaidFiddler.Plugin.Net.RPC.Data
{
    public class RpcRequest : RpcObject
    {
        public RpcRequest(string version, object id, Method method, object[] parameters) : base(version) { }

        public object ID { get; }
        public Method Method { get; }

        public object[] Parameters { get; }

        public static RpcRequest FromRawData(RpcRawData data, RpcManager manager)
        {
            Method method = manager.GetMethod(data.Method);
            if (method == null)
                throw new RpcException(RpcErrorCode.MethodNotFound, $"Method {data.Method} does not exist.");

            ParameterInfo[] methodParameters = method.MethodInfo.GetParameters();

            if (methodParameters.Length == 0 && data.Parameters != null)
                throw new RpcException(RpcErrorCode.InvalidParameters, $"Method {data.Method} has zero parameters.");

            Dictionary<string, object> paramObject;
            object[] parameters;
            if ((parameters = data.Parameters as object[]) != null)
            {
                if (parameters.Length < method.RequiredParametersCount || parameters.Length > methodParameters.Length)
                    throw new RpcException(RpcErrorCode.InvalidParameters,
                                           $"Method {data.Method} has {methodParameters.Length} parameters ({method.RequiredParametersCount} required). The request had only {parameters.Length} parameters.");
            }
            else if ((paramObject = data.Parameters as Dictionary<string, object>) != null)
            {
                Dictionary<string, ParameterInfo> methodParameterDictionary =
                        methodParameters.ToDictionary(pi => pi.Name);
                bool[] requiredCheckArray = new bool[method.RequiredParametersCount];
                parameters = new object[methodParameters.Length];

                foreach (KeyValuePair<string, object> parameter in paramObject)
                {
                    if (!methodParameterDictionary.TryGetValue(parameter.Key, out ParameterInfo parameterInfo))
                        throw new RpcException(RpcErrorCode.InvalidParameters,
                                               $"Method {data.Method} does not have a parameter with name {parameter.Key}.");
                    parameters[parameterInfo.Position] = parameter.Value;
                    if (!parameterInfo.IsOptional)
                        requiredCheckArray[parameterInfo.Position] = true;
                }

                for (int index = 0; index < requiredCheckArray.Length; index++)
                {
                    bool specified = requiredCheckArray[index];
                    if (!specified)
                        throw new RpcException(RpcErrorCode.InvalidParameters,
                                               $"Method {data.Method} requires parameter {methodParameters[index].Name} to be specified.");
                }
            }
            else
                throw new RpcException(RpcErrorCode.InvalidParameters,
                                       $"Parameters must be provided either as an array or as an object of named parameters.");
            return new RpcRequest(data.Version, data.Id, method, parameters);
        }

        public override void Process(RpcManager manager)
        {
            throw new NotImplementedException();
        }
    }
}