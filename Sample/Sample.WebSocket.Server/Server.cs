using SuperSocket.SocketBase.Config;
using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;

namespace Sample.WebSocket.Server
{
    public static class Server
    {
        private static WebSocketServer webSocketServer = new WebSocketServer();
        static List<WebSocketSession> sessions = new List<WebSocketSession>();

        public static void InitWebSocket()
        {
            ServerConfig serverConfig = new ServerConfig();
            serverConfig.Ip = "127.0.0.1";
            serverConfig.Port = 9090;

            if (!webSocketServer.Setup(serverConfig))
            {
                Console.WriteLine("配置信息设置异常");
                return;
            }

            if (!webSocketServer.Start())
            {
                Console.WriteLine("开启服务器失败！");
                return;
            }

            Console.WriteLine("WebSocket服务正在监听....");

            // 
            webSocketServer.NewSessionConnected += WebSocketServer_NewSessionConnected;
            webSocketServer.SessionClosed += WebSocketServer_SessionClosed;
            webSocketServer.NewMessageReceived += WebSocketServer_NewMessageReceived;
        }

        private static void WebSocketServer_NewSessionConnected(WebSocketSession session)
        {
            sessions.Add(session);
            // 
            Console.WriteLine($"有客户端接入：{session.SessionID}");
        }

        private static void WebSocketServer_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            sessions.RemoveAll(s => s.SessionID == session.SessionID);
        }

        private static void WebSocketServer_NewMessageReceived(WebSocketSession session, string value)
        {
            //接收消息知道是哪个客户端，通过session
            // 消息内容：Value
            Console.WriteLine($"接收到消息：{value}   来源于客户端：{session.SessionID}");
            session.Send("服务端回复：" + value);
        }
    }
}
