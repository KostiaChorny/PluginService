using System;
using System.IO;

namespace PluginService.Logic
{
    /// <summary>
    /// Class for tracking plugin folder
    /// </summary>
    public class PluginWatcher
    {
        FileSystemWatcher watcher;

        public event EventHandler<LoadPluginErrorEventArgs> Error;
        public event EventHandler<LoadPluginSuccessEventArgs> Success;
        /// <summary>
        /// Creates PluginWatcher instance
        /// </summary>
        /// <param name="path">Path to tracked folder</param>
        public PluginWatcher(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Directory {path} not found");

            watcher = new FileSystemWatcher(path, "*.dll");
            watcher.Created += Watcher_CreatedOrChanged;
            watcher.Changed += Watcher_CreatedOrChanged;
        }

        /// <summary>
        /// Starts watcher
        /// </summary>
        public void Start()
        {
            watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Stops watcher
        /// </summary>
        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
        }

        private void Watcher_CreatedOrChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                watcher.EnableRaisingEvents = false;
                if (Plugin.Run(e.FullPath))
                {
                    OnSuccess(e.FullPath);
                }                
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
            finally
            {
                watcher.EnableRaisingEvents = true;
            }
        }

        protected virtual void OnError(Exception ex)
        {
            if (Error != null)
            {
                Error.Invoke(this, new LoadPluginErrorEventArgs(ex));
            }
        }

        protected virtual void OnSuccess(string fullPath)
        {
            if (Success != null)
            {
                Success.Invoke(this, new LoadPluginSuccessEventArgs(fullPath));
            }
        }
    }
}
