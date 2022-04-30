using MQTTnet;
using MQTTnet.Protocol;
using System.Collections.Generic;
using System.ComponentModel;

namespace Sample.MQTT.WPFServer.Model
{
    public class TopicModel : MqttTopicFilter, INotifyPropertyChanged
    {
        public TopicModel(string topic, MqttQualityOfServiceLevel qualityOfServiceLevel)
        {
            clients = new List<string>();
            count = 0;
            base.Topic = topic;
            base.QualityOfServiceLevel = qualityOfServiceLevel;
        }

        private int count;
        /// <summary>
        /// 订阅此主题的客户端数量
        /// </summary>
        public int Count
        {
            get { return count; }
            set
            {
                if (count != value)
                {
                    count = value;
                    this.OnPropertyChanged("Count");
                }

            }
        }

        private List<string> clients;
        /// <summary>
        /// 订阅此主题的客户端
        /// </summary>
        public List<string> Clients
        {
            get { return clients; }
            set
            {
                if (clients != value)
                {
                    clients = value;
                    this.OnPropertyChanged("Clients");
                }

            }
        }


        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
