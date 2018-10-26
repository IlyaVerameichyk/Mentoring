using System;
using System.IO;

namespace SystemWatcher.Interfaces
{
    public interface IFile
    {
        string FullName { get; }
        DateTime CreationTime { get; }
        Stream ReadFile();
        void Remove();
    }
}