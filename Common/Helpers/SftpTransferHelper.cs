using log4net;
using RecurringIntegrationsScheduler.Common.Contracts;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RecurringIntegrationsScheduler.Common.Helpers
{
    public static class SftpTransferHelper
    {
        public static IReadOnlyCollection<string> DownloadFiles(
            SftpConfiguration configuration,
            string localFolder,
            ILog log)
        {
            if (configuration == null || !configuration.IsConfigured)
            {
                return Array.Empty<string>();
            }

            Directory.CreateDirectory(localFolder);
            var downloadedFiles = new List<string>();

            using (var client = CreateClient(configuration))
            {
                client.Connect();
                var entries = client.ListDirectory(configuration.RemoteFolder);
                foreach (var entry in entries.Where(e => !e.IsDirectory && !e.IsSymbolicLink))
                {
                    if (!MatchesMask(entry.Name, configuration.FileMask))
                    {
                        continue;
                    }

                    var localPath = ResolveUniqueLocalPath(localFolder, entry.Name, log);
                    using (var targetStream = File.Create(localPath))
                    {
                        client.DownloadFile(entry.FullName, targetStream);
                    }
                    client.DeleteFile(entry.FullName);
                    downloadedFiles.Add(localPath);
                    log?.InfoFormat(CultureInfo.InvariantCulture,
                        "SFTP download completed for remote file '{0}'.",
                        entry.FullName);
                }
                client.Disconnect();
            }

            return downloadedFiles;
        }

        public static void UploadFile(
            SftpConfiguration configuration,
            string localFilePath,
            ILog log)
        {
            if (configuration == null || !configuration.IsConfigured)
            {
                return;
            }

            using (var client = CreateClient(configuration))
            using (var sourceStream = File.OpenRead(localFilePath))
            {
                client.Connect();
                EnsureRemoteFolder(client, configuration.RemoteFolder);
                var remotePath = CombineRemotePath(configuration.RemoteFolder, Path.GetFileName(localFilePath));
                client.UploadFile(sourceStream, remotePath, true);
                log?.InfoFormat(CultureInfo.InvariantCulture,
                    "SFTP upload completed for local file '{0}'.",
                    localFilePath);
                client.Disconnect();
            }
        }

        public static void ValidateConnection(SftpConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (!configuration.IsConfigured)
            {
                throw new InvalidOperationException("SFTP configuration is incomplete.");
            }

            using (var client = CreateClient(configuration))
            {
                client.ConnectionInfo.Timeout = TimeSpan.FromSeconds(10);
                client.OperationTimeout = TimeSpan.FromSeconds(10);
                client.Connect();

                if (!client.IsConnected)
                {
                    throw new InvalidOperationException("Unable to establish SFTP connection.");
                }

                var probePath = string.IsNullOrWhiteSpace(configuration.RemoteFolder) ? "/" : configuration.RemoteFolder;
                client.ListDirectory(probePath);

                client.Disconnect();
            }
        }

        private static SftpClient CreateClient(SftpConfiguration configuration)
        {
            if (configuration.UsePrivateKey)
            {
                var keyFiles = string.IsNullOrWhiteSpace(configuration.PrivateKeyPassphrase)
                    ? new[] { new PrivateKeyFile(configuration.PrivateKeyPath) }
                    : new[] { new PrivateKeyFile(configuration.PrivateKeyPath, configuration.PrivateKeyPassphrase) };

                var auth = new PrivateKeyAuthenticationMethod(configuration.Username, keyFiles);
                var connectionInfo = new ConnectionInfo(configuration.Host, configuration.Port, configuration.Username, auth);
                return new SftpClient(connectionInfo);
            }

            var passwordAuth = new PasswordAuthenticationMethod(configuration.Username, configuration.Password);
            var info = new ConnectionInfo(configuration.Host, configuration.Port, configuration.Username, passwordAuth);
            return new SftpClient(info);
        }

        private static void EnsureRemoteFolder(SftpClient client, string remoteFolder)
        {
            if (client.Exists(remoteFolder))
            {
                return;
            }

            var segments = remoteFolder.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var path = remoteFolder.StartsWith("/", StringComparison.Ordinal) ? "/" : string.Empty;
            foreach (var segment in segments)
            {
                path = CombineRemotePath(path, segment);
                if (!client.Exists(path))
                {
                    client.CreateDirectory(path);
                }
            }
        }

        internal static string ResolveUniqueLocalPath(string localFolder, string fileName, ILog log)
        {
            Directory.CreateDirectory(localFolder);

            var safeFileName = Path.GetFileName(fileName) ?? fileName;
            var basePath = Path.Combine(localFolder, safeFileName);

            if (!File.Exists(basePath))
            {
                return basePath;
            }

            var nameWithoutExtension = Path.GetFileNameWithoutExtension(safeFileName);
            if (string.IsNullOrEmpty(nameWithoutExtension))
            {
                nameWithoutExtension = safeFileName;
            }
            var extension = Path.GetExtension(safeFileName) ?? string.Empty;

            var counter = 1;
            string candidatePath;
            do
            {
                var candidateName = string.Format(CultureInfo.InvariantCulture, "{0}_{1}{2}", nameWithoutExtension, counter, extension);
                candidatePath = Path.Combine(localFolder, candidateName);
                counter++;
            } while (File.Exists(candidatePath));

            log?.WarnFormat(CultureInfo.InvariantCulture,
                "Local file '{0}' already exists. Using '{1}' instead to avoid overwriting.",
                basePath,
                candidatePath);

            return candidatePath;
        }

        private static string CombineRemotePath(string part1, string part2)
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

        private static bool MatchesMask(string input, string mask)
        {
            if (string.IsNullOrWhiteSpace(mask) || mask == "*")
            {
                return true;
            }

            var pattern = "^" + Regex.Escape(mask)
                .Replace(@"\*", ".*")
                .Replace(@"\?", ".") + "$";

            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }
    }
}
