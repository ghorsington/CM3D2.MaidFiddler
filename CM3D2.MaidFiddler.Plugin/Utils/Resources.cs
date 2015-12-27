using System.Drawing;
using System.IO;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public static class Resources
    {
        static Resources()
        {
            InitThumbnail();
        }

        public static Image DefaultThumbnail { get; private set; }

        public static void InitThumbnail()
        {
            Debugger.Assert(
            () =>
            {
                Debugger.WriteLine("Loading generic maid thumbnail...");
                string thumbPath = Path.Combine(MaidFiddler.DATA_PATH, @"MaidFiddler\Images\no_thumbnail.png");
                Debugger.WriteLine($"Path: {thumbPath}");
                try
                {
                    DefaultThumbnail = File.Exists(thumbPath) ? Image.FromFile(thumbPath) : null;
                }
                catch (FileNotFoundException)
                {
                    Debugger.WriteLine(LogLevel.Error, "Could not find the default thumbnail!");
                    DefaultThumbnail = null;
                }
            },
            "Failed to load the generic maid thumbnail");
        }
    }
}