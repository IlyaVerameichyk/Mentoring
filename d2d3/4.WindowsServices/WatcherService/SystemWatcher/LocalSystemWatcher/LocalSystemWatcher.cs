using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemWatcher.Models;
using SystemWatcher.Models.Interfaces;

namespace SystemWatcher.LocalSystemWatcher
{
    public class LocalSystemWatcher : IFileSystemWatcher
    {
        private string[] _paths;
        private string _badFilesDirectory;
        private IList<FileSystemWatcher> _fileSystemWatchers;

        public LocalSystemWatcher(string badFilesDirectory, string[] paths)
        {
            _badFilesDirectory = badFilesDirectory;
            SetPaths(paths);
        }

        public void SetPaths(string[] paths)
        {
            _paths = paths;
            if (paths.Any(p => new Uri(p).IsUnc || !Directory.Exists(p)))
            {
                throw new ArgumentException("Paths must be a local existing directories");
            }
        }

        public void StartWatching()
        {
            _fileSystemWatchers = _paths.Select(p => new FileSystemWatcher(p)).ToList();

            foreach (var fileSystemWatcher in _fileSystemWatchers)
            {
                fileSystemWatcher.EnableRaisingEvents = true;
                fileSystemWatcher.Created += OnFileCreated;
            }
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            var attr = File.GetAttributes(e.FullPath);
            // is directory
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory) { return; }

            var filePath = e.FullPath;
            var file = new LocalFile(new FileInfo(filePath));

            try
            {
                using (var stream = file.ReadFile())
                {
                    var imgInput = System.Drawing.Image.FromStream(stream);
                }
            }
            catch (Exception)
            {
                if (!Directory.Exists(_badFilesDirectory))
                {
                    Directory.CreateDirectory(_badFilesDirectory);
                }
                File.Move(filePath, Path.Combine(_badFilesDirectory, Path.GetFileName(filePath)));
            }


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