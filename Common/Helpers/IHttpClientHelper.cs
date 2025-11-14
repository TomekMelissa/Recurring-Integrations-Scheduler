#nullable enable
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RecurringIntegrationsScheduler.Common.Helpers
{
    /// <summary>
    /// Abstraction over <see cref="HttpClientHelper"/> to support mocking in automated tests.
    /// </summary>
    public interface IHttpClientHelper : IDisposable
    {
        Task<HttpResponseMessage> PostStreamRequestAsync(Uri uri, Stream bodyStream, string? externalidentifier = null);
        Task<HttpResponseMessage> PostStringRequestAsync(Uri uri, string bodyString, string? externalidentifier = null);
        Task<HttpResponseMessage> GetRequestAsync(Uri uri, bool addAuthorization = true);
        Uri GetEnqueueUri(string? legalEntity = null);
        Uri GetDequeueUri();
        Uri GetAckUri();
        Uri GetJobStatusUri(string jobId);
        Task<HttpResponseMessage> GetAzureWriteUrl();
        Task<HttpResponseMessage> GetExecutionSummaryStatus(string executionId);
        Task<HttpResponseMessage> GetExportedPackageUrl(string executionId);
        Task<HttpResponseMessage> GetExecutionSummaryPageUrl(string executionId);
        Task<HttpResponseMessage> UploadContentsToBlob(Uri blobUrl, Stream stream);
        Task<HttpResponseMessage> ImportFromPackage(string packageUrl, string definitionGroupId, string executionId, bool execute, bool overwrite, string legalEntityId);
        Task<HttpResponseMessage> DeleteExecutionHistoryJob(string executionId);
        Task<HttpResponseMessage> ExportToPackage(string definitionGroupId, string packageName, string executionId, string legalEntityId, bool reExecute = false);
        Task<HttpResponseMessage> ExportFromPackage(string packageUrl, string definitionGroupId, string executionId, bool execute, bool overwrite, string legalEntityId);
        Task<HttpResponseMessage> GetMessageStatus(string messageId);
        Task<HttpResponseMessage> GenerateImportTargetErrorKeysFile(string executionId, string entityName);
        Task<HttpResponseMessage> GetImportTargetErrorKeysFileUrl(string executionId, string entityName);
        Task<HttpResponseMessage> GetExecutionErrors(string executionId);
    }
}
