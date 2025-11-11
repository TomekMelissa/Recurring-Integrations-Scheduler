/* Copyright (c) Microsoft Corporation. All rights reserved.
   Licensed under the MIT License. */

using Microsoft.Identity.Client;
using RecurringIntegrationsScheduler.Common.JobSettings;
using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using UrlCombineLib;

namespace RecurringIntegrationsScheduler.Common.Helpers
{
    /// <summary>
    /// Authentication helper class
    /// </summary>
    internal class AuthenticationHelper
    {
        private readonly Settings _settings;
        private string _authorizationHeader;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationHelper"/> class.
        /// </summary>
        /// <param name="jobSettings">Job settings</param>
        public AuthenticationHelper(Settings jobSettings)
        {
            _settings = jobSettings;
        }

        /// <summary>
        /// Authorization result property.
        /// </summary>
        /// <value>
        /// Authentication result.
        /// </value>
        private AuthenticationResult AuthenticationResult { get; set; }

        /// <summary>
        /// Sets authorization header.
        /// </summary>
        /// <returns>Authorization header</returns>
        private async Task<string> AuthorizationHeader()
        {
            if (!string.IsNullOrEmpty(_authorizationHeader) &&
                AuthenticationResult != null &&
                (DateTime.UtcNow.AddSeconds(60) < AuthenticationResult.ExpiresOn))
            {
                return _authorizationHeader;
            }
            IConfidentialClientApplication appConfidential;
            IPublicClientApplication appPublic;
            var aosUriAuthUri = new Uri(_settings.AosUri);
            string authority = UrlCombine.Combine(_settings.AzureAuthEndpoint, _settings.AadTenant);
            string[] scopes = new string[] { UrlCombine.Combine(aosUriAuthUri.AbsoluteUri,  ".default") };

            if (_settings.UseServiceAuthentication)
            {
                appConfidential = ConfidentialClientApplicationBuilder.Create(_settings.AadClientId.ToString())
                    .WithClientSecret(_settings.AadClientSecret)
                    .WithAuthority(authority)
                    .Build();
                AuthenticationResult = await appConfidential.AcquireTokenForClient(scopes).ExecuteAsync();
            }
            else
            {
                appPublic = PublicClientApplicationBuilder.Create(_settings.AadClientId.ToString())
                    .WithAuthority(authority)
                    .Build();
                var accounts = await appPublic.GetAccountsAsync();

                if (accounts.Any())
                {
                    AuthenticationResult = await appPublic.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
                }
                else
                {
                    var password = EnsureUserPassword();
                    try
                    {
#pragma warning disable 618
                        AuthenticationResult = await appPublic.AcquireTokenByUsernamePassword(scopes, _settings.UserName, password).ExecuteAsync();
#pragma warning restore 618
                    }
                    finally
                    {
                        password.Dispose();
                    }
                }
            }
            return _authorizationHeader = AuthenticationResult.CreateAuthorizationHeader();
        }

        private SecureString EnsureUserPassword()
        {
            if (_settings.UserPassword == null || _settings.UserPassword.Length == 0)
            {
                throw new InvalidOperationException("User password is not configured for interactive authentication.");
            }

            return _settings.UserPassword.Copy();
        }

        /// <summary>
        /// Gets valid authentication header
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        public async Task<string> GetValidAuthenticationHeader()
        {
            _authorizationHeader = await AuthorizationHeader();
            return _authorizationHeader;
        }
    }
}
