using System;
using RecurringIntegrationsScheduler.Common.JobSettings;

namespace RecurringIntegrationsScheduler.Common.Helpers
{
    /// <summary>
    /// Factory abstraction for creating <see cref="IHttpClientHelper"/> instances.
    /// </summary>
    public interface IHttpClientHelperFactory
    {
        IHttpClientHelper Create(Settings settings);
    }

    /// <summary>
    /// Default implementation that returns the concrete <see cref="HttpClientHelper"/>.
    /// </summary>
    public sealed class HttpClientHelperFactory : IHttpClientHelperFactory
    {
        public static IHttpClientHelperFactory Default { get; } = new HttpClientHelperFactory();

        public IHttpClientHelper Create(Settings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            return new HttpClientHelper(settings);
        }
    }
}
