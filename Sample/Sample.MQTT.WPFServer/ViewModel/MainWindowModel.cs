using Sample.MQTT.WPFServer.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Sample.MQTT.WPFServer.ViewModel
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        public MainWindowModel()
        {
            hostIP = "127.0.0.1";//绑定的IP地址
            hostPort = 1883;//绑定的端口号
            timeout = 3000;//连接超时时间
            username = "admin";//用户名
            password = "password";//密码
            allTopics = new ObservableCollection<TopicModel>();//主题
            allClients = new ObservableCollection<string>();//客户端
            addTopic = "";
        }

        private ObservableCollection<TopicModel> allTopics;

        public ObservableCollection<TopicModel> AllTopics
        {
            get { return allTopics; }
            set
            {
                if (allTopics != value)
                {
                    allTopics = value;
                    this.OnPropertyChanged("AllTopics");
                }

            }
        }

        private ObservableCollection<string> allClients;

        public ObservableCollection<string> AllClients
        {
            get { return allClients; }
            set
            {
                if (allClients != value)
                {
                    allClients = value;
                    this.OnPropertyChanged("AllClients");
                }

            }
        }


        private string hostIP;

        public string HostIP
        {
            get { return hostIP; }
            set
            {
                if (hostIP != value)
                {
                    hostIP = value;
                    this.OnPropertyChanged("HostIP");
                }

            }
        }

        private int hostPort;

        public int HostPort
        {
            get { return hostPort; }
            set
            {
                if (hostPort != value)
                {
                    hostPort = value;
                    this.OnPropertyChanged("HostPort");
                }

            }
        }

        private int timeout;

        public int Timeout
        {
            get { return timeout; }
            set
            {
                if (timeout != value)
                {
                    timeout = value;
                    this.OnPropertyChanged("Timeout");
                }

            }
        }

        private string username;

        public string UserName
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    this.OnPropertyChanged("UserName");
                }

            }
        }


        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    this.OnPropertyChanged("Password");
                }

            }
        }

        private string addTopic;

        public string AddTopic
        {
            get { return addTopic; }
            set
            {
                if (addTopic != value)
                {
                    addTopic = value;
                    this.OnPropertyChanged("AddTopic");
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
