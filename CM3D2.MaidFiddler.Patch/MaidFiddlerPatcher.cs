using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Patch.PatchJob;
using Mono.Cecil;
using Mono.Cecil.Inject;
using ReiPatcher;
using ReiPatcher.Patch;

namespace CM3D2.MaidFiddler.Patch
{
    public class MaidFiddlerPatcher : PatchBase
    {
        private const string Tag = "CM3D2_MAID_FIDDLER";
        public override string Name => "MaidFiddler Patcher";
        private AssemblyDefinition FiddlerAssembly { get; set; }

        public override bool CanPatch(PatcherArguments args)
        {
            return args.Assembly.Name.Name == "Assembly-CSharp" && !HasAttribute(args.Assembly, Tag);
        }

        public override void Patch(PatcherArguments args)
        {
            TypeDefinition maidParam = args.Assembly.MainModule.GetType("MaidParam");
            TypeDefinition playerParam = args.Assembly.MainModule.GetType("PlayerParam");
            TypeDefinition status = args.Assembly.MainModule.GetType("param.Status");

            IEnumerable<PatchJobCollection> jobs = GetType()
                    .Assembly.GetTypes()
                    .Where(type => type.Namespace == "CM3D2.MaidFiddler.Patch.Jobs" &&
                                   !type.IsAbstract &&
                                   typeof(PatchJobCollection).IsAssignableFrom(type))
                    .Select(Activator.CreateInstance)
                    .Cast<PatchJobCollection>();

            Console.WriteLine("Patching...");
            foreach (PatchJobCollection job in jobs)
            {
                job.Initialize(args.Assembly, FiddlerAssembly);
                job.Patch();
            }

            Console.WriteLine("Done. Patching class members:\n");
            maidParam.ChangeAccess("status_");
            playerParam.ChangeAccess("status_");
            status.ChangeAccess("kInitMaidPoint");

            SetPatchedAttribute(args.Assembly, Tag);
            SetCustomPatchedAttribute(args.Assembly);
            Console.WriteLine("\nPatching complete.");
        }

        public override void PrePatch()
        {
            Console.WriteLine("Requesting assembly");
            RPConfig.RequestAssembly("Assembly-CSharp.dll");
            Console.WriteLine("Loading assembly");
            FiddlerAssembly = AssemblyLoader.LoadAssembly(Path.Combine(AssembliesDir, "CM3D2.MaidFiddler.Hook.dll"));
        }

        private bool HasAttribute(AssemblyDefinition assembly, string tag)
        {
            return GetPatchedAttributes(assembly).Any(ass => ass.Info == tag);
        }

        private void SetCustomPatchedAttribute(AssemblyDefinition ass)
        {
            CustomAttribute attr =
                    new CustomAttribute(
                        ass.MainModule.Import(
                            typeof(MaidFiddlerPatchedAttribute).GetConstructor(new[] {typeof(uint)})));
            attr.ConstructorArguments.Add(
                new CustomAttributeArgument(ass.MainModule.Import(typeof(uint)),
                                            uint.Parse(Version.Replace(".", ""))));

            CustomAttribute attr2 =
                    new CustomAttribute(
                        ass.MainModule.Import(
                            typeof(MaidFiddlerPatcherAttribute).GetConstructor(new[] {typeof(uint)})));
            attr2.ConstructorArguments.Add(
                new CustomAttributeArgument(ass.MainModule.Import(typeof(uint)), (uint) PatcherType.ReiPatcher));

            ass.MainModule.GetType("Maid").CustomAttributes.Add(attr);
            ass.MainModule.GetType("Maid").CustomAttributes.Add(attr2);
        }
    }
}