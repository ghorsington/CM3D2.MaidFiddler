using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CM3D2.MaidFiddler.Plugin.Net.RPC;
using CM3D2.MaidFiddler.Plugin.Utils;
using JsonFx.Json;

namespace CM3D2.MaidFiddler.Plugin.Net
{
    public class Connection
    {
        private struct ConnectionState
        {
            public const int BufferSize = 1024;

            public Socket Socket { get; }

            public byte[] IncomingDataBuffer { get; }

            public StringBuilder IncomingData { get; }

            public Connection Connection { get; }

            public ConnectionState(Connection connection)
            {
                IncomingDataBuffer = new byte[BufferSize];
                IncomingData = new StringBuilder();
                Connection = connection;
                Socket = Connection.Client;
            }
        }
        
        public const int Port = 6969;

        public Connection(RpcManager rpcManager)
        {
            RpcManager = rpcManager;
            TcpListener = new TcpListener(IPAddress.Loopback, Port);
        }

        private TcpListener TcpListener { get; }

        private Socket Client { get; set; }

        private RpcManager RpcManager { get; }

        public void Start()
        {
            Client = null;
            TcpListener.Start();
            TcpListener.BeginAcceptSocket(OnClientAccepted, this);
            Debugger.WriteLine(LogLevel.Info,
                               $"Started waiting for connections on {TcpListener.LocalEndpoint} on thread {Thread.CurrentThread.ManagedThreadId}");
        }

        public void Stop()
        {
            Client?.Close();
            TcpListener.Stop();
        }

        private void OnClientAccepted(IAsyncResult ar)
        {
            Debugger.WriteLine(LogLevel.Info, $"Got connection! On thread: {Thread.CurrentThread.ManagedThreadId}");
            Connection connection = (Connection) ar.AsyncState;
            connection.TcpListener.Stop();

            connection.Client = connection.TcpListener.EndAcceptSocket(ar);
            ConnectionState state = new ConnectionState(connection);
            Debugger.WriteLine(LogLevel.Info, $"Accepted connection from {connection.Client.RemoteEndPoint}!");
            Debugger.WriteLine(LogLevel.Info, "Beginning listening for data...");

            state.Socket.BeginReceive(state.IncomingDataBuffer,
                                           0,
                                           ConnectionState.BufferSize,
                                           SocketFlags.None,
                                           OnDataReceived,
                                           state);
        }

        private void OnDataReceived(IAsyncResult ar)
        {
            Debugger.WriteLine(LogLevel.Info, $"Got data on thread {Thread.CurrentThread.ManagedThreadId}");
            ConnectionState state = (ConnectionState) ar.AsyncState;
            Socket socket = state.Socket;

            int dataRead = state.Socket.EndReceive(ar);

            if (dataRead == 0)
            {
                Debugger.WriteLine(LogLevel.Info, "Connection was terminated! Beginning listening again!");
                socket.Close();
                state.Connection.Start();
                return;
            }

            Debugger.WriteLine(LogLevel.Info, $"Data read: {dataRead}. Available data: {socket.Available}");
            if (dataRead > 0)
                state.IncomingData.Append(Encoding.UTF8.GetString(state.IncomingDataBuffer, 0, dataRead));
            if (socket.Available <= 0 && state.IncomingData.Length > 0)
            {
                Debugger.WriteLine(LogLevel.Info, $"Got message: {state.IncomingData}");
                RpcManager.ParseMessageAsync(state.IncomingData.ToString());
                state.IncomingData.Length = 0;
            }

            Debugger.WriteLine(LogLevel.Info, "Beginning listening for more data...");
            socket.BeginReceive(state.IncomingDataBuffer,
                                      0,
                                      ConnectionState.BufferSize,
                                      SocketFlags.None,
                                      OnDataReceived,
                                      state);
        }
    }
}