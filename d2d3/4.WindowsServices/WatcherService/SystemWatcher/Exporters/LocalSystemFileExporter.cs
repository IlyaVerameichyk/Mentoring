using System;
using System.IO;
using System.Linq;
using SystemWatcher.Interfaces;

namespace SystemWatcher.Exporters
{
    public class LocalSystemFileExporter : IFileExporter
    {
        private string _destinationPath;

        public LocalSystemFileExporter(string destinationPath)
        {
            if (!Directory.Exists(destinationPath))
            {
                throw new ArgumentException("Path must exist");
            }
            _destinationPath = destinationPath;
        }


        public void Export(IOrderedEnumerable<IFile> files)
        {
            foreach (var file in files)
            {
                file.Remove();
            }
        }
    }
}