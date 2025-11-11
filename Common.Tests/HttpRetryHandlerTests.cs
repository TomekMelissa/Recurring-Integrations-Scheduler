using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecurringIntegrationsScheduler.Common.Helpers;
using RecurringIntegrationsScheduler.Common.JobSettings;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RecurringIntegrationsScheduler.Common.Tests
{
    [TestClass]
    public class HttpRetryHandlerTests
    {
        private class TestHandler : HttpMessageHandler
        {
            private readonly Func<HttpRequestMessage, int, Task<HttpResponseMessage>> _handler;
            private int _callCount;

            public TestHandler(Func<HttpRequestMessage, int, Task<HttpResponseMessage>> handler)
            {
                _handler = handler;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                _callCount++;
                return _handler(request, _callCount);
            }
        }

        [TestMethod]
        public async Task Retries_On_Transient_Status_Code()
        {
            var settings = CreateSettings<DownloadJobSettings>(retryCount: 3, retryDelay: 1, jobKey: "RetryTest");

            var handler = new HttpRetryHandler(new TestHandler((_, attempt) =>
            {
                if (attempt < 2)
                {
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                }
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }), settings);

            var invoker = new HttpMessageInvoker(handler);
            var response = await invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost"), CancellationToken.None);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Stops_Retrying_When_Max_Attempts_Reached()
        {
            var settings = CreateSettings<DownloadJobSettings>(retryCount: 2, retryDelay: 1, jobKey: "RetryFail");

            var handler = new HttpRetryHandler(new TestHandler((_, __) =>
            {
                return Task.FromResult(new HttpResponseMessage((HttpStatusCode)429));
            }), settings);

            var invoker = new HttpMessageInvoker(handler);
            var response = await invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost"), CancellationToken.None);
            Assert.AreEqual((HttpStatusCode)429, response.StatusCode);
        }

        private static T CreateSettings<T>(int retryCount, int retryDelay, string jobKey) where T : Settings, new()
        {
            var settings = new T
            {
                RetryCount = retryCount,
                RetryDelay = retryDelay
            };

            var jobKeyProperty = typeof(Settings).GetProperty("JobKey", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            jobKeyProperty?.SetValue(settings, jobKey);
            return settings;
        }
    }
}
