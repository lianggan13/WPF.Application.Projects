using System.IO;

namespace Sample.NetSocket.Utility
{
    /// <summary>
    /// 数据缓冲区
    /// </summary>
    public class DataBuffer
    {
        /// <summary>
        /// 缓冲区
        /// </summary>
        public MemoryStream Stream { get; set; }

        /// <summary>
        /// 帧长度
        /// </summary>
        public int Length { get; set; }

        public DataBuffer()
        {
            Init();
        }

        public void Init()
        {
            Length = -1;
            if (Stream != null)
            {
                Stream.Flush();
                Stream.Close();
            }
            Stream = new MemoryStream();
        }
    }
}
