#nullable enable
using RecurringIntegrationsScheduler.Common.Helpers;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecurringIntegrationsScheduler.Tests.TestCommon.Mocks
{
    /// <summary>
    /// Lightweight stub that lets tests plug delegates for each HTTP helper interaction.
    /// Only the members configured by the test will execute; others throw to surface gaps.
    /// </summary>
    public class StubHttpClientHelper : IHttpClientHelper
    {
        public Func<Uri, Stream, string?, Task<HttpResponseMessage>> PostStreamRequestAsyncHandler { get; set; } = (u, s, id) => throw new NotImplementedException(nameof(PostStreamRequestAsync));
        public Func<Uri, string, string?, Task<HttpResponseMessage>> PostStringRequestAsyncHandler { get; set; } = (u, body, id) => throw new NotImplementedException(nameof(PostStringRequestAsync));
        public Func<Uri, bool, Task<HttpResponseMessage>> GetRequestAsyncHandler { get; set; } = (u, auth) => throw new NotImplementedException(nameof(GetRequestAsync));
        public Func<string?, Uri> GetEnqueueUriHandler { get; set; } = legalEntity => throw new NotImplementedException(nameof(GetEnqueueUri));
        public Func<Uri> GetDequeueUriHandler { get; set; } = () => throw new NotImplementedException(nameof(GetDequeueUri));
        public Func<Uri> GetAckUriHandler { get; set; } = () => throw new NotImplementedException(nameof(GetAckUri));
        public Func<string, Uri> GetJobStatusUriHandler { get; set; } = jobId => throw new NotImplementedException(nameof(GetJobStatusUri));
        public Func<Task<HttpResponseMessage>> GetAzureWriteUrlHandler { get; set; } = () => throw new NotImplementedException(nameof(GetAzureWriteUrl));
        public Func<string, Task<HttpResponseMessage>> GetExecutionSummaryStatusHandler { get; set; } = id => throw new NotImplementedException(nameof(GetExecutionSummaryStatus));
        public Func<string, Task<HttpResponseMessage>> GetExportedPackageUrlHandler { get; set; } = id => throw new NotImplementedException(nameof(GetExportedPackageUrl));
        public Func<string, Task<HttpResponseMessage>> GetExecutionSummaryPageUrlHandler { get; set; } = id => throw new NotImplementedException(nameof(GetExecutionSummaryPageUrl));
        public Func<Uri, Stream, Task<HttpResponseMessage>> UploadContentsToBlobHandler { get; set; } = (uri, stream) => throw new NotImplementedException(nameof(UploadContentsToBlob));
        public Func<string, string, string, bool, bool, string, Task<HttpResponseMessage>> ImportFromPackageHandler { get; set; } = (url, def, exec, execute, overwrite, le) => throw new NotImplementedException(nameof(ImportFromPackage));
        public Func<string, Task<HttpResponseMessage>> DeleteExecutionHistoryJobHandler { get; set; } = id => throw new NotImplementedException(nameof(DeleteExecutionHistoryJob));
        public Func<string, string, string, string, bool, Task<HttpResponseMessage>> ExportToPackageHandler { get; set; } = (def, package, exec, company, re) => throw new NotImplementedException(nameof(ExportToPackage));
        public Func<string, string, string, bool, bool, string, Task<HttpResponseMessage>> ExportFromPackageHandler { get; set; } = (url, def, exec, execute, overwrite, company) => throw new NotImplementedException(nameof(ExportFromPackage));
        public Func<string, Task<HttpResponseMessage>> GetMessageStatusHandler { get; set; } = id => throw new NotImplementedException(nameof(GetMessageStatus));
        public Func<string, string, Task<HttpResponseMessage>> GenerateImportTargetErrorKeysFileHandler { get; set; } = (id, entity) => throw new NotImplementedException(nameof(GenerateImportTargetErrorKeysFile));
        public Func<string, string, Task<HttpResponseMessage>> GetImportTargetErrorKeysFileUrlHandler { get; set; } = (id, entity) => throw new NotImplementedException(nameof(GetImportTargetErrorKeysFileUrl));
        public Func<string, Task<HttpResponseMessage>> GetExecutionErrorsHandler { get; set; } = id => throw new NotImplementedException(nameof(GetExecutionErrors));
        public Action? DisposeAction { get; set; }

        public Task<HttpResponseMessage> PostStreamRequestAsync(Uri uri, Stream bodyStream, string? externalidentifier = null)
            => PostStreamRequestAsyncHandler(uri, bodyStream, externalidentifier);

        public Task<HttpResponseMessage> PostStringRequestAsync(Uri uri, string bodyString, string? externalidentifier = null)
            => PostStringRequestAsyncHandler(uri, bodyString, externalidentifier);

        public Task<HttpResponseMessage> GetRequestAsync(Uri uri, bool addAuthorization = true)
            => GetRequestAsyncHandler(uri, addAuthorization);

        public Uri GetEnqueueUri(string? legalEntity = null)
            => GetEnqueueUriHandler(legalEntity);

        public Uri GetDequeueUri()
            => GetDequeueUriHandler();

        public Uri GetAckUri()
            => GetAckUriHandler();

        public Uri GetJobStatusUri(string jobId)
            => GetJobStatusUriHandler(jobId);

        public Task<HttpResponseMessage> GetAzureWriteUrl()
            => GetAzureWriteUrlHandler();

        public Task<HttpResponseMessage> GetExecutionSummaryStatus(string executionId)
            => GetExecutionSummaryStatusHandler(executionId);

        public Task<HttpResponseMessage> GetExportedPackageUrl(string executionId)
            => GetExportedPackageUrlHandler(executionId);

        public Task<HttpResponseMessage> GetExecutionSummaryPageUrl(string executionId)
            => GetExecutionSummaryPageUrlHandler(executionId);

        public Task<HttpResponseMessage> UploadContentsToBlob(Uri blobUrl, Stream stream)
            => UploadContentsToBlobHandler(blobUrl, stream);

        public Task<HttpResponseMessage> ImportFromPackage(string packageUrl, string definitionGroupId, string executionId, bool execute, bool overwrite, string legalEntityId)
            => ImportFromPackageHandler(packageUrl, definitionGroupId, executionId, execute, overwrite, legalEntityId);

        public Task<HttpResponseMessage> DeleteExecutionHistoryJob(string executionId)
            => DeleteExecutionHistoryJobHandler(executionId);

        public Task<HttpResponseMessage> ExportToPackage(string definitionGroupId, string packageName, string executionId, string legalEntityId, bool reExecute = false)
            => ExportToPackageHandler(definitionGroupId, packageName, executionId, legalEntityId, reExecute);

        public Task<HttpResponseMessage> ExportFromPackage(string packageUrl, string definitionGroupId, string executionId, bool execute, bool overwrite, string legalEntityId)
            => ExportFromPackageHandler(packageUrl, definitionGroupId, executionId, execute, overwrite, legalEntityId);

        public Task<HttpResponseMessage> GetMessageStatus(string messageId)
            => GetMessageStatusHandler(messageId);

        public Task<HttpResponseMessage> GenerateImportTargetErrorKeysFile(string executionId, string entityName)
            => GenerateImportTargetErrorKeysFileHandler(executionId, entityName);

        public Task<HttpResponseMessage> GetImportTargetErrorKeysFileUrl(string executionId, string entityName)
            => GetImportTargetErrorKeysFileUrlHandler(executionId, entityName);

        public Task<HttpResponseMessage> GetExecutionErrors(string executionId)
            => GetExecutionErrorsHandler(executionId);

        public void Dispose()
        {
            DisposeAction?.Invoke();
        }
    }
}
