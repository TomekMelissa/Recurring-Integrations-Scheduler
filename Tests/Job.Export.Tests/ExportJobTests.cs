#pragma warning disable MSTEST0037
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Quartz;
using RecurringIntegrationsScheduler.Common.Contracts;
using RecurringIntegrationsScheduler.Common.Helpers;
using RecurringIntegrationsScheduler.Tests.TestCommon.Fixtures;
using RecurringIntegrationsScheduler.Tests.TestCommon.Mocks;
using RecurringIntegrationsScheduler.Tests.TestCommon.Quartz;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ExportJobType = RecurringIntegrationsScheduler.Job.Export;

namespace RecurringIntegrationsScheduler.JobExport.Tests
{
    [TestClass]
    public class ExportJobTests
    {
        [TestMethod]
        public async Task Execute_ExportsPackageAndUploadsToSftp()
        {
            var testDataRoot = FixtureManager.MaterializeTestData("ExportJobTests");
            var downloadDir = Path.Combine(testDataRoot, "FileSystem", "Export", "Staging");
            var errorDir = Path.Combine(testDataRoot, "FileSystem", "Export", "Delivered");

            var blobPayload = Encoding.UTF8.GetBytes("export payload");
            var executionId = "EXEC-777";
            var exportToPackageResponse = new HttpResponseMessage(HttpStatusCode.OK);

            var statusResponses = new[] { "Executing", "Succeeded" };
            var statusIndex = 0;

            var httpHelper = new StubHttpClientHelper
            {
                ExportToPackageHandler = (definition, packageName, execId, company, re) =>
                {
                    Assert.AreEqual(executionId, packageName);
                    return Task.FromResult(exportToPackageResponse);
                },
                GetExecutionSummaryStatusHandler = id =>
                {
                    var status = statusResponses[Math.Min(statusIndex++, statusResponses.Length - 1)];
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(status)
                    });
                },
                GetExportedPackageUrlHandler = id => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("https://blob.contoso.com/export/package.zip")
                }),
                GetRequestAsyncHandler = (uri, addAuth) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(blobPayload)
                })
            };

            var httpFactory = new DelegatingHttpClientHelperFactory(_ => httpHelper);

            var outboundUploadCount = 0;
            var sftpStub = new StubSftpTransferService
            {
                UploadFileHandler = (config, filePath, log) =>
                {
                    outboundUploadCount++;
                    Assert.IsTrue(File.Exists(filePath));
                }
            };

            var job = new ExportJobType(httpFactory, sftpStub);
            var context = TestSchedulerBuilder.CreateContext(job, CreateJobDataMap(downloadDir, errorDir));

            await job.Execute(context);

            Assert.AreEqual(1, outboundUploadCount, "Exported package should be uploaded exactly once.");
            Assert.AreEqual(1, Directory.GetFiles(downloadDir).Length, "Package should exist in download directory.");
        }

        private static JobDataMap CreateJobDataMap(string downloadDir, string errorDir)
        {
            return new JobDataMap
            {
                { SettingsConstants.AosUri, "https://test.operations.dynamics.com" },
                { SettingsConstants.AzureAuthEndpoint, "https://login.microsoftonline.com" },
                { SettingsConstants.AadTenant, "contoso.onmicrosoft.com" },
                { SettingsConstants.UseServiceAuthentication, true },
                { SettingsConstants.AadClientId, Guid.NewGuid().ToString() },
                { SettingsConstants.AadClientSecret, EncryptDecrypt.Encrypt("secret") },
                { SettingsConstants.DownloadSuccessDir, downloadDir },
                { SettingsConstants.DownloadErrorsDir, errorDir },
                { SettingsConstants.DataProject, "CustExport" },
                { SettingsConstants.Company, "USMF" },
                { SettingsConstants.DelayBetweenStatusCheck, 0 },
                { SettingsConstants.DelayBetweenFiles, 0 },
                { SettingsConstants.RetryCount, 1 },
                { SettingsConstants.RetryDelay, 1 },
                { SettingsConstants.UseSftpOutbound, true },
                { SettingsConstants.SftpOutboundHost, "sftp.contoso.test" },
                { SettingsConstants.SftpOutboundPort, 22 },
                { SettingsConstants.SftpOutboundUsername, "exporter" },
                { SettingsConstants.SftpOutboundPassword, EncryptDecrypt.Encrypt("secret") },
                { SettingsConstants.SftpOutboundRemoteFolder, "/outbound/export" },
                { SettingsConstants.SftpOutboundFileMask, "*.zip" },
                { SettingsConstants.AddTimestamp, false },
                { SettingsConstants.DeletePackage, false },
                { SettingsConstants.PauseJobOnException, false }
            };
        }
    }
}
#pragma warning restore MSTEST0037
