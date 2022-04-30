// See https://aka.ms/new-console-template for more information
using Sample.NetSocket.Client;
using Sample.NetSocket.Utility.Base;
using System.Text;

CancellationTokenSource CTS = new CancellationTokenSource();
CancellationToken Token = new CancellationToken();

TcpClient client = new TcpClient("127.0.0.1", remotePort: 2345);
Task heartTask = new Task(SendHeart, Token);

client.Connected += Server_Connected;
client.Disconnected += Server_Disconnected;
client.ReceivedData += Client_ReceivedData;
client.Start();
Console.WriteLine("NetSocket client startuped.");


Console.ReadLine();

void Server_Connected(object? sender, SocketEventArgs e)
{
    if (heartTask.IsCompleted == true || heartTask.IsCanceled == true)
        heartTask = new Task(SendHeart, Token);
    heartTask.Start();
    Console.WriteLine("Client connected：" + $"{e.Address} [{e.Ip}]");
}

void Server_Disconnected(object? sender, SocketEventArgs e)
{
    CTS.Cancel();//通知关闭Task
    heartTask.Dispose();
    Console.WriteLine("Client disconnected：" + $"{e.Address} [{e.Ip}]");
}

void Client_ReceivedData(object? sender, SocketReceivedDataEventArgs e)
{
    string msg = Encoding.UTF8.GetString(e.Data);
    Console.WriteLine($">> Received: {msg} from [{e.Address}]");
}

async void SendHeart()
{
    while (true)
    {
        if (Token.IsCancellationRequested)
        {
            //或者判断状态关闭线程(二选一)
            break;
        }
        var msg = Encoding.UTF8.GetBytes("heart");
        client.Send(msg);

        await Task.Delay(5 * 1000);
    }
}

