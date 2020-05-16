using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using StravaSegmentHunter.OAuth;

namespace StravaSegmentHunter.OAuth.AccessTokenManagement
{
    /// <summary>
    /// Options-based configuration service for token clients
    /// </summary>
    public class DefaultTokenClientConfigurationService : ITokenClientConfigurationService
    {
        private readonly IOptionsMonitor<StravaAuthenticationOptions> _stravaOptions;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="stravaOptions"></param>
        public DefaultTokenClientConfigurationService(IOptionsMonitor<StravaAuthenticationOptions> stravaOptions)
        {
            _stravaOptions = stravaOptions;
        }

        /// <inheritdoc />
        public virtual RefreshTokenRequest GetRefreshTokenRequest()
        {
            var options = _stravaOptions.Get(StravaAuthenticationDefaults.AuthenticationScheme);

            var requestDetails = new RefreshTokenRequest
            {
                Address = options.TokenEndpoint,
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret,
                GrantType = "refresh_token"
            };
            
            return requestDetails;
        }

        /// <inheritdoc />
        public virtual TokenRevocationRequest GetTokenRevocationRequest()
        {
            var options = _stravaOptions.Get(StravaAuthenticationDefaults.AuthenticationScheme);
            
            var requestDetails = new TokenRevocationRequest
            {
                Address = options.TokenEndpoint,
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret
            };

            return requestDetails;
        }
        
    }
}