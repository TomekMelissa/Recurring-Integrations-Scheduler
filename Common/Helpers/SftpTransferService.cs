using log4net;
using RecurringIntegrationsScheduler.Common.Contracts;
using System.Collections.Generic;

namespace RecurringIntegrationsScheduler.Common.Helpers
{
    /// <summary>
    /// Default implementation of <see cref="ISftpTransferService"/> that delegates to <see cref="SftpTransferHelper"/>.
    /// </summary>
    public sealed class SftpTransferService : ISftpTransferService
    {
        public static ISftpTransferService Instance { get; } = new SftpTransferService();

        private SftpTransferService()
        {
        }

        public IReadOnlyCollection<string> DownloadFiles(SftpConfiguration configuration, string localFolder, ILog log)
        {
            return SftpTransferHelper.DownloadFiles(configuration, localFolder, log);
        }

        public void UploadFile(SftpConfiguration configuration, string localFilePath, ILog log)
        {
            SftpTransferHelper.UploadFile(configuration, localFilePath, log);
        }
    }
}
