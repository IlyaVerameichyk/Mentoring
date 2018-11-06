using System;

namespace SystemWatcher.Models.Interfaces
{
    public interface IFileSystemWatcher
    {
        void StartWatching();
        void StopWatching();

        event EventHandler<FileEventArgs> FileCreated;
    }
}