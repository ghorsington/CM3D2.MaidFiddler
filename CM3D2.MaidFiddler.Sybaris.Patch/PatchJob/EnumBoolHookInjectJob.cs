using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Sybaris.Patcher.PatchJob
{
    public class EnumBoolHookInjectJob : HookInjectJob
    {
        public EnumBoolHookInjectJob(string name,
                                     MethodDefinition target,
                                     MethodDefinition hook,
                                     FieldDefinition[] fields) : base(
            name,
            0,
            target,
            hook,
            InjectFlags.None,
            new int[0],
            fields)
        {
        }

        public override void Patch()
        {
            if (TargetMethod == null)
                return;

            MethodReference hookRef = TargetMethod.Module.Import(HookMethod);
            IEnumerable<FieldReference> fieldRefs = Fields.Select(fd => TargetMethod.Module.Import(fd));

            Instruction start = TargetMethod.Body.Instructions[0];
            ILProcessor il = TargetMethod.Body.GetILProcessor();

            il.InsertBefore(start, il.Create(OpCodes.Ldstr, Name));
            il.InsertBefore(start, il.Create(OpCodes.Ldarg_0));
            foreach (FieldReference fieldReference in fieldRefs)
                il.InsertBefore(start, il.Create(OpCodes.Ldflda, fieldReference));
            il.InsertBefore(start, il.Create(OpCodes.Ldarg_1));
            il.InsertBefore(start, il.Create(OpCodes.Ldarg_2));
            il.InsertBefore(start, il.Create(OpCodes.Call, hookRef));
        }
    }
}