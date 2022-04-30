namespace Sample.WebSocket.Client
{
    using WebSocket4Net;
    public static class Client
    {
        static WebSocket webSocket = null;
        public static void Connect()
        {
            // ws://127.0.0.1:9090
            // 客户端对象
            webSocket = new WebSocket("ws://127.0.0.1:9090");
            webSocket.Open();
            webSocket.Opened += WebSocket_Opened;
            webSocket.MessageReceived += WebSocket_MessageReceived;
            webSocket.Error += WebSocket_Error;


            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    webSocket.Send("Zhaoxi Jovan！");
                }
            });
        }

        private static void WebSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            System.Console.WriteLine("客户端接收到消息：" + e.Message);
        }

        private static void WebSocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            System.Console.WriteLine(e.Exception.Message);
        }

        private static void WebSocket_Opened(object sender, System.EventArgs e)
        {
            System.Console.WriteLine("客户端连接成功！");
        }
    }
}
