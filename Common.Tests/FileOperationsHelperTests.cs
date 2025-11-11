using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecurringIntegrationsScheduler.Common.Contracts;
using RecurringIntegrationsScheduler.Common.Helpers;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace RecurringIntegrationsScheduler.Common.Tests
{
    [TestClass]
    public class FileOperationsHelperTests
    {
        [TestMethod]
        public void GetFiles_SortsByFileNameAscending()
        {
            var tempDir = CreateTempDirectory();
            try
            {
                File.WriteAllText(Path.Combine(tempDir, "b.txt"), "b");
                File.WriteAllText(Path.Combine(tempDir, "a.txt"), "a");

                var files = FileOperationsHelper.GetFiles(MessageStatus.Enqueued, tempDir, "*.txt", SearchOption.TopDirectoryOnly, OrderByOptions.FileName).ToList();

                Assert.AreEqual(2, files.Count);
                Assert.AreEqual("a.txt", files[0].Name);
                Assert.AreEqual("b.txt", files[1].Name);
            }
            finally
            {
                Directory.Delete(tempDir, recursive: true);
            }
        }

        [TestMethod]
        public void UnzipPackage_SkipsEntriesOutsideBaseDirectory()
        {
            var tempDir = CreateTempDirectory();
            var zipPath = Path.Combine(tempDir, "test.zip");
            try
            {
                using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
                {
                    var safe = archive.CreateEntry("safe.txt");
                    using (var writer = new StreamWriter(safe.Open()))
                    {
                        writer.Write("ok");
                    }

                    var traversal = archive.CreateEntry("..\\evil.txt");
                    using (var writer = new StreamWriter(traversal.Open()))
                    {
                        writer.Write("bad");
                    }
                }

                FileOperationsHelper.UnzipPackage(zipPath, deletePackage: false, addTimestamp: false);

                Assert.IsTrue(File.Exists(Path.Combine(tempDir, "safe.txt")), "Expected safe entry to be extracted.");
                Assert.IsFalse(File.Exists(Path.Combine(Directory.GetParent(tempDir)!.FullName, "evil.txt")), "Traversal entry should not escape base directory.");
            }
            finally
            {
                Directory.Delete(tempDir, recursive: true);
            }
        }

        private static string CreateTempDirectory()
        {
            var path = Path.Combine(Path.GetTempPath(), "RIS-Tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
