﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using SystemWatcher.Models;
using SystemWatcher.Models.Interfaces;
using Microsoft.ServiceBus.Messaging;

namespace SystemWatcher.Exporters
{
    public class ServiceBusExporter : PdfExportedBase, IFileExporter
    {
        private const int BufferSize = 250 * 1024;
        private const string QueueName = "filequeue";

        public void Export(IOrderedEnumerable<IFile> files)
        {
            using (var pdfStream = GeneratePdf(files))
            {
                var queueClient = QueueClient.Create(QueueName);
                var messageName = new NameParser(Path.GetFileName(files.First().FullName)).Prefix;
                var byteChunks = SplitMemoryStream(pdfStream);

                var index = 0;
                var partitionKey = Guid.NewGuid().ToString();
                using (var transaction = new TransactionScope())
                {
                    foreach (var byteChunk in byteChunks)
                    {
                        var message = new BrokeredMessage(byteChunk)
                        {
                            Label = messageName,
                            PartitionKey = partitionKey, // for transaction purposes
                            Properties =
                            {
                                { "Size" , byteChunks.Count },
                                { "Position", index }
                            }
                        };
                        queueClient.Send(message);
                        index++;
                    }
                    transaction.Complete();
                }
                queueClient.Close();
            }
        }

        private List<byte[]> SplitMemoryStream(Stream ms)
        {
            var chunks = new List<byte[]>();
            var chunk = new byte[BufferSize];
            var bytesRead = ms.Read(chunk, 0, BufferSize);
            while (bytesRead > 0)
            {
                if (bytesRead != BufferSize)
                {
                    var tail = new byte[bytesRead];
                    Array.Copy(chunk, tail, bytesRead);
                    chunk = tail;
                    chunks.Add(chunk);
                    break;
                }
                chunks.Add(chunk);
                chunk = new byte[BufferSize];
                bytesRead = ms.Read(chunk, 0, BufferSize);
            }
            return chunks;
        }
    }
}