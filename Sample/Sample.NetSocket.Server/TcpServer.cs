using Sample.NetSocket.Utility;
using Sample.NetSocket.Utility.Base;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace Sample.NetSocket.Server
{
    public class TcpServer : TcpBase
    {
        private readonly int localPort;
        public readonly ConcurrentDictionary<string, Socket> Clients;

        public int MaxConnectionSize { get; set; } = 100;

        public TcpServer(int localPort)
            : base()
        {
            this.localPort = localPort;
            Clients = new ConcurrentDictionary<string, Socket>();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public override void Start()
        {
            running = true;
            EndPoint local = new IPEndPoint(IPAddress.Any, localPort);
            socket.Bind(local);
            socket.Listen(MaxConnectionSize);
            DoBeginAccept();
            CheckClients();
        }

        private void DoBeginAccept()
        {
            socket.BeginAccept(AcceptCallback, null);
        }

        private void AcceptCallback(IAsyncResult result)
        {
            Socket remote = socket.EndAccept(result);
            if (running)
            {
                DoBeginAccept();
            }
            (remote.RemoteEndPoint as IPEndPoint).GetIpAndPort(out string remoteIp, out int remotePort);
            string clientKey = $"{remoteIp}:{remotePort}";
            Clients[clientKey] = remote;
            OnConnected(remoteIp, remotePort);
            DoBeginReceive(remote, remoteIp, remotePort);
        }

        private void DoBeginReceive(Socket remote, string remoteIp, int remotePort)
        {
            if (remote == null)
            {
                return;
            }
            string clientKey = $"{remoteIp}:{remotePort}";
            while (running && Clients.ContainsKey(clientKey))
            {
                if (remote.Poll(ReceiveTimeoutMilliseconds * 1000, SelectMode.SelectRead))
                {
                    byte[] buffer = new byte[BufferSize];
                    TcpClientState state = new TcpClientState()
                    {
                        Remote = remote,
                        Buffer = buffer
                    };
                    remote.BeginReceive(buffer, 0, BufferSize, SocketFlags.None, ReceiveAsyncCallback, state);
                    //break;
                }
                else
                {
                    OnReceiveTimeout(remoteIp, remotePort);
                }

                try
                {
                    //if (remote.Poll(ReceiveTimeoutMilliseconds * 1000, SelectMode.SelectRead))
                    //{
                    //    byte[] buffer = new byte[BufferSize];
                    //    TcpClientState state = new TcpClientState()
                    //    {
                    //        Remote = remote,
                    //        Buffer = buffer
                    //    };
                    //    remote.BeginReceive(buffer, 0, BufferSize, SocketFlags.None, ReceiveAsyncCallback, state);
                    //    break;
                    //}
                    //else
                    //{
                    //    OnReceiveTimeout(remoteIp, remotePort);
                    //}
                }
                catch (Exception e)
                {
                    RemoveClient(remoteIp, remotePort);
                    OnException(e, remoteIp, remotePort);
                }
            }
        }

        private void ReceiveAsyncCallback(IAsyncResult result)
        {
            Socket remote = null;
            string remoteIp = "Unknown";
            int remotePort = -1;

            try
            {
                TcpClientState state = result.AsyncState as TcpClientState;
                remote = state.Remote;
                (remote.RemoteEndPoint as IPEndPoint).GetIpAndPort(out remoteIp, out remotePort);

                if (remote?.Connected == false)
                {
                    RemoveClient(remoteIp, remotePort);
                }

                int bytes = remote.EndReceive(result);

                if (bytes == 0)
                {
                    RemoveClient(remoteIp, remotePort);
                }
                else if (bytes > 0)
                {
                    OnReceivedData(remoteIp, remotePort, state.Buffer.Take(bytes).ToArray());
                }
            }
            catch (Exception e)
            {
                OnException(e, remoteIp, remotePort);
            }
            finally
            {
                //DoBeginReceive(remote, remoteIp, remotePort); 此句将导致 堆栈溢出
            }
        }

        public virtual bool Send(string ip, int port, byte[] data)
        {
            string key = ip + ":" + port;
            if (running && Clients.ContainsKey(key))
            {
                try
                {
                    Clients[key].Send(data);
                    return true;
                }
                catch (Exception e)
                {
                    OnException(e, ip, port);
                }
            }
            return false;
        }

        public virtual Task<bool> SendAsync(string ip, int port, byte[] data)
        {
            return Task.Run(() =>
            {
                return Send(ip, port, data);
            });
        }

        private void CheckClients()
        {
            Task.Factory.StartNew(() =>
            {
                while (running && EnableNetTouch)
                {
                    string[] keys = Clients.Keys.ToArray();
                    foreach (string key in keys)
                    {
                        if (Clients.ContainsKey(key))
                        {
                            string[] arr = key.Split(':');
                            if (!arr[0].Ping())
                            {
                                RemoveClient(arr[0], Convert.ToInt32(arr[1]));
                            }
                        }
                    }
                    Thread.Sleep(ConnectCheckInterval);
                }
            });
        }

        public ConcurrentDictionary<string, Socket> GetClients()
        {
            return Clients;
        }

        public virtual void RemoveClient(string remoteIp, int remotePort)
        {
            string clientKey = $"{remoteIp}:{remotePort}";
            if (Clients.TryRemove(clientKey, out Socket client))
            {
                client.Close();
                client.Dispose();
                OnDisconnected(remoteIp, remotePort);
            }
        }
    }

    internal class TcpClientState
    {
        public Socket Remote { get; set; }

        public byte[] Buffer { get; set; }
    }
}
