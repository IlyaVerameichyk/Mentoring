using System.IO;
using System.Linq;
using SystemWatcher.Models.Interfaces;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;

namespace SystemWatcher.Exporters
{
    public class PdfExportedBase
    {
        protected Stream GeneratePdf(IOrderedEnumerable<IFile> files)
        {
            var document = new Document();
            var section = document.AddSection();
            foreach (var file in files)
            {
                section.AddImage(file.FullName);
            }

            var pdfRenderer = new PdfDocumentRenderer(true, PdfFontEmbedding.Always) { Document = document };
            pdfRenderer.RenderDocument();
            var ms = new MemoryStream();
            pdfRenderer.PdfDocument.Save(ms, false);
            foreach (var file in files)
            {
                file.Remove();
            }
            return ms;
        }
    }
}