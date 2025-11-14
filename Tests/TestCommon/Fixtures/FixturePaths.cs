using System;
using System.IO;

namespace RecurringIntegrationsScheduler.Tests.TestCommon.Fixtures
{
    /// <summary>
    /// Centralized paths to fixture roots so tests can locate assets without hardcoding relative paths.
    /// </summary>
    public static class FixturePaths
    {
        private static readonly Lazy<string> SolutionRoot = new Lazy<string>(FindSolutionRoot, isThreadSafe: true);

        private static readonly Lazy<string> TestsRoot = new Lazy<string>(() => Path.Combine(SolutionRoot.Value, "Tests"), isThreadSafe: true);

        public static string TestDataRoot => Path.Combine(TestsRoot.Value, "TestData");
        public static string ConfigsRoot => Path.Combine(TestDataRoot, "Configs");
        public static string SchedulesRoot => Path.Combine(TestDataRoot, "Schedules");
        public static string FileSystemRoot => Path.Combine(TestDataRoot, "FileSystem");
        public static string SftpRoot => Path.Combine(TestDataRoot, "Sftp");

        private static string FindSolutionRoot()
        {
            var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            while (directory != null)
            {
                var candidate = Path.Combine(directory.FullName, "Recurring Integrations Scheduler.sln");
                if (File.Exists(candidate))
                {
                    return directory.FullName;
                }

                directory = directory.Parent;
            }

            throw new DirectoryNotFoundException("Unable to locate solution root starting from base directory.");
        }
    }
}
