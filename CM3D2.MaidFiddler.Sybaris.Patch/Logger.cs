using System;
using System.Diagnostics;

namespace CM3D2.MaidFiddler.Sybaris.Patcher
{
    public static class Logger
    {
        [Conditional("DEBUG")]
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}