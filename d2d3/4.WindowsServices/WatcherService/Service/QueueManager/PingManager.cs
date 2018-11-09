using System;
using System.Timers;
using SystemWatcher;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Service.QueueManager
{
    public class PingManager
    {
        private FileManager _fileManager;

        private const string ServiceBusConnectionString = "Endpoint=sb://mqmentoring.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=UlNEqYdOZZUz3SKwcoK7knvXl/3scewYuKWSSBNEvg8=";
        private const string TopicName = "settingstopic";

        private const string SubscriptionName = "PingSubscriptionNow";
        private readonly TopicClient _client;

        public Guid Guid { get; } = Guid.NewGuid();
        
        public PingManager(FileManager fileManager)
        {
            _fileManager = fileManager;
            var timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds)
            {
                AutoReset = true
            };
            timer.Elapsed += (sender, args) => SendStatus();
            timer.Start();

            _client = TopicClient.CreateFromConnectionString(ServiceBusConnectionString, TopicName);

            SetupPingNowListener();
        }

        private void SetupPingNowListener()
        {
            var nsm = NamespaceManager.CreateFromConnectionString(ServiceBusConnectionString);
            if (nsm.SubscriptionExists(TopicName, SubscriptionName))
            {
                nsm.DeleteSubscription(TopicName, SubscriptionName);
            }
            nsm.CreateSubscription(TopicName, SubscriptionName, new SqlFilter("sys.To = 'FileSender' AND ActionName = 'PingNow'"));

            var client = SubscriptionClient.CreateFromConnectionString(ServiceBusConnectionString, TopicName, SubscriptionName);
            client.OnMessage(ProcessPingNow);
        }

        private void ProcessPingNow(BrokeredMessage obj)
        {
            obj.Complete();
            SendStatus();
        }

        public void SendStatus()
        {
            var message = new BrokeredMessage(_fileManager.Status);
            message.Properties.Add("ActionName", "Ping");
            message.Properties.Add("Guid", Guid);
            message.To = "CentralQueueManager";
            _client.Send(message);
        }
    }
}