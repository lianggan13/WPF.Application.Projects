using System.Net.Sockets;

namespace Sample.NetSocket.Utility.Base
{
    public abstract class SocketBase : IDisposable
    {
        protected Socket socket;
        protected bool running = false;
        protected bool disposed;

        /// <summary>
        /// 收到数据触发事件
        /// </summary>
        public event EventHandler<SocketReceivedDataEventArgs> ReceivedData;

        /// <summary>
        /// 抛出异常触发事件
        public event EventHandler<SocketExceptionEventArgs> ThrownException;

        /// <summary>
        /// 缓冲区大小(字节) 默认为2048
        /// </summary>
        public int BufferSize { get; set; } = 2048;

        /// <summary>
        /// 启用实例
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// 关闭实例
        /// </summary>
        public virtual void Close()
        {
            if (running)
            {
                running = false;
                if (socket != null)
                {
                    socket.Close();
                }
            }
        }

        protected virtual void OnReceivedData(string ip, int port, byte[] data)
        {
            SocketReceivedDataEventArgs args = new SocketReceivedDataEventArgs(ip, port, data);
            ReceivedData?.Invoke(this, args);
        }

        protected virtual void OnException(Exception exception, string ip, int port)
        {
            SocketExceptionEventArgs args = new SocketExceptionEventArgs(exception, ip, port);
            ThrownException?.Invoke(this, args);
        }

        ~SocketBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // release managed resources...
                    running = false;
                }
                // release unmanaged resources or big object...
                socket.Dispose();
                disposed = true;
            }
        }
    }
}
