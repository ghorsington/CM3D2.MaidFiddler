using System;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Utils;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class LoadingBarGUI : Form
    {
        private readonly Action<LoadingBarGUI> action;

        public LoadingBarGUI(string title, string text, bool interval, Action<LoadingBarGUI> action)
        {
            InitializeComponent();
            ControlBox = false;
            Text = title;
            TextLabel.Text = text;
            this.action = action;
            Shown += OnShown;

            if (interval)
            {
                Timer = new Timer();
                Timer.Interval = 50;
                Timer.Tick += (o, args) =>
                {
                    if (ProgressBar.Value >= ProgressBar.Maximum) ProgressBar.Value = ProgressBar.Minimum;
                    ProgressBar.PerformStep();
                };
            }
            else Timer = null;
        }

        public ProgressBar ProgressBar { get; private set; }
        public Label TextLabel { get; private set; }
        public Timer Timer { get; }

        private void OnShown(object sender, EventArgs e)
        {
            Debugger.WriteLine(LogLevel.Info, $"Progress bar style: {ProgressBar.Style}");
            action?.Invoke(this);
        }
    }
}