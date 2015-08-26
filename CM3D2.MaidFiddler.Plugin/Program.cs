using System;
using System.Windows.Forms;
using CM3D2.MaidFiddler.Plugin.Gui;

namespace CM3D2.MaidFiddler.Plugin
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MaidFiddlerGUI());
        }
    }
}