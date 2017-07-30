using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Patch.PatchJob
{
    public abstract class PatchJobCollection
    {
        protected PatchJobCollection()
        {
            PatchTargets = new List<IPatchJob>();
        }

        protected abstract MethodDefinition HookMethod { get; set; }
        protected abstract InjectFlags InjectFlags { get; }
        protected abstract FieldDefinition[] MemberFields { get; set; }

        protected List<IPatchJob> PatchTargets { get; }
        protected abstract TypeDefinition TargetType { get; set; }

        public abstract void Initialize(AssemblyDefinition gameAssembly, AssemblyDefinition hookAssembly);

        public virtual void Patch()
        {
            LoadJobs();

            foreach (IPatchJob patchJob in PatchTargets)
                patchJob.Patch();
        }

        protected void Add(string name)
        {
            MethodWithPrefix("Add", name);
        }

        protected void Add(string name, int offset, params Type[] parameters)
        {
            MethodWithTag($"Add{name}", name, offset, parameters);
        }

        protected void AddSet(string name)
        {
            MethodWithPrefix("Add", name);
            MethodWithPrefix("Set", name);
        }


        protected void CustomMethod(string name, MethodDefinition hookMethod, int offset = 0)
        {
            CustomMethod(name, name, hookMethod, offset);
        }

        protected void CustomMethod(string name, string tag, MethodDefinition hookMethod, int offset = 0)
        {
            CustomMethod(name, offset, TargetType, hookMethod, InjectFlags, MemberFields);
        }

        protected void CustomMethod(string name,
                                    int offset,
                                    TypeDefinition targetType,
                                    MethodDefinition hookMethod,
                                    InjectFlags injectFlags,
                                    FieldDefinition[] memberFields)
        {
            CustomMethod(name, name, offset, targetType, hookMethod, injectFlags, memberFields);
        }

        protected void CustomMethod(string name,
                                    string tag,
                                    int offset,
                                    TypeDefinition targetType,
                                    MethodDefinition hookMethod,
                                    InjectFlags injectFlags,
                                    FieldDefinition[] memberFields)
        {
            MethodDefinition target = targetType.GetMethod(name);
            if (target == null)
            {
                Console.WriteLine($"Method {targetType.Name}.{name} not found, skipping...");
                return;
            }

            PatchTargets.Add(new HookInjectJob(tag, offset, target, hookMethod, injectFlags, new int[0], memberFields));
        }

        protected void CustomMethod(string name, MethodDefinition hookMethod, int offset, params Type[] parameters)
        {
            MethodDefinition target = TargetType.GetMethod(name, parameters);
            if (target == null)
            {
                Console.WriteLine($"Method {TargetType.Name}.{name} not found, skipping...");
                return;
            }

            PatchTargets.Add(new HookInjectJob(name,
                                               offset,
                                               target,
                                               hookMethod,
                                               InjectFlags,
                                               new int[0],
                                               MemberFields));
        }

        protected abstract void LoadJobs();

        protected void Method(string name, int offset = 0)
        {
            MethodWithTag(name, name, offset);
        }

        protected void MethodWithPrefix(string prefix, string name, int offset = 0)
        {
            MethodWithTag(prefix + name, name, offset);
        }

        protected void MethodWithTag(string name, string tag, int offset = 0)
        {
            MethodDefinition target = TargetType.GetMethod(name);
            if (target == null)
            {
                Console.WriteLine($"Method {TargetType.Name}.{name} not found, skipping...");
                return;
            }

            PatchTargets.Add(new HookInjectJob(tag, offset, target, HookMethod, InjectFlags, new int[0], MemberFields));
        }

        protected void MethodWithTag(string name, string tag, int offset, params Type[] parameters)
        {
            MethodDefinition target = TargetType.GetMethod(name, parameters);
            if (target == null)
            {
                Console.WriteLine($"Method {TargetType.Name}.{name} not found, skipping...");
                return;
            }

            PatchTargets.Add(new HookInjectJob(tag, offset, target, HookMethod, InjectFlags, new int[0], MemberFields));
        }

        protected void Set(string name, int offset = 0)
        {
            MethodWithPrefix("Set", name, offset);
        }

        protected void Set(string name, int offset, params Type[] parameters)
        {
            MethodWithTag($"Set{name}", name, offset, parameters);
        }
    }
}