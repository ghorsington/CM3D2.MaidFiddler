using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CM3D2.MaidFiddler.Plugin.Net.RPC;
using CM3D2.MaidFiddler.Plugin.Utils;

namespace CM3D2.MaidFiddler.Plugin.Net
{
    public class Connection
    {
        public const int Port = 6969;

        private Thread receiverThread;

        public Connection(RpcManager rpcManager)
        {
            RpcManager = rpcManager;
            TcpListener = new TcpListener(IPAddress.Loopback, Port);
        }

        private Socket Client { get; set; }

        private RpcManager RpcManager { get; }

        private TcpListener TcpListener { get; }

        public void Start()
        {
            RpcManager.Start();
            StartListening();
        }

        public void Stop()
        {
            receiverThread?.Abort();
            Client?.Shutdown(SocketShutdown.Both);
            Client?.Close();
            TcpListener.Stop();
            RpcManager.Stop();
        }

        private void ConnectionLoop()
        {
            Debugger.WriteLine(LogLevel.Info, $"Started waiting for connections on {TcpListener.LocalEndpoint}");
            Client = TcpListener.AcceptSocket();
            TcpListener.Stop();
            Debugger.WriteLine(LogLevel.Info, $"Got connection from {Client.RemoteEndPoint}");
            Debugger.WriteLine(LogLevel.Info, "Beggining listening for data...");

            NetworkStream ns = new NetworkStream(Client);
            StreamReader sr = new StreamReader(ns);

            StringBuilder buffer = new StringBuilder();

            int bracesCounter = -2;
            while (true)
            {
                int nextChar = sr.Read();
                if (nextChar == -1)
                {
                    Debugger.WriteLine(LogLevel.Info, "Connection was terminated! Beginning listening again!");
                    sr.Dispose();
                    ns.Dispose();
                    StartListening(true);
                    return;
                }

                char readChar = (char) nextChar;

                switch (readChar)
                {
                    case '{':
                        if (bracesCounter < 0)
                            bracesCounter = 0;
                        bracesCounter++;
                        break;
                    case '}':
                        bracesCounter--;
                        break;
                    default: break;
                }

                if (bracesCounter < 0)
                {
                    if (bracesCounter != -2)
                    {
                        Debugger.WriteLine(LogLevel.Warning, "Got invalid JSON message. Ignoring...");
                        buffer.Length = 0;
                        bracesCounter = -2;
                    }
                    continue;
                }

                buffer.Append(readChar);

                if (bracesCounter == 0)
                {
                    RpcManager.ParseMessage(buffer.ToString());
                    buffer.Length = 0;
                    bracesCounter = -2;
                }
            }
        }

        private void StartListening(bool restart = false)
        {
            if (restart)
            {
                Client.Shutdown(SocketShutdown.Both);
                Client.Close();
            }
            receiverThread = new Thread(ConnectionLoop);
            Client = null;
            TcpListener.Start();
            receiverThread.Start();
        }
    }
}