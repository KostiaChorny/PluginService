using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginService.Logic
{
    public class LoadPluginErrorEventArgs : EventArgs
    {
        public Exception Exception { get; }

        public LoadPluginErrorEventArgs(Exception ex)
        {
            Exception = ex;
        }
    }
}
