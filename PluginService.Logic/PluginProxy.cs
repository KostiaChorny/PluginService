using PluginService.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PluginService.Logic
{
    /// <summary>
    /// Represents proxy object for plugin
    /// </summary>
    public class PluginProxy : MarshalByRefObject
    {
        /// <summary>
        /// Path to plugin assebly file
        /// </summary>
        public string AssemlyPath { get; private set; }
        /// <summary>
        /// Returns true when plugin has one valid entry point and be able to execute
        /// </summary>
        public bool IsValid { get; private set; }
        /// <summary>
        /// Entry point method. If plugin is not valid returns null
        /// </summary>
        public MethodInfo EntryPoint { get; private set; }

        /// <summary>
        /// Load plugin assembly to current appdomain and validate it 
        /// </summary>
        /// <param name="fullPath">Path to plugin assembly file (dll)</param>
        public void LoadPlugin(string fullPath)
        {
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Plugin file not found", fullPath);

            Assembly pluginAssembly = Assembly.LoadFrom(fullPath);
            List<MethodInfo> entryPoints = FindEntryPoints(pluginAssembly);
            if (entryPoints.Count == 1)
            {
                EntryPoint = entryPoints[0];
                IsValid = true;
            }
        }

        private List<MethodInfo> FindEntryPoints(Assembly pluginAssembly)
        {
            List<MethodInfo> entryPoints = new List<MethodInfo>();
            Type[] types = pluginAssembly.GetTypes();
            foreach (var type in types)
            {
                MethodInfo[] methods = type.GetMethods();
                foreach (var method in methods)
                {
                    object[] attrib = method.GetCustomAttributes(typeof(EntryPointAttribute), false);
                    if (attrib.Length == 1 &&
                        method.ReturnType == typeof(void) &&
                        !method.IsGenericMethod &&
                        method.IsStatic &&
                        method.GetParameters().Length == 0)
                    {
                        entryPoints.Add(method);
                    }
                }
            }
            return entryPoints;
        }

        /// <summary>
        /// It will execute the plugin if it is valid
        /// </summary>
        public void Run()
        {
            if (!IsValid || EntryPoint == null)
                throw new InvalidOperationException("Can't run invalid plugin. Entry point not found.");

            EntryPoint.Invoke(null, null);
        }
    }
}
