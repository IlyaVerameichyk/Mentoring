using SystemWatcher.Analyzer;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Service.QueueManager
{
    public class SettingsManager
    {
        private const string TopicName = "settingstopic";
        private const string SubscriptionName = "SettingsChangeSubscription";
        private readonly BarcodeAnalyzer _analyzer;

        public SettingsManager(BarcodeAnalyzer analyzer)
        {
            _analyzer = analyzer;

            var nsm = NamespaceManager.Create();
            if (!nsm.SubscriptionExists(TopicName, SubscriptionName))
            {
                nsm.CreateSubscription(TopicName, SubscriptionName, new SqlFilter("sys.To = 'FileSender' AND ActionName = 'SetTerminateWord'"));
            }
            var client = SubscriptionClient.Create(TopicName, SubscriptionName);
            client.OnMessage(ProcessMessage);
        }

        private void ProcessMessage(BrokeredMessage obj)
        {
            var text = obj.GetBody<string>();
            _analyzer.SetTerminateText(text);
            obj.Complete();
        }
    }
}