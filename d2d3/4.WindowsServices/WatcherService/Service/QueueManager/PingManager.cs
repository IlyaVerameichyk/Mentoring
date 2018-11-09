using System;
using System.Timers;
using SystemWatcher;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Service.QueueManager
{
    public class PingManager
    {
        private readonly FileManager _fileManager;
        private readonly TopicClient _client;

        private const string TopicName = "settingstopic";
        private const string SubscriptionName = "PingSubscriptionNow";

        public Guid Guid { get; } = Guid.NewGuid();
        
        public PingManager(FileManager fileManager)
        {
            _fileManager = fileManager;
            _client = TopicClient.Create(TopicName);

            SetupPingOverTime();
            SetupPingNowListener();
        }

        private void SetupPingOverTime()
        {
            var timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds)
            {
                AutoReset = true
            };
            timer.Elapsed += (sender, args) => SendStatus();
            timer.Start();
        }

        private void SetupPingNowListener()
        {
            var nsm = NamespaceManager.Create();
            if (!nsm.SubscriptionExists(TopicName, SubscriptionName))
            {
                nsm.CreateSubscription(TopicName, SubscriptionName, new SqlFilter("sys.To = 'FileSender' AND ActionName = 'PingNow'"));
            }

            var client = SubscriptionClient.Create(TopicName, SubscriptionName);
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