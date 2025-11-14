using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RecurringIntegrationsScheduler.Tests.TestCommon.Sftp
{
    /// <summary>
    /// Provides helpers for preparing remote folders on a localhost SFTP server used by integration tests.
    /// The server is assumed to be reachable at <c>localhost:22</c> with username <c>tester</c> and password <c>password</c>.
    /// </summary>
    public static class SftpTestEnvironment
    {
        public const string Host = "localhost";
        public const int Port = 22;
        public const string Username = "tester";
        public const string Password = "password";

        private const string RootFolder = "/ris-tests";

        /// <summary>
        /// Creates a unique remote folder for the specified scenario and removes any files inside it.
        /// </summary>
        public static string PrepareRemoteFolder(string scenario)
        {
            var folder = $"{RootFolder}/{scenario}/{Guid.NewGuid():N}";

            using var client = CreateClient();
            client.Connect();
            EnsureDirectory(client, folder);
            DeleteFiles(client, folder);
            client.Disconnect();

            return folder;
        }

        /// <summary>
        /// Uploads a file to the given remote folder, creating the folder tree if needed.
        /// </summary>
        public static void UploadFile(string remoteFolder, string fileName, byte[] contents)
        {
            using var stream = new MemoryStream(contents);
            UploadFile(remoteFolder, fileName, stream);
        }

        public static void UploadFile(string remoteFolder, string fileName, Stream contents)
        {
            using var client = CreateClient();
            client.Connect();
            EnsureDirectory(client, remoteFolder);
            var remotePath = CombineRemote(remoteFolder, fileName);
            client.UploadFile(contents, remotePath, true);
            client.Disconnect();
        }

        /// <summary>
        /// Returns the list of file names currently present in the target remote folder.
        /// </summary>
        public static IReadOnlyCollection<string> ListFiles(string remoteFolder)
        {
            using var client = CreateClient();
            client.Connect();
            EnsureDirectory(client, remoteFolder);
            var files = client.ListDirectory(remoteFolder)
                .Where(entry => !entry.IsDirectory && !entry.IsSymbolicLink)
                .Select(entry => entry.Name)
                .ToArray();
            client.Disconnect();
            return files;
        }

        /// <summary>
        /// Removes every file inside a remote folder if it exists.
        /// </summary>
        public static void ClearFolder(string remoteFolder)
        {
            using var client = CreateClient();
            client.Connect();
            if (client.Exists(remoteFolder))
            {
                DeleteFiles(client, remoteFolder);
            }
            client.Disconnect();
        }

        private static SftpClient CreateClient()
        {
            return new SftpClient(Host, Port, Username, Password);
        }

        private static void EnsureDirectory(SftpClient client, string remoteFolder)
        {
            var segments = remoteFolder.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var path = remoteFolder.StartsWith("/", StringComparison.Ordinal) ? "/" : string.Empty;
            foreach (var segment in segments)
            {
                path = CombineRemote(path, segment);
                if (!client.Exists(path))
                {
                    client.CreateDirectory(path);
                }
            }
        }

        private static void DeleteFiles(SftpClient client, string remoteFolder)
        {
            foreach (var entry in client.ListDirectory(remoteFolder))
            {
                if (entry.IsDirectory || entry.IsSymbolicLink)
                {
                    continue;
                }

                client.DeleteFile(entry.FullName);
            }
        }

        private static string CombineRemote(string part1, string part2)
        {
            if (string.IsNullOrEmpty(part1))
            {
                return "/" + part2.TrimStart('/');
            }

            if (part1.EndsWith("/", StringComparison.Ordinal))
            {
                return part1 + part2.TrimStart('/');
            }

            return part1 + "/" + part2.TrimStart('/');
        }
    }
}
