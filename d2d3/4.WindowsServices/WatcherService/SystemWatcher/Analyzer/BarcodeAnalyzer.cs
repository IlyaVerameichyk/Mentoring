using System;
using System.Drawing;
using System.IO;
using SystemWatcher.Models.Interfaces;
using ZXing;

namespace SystemWatcher.Analyzer
{
    public class BarcodeAnalyzer : IFilesAnalyzer
    {
        private string _terminateText = "asdasd";

        public void SetTerminateText(string text)
        {
            _terminateText = text;
        }

        public bool IsLast(Stream fileStream)
        {
            var barcodeReader = new BarcodeReader();
            var barcodeBitmap = (Bitmap)Image.FromStream(fileStream);

            var result = barcodeReader.Decode(barcodeBitmap);
            return result != null && result.BarcodeFormat == BarcodeFormat.QR_CODE && _terminateText.Equals(result.Text, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}