#pragma warning disable MSTEST0037
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using RecurringIntegrationsScheduler.Common.Contracts;
using RecurringIntegrationsScheduler.Common.Helpers;
using RecurringIntegrationsScheduler.Tests.TestCommon.Fixtures;
using RecurringIntegrationsScheduler.Tests.TestCommon.Mocks;
using RecurringIntegrationsScheduler.Tests.TestCommon.Quartz;
using RecurringIntegrationsScheduler.Tests.TestCommon.Sftp;
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
            var exportToPackageResponse = new HttpResponseMessage(HttpStatusCode.OK);

            var statusResponses = new[] { "Executing", "Succeeded" };
            var statusIndex = 0;

            static HttpResponseMessage JsonValue(string value)
            {
                var payload = $"{{\"value\":\"{value}\"}}";
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(payload, Encoding.UTF8, "application/json")
                };
            }

            var httpHelper = new StubHttpClientHelper
            {
                ExportToPackageHandler = (definition, packageName, execId, company, re) =>
                {
                    Assert.AreEqual(packageName, execId, "Export job should use the generated execution id as package name.");
                    return Task.FromResult(exportToPackageResponse);
                },
                GetExecutionSummaryStatusHandler = id =>
                {
                    var status = statusResponses[Math.Min(statusIndex++, statusResponses.Length - 1)];
                    return Task.FromResult(JsonValue(status));
                },
                GetExportedPackageUrlHandler = id =>
                    Task.FromResult(JsonValue("https://blob.contoso.com/export/package.zip")),
                GetRequestAsyncHandler = (uri, addAuth) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(blobPayload)
                })
            };

            var httpFactory = new DelegatingHttpClientHelperFactory(_ => httpHelper);

            var remoteOutboundFolder = SftpTestEnvironment.PrepareRemoteFolder("ExportJobOutbound");

            var job = new ExportJobType(httpFactory, SftpTransferService.Instance);
            var context = TestSchedulerBuilder.CreateContext(job, CreateJobDataMap(downloadDir, errorDir, remoteOutboundFolder));

            await job.Execute(context);

            var uploadedDir = Path.Combine(downloadDir, "Uploaded");
            Assert.IsTrue(Directory.Exists(uploadedDir), "Uploaded folder should be created after SFTP push.");
            Assert.AreEqual(1, Directory.GetFiles(uploadedDir).Length, "Package should exist in the Uploaded folder.");
            Assert.AreEqual(1, SftpTestEnvironment.ListFiles(remoteOutboundFolder).Count, "SFTP outbound folder should contain the uploaded package.");
        }

        private static JobDataMap CreateJobDataMap(string downloadDir, string errorDir, string remoteOutboundFolder)
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
                { SettingsConstants.SftpOutboundHost, SftpTestEnvironment.Host },
                { SettingsConstants.SftpOutboundPort, SftpTestEnvironment.Port },
                { SettingsConstants.SftpOutboundUsername, SftpTestEnvironment.Username },
                { SettingsConstants.SftpOutboundPassword, EncryptDecrypt.Encrypt(SftpTestEnvironment.Password) },
                { SettingsConstants.SftpOutboundRemoteFolder, remoteOutboundFolder },
                { SettingsConstants.SftpOutboundFileMask, "*.zip" },
                { SettingsConstants.AddTimestamp, false },
                { SettingsConstants.DeletePackage, false },
                { SettingsConstants.PauseJobOnException, false }
            };
        }
    }
}
#pragma warning restore MSTEST0037
