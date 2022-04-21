using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;
using Sample.MQTT.WPFClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sample.MQTT.WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ManagedMqttClient? managedMqttClient;
        public MainWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 数据模型转换
        /// </summary>
        /// <param name="topics"></param>
        /// <returns></returns>
        private List<MqttTopicFilter> ConvertTopics(List<TopicModel> topics)
        {
            //MQTTnet.MqttTopicFilter
            List<MqttTopicFilter> filters = new List<MqttTopicFilter>();
            foreach (TopicModel model in topics)
            {
                MqttTopicFilter filter = new MqttTopicFilter()
                {
                    Topic = model.Topic,
                    QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce
                };
                filters.Add(filter);

            }
            return filters;
        }


        /// <param name="message"></param>
        public void WriteToStatus(string message)
        {
            if (!(txtRich.CheckAccess()))
            {
                this.Dispatcher.Invoke(() =>
                    WriteToStatus(message)
                    );
                return;
            }
            string strTime = "[" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] ";
            txtRich.AppendText(strTime + message + "\r");
            if (txtRich.ExtentHeight > 200)
            {
                txtRich.Document.Blocks.Clear();
            }
        }

        /// <summary>
        /// 保存订阅的主题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            List<TopicModel> topics = vm.AllTopics.Where(t => t.IsSelected == true).ToList();

            vm.SelectedTopics = ConvertTopics(topics);
            SubscribeTopics(vm.SelectedTopics);
        }

        private void SubscribeTopics(List<MqttTopicFilter> filters)
        {
            if (managedMqttClient != null)
            {
                managedMqttClient.SubscribeAsync(filters);
                string tmp = "";
                foreach (var filter in filters)
                {
                    tmp += filter.Topic;
                    tmp += ",";
                }
                if (tmp.Length > 1)
                {
                    tmp = tmp.Substring(0, tmp.Length - 1);
                }
                WriteToStatus("成功订阅主题：" + tmp);
            }
            else
            {
                ShowDialog("提示", "请连接服务端后订阅主题！");
            }
        }


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (vm.ServerUri != null && vm.ServerPort > 0)
            {
                InitClient(vm.ClientID, vm.ServerUri, vm.ServerPort);
            }
            else
            {
                ShowDialog("提示", "服务端地址或端口号不能为空！");
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (managedMqttClient != null)
            {
                managedMqttClient.StopAsync();
            }
        }

        private void InitClient(string id, string url = "127.0.0.1", int port = 1883)
        {
            //var mqttClient1 = new MqttFactory().CreateMqttClient();
            //var managedClient = new ManagedMqttClient(testEnvironment.CreateClient(), new MqttNetEventLogger());
            //var mqttClient = new MqttFactory().CreateMqttClient() as MqttClient;
            managedMqttClient = new MqttFactory().CreateManagedMqttClient() as ManagedMqttClient;
            managedMqttClient.UseConnectedHandler((Func<MQTTnet.Client.Connecting.MqttClientConnectedEventArgs, Task>)(async (e) =>
            {
                this.vm.IsConnected = true;
                this.vm.IsDisConnected = false;

                WriteToStatus(">> Connected: " + e.ConnectResult.ResultCode);
                await Task.CompletedTask;
            }));
            managedMqttClient.UseDisconnectedHandler((Func<MQTTnet.Client.Disconnecting.MqttClientDisconnectedEventArgs, Task>)(async (e) =>
            {
                this.vm.IsConnected = false;
                this.vm.IsDisConnected = true;
                WriteToStatus(">> Disconnected: " + e.Reason);
                await Task.CompletedTask;
            }));

            var mqttClientBuilder = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer(url, port)
                .WithCleanSession(true)
                .WithCommunicationTimeout(TimeSpan.FromSeconds(6))
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(100))
                .WithCredentials(vm.UserName, vm.Password);
            //mqttClientBuilder.WithWebSocketServer("broker.hivemq.com:8000/mqtt");

            // Build
            var managedMqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(mqttClientBuilder)
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))

                .WithStorage(new ManagedMqttClientTestStorage())
                .Build();

            // Connect
            managedMqttClient?.StartAsync(managedMqttClientOptions).GetAwaiter().GetResult();

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
        }


        private void ReceiveMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            WriteToStatus($">> Received:  {e.ClientId} {topic}:{payload}");
            //WriteToStatus("收到来自客户端" + e.ClientId + "，主题为" + e.ApplicationMessage.Topic + "的消息：" + Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
        }


        private void btnPublish_Click(object sender, RoutedEventArgs e)
        {
            if (managedMqttClient != null)
            {
                if (this.comboTopics.SelectedIndex < 0)
                {
                    ShowDialog("提示", "请选择要发布的主题！");
                    return;
                }
                if (string.IsNullOrEmpty(txtContent.Text))
                {
                    ShowDialog("提示", "消息内容不能为空！");
                    return;
                }
                string topic = comboTopics.SelectedValue as string;
                string content = txtContent.Text;
                MqttApplicationMessage msg = new MqttApplicationMessage
                {
                    Topic = topic,
                    Payload = Encoding.UTF8.GetBytes(content),
                    QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce,
                    Retain = false
                };
                managedMqttClient.PublishAsync(msg);
                WriteToStatus("成功发布主题为" + topic + "的消息！");
            }
            else
            {
                ShowDialog("提示", "请连接服务端后发布消息！");
                return;
            }
        }

        public void ShowDialog(string title, string content)
        {
            _showDialog(title, content);
        }
        /// <summary>
        /// 提示框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        private void _showDialog(string title, string content)
        {
            MessageBox.Show(title, content);
        }

        private void btnSaveConfig_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
