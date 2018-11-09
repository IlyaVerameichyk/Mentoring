using Microsoft.ServiceBus.Messaging;

namespace QueueManager
{
    public class FileReadersSettingsManager
    {
        private const string TopicName = "settingstopic";
        private readonly TopicClient _client;

        public FileReadersSettingsManager()
        {
            _client = TopicClient.Create(TopicName);
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
            message.Properties.Add("ActionName", "PingNow");
            message.To = "FileSender";
            _client.Send(message);
        }
    }
}