using log4net;
using RecurringIntegrationsScheduler.Common.Contracts;
using RecurringIntegrationsScheduler.Common.Helpers;
using System;
using System.Collections.Generic;

namespace RecurringIntegrationsScheduler.Tests.TestCommon.Mocks
{
    /// <summary>
    /// Simple delegate-driven stub for SFTP operations.
    /// </summary>
    public class StubSftpTransferService : ISftpTransferService
    {
        public Func<SftpConfiguration, string, ILog, IReadOnlyCollection<string>> DownloadFilesHandler { get; set; }
            = (config, folder, log) => Array.Empty<string>();

        public Action<SftpConfiguration, string, ILog> UploadFileHandler { get; set; }
            = (config, file, log) => { };

        public IReadOnlyCollection<string> DownloadFiles(SftpConfiguration configuration, string localFolder, ILog log)
        {
            return DownloadFilesHandler(configuration, localFolder, log);
        }

        public void UploadFile(SftpConfiguration configuration, string localFilePath, ILog log)
        {
            UploadFileHandler(configuration, localFilePath, log);
        }
    }
}
