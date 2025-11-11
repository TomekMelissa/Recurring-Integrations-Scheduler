//https://github.com/quartznet/quartznet
#nullable enable
using System;
using System.Collections.Specialized;
using System.Configuration;
using log4net;
using System.Text.RegularExpressions;

namespace RecurringIntegrationsScheduler.Server
{
	/// <summary>
	/// Configuration for the Quartz server.
	/// </summary>
	public class Configuration
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Configuration));

		private const string PrefixServerConfiguration = "quartz.server";
		private const string KeyServiceName = PrefixServerConfiguration + ".serviceName";
		private const string KeyServiceDisplayName = PrefixServerConfiguration + ".serviceDisplayName";
		private const string KeyServiceDescription = PrefixServerConfiguration + ".serviceDescription";
        private const string KeyServerImplementationType = PrefixServerConfiguration + ".type";
		
		private const string DefaultServiceName = "QuartzServer";
		private const string DefaultServiceDisplayName = "Quartz Server";
		private const string DefaultServiceDescription = "Quartz Job Scheduling Server";
        private const int MaxServiceDisplayLength = 256;
        private const int MaxServiceDescriptionLength = 512;
        private static readonly Regex ServiceNameRegex = new Regex(@"^[A-Za-z0-9_.-]+$", RegexOptions.Compiled);
	    private static readonly string DefaultServerImplementationType = typeof(QuartzServer).AssemblyQualifiedName!;

	    private static readonly NameValueCollection? configuration;

        /// <summary>
        /// Initializes the <see cref="Configuration"/> class.
        /// </summary>
		static Configuration()
		{
			try
			{
				configuration = (NameValueCollection) ConfigurationManager.GetSection("quartz");
			}
			catch (Exception e)
			{
				log.Warn("could not read configuration using ConfigurationManager.GetSection: " + e.Message);
			}
		}

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
		public static string ServiceName => GetConfigurationOrDefault(KeyServiceName, DefaultServiceName, IsValidServiceName, "service name");

	    /// <summary>
        /// Gets the display name of the service.
        /// </summary>
        /// <value>The display name of the service.</value>
		public static string ServiceDisplayName => GetConfigurationOrDefault(KeyServiceDisplayName, DefaultServiceDisplayName, value => value.Length <= MaxServiceDisplayLength, "service display name");

	    /// <summary>
        /// Gets the service description.
        /// </summary>
        /// <value>The service description.</value>
		public static string ServiceDescription => GetConfigurationOrDefault(KeyServiceDescription, DefaultServiceDescription, value => value.Length <= MaxServiceDescriptionLength, "service description");

	    /// <summary>
        /// Gets the type name of the server implementation.
        /// </summary>
        /// <value>The type of the server implementation.</value>
	    public static string ServerImplementationType => GetServerImplementationType();

	    /// <summary>
		/// Returns configuration value with given key. If configuration
		/// for the does not exists, return the default value.
		/// </summary>
		/// <param name="configurationKey">Key to read configuration with.</param>
		/// <param name="defaultValue">Default value to return if configuration is not found</param>
		/// <returns>The configuration value.</returns>
		private static string GetConfigurationOrDefault(string configurationKey, string defaultValue, Func<string, bool>? validator = null, string? propertyName = null)
		{
			var rawValue = configuration?[configurationKey];
			if (string.IsNullOrWhiteSpace(rawValue))
			{
				return defaultValue;
			}

			var trimmedValue = rawValue!.Trim();
			if (validator != null && !validator(trimmedValue))
			{
				if (!string.IsNullOrEmpty(propertyName))
				{
					log.Warn($"Invalid {propertyName} '{trimmedValue}'. Falling back to default value '{defaultValue}'.");
				}
				return defaultValue;
			}

			return trimmedValue;
		}

        private static bool IsValidServiceName(string value)
        {
            return value.Length <= MaxServiceDisplayLength && ServiceNameRegex.IsMatch(value);
        }

	        private static string GetServerImplementationType()
	        {
	            var configured = configuration?[KeyServerImplementationType];
	            if (string.IsNullOrWhiteSpace(configured))
	            {
	                return DefaultServerImplementationType;
	            }

	            var trimmed = configured!.Trim();
            if (Type.GetType(trimmed, throwOnError: false) == null)
            {
                log.Warn($"Server implementation type '{trimmed}' could not be loaded. Falling back to default.");
                return DefaultServerImplementationType;
            }

            return trimmed;
        }
	}
}
