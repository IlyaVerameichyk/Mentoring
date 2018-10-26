using System.Linq;

namespace SystemWatcher.Interfaces
{
    public interface IFileExporter
    {
        void Export(IOrderedEnumerable<IFile> files);
    }
}