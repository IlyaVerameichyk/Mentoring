using System.Threading;

namespace Async.Task2
{
    public class DownloadEntry
    {
        public string Text { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public override string ToString()
        {
            return Text;
        }
    }
}