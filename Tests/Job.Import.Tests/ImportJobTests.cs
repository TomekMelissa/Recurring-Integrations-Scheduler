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
using ImportJobType = RecurringIntegrationsScheduler.Job.Import;

namespace RecurringIntegrationsScheduler.JobImport.Tests
{
    [TestClass]
    public class ImportJobTests
    {
        [TestMethod]
        public async Task Execute_SubmitsPackageForDerivedLegalEntity()
        {
            var testDataRoot = FixtureManager.MaterializeTestData("ImportJobTests");
            var inputDir = Path.Combine(testDataRoot, "FileSystem", "Upload", "Input");
            var successDir = Path.Combine(testDataRoot, "FileSystem", "Upload", "Working");
            var errorDir = Path.Combine(testDataRoot, "FileSystem", "Upload", "History");

            var legalEntityFolder = Path.Combine(inputDir, "USMF");
            Directory.CreateDirectory(legalEntityFolder);
            var packagePath = Path.Combine(legalEntityFolder, "customers.zip");
            File.WriteAllText(packagePath, "payload");

            var dequeueBlobUrl = "https://blob.contoso.com/upload/customers.zip";
            var getAzureResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    value = JsonConvert.SerializeObject(new { BlobUrl = dequeueBlobUrl })
                }), Encoding.UTF8, "application/json")
            };

            var blobUploadCallCount = 0;
            var importCallCount = 0;
            string capturedLegalEntity = null;

            var httpHelper = new StubHttpClientHelper
            {
                GetAzureWriteUrlHandler = () => Task.FromResult(getAzureResponse),
                UploadContentsToBlobHandler = (uri, stream) =>
                {
                    blobUploadCallCount++;
                    Assert.AreEqual(dequeueBlobUrl, uri.AbsoluteUri);
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                },
                ImportFromPackageHandler = (packageUrl, defGroup, executionId, execute, overwrite, legalEntity) =>
                {
                    importCallCount++;
                    capturedLegalEntity = legalEntity;
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("{\"value\":\"EXEC-123\"}")
                    });
                }
            };

            var httpFactory = new DelegatingHttpClientHelperFactory(_ => httpHelper);
            var sftpStub = new StubSftpTransferService();

            var job = new ImportJobType(httpFactory, sftpStub);
            var context = TestSchedulerBuilder.CreateContext(job, CreateJobDataMap(inputDir, successDir, errorDir));

            await job.Execute(context);

            Assert.AreEqual(1, blobUploadCallCount, "Blob upload should occur once.");
            Assert.AreEqual(1, importCallCount, "ImportFromPackage should be invoked once.");
            Assert.AreEqual("USMF", capturedLegalEntity, "Legal entity should be derived from subfolder name.");
            Assert.IsFalse(File.Exists(packagePath), "Original package should be moved out of the input folder.");
            Assert.IsTrue(File.Exists(Path.Combine(successDir, "customers.zip")), "Package should land in the success folder.");
        }

        private static JobDataMap CreateJobDataMap(string inputDir, string successDir, string errorDir)
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
            map.Put(SettingsConstants.UploadErrorsDir, errorDir);
            map.Put(SettingsConstants.StatusFileExtension, "status");
            map.Put(SettingsConstants.SearchPattern, "*.zip");
            map.Put(SettingsConstants.OrderBy, OrderByOptions.Created.ToString());
            map.Put(SettingsConstants.DataProject, "Customers");
            map.Put(SettingsConstants.ExecuteImport, true);
            map.Put(SettingsConstants.OverwriteDataProject, false);
            map.Put(SettingsConstants.InputFilesArePackages, true);
            map.Put(SettingsConstants.MultiCompanyImport, true);
            map.Put(SettingsConstants.GetLegalEntityFromSubfolder, true);
            map.Put(SettingsConstants.GetLegalEntityFromFilename, false);
            map.Put(SettingsConstants.DelayBetweenFiles, 0);
            map.Put(SettingsConstants.RetryCount, 1);
            map.Put(SettingsConstants.RetryDelay, 1);
            map.Put(SettingsConstants.PauseJobOnException, false);
            return map;
        }
    }
}
