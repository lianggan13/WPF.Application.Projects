using Microsoft.AspNetCore.SignalR;
using System.Net.WebSockets;

namespace SignalRDemo1.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
    }
    public class StronglyTypedChatHub : Hub<IChatClient>
    {
        public async Task SendMessage(string user, string message)
            => await Clients.All.ReceiveMessage(user, message);

        public async Task SendMessageToCaller(string user, string message)
            => await Clients.Caller.ReceiveMessage(user, message);

        public async Task SendMessageToGroup(string user, string message)
            => await Clients.Group("SignalR Users").ReceiveMessage(user, message);



    }

    /// <summary>
    /// 在 Hub 类中编写的方法，都是被 Clients 调用的方法
    /// </summary>
    public class ChatHub : Hub
    {
        /*
         * on --> register
         * invoke --> execute (hub method)
         * sendasync --> execute (connection on:  web & wpf clients)
         */

        // 服务器主动调 clients 方法
        public void Hello()
        {
            //Clients.All.hello();
        }

        [HubMethodName("SendMessageToUser")]
        public async Task DirectMessage(string user, string message)
  => await Clients.User(user).SendAsync("ReceiveMessage", user, message);

        public Task ThrowException()
    => throw new HubException("This error will be sent to the client!");


        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task Login(string name)
        {
            await Clients.AllExcept(Context.ConnectionId).
                SendAsync("online", $"{name} 进入了群聊！");
        }


        public async Task SignOut(string name)
        {
            await Clients.AllExcept(Context.ConnectionId)
                .SendAsync("online", $"{name} 离开了群聊！");

        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageByServer(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, "系统通知:" + message);
        }

        public async Task SendMessageToGroup(string user, string message)
            => await Clients.Group("SignalR Users").SendAsync("ReceiveMessage", user, message);


        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }


        //        connection.invoke("GetTotalLength", { param1: "value1" });
        //connection.invoke("GetTotalLength", { param1: "value1", param2: "value2" });

        public async Task<int> GetTotalLength(TotalLengthRequest req)
        {
            var length = req.Param1.Length;
            if (req.Param2 != null)
            {
                length += req.Param2.Length;
            }
            return await Task.FromResult(length);
        }

        public async Task Broadcast(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", new
            {
                Sender = Context.User.Identity.Name,
                Message = message
            });
        }

    }

    public class TotalLengthRequest
    {
        public string Param1 { get; set; }
        public string Param2 { get; set; }
    }
}
