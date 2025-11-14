using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RecurringIntegrationsScheduler.Scheduler.Tests
{
    [TestClass]
    public static class AssemblyResolver
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (_, args) =>
            {
                var assemblyName = new AssemblyName(args.Name).Name;
                if (assemblyName != null && assemblyName.StartsWith("RecurringIntegrationsScheduler.Job", StringComparison.OrdinalIgnoreCase))
                {
                    var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var candidate = Path.Combine(baseDir ?? string.Empty, assemblyName + ".dll");
                    if (File.Exists(candidate))
                    {
                        return Assembly.LoadFrom(candidate);
                    }
                }
                return null;
            };
        }
    }
}
