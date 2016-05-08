using System;
using System.ServiceProcess;
using PluginService.Logic;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace PluginService
{
    public partial class PluginService : ServiceBase
    {
        PluginWatcher watcher;
        string pluginPath;

        public PluginService()
        {
            InitializeComponent();

            try
            {
                pluginPath = ConfigurationManager.AppSettings["PluginDirectory"];
                watcher = new PluginWatcher(pluginPath);
                watcher.Error += Watcher_Error;
                watcher.Success += Watcher_Success;
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Service initialization error: {ex.Message}");
                throw;
            }

        }

        protected override void OnStart(string[] args)
        {
            try
            {
                watcher.Start();
                ExecPlugins(pluginPath);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Service start error: {ex.Message}");
                throw;
            }
        }

        private void ExecPlugins(string pluginPath)
        {
            string[] files = Directory.GetFiles(pluginPath, "*.dll");
            foreach (var file in files)
            {
                try
                {
                    if (Plugin.Run(file))
                    {
                        Trace.TraceInformation($"Plugin {file} successfully executed");
                    }                    
                }
                catch (Exception ex)
                {
                    Trace.TraceWarning($"Unable to run the plugin: {ex.Message}");
                }
            }
        }

        protected override void OnStop()
        {
            watcher.Stop();
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }

        private void Watcher_Error(object sender, LoadPluginErrorEventArgs e)
        {
            Trace.TraceWarning($"Plugin watcher can't exec plugin with error: {e.Exception.Message}");
        }

        private void Watcher_Success(object sender, LoadPluginSuccessEventArgs e)
        {
            Trace.TraceInformation($"Plugin {e.FullPath} successfully executed");
        }
    }
}
