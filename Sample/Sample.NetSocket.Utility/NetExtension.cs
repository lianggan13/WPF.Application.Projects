using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Sample.NetSocket.Utility
{
    public static class NetExtension
    {
        public static void GetIpAndPort(this IPEndPoint iPEndPoint, out string ip, out int port)
        {
            ip = iPEndPoint.Address.MapToIPv4().ToString();
            port = iPEndPoint.Port;
        }

        public static void GetIpAndPort(this string address, out string ip, out int port)
        {
            string[] array = address.Split(':');
            if (array.Length == 1 && IPAddress.TryParse(array[0], out _))
            {
                ip = array[0];
                port = 80;
            }
            else if (array.Length == 2 && IPAddress.TryParse(array[0], out _) && int.TryParse(array[1], out port))
            {
                ip = array[0];
            }
            else
            {
                throw new FormatException($"address: {address} is illegal ipv4 string.");
            }
        }

        public static bool Ping(this string ip, int times = 4, int timeout = 200)
        {
            Ping ping = new Ping();
            PingOptions options = new PingOptions(0x80, true);
            byte[] data = new byte[1];
            for (int i = 0; i < times; i++)
            {
                PingReply reply = ping.Send(ip, timeout, data, options);
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
            }
            return false;
        }

        public static string ToHex(this byte[] data, string split = "")
        {
            StringBuilder builder = new StringBuilder();
            foreach (byte b in data)
            {
                builder.Append(b.ToString("X2") + split);
            }
            return builder.ToString();
        }
    }
}
