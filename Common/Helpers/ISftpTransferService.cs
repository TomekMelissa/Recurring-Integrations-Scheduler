using log4net;
using RecurringIntegrationsScheduler.Common.Contracts;
using System.Collections.Generic;

namespace RecurringIntegrationsScheduler.Common.Helpers
{
    /// <summary>
    /// Abstraction for SFTP transfer operations so jobs can be tested without real servers.
    /// </summary>
    public interface ISftpTransferService
    {
        IReadOnlyCollection<string> DownloadFiles(SftpConfiguration configuration, string localFolder, ILog log);

        void UploadFile(SftpConfiguration configuration, string localFilePath, ILog log);
    }
}
