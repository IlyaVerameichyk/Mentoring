using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemWatcher.Interfaces;
using SystemWatcher.Models;

namespace SystemWatcher
{
    public class FileManager
    {
        private readonly IDictionary<string, IList<IFile>> _filesCollections;
        private readonly IFileSystemWatcher _fileSystemWatcher;
        private readonly IFilesAnalyzer _filesAnalyzer;
        private readonly IFileExporter _filesExporter;

        public FileManager(IFileSystemWatcher fileSystemWatcher, IFilesAnalyzer filesAnalyzer, IFileExporter filesExporter)
        {
            _fileSystemWatcher = fileSystemWatcher;
            _filesAnalyzer = filesAnalyzer;
            _filesExporter = filesExporter;
            _filesCollections = new Dictionary<string, IList<IFile>>();

            _fileSystemWatcher.FileCreated += OnFileCreated;
            _fileSystemWatcher.StartWatching();
        }

        public void StopListen()
        {
            _fileSystemWatcher.StopWatching();
            _fileSystemWatcher.FileCreated -= OnFileCreated;
        }

        private void OnFileCreated(object sender, FileEventArgs e)
        {
            var nameParser = new NameParser(Path.GetFileName(e.File.FullName));

            if (!_filesCollections.ContainsKey(nameParser.Prefix))
            {
                _filesCollections.Add(nameParser.Prefix, new List<IFile>());
            }
            _filesCollections[nameParser.Prefix].Add(e.File);

            using (var stream = e.File.ReadFile())
            {
                if (!_filesAnalyzer.IsLast(stream)) { return; }
            }

            _filesExporter.Export(_filesCollections[nameParser.Prefix].OrderBy(file => new NameParser(Path.GetFileName(e.File.FullName)).Number));
            _filesCollections.Remove(nameParser.Prefix);
        }
    }
}
