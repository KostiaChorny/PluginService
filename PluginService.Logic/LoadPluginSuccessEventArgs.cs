using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginService.Logic
{
    public class LoadPluginSuccessEventArgs : EventArgs
    {
        public string FullPath { get; }

        public LoadPluginSuccessEventArgs(string fullPath)
        {
            FullPath = fullPath;
        }
    }
}
