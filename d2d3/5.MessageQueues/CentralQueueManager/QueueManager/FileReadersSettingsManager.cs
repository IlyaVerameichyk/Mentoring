using Microsoft.ServiceBus.Messaging;

namespace QueueManager
{
    public class FileReadersSettingsManager
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://mqmentoring.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=UlNEqYdOZZUz3SKwcoK7knvXl/3scewYuKWSSBNEvg8=";
        private const string TopicName = "settingstopic";
        private TopicClient _client;

        public FileReadersSettingsManager()
        {
            _client = TopicClient.CreateFromConnectionString(ServiceBusConnectionString, TopicName);
        }

        public void SetQrTerminateWord(string terminateWord)
        {
            var message = new BrokeredMessage(terminateWord);
            message.Properties.Add("ActionName", "SetTerminateWord");
            message.To = "FileSender";
            _client.Send(message);
        }

        public void PingNow()
        {
            var message = new BrokeredMessage();
            message.Properties.Add("ActionName", "Ping");
            message.To = "FileSender";
            _client.Send(message);
        }
    }
}