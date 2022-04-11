using System;
using System.Net.Sockets;

namespace Sample.NetSocket.Utility.Base
{
    public abstract class TcpBase : SocketBase
    {
        /// <summary>
        /// 获取或设置本地通信端口
        /// </summary>
        public int LocalPort { get; set; } = -1;

        /// <summary>
        /// 获取或设置是否开启网络探测
        /// </summary>
        public bool EnableNetTouch { get; set; }

        /// <summary>
        /// 获取或设置连接状态检测间隔(毫秒) 默认为 3 * 1000
        /// </summary>
        public int ConnectCheckInterval { get; set; } = 3 * 1000;

        /// <summary>
        /// 获取或设置接收数据超时时间(毫秒) 默认为 int.MaxValue
        /// </summary>
        public int ReceiveTimeoutMilliseconds { get; set; } = int.MaxValue;

        protected TcpBase()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// 建立连接触发事件
        /// </summary>
        public event EventHandler<SocketEventArgs> Connected;

        /// <summary>
        /// 断开连接触发事件
        /// </summary>
        public event EventHandler<SocketEventArgs> Disconnected;

        /// <summary>
        /// 接收数据超时时间
        /// </summary>
        public event EventHandler<SocketEventArgs> ReceiveTimeout;

        protected virtual void OnConnected(string ip, int port)
        {
            SocketEventArgs args = new SocketEventArgs(ip, port);
            Connected?.Invoke(this, args);
        }

        protected virtual void OnDisconnected(string ip, int port)
        {
            SocketEventArgs args = new SocketEventArgs(ip, port);
            Disconnected?.Invoke(this, args);
        }

        protected virtual void OnReceiveTimeout(string ip, int port)
        {
            SocketEventArgs args = new SocketEventArgs(ip, port);
            ReceiveTimeout?.Invoke(this, args);
        }
    }
}
