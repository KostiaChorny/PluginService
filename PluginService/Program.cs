using System;
using System.ServiceProcess;

namespace PluginService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                PluginService service = new PluginService();
                service.TestStartupAndStop(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new PluginService()
                };
                ServiceBase.Run(ServicesToRun);
            }

        }
    }
}
