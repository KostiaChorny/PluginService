using System;
using PluginService.Infrastructure;
using System.IO;

namespace Plugin
{
    public class Class1
    {
        [EntryPoint]
        public static void Method()
        {
            using (StreamWriter writer = new StreamWriter("file.txt", true))
            {
                writer.WriteLine("Plugin loaded");
                writer.WriteLine(AppDomain.CurrentDomain.FriendlyName);
            }                
        }
    }
}
