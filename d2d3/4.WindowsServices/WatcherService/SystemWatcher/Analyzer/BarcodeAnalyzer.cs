using System.Drawing;
using System.IO;
using SystemWatcher.Models.Interfaces;
using ZXing;

namespace SystemWatcher.Analyzer
{
    public class BarcodeAnalyzer : IFilesAnalyzer
    {
        public bool IsLast(Stream fileStream)
        {
            var barcodeReader = new BarcodeReader();
            var barcodeBitmap = (Bitmap)Image.FromStream(fileStream);

            var result = barcodeReader.Decode(barcodeBitmap);
            return result != null && result.BarcodeFormat == BarcodeFormat.QR_CODE;
        }
    }
}