using SystemWatcher.Analyzer;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Service.QueueManager
{
    public class SettingsManager
    {
        private const string ServiceBusConnectionString = "Endpoint=sb://mqmentoring.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=UlNEqYdOZZUz3SKwcoK7knvXl/3scewYuKWSSBNEvg8=";
        private const string TopicName = "settingstopic";
        private const string SubscriptionName = "SettingsChangeSubscription";
        private readonly BarcodeAnalyzer _analyzer;

        public SettingsManager(BarcodeAnalyzer analyzer)
        {
            _analyzer = analyzer;

            var nsm = NamespaceManager.CreateFromConnectionString(ServiceBusConnectionString);
            nsm.DeleteSubscription(TopicName, SubscriptionName);
            nsm.CreateSubscription(TopicName, SubscriptionName, new SqlFilter("sys.To = 'FileSender' AND ActionName = 'SetTerminateWord'"));

            var client = SubscriptionClient.CreateFromConnectionString(ServiceBusConnectionString, TopicName, SubscriptionName);
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