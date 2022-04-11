using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System.Net;
using System.Text;

namespace Sample.MQTT.Server
{
    public class Server
    {
        static IMqttServer mqttServer = null;

        public static Dictionary<string, string> Clients { get; private set; }

        public static void InitMqttServer()
        {
            Clients = new Dictionary<string, string>();

            mqttServer = new MQTTnet.MqttFactory().CreateMqttServer();
            mqttServer.StartedHandler = new MqttServerStartedHandlerDelegate(OnMqttServerStarted);
            mqttServer.StoppedHandler = new MqttServerStoppedHandlerDelegate(OnMqttServerStopped);
            mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(new Action<MqttServerClientConnectedEventArgs>(OnMqttServerClientConnected));
            mqttServer.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(OnMqttServerClientDisconnected);
            mqttServer.ClientSubscribedTopicHandler = new MqttServerClientSubscribedTopicHandlerDelegate(OnMqttServerClientSubscribedTopic);
            mqttServer.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(OnMqttServerClientUnsubscribedTopic);
            mqttServer.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(ReceiveMessage);

            var options = new MqttServerOptionsBuilder()
                .WithDefaultEndpointBoundIPAddress(IPAddress.Parse("127.0.0.1"))
                .WithEncryptedEndpointPort(1883)// 端口号必须是1883，否则客户端连接不上
                .WithConnectionValidator(ConnectionValidator)
                .WithSubscriptionInterceptor(
                 c =>
                 {
                     c.AcceptSubscription = true;
                 }).WithApplicationMessageInterceptor(
                 c =>
                 {
                     c.AcceptPublish = true;
                 })
                 .Build();


            mqttServer.StartAsync(options).GetAwaiter().GetResult();
            //Pushlish_Using_Timer();

            Console.WriteLine("MQTT server startuped.");

        }

        private static void OnMqttServerStarted(EventArgs obj)
        {
        }

        private static void OnMqttServerStopped(EventArgs obj)
        {
        }

        static void OnMqttServerClientConnected(MqttServerClientConnectedEventArgs e)
        {
            Clients.Add(e.ClientId, e.Endpoint);
            Console.WriteLine("Client connected：" + $"{e.ClientId} [{e.Endpoint}]");
        }


        private static void OnMqttServerClientDisconnected(MqttServerClientDisconnectedEventArgs e)
        {
            Clients.Remove(e.ClientId);
            Console.WriteLine("Client disconnected：" + $"{e.ClientId} [{e.Endpoint}]");
        }

        private static void OnMqttServerClientSubscribedTopic(MqttServerClientSubscribedTopicEventArgs e)
        {
            //mqttServer.PublishAsync(e.TopicFilter.Topic, "123");
            //mqttServer.PublishAsync("CustomeTopic", "123");
        }

        private static void OnMqttServerClientUnsubscribedTopic(MqttServerClientUnsubscribedTopicEventArgs obj)
        {

        }

        static void ConnectionValidator(MqttConnectionValidatorContext context)
        {
            // 当客户端连接的时候，会触发这个委托 
            if (context.Username == "lianggan13" && context.Password == "1918")
            {
                context.ReasonCode = MqttConnectReasonCode.Success;
            }
            else
            {
                context.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
            }
        }

        static void ReceiveMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            if (Clients.TryGetValue(e.ClientId ?? "", out string ip))
            {
                var topic = e.ApplicationMessage.Topic;
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine($">> Received: {topic}:{payload} from [{ip}]");
            }
        }


        public static void Pushlish_Using_Timer()
        {
            _ = Task.Run(async () =>
            {
                // User proper cancellation and no while(true).
                while (true)
                {
                    try
                    {
                        await mqttServer.PublishAsync("CustomeTopic", $"{DateTime.Now}");
                    }
                    catch
                    {
                        // Handle the exception properly (logging etc.).
                    }
                    finally
                    {
                        // Check the connection state every 5 seconds
                        await Task.Delay(TimeSpan.FromSeconds(5));
                    }
                }
            });
        }
    }
}
