using RecurringIntegrationsScheduler.Common.Helpers;
using RecurringIntegrationsScheduler.Common.JobSettings;
using System;

namespace RecurringIntegrationsScheduler.Tests.TestCommon.Mocks
{
    /// <summary>
    /// Helper factory that lets tests inject a custom <see cref="IHttpClientHelper"/> creator.
    /// </summary>
    public sealed class DelegatingHttpClientHelperFactory : IHttpClientHelperFactory
    {
        private readonly Func<Settings, IHttpClientHelper> _factory;

        public DelegatingHttpClientHelperFactory(Func<Settings, IHttpClientHelper> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IHttpClientHelper Create(Settings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            return _factory(settings);
        }
    }
}
