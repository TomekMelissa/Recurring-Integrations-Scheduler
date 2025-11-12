/* Copyright (c) Microsoft Corporation. All rights reserved.
   Licensed under the MIT License. */

using System;

namespace RecurringIntegrationsScheduler.Common.Contracts
{
    /// <summary>
    /// Represents SFTP connection parameters serialized into job data map.
    /// </summary>
    public sealed class SftpConfiguration
    {
        public SftpConfiguration(
            string host,
            int port,
            string username,
            string password,
            bool usePrivateKey,
            string privateKeyPath,
            string privateKeyPassphrase,
            string remoteFolder,
            string fileMask)
        {
            Host = host;
            Port = port <= 0 ? 22 : port;
            Username = username;
            Password = password;
            UsePrivateKey = usePrivateKey;
            PrivateKeyPath = privateKeyPath;
            PrivateKeyPassphrase = privateKeyPassphrase;
            RemoteFolder = string.IsNullOrWhiteSpace(remoteFolder) ? "/" : remoteFolder.Trim();
            FileMask = string.IsNullOrWhiteSpace(fileMask) ? "*.*" : fileMask;
        }

        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(Host) &&
            (!UsePrivateKey && !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password)
             || UsePrivateKey && !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(PrivateKeyPath));

        public string Host { get; }

        public int Port { get; }

        public string Username { get; }

        public string Password { get; }

        public bool UsePrivateKey { get; }

        public string PrivateKeyPath { get; }

        public string PrivateKeyPassphrase { get; }

        public string RemoteFolder { get; }

        public string FileMask { get; }
    }
}
