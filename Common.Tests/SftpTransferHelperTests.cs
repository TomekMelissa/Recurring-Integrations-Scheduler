using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecurringIntegrationsScheduler.Common.Helpers;
using System;
using System.IO;

namespace RecurringIntegrationsScheduler.Common.Tests
{
    [TestClass]
    public class SftpTransferHelperTests
    {
        [TestMethod]
        public void ResolveUniqueLocalPath_ReturnsPreferredName_WhenNoCollision()
        {
            var tempDir = CreateTempDirectory();
            try
            {
                var result = SftpTransferHelper.ResolveUniqueLocalPath(tempDir, "data.txt", log: null);
                Assert.AreEqual(Path.Combine(tempDir, "data.txt"), result);
            }
            finally
            {
                Directory.Delete(tempDir, recursive: true);
            }
        }

        [TestMethod]
        public void ResolveUniqueLocalPath_AppendsCounter_WhenCollisionDetected()
        {
            var tempDir = CreateTempDirectory();
            try
            {
                File.WriteAllText(Path.Combine(tempDir, "data.txt"), "existing");
                File.WriteAllText(Path.Combine(tempDir, "data_1.txt"), "existing");

                var result = SftpTransferHelper.ResolveUniqueLocalPath(tempDir, "data.txt", log: null);
                Assert.AreEqual(Path.Combine(tempDir, "data_2.txt"), result);
            }
            finally
            {
                Directory.Delete(tempDir, recursive: true);
            }
        }

        private static string CreateTempDirectory()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempPath);
            return tempPath;
        }
    }
}
