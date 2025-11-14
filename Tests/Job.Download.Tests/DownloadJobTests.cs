#pragma warning disable MSTEST0037
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Quartz;
using RecurringIntegrationsScheduler.Common.Contracts;
using RecurringIntegrationsScheduler.Common.Helpers;
using RecurringIntegrationsScheduler.Tests.TestCommon.Fixtures;
using RecurringIntegrationsScheduler.Tests.TestCommon.Mocks;
using RecurringIntegrationsScheduler.Tests.TestCommon.Quartz;
using RecurringIntegrationsScheduler.Tests.TestCommon.Sftp;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DownloadJobType = RecurringIntegrationsScheduler.Job.Download;

namespace RecurringIntegrationsScheduler.JobDownload.Tests
{
    [TestClass]
    public class DownloadJobTests
    {
        [TestMethod]
        public async Task Execute_AcksSuccessfulDownloadsAndUploadsToSftp()
        {
            var testDataRoot = FixtureManager.MaterializeTestData("DownloadJobTests");
            var successDir = Path.Combine(testDataRoot, "FileSystem", "Download", "Dropbox");
            var errorsDir = Path.Combine(testDataRoot, "FileSystem", "Download", "Errors");

            var dequeueUri = new Uri("https://test.operations.dynamics.com/dequeue");
            var ackUri = new Uri("https://test.operations.dynamics.com/ack");
            var downloadUri = new Uri("https://storage.contoso.com/packages/customers.zip");

            var dequeuedMessage = new DataMessage
            {
                MessageId = "MSG-42",
                Name = "customers.zip",
                DownloadLocation = downloadUri.AbsoluteUri,
                CorrelationId = Guid.NewGuid().ToString(),
                PopReceipt = "receipt",
                MessageStatus = MessageStatus.Enqueued,
                DataJobState = DataJobState.Enqueued
            };

            var dequeued = false;
            var ackCallCount = 0;
            var httpHelper = new StubHttpClientHelper
            {
                GetDequeueUriHandler = () => dequeueUri,
                GetAckUriHandler = () => ackUri,
                GetRequestAsyncHandler = (uri, addAuth) =>
                {
                    if (UriEquals(uri, dequeueUri))
                    {
                        if (dequeued)
                        {
                            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NoContent));
                        }

                        dequeued = true;
                        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(JsonConvert.SerializeObject(dequeuedMessage), Encoding.UTF8, "application/json")
                        });
                    }

                    if (UriEquals(uri, downloadUri))
                    {
                        var packageBytes = Encoding.UTF8.GetBytes("payload");
                        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ByteArrayContent(packageBytes)
                        });
                    }

                    throw new InvalidOperationException($"Unexpected GET request to {uri}");
                },
                PostStringRequestAsyncHandler = (uri, body, id) =>
                {
                    if (!UriEquals(uri, ackUri))
                    {
                        throw new InvalidOperationException("Unexpected ACK endpoint");
                    }

                    ackCallCount++;
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                }
            };

            var httpFactory = new DelegatingHttpClientHelperFactory(_ => httpHelper);

            var remoteOutboundFolder = SftpTestEnvironment.PrepareRemoteFolder("DownloadJobOutbound");

            var job = new DownloadJobType(httpFactory, SftpTransferService.Instance);
            var context = TestSchedulerBuilder.CreateContext(job, CreateJobDataMap(successDir, errorsDir, remoteOutboundFolder));

            await job.Execute(context);

            Assert.AreEqual(1, ackCallCount, "Download acknowledgement should be sent once.");
            var uploadedDir = Path.Combine(successDir, "Uploaded");
            Assert.IsTrue(Directory.Exists(uploadedDir), "Uploaded folder should be created.");
            Assert.IsTrue(Directory.GetFiles(uploadedDir).Length == 1, "Uploaded folder should contain the delivered package.");
            Assert.IsFalse(Directory.EnumerateFiles(errorsDir, "*.zip").Any(), "No error files should be created.");
            Assert.AreEqual(1, SftpTestEnvironment.ListFiles(remoteOutboundFolder).Count, "SFTP outbound folder should contain the uploaded package.");
        }

        private static JobDataMap CreateJobDataMap(string successDir, string errorsDir, string remoteOutboundFolder)
        {
            var map = new JobDataMap();
            map.Put(SettingsConstants.AosUri, "https://test.operations.dynamics.com");
            map.Put(SettingsConstants.AzureAuthEndpoint, "https://login.microsoftonline.com");
            map.Put(SettingsConstants.AadTenant, "contoso.onmicrosoft.com");
            map.Put(SettingsConstants.UseServiceAuthentication, true);
            map.Put(SettingsConstants.AadClientId, Guid.NewGuid().ToString());
            map.Put(SettingsConstants.AadClientSecret, EncryptDecrypt.Encrypt("secret"));
            map.Put(SettingsConstants.ActivityId, Guid.NewGuid().ToString());
            map.Put(SettingsConstants.DownloadSuccessDir, successDir);
            map.Put(SettingsConstants.DownloadErrorsDir, errorsDir);
            map.Put(SettingsConstants.UseSftpOutbound, true);
            map.Put(SettingsConstants.SftpOutboundHost, SftpTestEnvironment.Host);
            map.Put(SettingsConstants.SftpOutboundPort, SftpTestEnvironment.Port);
            map.Put(SettingsConstants.SftpOutboundUsername, SftpTestEnvironment.Username);
            map.Put(SettingsConstants.SftpOutboundPassword, EncryptDecrypt.Encrypt(SftpTestEnvironment.Password));
            map.Put(SettingsConstants.SftpOutboundRemoteFolder, remoteOutboundFolder);
            map.Put(SettingsConstants.SftpOutboundFileMask, "*.zip");
            map.Put(SettingsConstants.UnzipPackage, false);
            map.Put(SettingsConstants.AddTimestamp, false);
            map.Put(SettingsConstants.DeletePackage, false);
            map.Put(SettingsConstants.RetryCount, 1);
            map.Put(SettingsConstants.RetryDelay, 1);
            map.Put(SettingsConstants.DelayBetweenStatusCheck, 1);
            map.Put(SettingsConstants.PauseJobOnException, false);
            return map;
        }
        private static bool UriEquals(Uri left, Uri right)
        {
            return string.Equals(left?.AbsoluteUri, right?.AbsoluteUri, StringComparison.OrdinalIgnoreCase);
        }
    }
}
#pragma warning restore MSTEST0037
