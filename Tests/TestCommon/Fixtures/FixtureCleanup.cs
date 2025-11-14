using System;
using System.Diagnostics;
using System.IO;

namespace RecurringIntegrationsScheduler.Tests.TestCommon.Fixtures
{
    /// <summary>
    /// Provides helper routines for clearing materialized fixture trees to avoid stale data between test runs.
    /// </summary>
    public static class FixtureCleanup
    {
        private static readonly string FixtureRoot = Path.Combine(Path.GetTempPath(), "RIS", "TestData");

        /// <summary>
        /// Deletes all previously materialized fixture directories under the shared test-data root.
        /// </summary>
        public static void DeleteAllMaterializedTestData()
        {
            if (!Directory.Exists(FixtureRoot))
            {
                return;
            }

            foreach (var directory in Directory.GetDirectories(FixtureRoot))
            {
                TryDelete(directory);
            }
        }

        private static void TryDelete(string directory)
        {
            try
            {
                Directory.Delete(directory, recursive: true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FixtureCleanup: failed to delete '{directory}'. {ex.Message}");
            }
        }
    }
}
