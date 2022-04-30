using System;

namespace Sample.WebSocket.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server.InitWebSocket();

            Console.ReadLine();
        }
    }
}
