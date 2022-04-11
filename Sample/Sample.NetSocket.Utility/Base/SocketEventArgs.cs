namespace Sample.NetSocket.Utility.Base
{
    public class SocketEventArgs : EventArgs
    {
        public SocketEventArgs(string ip, int port)
        {
            Ip = ip;
            Port = port;
            Address = ip + ":" + port;
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        public string Ip { get; private set; }

        /// <summary>
        /// 获取通信端口
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// 获取通信地址
        /// </summary>
        public string Address { get; private set; }
    }

    public class SocketReceivedDataEventArgs : SocketEventArgs
    {
        public SocketReceivedDataEventArgs(string ip, int port, byte[] data)
            : base(ip, port)
        {
            Data = data;
        }

        /// <summary>
        /// 获取接收的数据
        /// </summary>
        public byte[] Data { get; private set; }
    }

    public class SocketExceptionEventArgs : SocketEventArgs
    {
        public SocketExceptionEventArgs(Exception exception, string ip, int port)
            : base(ip, port)
        {
            Exception = exception;
        }

        /// <summary>
        /// 获取异常信息
        /// </summary>
        public Exception Exception { get; private set; }
    }
}
