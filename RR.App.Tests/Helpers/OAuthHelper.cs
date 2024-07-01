using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;

namespace RR.Tests.Data
{
    internal class OAuthHelper
    {
        private readonly IConfidentialClientApplication _app;
        private readonly string[] _scopes;

        public OAuthHelper(string clientId, string clientSecret, string authority, string[] scopes)
        {
            _app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri(authority))
                .Build();
            _scopes = scopes;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var result = await _app.AcquireTokenForClient(_scopes)
                .ExecuteAsync();
            return result.AccessToken;
        }
    }
}
