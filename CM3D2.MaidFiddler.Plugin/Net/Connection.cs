using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CM3D2.MaidFiddler.Plugin.Utils;
using JsonFx.Json;

namespace CM3D2.MaidFiddler.Plugin.Net
{
    public class Connection
    {
        private const int BufferSize = 1024;
        public const int Port = 6969;

        public Connection()
        {
            TcpListener = new TcpListener(IPAddress.Loopback, Port);
            IncomingDataBuffer = new byte[BufferSize];
            IncomingData = new StringBuilder();
        }

        private TcpListener TcpListener { get; }

        private Socket Client { get; set; }

        private byte[] IncomingDataBuffer { get; }

        private StringBuilder IncomingData { get; }

        public void Start()
        {
            Debugger.WriteLine(LogLevel.Info,
                               $"Started waiting for connections on {TcpListener.LocalEndpoint} on thread {Thread.CurrentThread.ManagedThreadId}");
            TcpListener.Start();
            TcpListener.BeginAcceptSocket(OnClientAccepted, this);
        }

        public void Stop()
        {
            Client?.Close();
            TcpListener.Stop();
        }

        private void OnClientAccepted(IAsyncResult ar)
        {
            TcpListener.Stop();
            Debugger.WriteLine(LogLevel.Info, $"Got connection! On thread: {Thread.CurrentThread.ManagedThreadId}");
            Connection connection = (Connection) ar.AsyncState;

            connection.Client = connection.TcpListener.EndAcceptSocket(ar);
            Debugger.WriteLine(LogLevel.Info, $"Accepted connection from {connection.Client.RemoteEndPoint}!");
            Debugger.WriteLine(LogLevel.Info, "Beginning listening for data...");

            connection.Client.BeginReceive(connection.IncomingDataBuffer,
                                           0,
                                           BufferSize,
                                           SocketFlags.None,
                                           OnDataReceived,
                                           connection);
        }

        private void OnDataReceived(IAsyncResult ar)
        {
            Debugger.WriteLine(LogLevel.Info, $"Got data on thread {Thread.CurrentThread.ManagedThreadId}");
            Connection connection = (Connection) ar.AsyncState;
            Socket clientSocket = connection.Client;

            int dataRead = clientSocket.EndReceive(ar);

            if (dataRead == 0)
            {
                Debugger.WriteLine(LogLevel.Info, "Connection was terminated! Beginning listening again!");
                clientSocket.Close();
                connection.Client = null;
                connection.Start();
                return;
            }

            Debugger.WriteLine(LogLevel.Info, $"Data read: {dataRead}. Available data: {clientSocket.Available}");
            if (dataRead > 0)
                connection.IncomingData.Append(Encoding.UTF8.GetString(connection.IncomingDataBuffer, 0, dataRead));
            if (clientSocket.Available <= 0 && connection.IncomingData.Length > 0)
            {
                Debugger.WriteLine(LogLevel.Info, $"Got message: {connection.IncomingData}");
                Debugger.WriteLine(LogLevel.Info, "Decoding message");

                try
                {
                    JsonReader reader = new JsonReader(connection.IncomingData);
                    Dictionary<string, object> result = reader.Deserialize<Dictionary<string, object>>();
                    Debugger.WriteLine(LogLevel.Info,
                                       $"Got JSON: {{ {string.Join(",", result.Select(k => k.Key + " : " + k.Value).ToArray())} }}");
                }
                catch (Exception e)
                {
                    Debugger.WriteLine(LogLevel.Error, $"Failed to parse JSON: {e}");
                }
                finally
                {
                    connection.IncomingData.Length = 0;
                }
            }

            Debugger.WriteLine(LogLevel.Info, "Beginning listening for more data...");
            clientSocket.BeginReceive(connection.IncomingDataBuffer,
                                      0,
                                      BufferSize,
                                      SocketFlags.None,
                                      OnDataReceived,
                                      connection);
        }
    }
}