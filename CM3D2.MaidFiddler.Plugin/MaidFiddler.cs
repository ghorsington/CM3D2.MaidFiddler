using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
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
    public class MaidFiddler : PluginBase
    {
        public const string CONTRIBUTORS = "denikson";
        public const string VERSION = "BETA 0.11.3";
        public const string VERSION_TAG = "Beta-0.11.3";
        public const string PROJECT_PAGE = "https://github.com/denikson/CM3D2.MaidFiddler";
        public const string RESOURCE_URL = "https://raw.githubusercontent.com/denikson/CM3D2.MaidFiddler/master";

        public const string RELEASES_LATEST_REQUEST_URL =
        "https://api.github.com/repos/denikson/CM3D2.MaidFiddler/releases/latest";

        public const string RELEASES_URL = "https://www.github.com/denikson/CM3D2.MaidFiddler/releases";
        public const int SUPPORTED_MIN_CM3D2_VERSION = 109;
        public const uint SUPPORTED_PATCH_MAX = 1310;
        public const uint SUPPORTED_PATCH_MIN = 1310;
        private const bool DEFAULT_USE_JAPANESE_NAME_STYLE = false;
        private const bool DEFAULT_OPEN_ON_STARTUP = false;
        private const bool DEFAULT_CHECK_FOR_UPDATES = true;
        private const MaidOrderDirection DEFAULT_ORDER_DIRECTION = Plugin.MaidOrderDirection.Ascending;
        private const string DEFAULT_LANGUAGE_FILE = "ENG";
        private static readonly KeyCode[] DEFAULT_KEY_CODE = {KeyCode.A};
        private readonly List<MaidOrderStyle> DEFAULT_ORDER_STYLES = new List<MaidOrderStyle> {MaidOrderStyle.GUID};
        public MaidFiddlerGUI.MaidCompareMethod[] COMPARE_METHODS;
        private bool isUpdatePromptShowed;
        private KeyHelper keyCreateGUI;

        public bool CFGCheckForUpdates
        {
            get
            {
                IniKey value = Preferences["GUI"]["CheckForUpdates"];
                bool checkForUpdates = DEFAULT_CHECK_FOR_UPDATES;
                if (!string.IsNullOrEmpty(value.Value) && bool.TryParse(value.Value, out checkForUpdates))
                    return checkForUpdates;
                Debugger.WriteLine(LogLevel.Warning, "Failed to get CheckForUpdates value. Setting to default...");
                value.Value = DEFAULT_CHECK_FOR_UPDATES.ToString();
                SaveConfig();
                return checkForUpdates;
            }
            set
            {
                Preferences["GUI"]["CheckForUpdates"].Value = value.ToString();
                SaveConfig();
            }
        }

        public bool CFGOpenOnStartup
        {
            get
            {
                IniKey value = Preferences["GUI"]["OpenOnStartup"];
                bool openOnStartup = DEFAULT_OPEN_ON_STARTUP;
                if (!string.IsNullOrEmpty(value.Value) && bool.TryParse(value.Value, out openOnStartup))
                    return openOnStartup;
                Debugger.WriteLine(LogLevel.Warning, "Failed to get OpenOnStartup value. Setting to default...");
                value.Value = DEFAULT_OPEN_ON_STARTUP.ToString();
                SaveConfig();
                return openOnStartup;
            }
            set
            {
                Preferences["GUI"]["OpenOnStartup"].Value = value.ToString();
                SaveConfig();
            }
        }

        public MaidOrderDirection CFGOrderDirection
        {
            get
            {
                IniKey value = Preferences["GUI"]["OrderDirection"];
                MaidOrderDirection orderDirection = DEFAULT_ORDER_DIRECTION;
                if (!string.IsNullOrEmpty(value.Value) && EnumHelper.TryParse(value.Value, out orderDirection, true))
                    return orderDirection;
                Debugger.WriteLine(LogLevel.Warning, "Failed to get order direction. Setting do default...");
                value.Value = EnumHelper.GetName(DEFAULT_ORDER_DIRECTION);
                SaveConfig();

                return orderDirection;
            }

            set
            {
                Preferences["GUI"]["OrderDirection"].Value = value.ToString();
                SaveConfig();
                MaidOrderDirection = (int) value;
            }
        }

        public List<MaidOrderStyle> CFGOrderStyle
        {
            get
            {
                IniKey value = Preferences["GUI"]["OrderStyle"];
                List<MaidOrderStyle> orderStyles;
                if (string.IsNullOrEmpty(value.Value))
                {
                    Debugger.WriteLine(LogLevel.Warning, "Failed to get order style. Setting do default...");
                    value.Value = EnumHelper.EnumsToString(DEFAULT_ORDER_STYLES, '|');
                    orderStyles = DEFAULT_ORDER_STYLES;
                    SaveConfig();
                }
                else
                {
                    orderStyles = EnumHelper.ParseEnums<MaidOrderStyle>(value.Value, '|');
                    if (orderStyles.Count != 0)
                        return orderStyles;
                    Debugger.WriteLine(LogLevel.Warning, "Failed to get order style. Setting do default...");
                    value.Value = EnumHelper.EnumsToString(DEFAULT_ORDER_STYLES, '|');
                    orderStyles = DEFAULT_ORDER_STYLES;
                    SaveConfig();
                }
                return orderStyles;
            }
            set
            {
                MaidCompareMethods = value.Select(o => COMPARE_METHODS[(int) o]).ToArray();
                Preferences["GUI"]["OrderStyle"].Value = EnumHelper.EnumsToString(value, '|');
                SaveConfig();
            }
        }

        public string CFGSelectedDefaultLanguage
        {
            get
            {
                string result = Preferences["GUI"]["DefaultTranslation"].Value;
                if (!string.IsNullOrEmpty(result) && Translation.Exists(result))
                    return result;
                Preferences["GUI"]["DefaultTranslation"].Value = result = DEFAULT_LANGUAGE_FILE;
                SaveConfig();
                return result;
            }
            set
            {
                if (value != null && (value = value.Trim()) != string.Empty && Translation.Exists(value))
                    Preferences["GUI"]["DefaultTranslation"].Value = value;
                else
                    Preferences["GUI"]["DefaultTranslation"].Value = DEFAULT_LANGUAGE_FILE;
                SaveConfig();
            }
        }

        public List<KeyCode> CFGStartGUIKey
        {
            get
            {
                List<KeyCode> keys = new List<KeyCode>();
                IniKey value = Preferences["Keys"]["StartGUIKey"];
                if (string.IsNullOrEmpty(value.Value))
                {
                    value.Value = EnumHelper.EnumsToString(DEFAULT_KEY_CODE, '+');
                    keys.AddRange(DEFAULT_KEY_CODE);
                    SaveConfig();
                }
                else
                {
                    keys = EnumHelper.ParseEnums<KeyCode>(value.Value, '+');

                    if (keys.Count != 0)
                        return keys;
                    Debugger.WriteLine(LogLevel.Warning, "Failed to parse given key combo. Using default combination");
                    keys = DEFAULT_KEY_CODE.ToList();
                    value.Value = EnumHelper.EnumsToString(keys, '+');
                    SaveConfig();
                }
                return keys;
            }

            set
            {
                Preferences["Keys"]["StartGUIKey"].Value = EnumHelper.EnumsToString(value, '+');
                keyCreateGUI.Keys = value.ToArray();
                SaveConfig();
            }
        }

        public bool CFGUseJapaneseNameStyle
        {
            get
            {
                IniKey value = Preferences["GUI"]["UseJapaneseNameStyle"];
                bool useJapNameStyle = DEFAULT_USE_JAPANESE_NAME_STYLE;
                if (!string.IsNullOrEmpty(value.Value) && bool.TryParse(value.Value, out useJapNameStyle))
                    return useJapNameStyle;
                Debugger.WriteLine(LogLevel.Warning, "Failed to get UseJapaneseNameStyle value. Setting to default...");
                value.Value = DEFAULT_USE_JAPANESE_NAME_STYLE.ToString();
                SaveConfig();
                return useJapNameStyle;
            }
            set
            {
                UseJapaneseNameStyle = value;
                Preferences["GUI"]["UseJapaneseNameStyle"].Value = value.ToString();
                SaveConfig();
            }
        }

        public static string DATA_PATH { get; private set; }
        public MaidFiddlerGUI Gui { get; set; }
        public Thread GuiThread { get; set; }
        public MaidFiddlerGUI.MaidCompareMethod[] MaidCompareMethods { get; private set; }
        public int MaidOrderDirection { get; private set; }

        public static bool RunningOnSybaris
        {
            get
            {
                object[] attributes = typeof (Maid).GetCustomAttributes(typeof (MaidFiddlerPatcherAttribute), false);
                return attributes.Length == 1
                       && (PatcherType) ((MaidFiddlerPatcherAttribute) attributes[0]).PatcherType == PatcherType.Sybaris;
            }
        }

        public bool UseJapaneseNameStyle { get; private set; }

        public void Dispose()
        {
            Gui?.Dispose();
        }

        public void Awake()
        {
            DontDestroyOnLoad(this);

            ServicePointManager.ServerCertificateValidationCallback += FiddlerUtils.RemoteCertificateValidationCallback;
            Debugger.ErrorOccured += (exception, message) => FiddlerUtils.ThrowErrorMessage(exception, message, this);

            COMPARE_METHODS = new MaidFiddlerGUI.MaidCompareMethod[]
            {
                MaidCompareID,
                MaidCompareCreateTime,
                MaidCompareFirstName,
                MaidCompareLastName,
                MaidCompareEmployedDay
            };

            DATA_PATH = RunningOnSybaris
                        ? Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Config") : DataPath;

            Debugger.WriteLine(LogLevel.Info, $"Data path: {DATA_PATH}");
            LoadConfig();

            if (!FiddlerUtils.CheckPatcherVersion())
            {
                Destroy(this);
                return;
            }

            FiddlerHooks.SaveLoadedEvent += OnSaveLoaded;

            Debugger.WriteLine(LogLevel.Info, "Creating the GUI thread");
            GuiThread = new Thread(LoadGUI);
            GuiThread.Start();

            Debugger.WriteLine($"MaidFiddler {VERSION} loaded!");

            if (CFGCheckForUpdates)
            {
                Thread updateCheckThread = new Thread(FiddlerUtils.RunUpdateChecker);
                updateCheckThread.Start();
            }
            else
            {
                Debugger.WriteLine(LogLevel.Info, "Skipping update checking!");
            }
            
        }

        public void LateUpdate()
        {
            Gui?.DoIfVisible(Gui.UpdateSelectedMaidValues);
            Gui?.DoIfVisible(Gui.UpdatePlayerValues);
        }

        private void LoadConfig()
        {
            Debugger.WriteLine(LogLevel.Info, "Loading launching key combination...");
            keyCreateGUI = new KeyHelper(CFGStartGUIKey.ToArray());
            Debugger.WriteLine(
            LogLevel.Info,
            $"Loaded {keyCreateGUI.Keys.Length} long key combo: {EnumHelper.EnumsToString(keyCreateGUI.Keys, '+')}");

            Debugger.WriteLine(LogLevel.Info, "Loading name style info...");
            UseJapaneseNameStyle = CFGUseJapaneseNameStyle;
            Debugger.WriteLine(LogLevel.Info, $"Using Japanese name style: {UseJapaneseNameStyle}");

            Debugger.WriteLine(LogLevel.Info, "Loading order style info...");
            List<MaidOrderStyle> orderStyles = CFGOrderStyle;
            MaidCompareMethods = orderStyles.Select(o => COMPARE_METHODS[(int) o]).ToArray();
            Debugger.WriteLine(
            LogLevel.Info,
            $"Sorting maids by method order {EnumHelper.EnumsToString(orderStyles, '>')}");


            Debugger.WriteLine(LogLevel.Info, "Loading order direction info...");
            MaidOrderDirection = (int) CFGOrderDirection;
            Debugger.WriteLine(
            LogLevel.Info,
            $"Sorting maids in {EnumHelper.GetName((MaidOrderDirection) MaidOrderDirection)} direction");

            Translation.LoadTranslation(CFGSelectedDefaultLanguage);
        }

        public void LoadGUI()
        {
            try
            {
                Application.SetCompatibleTextRenderingDefault(false);
                if (Gui == null)
                    Gui = new MaidFiddlerGUI(this);
                Application.Run(Gui);
            }
            catch (Exception e)
            {
                FiddlerUtils.ThrowErrorMessage(e, "Generic error", this);
            }
        }

        public void OnDestroy()
        {
            if (Gui == null)
                return;
            Debugger.WriteLine("Closing GUI...");
            Gui.Close(true);
            Gui = null;
            Debugger.WriteLine("GUI closed. Suspending the thread...");
            GuiThread.Abort();
            Debugger.WriteLine("Thread suspended");
        }

        public void OnSaveLoaded(int saveNo)
        {
            Debugger.WriteLine(LogLevel.Info, $"Level loading! Save no. {saveNo}");
            if (!(Gui?.Visible).GetValueOrDefault(true))
                Gui?.UnloadMaids();
            Gui?.DoIfVisible(Gui.ReloadMaids);
            Gui?.DoIfVisible(Gui.ReloadPlayer);
        }

        public void OpenGUI()
        {
            Gui?.Show();
        }

        public void Update()
        {
            if (!isUpdatePromptShowed && FiddlerUtils.UpdatesChecked)
            {
                isUpdatePromptShowed = true;
                if (FiddlerUtils.UpdateInfo.IsAvailable)
                    ShowUpdatePrompt();
            }

            keyCreateGUI.Update();

            if (keyCreateGUI.HasBeenPressed())
                OpenGUI();
            Gui?.DoIfVisible(Gui.UpdateMaids);
        }

        private void ShowUpdatePrompt()
        {
            Debugger.WriteLine(LogLevel.Info, "Latest version is newer than the current one! Showing update prompt!");
            ManualResetEvent mre = new ManualResetEvent(false);
            // ReSharper disable once UseObjectOrCollectionInitializer
            NotifyIcon notification = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                BalloonTipIcon = ToolTipIcon.Info,
                BalloonTipTitle =
                    Translation.IsTranslated("INFO_UPDATE_AVAILABLE_BUBBLE_TITLE")
                    ? string.Format(
                    Translation.GetTranslation("INFO_UPDATE_AVAILABLE_BUBBLE_TITLE"),
                    FiddlerUtils.UpdateInfo.Version)
                    : $"Maid Fiddler version {FiddlerUtils.UpdateInfo.Version} is available!",
                BalloonTipText =
                    Translation.IsTranslated("INFO_UPDATE_AVAILABLE_BUBBLE")
                    ? Translation.GetTranslation("INFO_UPDATE_AVAILABLE_BUBBLE")
                    : "Download the new version from Maid Fiddler GitHub page!"
            };
            notification.BalloonTipClosed += (sender, args) =>
            {
                Debugger.WriteLine(LogLevel.Info, "Closing the notification!");
                mre.Set();
            };
            notification.BalloonTipClicked += (sender, args) =>
            {
                Debugger.WriteLine(LogLevel.Info, "Showing update information!");
                mre.Set();
                string title = Translation.IsTranslated("INFO_UPDATE_AVAILABLE_TITLE")
                               ? string.Format(
                               Translation.GetTranslation("INFO_UPDATE_AVAILABLE_TITLE"),
                               FiddlerUtils.UpdateInfo.Version)
                               : $"Version {FiddlerUtils.UpdateInfo.Version} is available!";
                string text = Translation.IsTranslated("INFO_UPDATE_AVAILABLE")
                              ? string.Format(
                              Translation.GetTranslation("INFO_UPDATE_AVAILABLE"),
                              FiddlerUtils.UpdateInfo.Version,
                              FiddlerUtils.UpdateInfo.Changelog,
                              RELEASES_URL)
                              : $"Maid Fiddler version {FiddlerUtils.UpdateInfo.Version} is available to download!\nUpdate log:\n{FiddlerUtils.UpdateInfo.Changelog}\n\nHead to {RELEASES_URL} to download the latest version.";
                MessageBox.Show(text, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            notification.Visible = true;
            notification.ShowBalloonTip(5000);
            Thread showThread = new Thread(
            () =>
            {
                mre.WaitOne(6000);
                Debugger.WriteLine(LogLevel.Info, "Closing notification icon...");
                notification.Visible = false;
                notification.Dispose();
            });
            showThread.Start();
        }

        public int MaidCompareEmployedDay(Maid x, Maid y)
        {
            return ComputeOrder(x.Param.status.employment_day, y.Param.status.employment_day);
        }

        public int MaidCompareCreateTime(Maid x, Maid y)
        {
            return ComputeOrder(x.Param.status.create_time_num, y.Param.status.create_time_num);
        }

        private int ComputeOrder<T>(T x, T y) where T : IComparable<T>
        {
            return MaidOrderDirection * x.CompareTo(y);
        }

        public int MaidCompareFirstName(Maid x, Maid y)
        {
            return MaidOrderDirection
                   * string.CompareOrdinal(
                   x.Param.status.first_name.ToUpperInvariant(),
                   y.Param.status.first_name.ToUpperInvariant());
        }

        public int MaidCompareID(Maid x, Maid y)
        {
            return MaidOrderDirection * string.CompareOrdinal(x.Param.status.guid, y.Param.status.guid);
        }

        public int MaidCompareLastName(Maid x, Maid y)
        {
            return MaidOrderDirection
                   * string.CompareOrdinal(
                   x.Param.status.last_name.ToUpperInvariant(),
                   y.Param.status.last_name.ToUpperInvariant());
        }
    }

    public enum MaidOrderStyle
    {
        GUID = 0,
        CreationTime = 1,
        FirstName = 2,
        LastName = 3,
        EmployedDay = 4
    }

    public enum MaidOrderDirection
    {
        Descending = -1,
        Ascending = 1
    }
}