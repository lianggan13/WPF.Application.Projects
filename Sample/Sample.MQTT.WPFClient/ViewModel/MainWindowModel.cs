using MQTTnet;
using Sample.MQTT.WPFClient.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sample.MQTT.WPFClient.ViewModel
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        public MainWindowModel()
        {
            SelectedTopics = new List<MqttTopicFilter>();
            ServerUri = "127.0.0.1";
            CurrentTopic = null;
            ServerPort = 1883;
            ClientID = Guid.NewGuid().ToString("N");

            AllTopics = new List<TopicModel>();
            AllTopics.Add(new TopicModel("/environ/temp", "环境-温度"));
            AllTopics.Add(new TopicModel("/environ/hum", "环境-湿度"));
            //topics.Add(new TopicModel("/environ/pm25", "环境-PM2.5"));
            //topics.Add(new TopicModel("/environ/CO2", "环境-二氧化碳"));
            //topics.Add(new TopicModel("/energy/electric", "能耗-电"));
            //topics.Add(new TopicModel("/energy/water", "环境-水"));
            //topics.Add(new TopicModel("/energy/gas", "环境-电"));
            AllTopics.Add(new TopicModel("/data/alarm", "数据-报警"));
            AllTopics.Add(new TopicModel("/data/message", "数据-消息"));
            AllTopics.Add(new TopicModel("/data/notify", "数据-通知"));
        }


        private List<TopicModel> _allTopics;

        public List<TopicModel> AllTopics
        {
            get { return _allTopics; }
            set
            {
                if (_allTopics != value)
                {
                    _allTopics = value;
                    OnPropertyChanged("AllTopics");
                }
            }
        }

        private List<MqttTopicFilter> _selectedTopics;

        public List<MqttTopicFilter> SelectedTopics
        {
            get { return _selectedTopics; }
            set
            {
                if (_selectedTopics != value)
                {
                    _selectedTopics = value;
                    OnPropertyChanged("SelectedTopics");
                }
            }
        }

        private string _serverUri;

        public string ServerUri
        {
            get { return _serverUri; }
            set
            {
                if (_serverUri != value)
                {
                    _serverUri = value;
                    OnPropertyChanged("ServerUri");
                }
            }
        }

        private int _serverPort;

        public int ServerPort
        {
            get { return _serverPort; }
            set
            {
                if (_serverPort != value)
                {
                    _serverPort = value;
                    OnPropertyChanged("ServerPort");
                }
            }
        }


        private string _clientId;

        public string ClientID
        {
            get { return _clientId; }
            set
            {
                if (_clientId != value)
                {
                    _clientId = value;
                    OnPropertyChanged("ClientID");
                }
            }
        }

        private MqttTopicFilter _currentTopic;

        public MqttTopicFilter CurrentTopic
        {
            get { return _currentTopic; }
            set
            {
                if (_currentTopic != value)
                {
                    _currentTopic = value;
                    OnPropertyChanged("CurrentTopic");
                }
            }
        }

        private bool? _isConnected = false;

        public bool? IsConnected
        {
            get { return _isConnected; }
            set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    OnPropertyChanged("IsConnected");
                }
            }
        }

        private bool _isDisConnected = true;

        public bool IsDisConnected
        {
            get { return _isDisConnected; }
            set
            {
                if (_isDisConnected != value)
                {
                    _isDisConnected = value;
                    this.OnPropertyChanged("IsDisConnected");
                }
            }
        }

        private string _userName = "admin";

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    this.OnPropertyChanged("UserName");
                }

            }
        }

        private string _password = "password";

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    this.OnPropertyChanged("Password");
                }

            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
