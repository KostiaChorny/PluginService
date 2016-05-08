using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace PluginService
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            InitializeComponent();
            Installers.Add(new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            });
            Installers.Add(new ServiceInstaller
            {
                ServiceName = "PluginService",
                Description = "Service automaticaly loads plugins",
                StartType = ServiceStartMode.Automatic
            });
        }
    }
}
