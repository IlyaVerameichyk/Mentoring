using System.IO;

namespace SystemWatcher.Interfaces
{
    public interface IFilesAnalyzer
    {
        bool IsLast(Stream fileStream);
    }
}