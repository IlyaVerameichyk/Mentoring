using System.Linq;

namespace SystemWatcher.Models.Interfaces
{
    public interface IFileExporter
    {
        void Export(IOrderedEnumerable<IFile> files);
    }
}