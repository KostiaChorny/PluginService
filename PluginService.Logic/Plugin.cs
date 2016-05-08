using System;
using System.IO;
using System.Reflection;

namespace PluginService.Logic
{
    /// <summary>
    /// This class represetns dynamically linked plugin
    /// </summary>
    public class Plugin : IDisposable
    {
        AppDomain pluginDomain;
        PluginProxy proxy;

        /// <summary>
        /// Returns true when plugin has one valid entry point and be able to execute
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Creates new instance of Plugin class
        /// </summary>
        /// <param name="fullPath">Path to plugin assembly file (dll)</param>
        public Plugin(string fullPath)
        {
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Plugin file not found", fullPath);

            pluginDomain = AppDomain.CreateDomain(Path.GetFileName(fullPath));
            LoadPlugin(fullPath);
        }

        private void LoadPlugin(string fullPath)
        {
            Assembly proxyAssembly = pluginDomain.Load(typeof(PluginProxy).Assembly.GetName());
            proxy = pluginDomain.CreateInstanceAndUnwrap(proxyAssembly.FullName, typeof(PluginProxy).FullName) as PluginProxy;
            proxy.LoadPlugin(fullPath);
            IsValid = proxy.IsValid;
        }

        /// <summary>
        /// It will execute the plugin if it is valid
        /// </summary>
        public void Run()
        {
            if (!IsValid)
                throw new InvalidOperationException("Can't invoke invalid plugin");

            proxy.Run();
        }

        /// <summary>
        /// Executes plugin from file
        /// </summary>
        /// <param name="fullPath">Path to plugin assembly file (dll)</param>
        /// <returns>Returns true if plugin run successfully or false</returns>
        public static bool Run(string fullPath)
        {
            using (Plugin plugin = new Plugin(fullPath))
            {
                if (plugin.IsValid)
                {
                    plugin.Run();
                    return true;
                }
                else
                {
                    return false;
                }                    
            }
        }

        /// <summary>
        /// Dispose managed state and unload plugin application domain
        /// </summary>
        public void Dispose()
        {
            AppDomain.Unload(pluginDomain);
        }
    }
}
