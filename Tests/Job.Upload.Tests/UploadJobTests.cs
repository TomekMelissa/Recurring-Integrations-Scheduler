using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using RecurringIntegrationsScheduler.Common.Contracts;
using RecurringIntegrationsScheduler.Common.Helpers;
using RecurringIntegrationsScheduler.Tests.TestCommon.Fixtures;
using RecurringIntegrationsScheduler.Tests.TestCommon.Mocks;
using RecurringIntegrationsScheduler.Tests.TestCommon.Sftp;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UploadJobType = RecurringIntegrationsScheduler.Job.Upload;

namespace RecurringIntegrationsScheduler.JobUpload.Tests
{
    [TestClass]
    public class UploadJobTests
    {
        [TestMethod]
        public async Task Execute_DownloadsFilesFromSftpAndMovesToSuccess()
        {
            var testDataRoot = FixtureManager.MaterializeTestData("UploadJobTests");
            var inputDir = Path.Combine(testDataRoot, "FileSystem", "Upload", "Input");
            var successDir = Path.Combine(testDataRoot, "FileSystem", "Upload", "Working");
            var errorsDir = Path.Combine(testDataRoot, "FileSystem", "Upload", "History");

            var httpHelper = new StubHttpClientHelper
            {
                GetEnqueueUriHandler = _ => new Uri("https://test.operations.dynamics.com/enqueue"),
                PostStreamRequestAsyncHandler = (uri, stream, externalId) =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("MSG-001")
                    };
                    return Task.FromResult(response);
                }
            };
            var httpFactory = new DelegatingHttpClientHelperFactory(_ => httpHelper);

            var inboundRemoteFolder = SftpTestEnvironment.PrepareRemoteFolder("UploadJobInbound");
            SftpTestEnvironment.UploadFile(inboundRemoteFolder, "customers.zip", Encoding.UTF8.GetBytes("payload"));

            var job = new UploadJobType(httpFactory, SftpTransferService.Instance);
            var context = CreateJobExecutionContext(job, CreateJobDataMap(inputDir, successDir, errorsDir, inboundRemoteFolder));

            await job.Execute(context);

            Assert.IsTrue(File.Exists(Path.Combine(successDir, "customers.zip")), "Uploaded file should be moved to success folder.");
            Assert.IsFalse(File.Exists(Path.Combine(inputDir, "customers.zip")), "Input file should be moved out of the input folder.");
            Assert.AreEqual(0, SftpTestEnvironment.ListFiles(inboundRemoteFolder).Count, "Inbound SFTP folder should be empty after download.");
        }

        private static JobDataMap CreateJobDataMap(string inputDir, string successDir, string errorsDir, string remoteInboundFolder)
        {
            var map = new JobDataMap();
            map.Put(SettingsConstants.AosUri, "https://test.operations.dynamics.com");
            map.Put(SettingsConstants.AzureAuthEndpoint, "https://login.microsoftonline.com");
            map.Put(SettingsConstants.AadTenant, "contoso.onmicrosoft.com");
            map.Put(SettingsConstants.UseServiceAuthentication, true);
            map.Put(SettingsConstants.AadClientId, Guid.NewGuid().ToString());
            map.Put(SettingsConstants.AadClientSecret, EncryptDecrypt.Encrypt("secret"));
            map.Put(SettingsConstants.InputDir, inputDir);
            map.Put(SettingsConstants.UploadSuccessDir, successDir);
            map.Put(SettingsConstants.UploadErrorsDir, errorsDir);
            map.Put(SettingsConstants.EntityName, "Customers");
            map.Put(SettingsConstants.IsDataPackage, true);
            map.Put(SettingsConstants.ActivityId, Guid.NewGuid().ToString());
            map.Put(SettingsConstants.StatusFileExtension, "status");
            map.Put(SettingsConstants.SearchPattern, "*.zip");
            map.Put(SettingsConstants.OrderBy, OrderByOptions.Created.ToString());
            map.Put(SettingsConstants.ProcessingJobPresent, false);
            map.Put(SettingsConstants.DelayBetweenFiles, 0);
            map.Put(SettingsConstants.RetryCount, 1);
            map.Put(SettingsConstants.RetryDelay, 1);
            map.Put(SettingsConstants.UseSftpInbound, true);
            map.Put(SettingsConstants.SftpInboundHost, SftpTestEnvironment.Host);
            map.Put(SettingsConstants.SftpInboundPort, SftpTestEnvironment.Port);
            map.Put(SettingsConstants.SftpInboundUsername, SftpTestEnvironment.Username);
            map.Put(SettingsConstants.SftpInboundPassword, EncryptDecrypt.Encrypt(SftpTestEnvironment.Password));
            map.Put(SettingsConstants.SftpInboundRemoteFolder, remoteInboundFolder);
            map.Put(SettingsConstants.SftpInboundFileMask, "*.zip");
            map.Put(SettingsConstants.UploadInOrder, true);
            map.Put(SettingsConstants.LogVerbose, false);
            return map;
        }

        private static IJobExecutionContext CreateJobExecutionContext(UploadJobType job, JobDataMap map)
        {
            var jobDetail = JobBuilder.Create<UploadJobType>()
                .WithIdentity("UploadTestJob", "Tests")
                .UsingJobData(map)
                .Build();

            var trigger = (IOperableTrigger)TriggerBuilder.Create()
                .WithIdentity("UploadTestTrigger", "Tests")
                .StartNow()
                .Build();

            var bundle = new TriggerFiredBundle(jobDetail, trigger, null, false, DateTimeOffset.UtcNow, null, null, null);
            var scheduler = new Mock<IScheduler>();
            return new JobExecutionContextImpl(scheduler.Object, bundle, job);
        }
    }
}
