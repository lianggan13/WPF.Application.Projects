using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;
using System.Text;

namespace Sample.MQTT.Client
{
    public class Client
    {
        //static MqttClient mqttClient;
        static ManagedMqttClient managedMqttClient;

        public static async void Connect()
        {
            //var mqttClient1 = new MqttFactory().CreateMqttClient();
            // Create
            //var managedClient = new ManagedMqttClient(testEnvironment.CreateClient(), new MqttNetEventLogger());
            var mqttFactory = new MqttFactory();

            //mqttClient = mqttFactory.CreateMqttClient() as MqttClient;
            managedMqttClient = mqttFactory.CreateManagedMqttClient() as ManagedMqttClient;
            managedMqttClient.UseConnectedHandler(async (e) =>
            {
                Console.WriteLine(">> Connected: " + e.ConnectResult.ResultCode);
                await Task.CompletedTask;
            });
            managedMqttClient.UseDisconnectedHandler(async (e) =>
            {
                Console.WriteLine(">> Disconnected: " + e.Reason);
                await Task.CompletedTask;
            });

            var mqttClientBuilder = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer("127.0.0.1", 1883)
                .WithCleanSession(true)
                .WithCommunicationTimeout(TimeSpan.FromSeconds(6))
                .WithCredentials("lianggan13", "1918");
            //mqttClientBuilder.WithWebSocketServer("broker.hivemq.com:8000/mqtt");

            // Build
            var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(mqttClientBuilder)
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithStorage(new ManagedMqttClientTestStorage())
                .Build();

            // Connect
            managedMqttClient.StartAsync(managedMqttClientOptions).GetAwaiter().GetResult();

            // Receive
            managedMqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(ReceiveMessage);

            // Ping 
            //Task task = managedMqttClient.PingAsync(CancellationToken.None);
            //task.GetAwaiter().GetResult();

            // Subscribe
            //var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
            // .WithTopicFilter(f => { f.WithTopic("CustomeTopic"); })
            // .Build();

            ////await managedMqttClient.SubscribeAsync("CustomeTopic");
            //await managedMqttClient.SubscribeAsync(mqttSubscribeOptions.TopicFilters); //.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
            Pushlish_Using_Timer();

            Console.WriteLine("MQTT client startuped.");
        }

        private static void ReceiveMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            Console.WriteLine($">> Received: {topic}:{payload}");
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
                        await managedMqttClient.PublishAsync("HeartAckTopic", $"{DateTime.Now}");
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


        public class ManagedMqttClientTestStorage : IManagedMqttClientStorage
        {
            IList<ManagedMqttApplicationMessage> _messages;

            public int GetMessageCount()
            {
                return _messages.Count;
            }

            public Task<IList<ManagedMqttApplicationMessage>> LoadQueuedMessagesAsync()
            {
                if (_messages == null)
                {
                    _messages = new List<ManagedMqttApplicationMessage>();
                }

                return Task.FromResult(_messages);
            }

            public Task SaveQueuedMessagesAsync(IList<ManagedMqttApplicationMessage> messages)
            {
                _messages = messages;
                return Task.FromResult(0);
            }
        }




    }
}
