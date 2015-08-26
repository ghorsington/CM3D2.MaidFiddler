using System;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        private bool creatingHandle;
        private bool destroyGUI;

        public void Close(bool exit)
        {
            destroyGUI = exit;
            Close();
        }

        public void InvokeAsync(Delegate method, params object[] args)
        {
            if (!IsHandleCreated && !creatingHandle)
            {
                creatingHandle = true;
                Debugger.WriteLine(LogLevel.Info, "GUI: No handle! Creating one...");
                CreateHandle();
                creatingHandle = false;
                InvokeAsync(method, args);
            } else if (creatingHandle)
            {
                InvokeAsync(method, args);
                return;
            }

            BeginInvoke(method, args);
        }
    }
}