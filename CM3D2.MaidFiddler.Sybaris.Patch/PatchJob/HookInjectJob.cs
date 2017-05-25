using System;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Sybaris.Patcher.PatchJob
{
    public class HookInjectJob : IPatchJob
    {
        public HookInjectJob(string name,
                             int offset,
                             MethodDefinition targetMethod,
                             MethodDefinition hookMethod,
                             InjectFlags flags,
                             int[] localIDs,
                             FieldDefinition[] fields)
        {
            Name = name;
            Offset = offset;
            TargetMethod = targetMethod;
            HookMethod = hookMethod;
            InjectFlags = flags;
            LocalIds = localIDs;
            Fields = fields;
        }

        protected string Name { get; }

        protected MethodDefinition TargetMethod { get; }

        protected MethodDefinition HookMethod { get; }

        protected InjectFlags InjectFlags { get; }

        protected int[] LocalIds { get; }

        protected FieldDefinition[] Fields { get; }

        protected int Offset { get; }

        public virtual void Patch()
        {
            try
            {
                TargetMethod.InjectWith(HookMethod,
                                        Offset,
                                        Name,
                                        InjectFlags,
                                        InjectDirection.Before,
                                        LocalIds,
                                        Fields);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to patch  {TargetMethod.DeclaringType.Name}.{TargetMethod.Name}", e);
            }
        }
    }
}