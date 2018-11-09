using System;
using System.IO;
using System.Linq;
using SystemWatcher.Models;
using SystemWatcher.Models.Interfaces;

namespace SystemWatcher.Exporters
{
    public class LocalSystemFileExporter : PdfExportedBase, IFileExporter
    {
        private readonly string _destinationPath;

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
            var path = Path.Combine(_destinationPath,
                new NameParser(Path.GetFileName(files.First().FullName)).Prefix + ".pdf");
            using (var fs = new FileStream(path, FileMode.Create))
            {
                using (var ms = GeneratePdf(files))
                {
                    ms.CopyTo(fs);
                }
            }
        }
    }
}