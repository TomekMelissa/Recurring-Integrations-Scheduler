using RecurringIntegrationsScheduler.Common.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace RecurringIntegrationsScheduler.Tests.TestCommon.Fixtures
{
    /// <summary>
    /// Utilities for copying fixture trees and customizing tokenized files.
    /// </summary>
    public static class FixtureManager
    {
        private const string EncryptedClientSecretToken = "{{EncryptedClientSecret}}";
        private const string EncryptedUserPasswordToken = "{{EncryptedUserPassword}}";
        private const string TestDataRootToken = "{{TestDataRoot}}";

        /// <summary>
        /// Copies <c>Tests/TestData</c> into a unique temp folder.
        /// </summary>
        public static string MaterializeTestData(string scenarioName)
        {
            if (string.IsNullOrWhiteSpace(scenarioName))
            {
                scenarioName = "Scenario";
            }

            var tempRoot = Path.Combine(Path.GetTempPath(), "RIS", "TestData", $"{scenarioName}_{Guid.NewGuid():N}");
            if (Directory.Exists(tempRoot))
            {
                Directory.Delete(tempRoot, recursive: true);
            }

            CopyDirectory(FixturePaths.TestDataRoot, tempRoot);
            return tempRoot;
        }

        /// <summary>
        /// Replaces the standard tokens inside an XML or config file.
        /// </summary>
        public static void ApplyStandardTokens(string filePath, string testDataRoot, string clientSecretPlainText, string userPasswordPlainText)
        {
            var tokens = BuildDefaultTokens(testDataRoot, clientSecretPlainText, userPasswordPlainText);
            ReplaceTokens(filePath, tokens);
        }

        /// <summary>
        /// Replaces arbitrary tokens inside the target file.
        /// </summary>
        public static void ReplaceTokens(string filePath, IDictionary<string, string> tokens)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Cannot apply fixture tokens because '{filePath}' does not exist.", filePath);
            }

            var contents = File.ReadAllText(filePath);
            foreach (var token in tokens)
            {
                contents = contents.Replace(token.Key, token.Value);
            }

            File.WriteAllText(filePath, contents);
        }

        /// <summary>
        /// Builds a token map for use with the schedule/config templates.
        /// </summary>
        public static IDictionary<string, string> BuildDefaultTokens(string testDataRoot, string clientSecretPlainText, string userPasswordPlainText)
        {
            if (string.IsNullOrWhiteSpace(testDataRoot))
            {
                throw new ArgumentException("Test data root must be provided", nameof(testDataRoot));
            }

            var tokens = new Dictionary<string, string>
            {
                { TestDataRootToken, testDataRoot.Replace('\\', '/') }
            };

            if (!string.IsNullOrEmpty(clientSecretPlainText))
            {
                tokens[EncryptedClientSecretToken] = EncryptDecrypt.Encrypt(clientSecretPlainText);
            }

            if (!string.IsNullOrEmpty(userPasswordPlainText))
            {
                tokens[EncryptedUserPasswordToken] = EncryptDecrypt.Encrypt(userPasswordPlainText);
            }

            return tokens;
        }

        private static void CopyDirectory(string sourceDir, string destinationDir)
        {
            var sourceInfo = new DirectoryInfo(sourceDir);
            if (!sourceInfo.Exists)
            {
                throw new DirectoryNotFoundException($"Fixture source directory '{sourceDir}' was not found.");
            }

            Directory.CreateDirectory(destinationDir);

            foreach (var file in sourceInfo.GetFiles())
            {
                var targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath, overwrite: true);
            }

            foreach (var subDir in sourceInfo.GetDirectories())
            {
                var nextDest = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, nextDest);
            }
        }
    }
}
