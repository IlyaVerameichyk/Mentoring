﻿using System;
using SystemWatcher.Interfaces;

namespace SystemWatcher.Models
{
    public class FileEventArgs : EventArgs
    {
        public FileEventArgs(IFile file)
        {
            File = file;
        }
        public IFile File { get; }
    }
}