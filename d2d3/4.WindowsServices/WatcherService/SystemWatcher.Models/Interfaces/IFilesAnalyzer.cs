using System.IO;

namespace SystemWatcher.Models.Interfaces
{
    public interface IFilesAnalyzer
    {
        bool IsLast(Stream fileStream);
    }
}