using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using QueueManager.Models;

namespace QueueManager.FileReceiving
{
    public class FileManager
    {
        private readonly QueueReader _queueReader;
        private readonly IDictionary<string, List<BrokeredMessageParser>> _receivedMessages;

        public FileManager(QueueReader queueReader)
        {
            _receivedMessages = new Dictionary<string, List<BrokeredMessageParser>>();
            _queueReader = queueReader;
            _queueReader.MessageReceived += MessageReceived;
            _queueReader.StartListen();
        }

        private void MessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var parser = messageEventArgs.BrokeredMessageParser;
            if (!_receivedMessages.ContainsKey(parser.FileName))
            {
                _receivedMessages.Add(parser.FileName, new List<BrokeredMessageParser>());
            }

            _receivedMessages[parser.FileName].Add(parser);
            if (parser.Size == _receivedMessages[parser.FileName].Count)
            {
                GenerateFile(parser.FileName);
            }
        }

        private void GenerateFile(string fileName)
        {
            var messages = _receivedMessages[fileName].OrderBy(p => p.Position);
            using (var resultFileMemoryStream = new FileStream(Path.Combine(ConfigurationManager.AppSettings["ExportFileDirectory"], $"{fileName}.pdf"), FileMode.Create))
            {
                foreach (var messageParser in messages)
                {
                    using (var ms = messageParser.GetStream())
                    {
                        ms.CopyTo(resultFileMemoryStream);
                    }
                }
            }
            _receivedMessages[fileName].Clear();
        }
    }
}