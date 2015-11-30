using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    [PluginName("Maid Fiddler"), PluginVersion(VERSION)]
    public class MaidFiddler : PluginBase, IDisposable
    {
        public const string VERSION = "BETA 0.7";
        public const uint SUPPORTED_PATCH_MAX = 1000;
        public const uint SUPPORTED_PATCH_MIN = 1000;
        private const bool DEFAULT_USE_JAPANESE_NAME_STYLE = false;
        private const MaidOrderStyle DEFAULT_ORDER_STYLE = MaidOrderStyle.GUID;
        private const MaidOrderDirection DEFAULT_ORDER_DIRECTION = Plugin.MaidOrderDirection.Ascending;
        private static readonly KeyCode[] DEFAULT_KEY_CODE = {KeyCode.KeypadEnter, KeyCode.Keypad0};

        private static readonly MaidFiddlerGUI.MaidCompareMethod[] COMPARE_METHODS =
        {
            MaidFiddlerGUI.MaidCompareID,
            MaidFiddlerGUI.MaidCompareCreateTime,
            MaidFiddlerGUI.MaidCompareFirstLastName,
            MaidFiddlerGUI.MaidCompareLastFirstName
        };

        private KeyHelper keyCreateGUI;
        public static string DATA_PATH { get; private set; }
        public static MaidFiddlerGUI Gui { get; set; }
        public static Thread GuiThread { get; set; }
        public static MaidFiddlerGUI.MaidCompareMethod MaidCompare { get; private set; }
        public static int MaidOrderDirection { get; private set; }
        public static bool UseJapaneseNameStyle { get; private set; }

        public void Dispose()
        {
            Gui?.Dispose();
        }

        public void Awake()
        {
            if (!FiddlerUtils.CheckPatcherVersion())
            {
                Destroy(this);
                return;
            }

            DATA_PATH = DataPath;
            LoadConfig();

            GuiThread = new Thread(LoadGUI);

            FiddlerHooks.SaveLoadedEvent += OnSaveLoaded;
            Debugger.WriteLine($"MaidFiddler {VERSION} loaded!");
        }

        private static string GetKeyCombo(IList<KeyCode> keys)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < keys.Count; i++)
            {
                sb.Append(EnumHelper.GetName(keys[i]));
                if (i != keys.Count - 1)
                    sb.Append('+');
            }

            return sb.ToString();
        }

        public void LateUpdate()
        {
            Gui?.UpdateSelectedMaidValues();
            Gui?.UpdatePlayerValues();
        }

        private void LoadConfig()
        {
            Debugger.WriteLine(LogLevel.Info, "Loading launching key combination...");
            List<KeyCode> keys = new List<KeyCode>();
            IniKey value = Preferences["Keys"]["StartGUIKey"];
            if (value.Value == null || value.Value.Trim() == string.Empty)
            {
                value.Value = GetKeyCombo(DEFAULT_KEY_CODE);
                keys.AddRange(DEFAULT_KEY_CODE);
                SaveConfig();
            }
            else
            {
                try
                {
                    string[] keyCodes = value.Value.Split(new[] {'+'}, StringSplitOptions.RemoveEmptyEntries);
                    if (keyCodes.Length == 0)
                        throw new Exception();
                    foreach (KeyCode kc in
                    keyCodes.Select(keyCode => (KeyCode) Enum.Parse(typeof (KeyCode), keyCode.Trim(), true))
                            .Where(kc => !keys.Contains(kc)))
                        keys.Add(kc);
                    if (keyCodes.Length != keys.Count)
                    {
                        value.Value = GetKeyCombo(keys);
                        SaveConfig();
                    }
                }
                catch (Exception)
                {
                    Debugger.WriteLine(LogLevel.Warning, "Failed to parse given key combo. Using default combination");
                    value.Value = GetKeyCombo(DEFAULT_KEY_CODE);
                    keys.AddRange(DEFAULT_KEY_CODE);
                    SaveConfig();
                }
            }
            keyCreateGUI = new KeyHelper(keys.ToArray());
            Debugger.WriteLine(LogLevel.Info, $"Loaded {keys.Count} long key combo: {GetKeyCombo(keys)}");

            Debugger.WriteLine(LogLevel.Info, "Loading name style info...");
            value = Preferences["GUI"]["UseJapaneseNameStyle"];
            bool useJapNameStyle;
            if (value.Value == null || value.Value.Trim() == string.Empty
                || !bool.TryParse(value.Value, out useJapNameStyle))
            {
                Debugger.WriteLine(LogLevel.Warning, "Failed to get name style info. Setting do default...");
                value.Value = DEFAULT_USE_JAPANESE_NAME_STYLE.ToString();
                UseJapaneseNameStyle = DEFAULT_USE_JAPANESE_NAME_STYLE;
                SaveConfig();
            }
            else
                UseJapaneseNameStyle = useJapNameStyle;

            Debugger.WriteLine(LogLevel.Info, $"Using Japanese name style: {UseJapaneseNameStyle}");


            Debugger.WriteLine(LogLevel.Info, "Loading order style info...");
            value = Preferences["GUI"]["OrderStyle"];
            MaidOrderStyle orderStyle;
            string v;
            if (value.Value == null || (v = value.Value.Trim()) == string.Empty
                || !EnumHelper.TryParse(v, out orderStyle, true))
            {
                Debugger.WriteLine(LogLevel.Warning, "Failed to get order style. Setting do default...");
                value.Value = EnumHelper.GetName(DEFAULT_ORDER_STYLE);
                MaidCompare = COMPARE_METHODS[(int) DEFAULT_ORDER_STYLE];
                SaveConfig();
            }
            else
                MaidCompare = COMPARE_METHODS[(int) orderStyle];

            Debugger.WriteLine(LogLevel.Info, $"Sorting maids by {MaidCompare.Method.Name} method");


            Debugger.WriteLine(LogLevel.Info, "Loading order direction info...");
            value = Preferences["GUI"]["OrderDirection"];
            MaidOrderDirection orderDirection;
            if (value.Value == null || (v = value.Value.Trim()) == string.Empty
                || !EnumHelper.TryParse(v, out orderDirection, true))
            {
                Debugger.WriteLine(LogLevel.Warning, "Failed to get order direction. Setting do default...");
                value.Value = EnumHelper.GetName(DEFAULT_ORDER_DIRECTION);
                MaidOrderDirection = (int) DEFAULT_ORDER_DIRECTION;
                SaveConfig();
            }
            else
                MaidOrderDirection = (int) orderDirection;

            Debugger.WriteLine(
            LogLevel.Info,
            $"Sorting maids in {EnumHelper.GetName((MaidOrderDirection) MaidOrderDirection)} direction");
        }

        public void LoadGUI()
        {
            try
            {
                if (Gui == null)
                    Gui = new MaidFiddlerGUI();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(Gui);
            }
            catch (Exception e)
            {
                FiddlerUtils.ThrowErrorMessage(e, "Generic error");
            }
        }

        public void OnDestroy()
        {
            if (Gui == null)
                return;
            Debugger.WriteLine("Closing GUI...");
            Gui.Close(true);
            GuiThread.Abort();
            Gui = null;
        }

        public void OnSaveLoaded(int saveNo)
        {
            Debugger.WriteLine(LogLevel.Info, $"Level loading! Save no. {saveNo}");
            Gui?.ReloadMaids();
            Gui?.ReloadPlayer();
        }

        public void OpenGUI()
        {
            if (GuiThread.ThreadState != ThreadState.Running)
                GuiThread.Start();
            else
                Gui?.Show();
        }

        public void Update()
        {
            keyCreateGUI.Update();

            if (keyCreateGUI.HasBeenPressed())
                OpenGUI();
            Gui?.UpdateMaids();
        }
    }

    public enum MaidOrderStyle
    {
        GUID = 0,
        CreationTime = 1,
        FirstName_LastName = 2,
        LastName_FirstName = 3
    }

    public enum MaidOrderDirection
    {
        Descending = -1,
        Ascending = 1
    }
}