using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Sample.MQTT.WPFServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Sample.MQTT.WPFServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IMqttServer? mqttServer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            mqttServer = new MQTTnet.MqttFactory().CreateMqttServer();
            mqttServer.StartedHandler = new MqttServerStartedHandlerDelegate(OnMqttServerStarted);
            mqttServer.StoppedHandler = new MqttServerStoppedHandlerDelegate(OnMqttServerStopped);
            mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(new Action<MqttServerClientConnectedEventArgs>(OnMqttServerClientConnected));
            mqttServer.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(OnMqttServerClientDisconnected);
            mqttServer.ClientSubscribedTopicHandler = new MqttServerClientSubscribedTopicHandlerDelegate(OnMqttServerClientSubscribedTopic);
            mqttServer.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(OnMqttServerClientUnsubscribedTopic);
            mqttServer.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(ReceiveMessage);

            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpointBoundIPAddress(IPAddress.Parse(vm.HostIP))
                .WithEncryptedEndpointPort(vm.HostPort)// 端口号必须是1883，否则客户端连接不上
                .WithConnectionValidator(t =>
                {
                    if (t.Username != vm.UserName || t.Password != vm.Password)
                    {
                        t.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                    }
                    else
                    {
                        t.ReasonCode = MqttConnectReasonCode.Success;
                    }
                })
                .WithSubscriptionInterceptor(
                 c =>
                 {
                     c.AcceptSubscription = true;
                 }).WithApplicationMessageInterceptor(
                 c =>
                 {
                     c.AcceptPublish = true;
                 });

            var options = optionsBuilder.Build();

            mqttServer.StartAsync(options).GetAwaiter().GetResult();
        }

        private void OnMqttServerStarted(EventArgs e)
        {
            WriteToStatus($"服务端已启动！{vm.HostIP}:{vm.HostPort}");
        }

        private void OnMqttServerStopped(EventArgs e)
        {
            WriteToStatus($"服务端已停止！{vm.HostIP}:{vm.HostPort}");
        }

        private void OnMqttServerClientConnected(MqttServerClientConnectedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                vm.AllClients.Add(e.ClientId);
            });
            WriteToStatus("Client connected：" + $"{e.ClientId} [{e.Endpoint}]");
        }

        private void OnMqttServerClientDisconnected(MqttServerClientDisconnectedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                vm.AllClients.Remove(e.ClientId);
                var query = vm.AllTopics.Where(t => t.Clients.Contains(e.ClientId));
                if (query.Any())
                {
                    var tmp = query.ToList();
                    foreach (var model in tmp)
                    {
                        vm.AllTopics.Remove(model);
                        model.Clients.Remove(e.ClientId);
                        model.Count--;
                        vm.AllTopics.Add(model);
                    }
                }
            });

            WriteToStatus("Client disconnected：" + $"{e.ClientId} [{e.Endpoint}]");
        }

        private void OnMqttServerClientSubscribedTopic(MqttServerClientSubscribedTopicEventArgs e)
        {
            //mqttServer.PublishAsync(e.TopicFilter.Topic, "123");
            //mqttServer.PublishAsync("CustomeTopic", "123");

            this.Dispatcher.Invoke(() =>
            {
                if (vm.AllTopics.Any(t => t.Topic == e.TopicFilter.Topic))
                {
                    TopicModel model = vm.AllTopics.First(t => t.Topic == e.TopicFilter.Topic);
                    vm.AllTopics.Remove(model);
                    model.Clients.Add(e.ClientId);
                    model.Count++;
                    vm.AllTopics.Add(model);
                }
                else
                {
                    TopicModel model = new TopicModel(e.TopicFilter.Topic, e.TopicFilter.QualityOfServiceLevel)
                    {
                        Clients = new List<string> { e.ClientId },
                        Count = 1
                    };
                    vm.AllTopics.Add(model);
                }
            });

            WriteToStatus("客户端" + e.ClientId + "订阅主题" + e.TopicFilter.Topic);
        }

        private void OnMqttServerClientUnsubscribedTopic(MqttServerClientUnsubscribedTopicEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (vm.AllTopics.Any(t => t.Topic == e.TopicFilter))
                {
                    TopicModel model = vm.AllTopics.First(t => t.Topic == e.TopicFilter);
                    vm.AllTopics.Remove(model);
                    model.Clients.Remove(e.ClientId);
                    model.Count--;
                    if (model.Count > 0)
                    {
                        vm.AllTopics.Add(model);
                    }
                }
            });
            WriteToStatus("客户端" + e.ClientId + "退订主题" + e.TopicFilter);
        }

        private void ReceiveMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            //if (Clients.TryGetValue(e.ClientId ?? "", out string ip))
            //{
            //    var topic = e.ApplicationMessage.Topic;
            //    var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            //    Console.WriteLine($">> Received: {topic}:{payload} from [{ip}]");
            //}

            if (e.ApplicationMessage.Topic == "/environ/temp")
            {
                string str = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                double tmp;
                bool isdouble = double.TryParse(str, out tmp);
                if (isdouble)
                {
                    string result = "";
                    if (tmp > 40)
                    {
                        result = "温度过高！";
                    }
                    else if (tmp < 10)
                    {
                        result = "温度过低！";
                    }
                    else
                    {
                        result = "温度正常！";
                    }
                    MqttApplicationMessage message = new MqttApplicationMessage()
                    {
                        Topic = e.ApplicationMessage.Topic,
                        Payload = Encoding.UTF8.GetBytes(result),
                        QualityOfServiceLevel = e.ApplicationMessage.QualityOfServiceLevel,
                        Retain = e.ApplicationMessage.Retain
                    };
                    mqttServer.PublishAsync(message);
                }
            }
            WriteToStatus("收到消息" + e.ApplicationMessage.ConvertPayloadToString() + ",来自客户端" + e.ClientId + ",主题为" + e.ApplicationMessage.Topic);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mqttServer.StopAsync().GetAwaiter().GetResult();
        }

        private void btnAddTopic_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(vm.AddTopic) && mqttServer != null)
            {
                TopicModel topic = new TopicModel(vm.AddTopic, MqttQualityOfServiceLevel.AtLeastOnce);
                foreach (string clientId in vm.AllClients)
                {
                    mqttServer.SubscribeAsync(clientId, new List<MqttTopicFilter> { topic });
                }
                vm.AllTopics.Add(topic);
            }
        }

        private void menuClear_Click(object sender, RoutedEventArgs e)
        {
            txtRich.Document.Blocks.Clear();
        }

        public void WriteToStatus(string message)
        {
            if (!(txtRich.CheckAccess()))
            {
                this.Dispatcher.Invoke(
                        () => WriteToStatus(message));
                return;
            }
            string strTime = "[" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] ";
            txtRich.AppendText(strTime + message + "\r");

        }


        public void Pushlish_Using_Timer()
        {
            _ = Task.Run(async () =>
            {
                // User proper cancellation and no while(true).
                while (true)
                {
                    try
                    {
                        await mqttServer.PublishAsync("HeartAckTopic", $"{DateTime.Now}");
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

        public static void Reconnect_Using_Timer()
        {
            /*
             * This sample shows how to reconnect when the connection was dropped.
             * This approach uses a custom Task/Thread which will monitor the connection status.
             * This is the recommended way but requires more custom code!
             */

            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("broker.hivemq.com").Build();

                _ = Task.Run(
                    async () =>
                    {
                        // User proper cancellation and no while(true).
                        while (true)
                        {
                            try
                            {
                                // This code will also do the very first connect! So no call to _ConnectAsync_ is required
                                // in the first place.
                                if (!mqttClient.IsConnected)
                                {
                                    await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                                    // Subscribe to topics when session is clean etc.

                                    Console.WriteLine("The MQTT client is connected.");
                                }
                            }
                            catch
                            {
                                // Handle the exception properly (logging etc.).
                            }
                            finally
                            {
                                // Check the connection state every 5 seconds and perform a reconnect if required.
                                await Task.Delay(TimeSpan.FromSeconds(5));
                            }
                        }
                    });
            }
        }

    }
}
