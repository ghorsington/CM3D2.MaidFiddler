using System;
using System.Linq;
using System.Reflection;

namespace CM3D2.MaidFiddler.Plugin.Net.RPC
{
    public class Method
    {
        private int requiredParameters = -1;

        public Method(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;
        }

        public MethodInfo MethodInfo { get; }

        public int RequiredParametersCount
        {
            get
            {
                if (requiredParameters < 0)
                    requiredParameters = ComputeRequiredParameters();
                return requiredParameters;
            }
        }

        public virtual void Invoke(params object[] parameters)
        {
            MethodInfo resolvedMethodInfo = MethodInfo;
            if (MethodInfo.ContainsGenericParameters)
            {
                Type[] genericArgs = new Type[MethodInfo.GetGenericArguments().Length];
                ParameterInfo[] paramterInfos = MethodInfo.GetParameters();
                for (int index = 0; index < paramterInfos.Length; index++)
                {
                    ParameterInfo parameterInfo = paramterInfos[index];
                    Type genericParamType = parameterInfo.ParameterType;

                    if (!genericParamType.IsGenericType)
                        continue;

                    int position = genericParamType.GenericParameterPosition;
                    Type paramType = parameters[index].GetType();

                    if (genericArgs[position] != null && genericArgs[position] != paramType)
                        throw new
                                Exception($"Tried to call {MethodInfo.Name}, but same generic argument is defined by two different parameter types.");

                    genericArgs[position] = paramType;
                }
                resolvedMethodInfo = MethodInfo.MakeGenericMethod(genericArgs);
            }
            resolvedMethodInfo.Invoke(null, parameters);
        }

        private int ComputeRequiredParameters()
        {
            return MethodInfo.GetParameters().Count(pi => !pi.IsOptional);
        }
    }
}