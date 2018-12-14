using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SystemWatcher.Aspects;
using SystemWatcher.Models;
using SystemWatcher.Models.Interfaces;

namespace SystemWatcher
{
    public class FileManager
    {
        private static readonly string[] ImageExtensions = { "png", "jpg", "jpeg" };
        private readonly IDictionary<string, IList<IFile>> _filesCollections;
        private readonly IFileSystemWatcher _fileSystemWatcher;
        private readonly IFilesAnalyzer _filesAnalyzer;
        private readonly IFileExporter _filesExporter;

        public WatcherStatus Status { get; private set; }

        public FileManager(IFileSystemWatcher fileSystemWatcher, IFilesAnalyzer filesAnalyzer, IFileExporter filesExporter)
        {
            _fileSystemWatcher = fileSystemWatcher;
            _filesAnalyzer = filesAnalyzer;
            _filesExporter = filesExporter;
            _filesCollections = new ConcurrentDictionary<string, IList<IFile>>();
        }

        public void StartListen()
        {
            _fileSystemWatcher.FileCreated += OnFileCreated;
            _fileSystemWatcher.StartWatching();
            Status = WatcherStatus.Watching;
        }

        public void StopListen()
        {
            _fileSystemWatcher.StopWatching();
            _fileSystemWatcher.FileCreated -= OnFileCreated;
        }

        [Trace]
        private void OnFileCreated(object sender, FileEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Status = WatcherStatus.GeneratingFiles;
                var nameParser = new NameParser(Path.GetFileName(e.File.FullName));
                if (!ImageExtensions.Contains(nameParser.Extension, StringComparer.InvariantCultureIgnoreCase))
                {
                    Status = WatcherStatus.Watching;
                    return;
                }
                if (!_filesCollections.ContainsKey(nameParser.Prefix))
                {
                    _filesCollections.Add(nameParser.Prefix, new List<IFile>());
                }
                _filesCollections[nameParser.Prefix].Add(e.File);

                using (var stream = e.File.ReadFile())
                {
                    if (!_filesAnalyzer.IsLast(stream))
                    {
                        Status = WatcherStatus.Watching;
                        return;
                    }
                }

                Status = WatcherStatus.SendingFiles;
                _filesExporter.Export(_filesCollections[nameParser.Prefix]
                    .OrderBy(file => new NameParser(Path.GetFileName(e.File.FullName)).Number));
                _filesCollections.Remove(nameParser.Prefix);
                Status = WatcherStatus.Watching;
            });
        }
    }
}
