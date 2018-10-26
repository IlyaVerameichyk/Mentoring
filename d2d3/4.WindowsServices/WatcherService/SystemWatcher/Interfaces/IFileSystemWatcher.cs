using System;
using SystemWatcher.Models;

namespace SystemWatcher.Interfaces
{
    public interface IFileSystemWatcher
    {
        void StartWatching();
        void StopWatching();

        event EventHandler<FileEventArgs> FileCreated;
    }
}