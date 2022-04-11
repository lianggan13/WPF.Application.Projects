using Sample.NetSocket.Utility;
using Sample.NetSocket.Utility.Base;
using System.Net;
using System.Net.Sockets;

namespace Sample.NetSocket.Client
{
    public class TcpClient : TcpBase
    {
        public readonly string remoteIp;
        public readonly int remotePort;
        public readonly EndPoint remote;

        public bool IsConnected { get; set; }

        public TcpClient(string remoteIp, int remotePort)
            : base()
        {
            this.remoteIp = remoteIp;
            this.remotePort = remotePort;
            remote = new IPEndPoint(IPAddress.Parse(remoteIp), remotePort);
        }

        public override void Start()
        {
            running = true;
            DoBeginConnect();
        }

        private async void DoBeginConnect()
        {
            while (running)
            {
                if (IsConnected && socket.Connected)
                {
                    if (EnableNetTouch && !remoteIp.Ping())
                    {
                        IsConnected = false;
                        OnDisconnected(remoteIp, remotePort);
                    }
                    else
                    {
                        await Task.Delay(ConnectCheckInterval);
                        continue;
                    }
                }
                if (socket != null)
                {
                    socket.Close();
                    socket.Dispose();
                    socket = null;
                }
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if (LocalPort != -1)
                {
                    socket.Bind(new IPEndPoint(IPAddress.Any, LocalPort));
                }
                socket.BeginConnect(remote, ConnectAsyncCallback, remote);
                break;
            }
        }

        private void ConnectAsyncCallback(IAsyncResult result)
        {
            try
            {
                if (socket == null)
                    return;
                socket.EndConnect(result);
                if (socket.Connected)
                {
                    IsConnected = true;
                    var ipep = socket.LocalEndPoint as IPEndPoint;
                    Console.WriteLine("Conected by local: " + ipep.ToString());
                    OnConnected(remoteIp, remotePort);
                    Task.Factory.StartNew(DoBeginReceive);
                }
            }
            catch (Exception e)
            {
                IsConnected = false;
                OnException(e, remoteIp, remotePort);
            }
            finally
            {
                DoBeginConnect();
            }
        }

        private void DoBeginReceive()
        {
            while (running && IsConnected)
            {
                try
                {
                    if (socket.Poll(ReceiveTimeoutMilliseconds * 1000, SelectMode.SelectRead))
                    {
                        byte[] buffer = new byte[BufferSize];
                        socket.BeginReceive(buffer, 0, BufferSize, SocketFlags.None, ReceiveAsyncCallback, buffer);
                        //break;
                    }
                    else
                    {
                        OnReceiveTimeout(remoteIp, remotePort);
                    }
                }
                catch (Exception e)
                {
                    IsConnected = false;
                    OnException(e, remoteIp, remotePort);
                }
            }
        }

        private void ReceiveAsyncCallback(IAsyncResult result)
        {
            try
            {
                int bytes = socket.EndReceive(result);
                if (bytes > 0)
                {
                    byte[] data = result.AsyncState as byte[];
                    if (bytes == 0)
                    {
                        IsConnected = false;
                    }
                    else if (bytes > 0)
                    {
                        OnReceivedData(remoteIp, remotePort, data.Take(bytes).ToArray());
                    }
                }
            }
            catch (Exception e)
            {
                IsConnected = false;
                OnDisconnected(remoteIp, remotePort);
                OnException(e, remoteIp, remotePort);
            }
            finally
            {
                //DoBeginReceive(remote, remoteIp, remotePort); 此句将导致 堆栈溢出
            }
        }

        public virtual bool Send(byte[] data)
        {
            if (socket != null && IsConnected)
            {
                socket.Send(data);
                return true;
            }
            return false;
        }

        public virtual Task<bool> SendAsync(byte[] data)
        {
            return Task.Run(() =>
            {
                return Send(data);
            });
        }
    }
}
