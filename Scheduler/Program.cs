/* Copyright (c) Microsoft Corporation. All rights reserved.
   Licensed under the MIT License. */

using Bluegrams.Application;
using log4net;
using log4net.Config;
using RecurringIntegrationsScheduler.Forms;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace RecurringIntegrationsScheduler
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ConfigureLogging();
            PortableSettingsProvider.ApplyProvider(Properties.Settings.Default);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static void ConfigureLogging()
        {
            var repository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
            if (!repository.Configured)
            {
                XmlConfigurator.Configure(repository, new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            }
        }
    }
}
