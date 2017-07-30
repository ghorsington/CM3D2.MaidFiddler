using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Sybaris.Patcher.PatchJob;
using Mono.Cecil;
using Mono.Cecil.Inject;

namespace CM3D2.MaidFiddler.Sybaris.Patcher
{
    public static class MaidFiddlerPatcher
    {
        public static readonly string[] TargetAssemblyNames = {"Assembly-CSharp.dll"};

        private const string AssembliesDir = @"..\Plugins\Managed\";

        private static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static void Patch(AssemblyDefinition assembly)
        {
            AssemblyDefinition hookAssembly = LoadAssembly("CM3D2.MaidFiddler.Hook.dll");
            TypeDefinition maidParam = assembly.MainModule.GetType("MaidParam");
            TypeDefinition playerParam = assembly.MainModule.GetType("PlayerParam");
            TypeDefinition status = assembly.MainModule.GetType("param.Status");

            IEnumerable<PatchJobCollection> jobs = Assembly.GetExecutingAssembly()
                                                           .GetTypes()
                                                           .Where(type => type.Namespace
                                                                          == "CM3D2.MaidFiddler.Patch.Jobs"
                                                                          && !type.IsAbstract
                                                                          && typeof(PatchJobCollection)
                                                                                  .IsAssignableFrom(type))
                                                           .Select(Activator.CreateInstance)
                                                           .Cast<PatchJobCollection>();

            foreach (PatchJobCollection job in jobs)
            {
                job.Initialize(assembly, hookAssembly);
                job.Patch();
            }

            maidParam.ChangeAccess("status_");
            playerParam.ChangeAccess("status_");
            status.ChangeAccess("kInitMaidPoint");

            SetCustomPatchedAttribute(assembly);
        }

        private static AssemblyDefinition LoadAssembly(string name)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string managedDir = Path.Combine(path, AssembliesDir);
            return AssemblyLoader.LoadAssembly(Path.Combine(managedDir, name));
        }

        private static void SetCustomPatchedAttribute(AssemblyDefinition ass)
        {
            CustomAttribute attr =
                    new CustomAttribute(ass.MainModule.Import(typeof(MaidFiddlerPatchedAttribute).GetConstructor(new[]
                    {
                        typeof(uint)
                    })));
            attr.ConstructorArguments.Add(new CustomAttributeArgument(ass.MainModule.Import(typeof(uint)),
                                                                      uint.Parse(Version.Replace(".", ""))));

            CustomAttribute attr2 =
                    new CustomAttribute(ass.MainModule.Import(typeof(MaidFiddlerPatcherAttribute).GetConstructor(new[]
                    {
                        typeof(uint)
                    })));
            attr2.ConstructorArguments.Add(new CustomAttributeArgument(ass.MainModule.Import(typeof(uint)),
                                                                       (uint) PatcherType.Sybaris));

            ass.MainModule.GetType("Maid").CustomAttributes.Add(attr);
            ass.MainModule.GetType("Maid").CustomAttributes.Add(attr2);
        }
    }
}