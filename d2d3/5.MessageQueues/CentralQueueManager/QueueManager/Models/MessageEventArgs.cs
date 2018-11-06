using System;

namespace QueueManager.Models
{
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(BrokeredMessageParser brokeredMessageParser)
        {
            BrokeredMessageParser = brokeredMessageParser;
        }

        public BrokeredMessageParser BrokeredMessageParser { get; }
    }
}