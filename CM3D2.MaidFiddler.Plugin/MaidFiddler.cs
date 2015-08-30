using System;
using System.Threading;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Gui;
using CM3D2.MaidFiddler.Plugin.Utils;
using ExIni;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;
using Application = System.Windows.Forms.Application;

namespace CM3D2.MaidFiddler.Plugin
{
    [PluginName("Maid Fiddler"), PluginVersion("BETA 0.1")]
    public class MaidFiddler : PluginBase, IDisposable
    {
        private const KeyCode DEFAULT_KEY_CODE = KeyCode.N;
        private MaidFiddlerGUI gui;
        private Thread guiThread;
        private KeyCode keyCode;
        private KeyHelper keyCreateGUI;
        public static string DATA_PATH { get; private set; }

        public void Dispose()
        {
            gui?.Dispose();
        }

        public void Awake()
        {
            DATA_PATH = DataPath;
            LoadConfig();
            keyCreateGUI = new KeyHelper(keyCode);
            guiThread = new Thread(LoadGUI);

            FiddlerHooks.SaveLoadedEvent += OnSaveLoaded;
            Debugger.WriteLine("MaidFiddler loaded!");
        }

        public void LateUpdate()
        {
            gui?.UpdateSelectedMaidValues();
            gui?.UpdatePlayerValues();
        }

        private void LoadConfig()
        {
            IniKey value = Preferences["Keys"]["StartGUIKey"];
            if (value.Value == null)
            {
                value.Value = EnumHelper.GetName(DEFAULT_KEY_CODE);
                keyCode = DEFAULT_KEY_CODE;
                SaveConfig();
            }
            else
            {
                try
                {
                    keyCode = (KeyCode) Enum.Parse(typeof (KeyCode), value.Value, true);
                }
                catch (Exception)
                {
                    value.Value = EnumHelper.GetName(DEFAULT_KEY_CODE);
                    keyCode = DEFAULT_KEY_CODE;
                    SaveConfig();
                }
            }
        }

        public void LoadGUI()
        {
            try
            {
                if (gui == null)
                    gui = new MaidFiddlerGUI();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(gui);
            }
            catch (Exception e)
            {
                ErrorLog.ThrowErrorMessage(e);
            }
        }

        public void OnDestroy()
        {
            if (gui == null)
                return;
            Debugger.WriteLine("Closing GUI...");
            gui.Close(true);
        }

        public void OnSaveLoaded(int saveNo)
        {
            Debugger.WriteLine(LogLevel.Info, $"Level loading! Save no. {saveNo}");
            gui?.ReloadMaids();
            gui?.ReloadPlayer();
        }

        public void OpenGUI()
        {
            if (guiThread.ThreadState != ThreadState.Running)
                guiThread.Start();
            else
                gui?.Show();
        }

        public void Update()
        {
            keyCreateGUI.Update();

            if (keyCreateGUI.HasBeenPressed())
                OpenGUI();
            gui?.UpdateMaids();
        }
    }
}