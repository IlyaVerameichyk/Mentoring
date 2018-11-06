using System;
using System.IO;
using System.Linq;
using System.Threading;
using SystemWatcher.Models.Interfaces;

namespace SystemWatcher.LocalSystemWatcher
{
    public class LocalFile : IFile
    {
        private readonly FileSystemInfo _fileSystemInfo;
        public LocalFile(FileSystemInfo fileSystemInfo)
        {
            _fileSystemInfo = fileSystemInfo;
        }

        public string FullName => _fileSystemInfo.FullName;

        public DateTime CreationTime => _fileSystemInfo.CreationTime;

        public Stream ReadFile()
        {
            foreach (var i in Enumerable.Range(0, 3))
            {
                try
                {
                    var stream = new FileStream(_fileSystemInfo.FullName, FileMode.Open, FileAccess.Read,
                        FileShare.Read);
                    return stream;
                }
                catch (IOException)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                }
            }
            throw new IOException("File is used by another process");
        }

        public void Remove()
        {
            _fileSystemInfo.Delete();
        }
    }
}