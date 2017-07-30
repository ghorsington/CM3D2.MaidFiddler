using System.IO;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Net;
using CM3D2.MaidFiddler.Plugin.Net.RPC;
using CM3D2.MaidFiddler.Plugin.Utils;
using UnityInjector;
using UnityInjector.Attributes;

namespace CM3D2.MaidFiddler.Plugin
{
    [PluginName("Maid Fiddler")]
    [PluginVersion(Version)]
    public class MaidFiddler : PluginBase
    {
        public const string Version = "1.0.0.0";
        public const string VersionTag = "v1.0.0.0";

        public static string DataPathReal { get; private set; }

        public static bool RunningOnSybaris
        {
            get
            {
                object[] attributes = typeof(Maid).GetCustomAttributes(typeof(MaidFiddlerPatcherAttribute), false);
                return attributes.Length == 1
                       && (PatcherType) ((MaidFiddlerPatcherAttribute) attributes[0]).PatcherType
                       == PatcherType.Sybaris;
            }
        }

        private Connection Connection { get; set; }

        public void Awake()
        {
            DontDestroyOnLoad(this);

            //Debugger.ErrorOccured += (exception, message) => FiddlerUtils.ThrowErrorMessage(exception, message, this);

            DataPathReal = RunningOnSybaris
                               ? Path.Combine(DataPath, "..\\..\\Sybaris\\Plugins\\UnityInjector\\Config\\")
                               : DataPath;

            Debugger.WriteLine(LogLevel.Info, $"Data path: {DataPathReal}");
            //LoadConfig();

            /*
            if (!FiddlerUtils.CheckPatcherVersion())
            {
                Destroy(this);
                return;
            }
            */

            // FiddlerHooks.SaveLoadedEvent += OnSaveLoaded;

            Debugger.WriteLine(LogLevel.Info, "Starting data connection");
            RpcManager manager = new RpcManager();
            Connection = new Connection(manager);
            Connection.Start();
            //GuiThread = new Thread(LoadGUI);
            //GuiThread.Start();

            Debugger.WriteLine($"MaidFiddler {Version} loaded!");
        }

        public void Dispose() { }

        public void LateUpdate() { }

        public void OnDestroy()
        {
            Connection.Stop();
            Debugger.WriteLine("Thread suspended");
        }

        public void OnSaveLoaded(int saveNo)
        {
            Debugger.WriteLine(LogLevel.Info, $"Level loading! Save no. {saveNo}");
        }

        [RpcCall]
        public static void TestMethod() { }

        public void Update() { }

        private void LoadConfig() { }
    }
}