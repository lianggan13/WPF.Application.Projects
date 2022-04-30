using MQTTnet.Extensions.ManagedClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.MQTT.WPFClient.Model
{
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
