using System;
using System.IO;
using System.Linq;
using SystemWatcher.Interfaces;
using SystemWatcher.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;

namespace SystemWatcher.Exporters
{
    public class LocalSystemFileExporter : IFileExporter
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
            var document = new Document();
            var section = document.AddSection();
            foreach (var file in files)
            {
                section.AddImage(file.FullName);
            }

            var pdfRenderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always) { Document = document };
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save(Path.Combine(_destinationPath, new NameParser(Path.GetFileName(files.First().FullName)).Prefix+ ".pdf"));
            foreach (var file in files)
            {
                file.Remove();
            }
        }
    }
}