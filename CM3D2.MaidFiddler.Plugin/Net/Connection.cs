namespace CM3D2.MaidFiddler.Plugin.Net
{
    public class Connection
    {
        private static Connection connection;

        private Connection()
        {
        }

        public static Connection Instance => connection ?? (connection = new Connection());
    }
}