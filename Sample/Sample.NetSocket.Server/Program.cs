// See https://aka.ms/new-console-template for more information
using Sample.NetSocket.Server;
using Sample.NetSocket.Utility.Base;
using System.Text;

TcpServer server = new TcpServer(localPort: 2345);
server.Connected += Client_Connected;
server.Disconnected += Client_Disconnected;
server.ReceivedData += Server_ReceivedData;

server.Start();

Console.WriteLine("NetSocket server startuped.");
Console.ReadLine();


void Client_Connected(object? sender, SocketEventArgs e)
{
    Console.WriteLine("Client connected：" + $"{e.Address} [{e.Ip}]");
}

void Client_Disconnected(object? sender, SocketEventArgs e)
{
    Console.WriteLine("Client disconnected：" + $"{e.Address} [{e.Ip}]");
}

void Server_ReceivedData(object? sender, SocketReceivedDataEventArgs e)
{
    string msg = Encoding.UTF8.GetString(e.Data);
    Console.WriteLine($">> Received: {msg} from [{e.Address}]");

    var rmsg = "Server ack " + Encoding.UTF8.GetString(e.Data);
    server.Send(e.Ip, e.Port, Encoding.UTF8.GetBytes(rmsg));
}

