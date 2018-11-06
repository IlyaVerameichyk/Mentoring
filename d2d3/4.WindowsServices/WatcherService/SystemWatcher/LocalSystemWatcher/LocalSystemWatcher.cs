using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemWatcher.Models;
using SystemWatcher.Models.Interfaces;

namespace SystemWatcher.LocalSystemWatcher
{
    public class LocalSystemWatcher : IFileSystemWatcher
    {
        private readonly IList<FileSystemWatcher> _fileSystemWatchers;

        public LocalSystemWatcher(string[] paths)
        {
            if (paths.Any(p => new Uri(p).IsUnc || !Directory.Exists(p)))
            {
                throw new ArgumentException("Paths must be a local existing directories");
            }
            _fileSystemWatchers = paths.Select(p => new FileSystemWatcher(p)).ToList();

        }

        public void StartWatching()
        {
            foreach (var fileSystemWatcher in _fileSystemWatchers)
            {
                fileSystemWatcher.EnableRaisingEvents = true;
                fileSystemWatcher.Created += OnFileCreated;
            }
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath;
            var file = new LocalFile(new FileInfo(filePath));
            FileCreated?.Invoke(this, new FileEventArgs(file));
        }

        public void StopWatching()
        {
            foreach (var fileSystemWatcher in _fileSystemWatchers)
            {
                fileSystemWatcher.EnableRaisingEvents = false;
                fileSystemWatcher.Created -= OnFileCreated;
            }
        }

        public event EventHandler<FileEventArgs> FileCreated;
    }
}