using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace StravaSegmentHunter.OAuth.AccessTokenManagement
{
    /// <summary>
    /// Options-based configuration service for token clients
    /// </summary>
    public class DefaultTokenClientConfigurationService : ITokenClientConfigurationService
    {
        private readonly IOptionsMonitor<AuthenticationOptions> _stravaOptions;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="stravaOptions"></param>
        public DefaultTokenClientConfigurationService(IOptionsMonitor<AuthenticationOptions> stravaOptions)
        {
            _stravaOptions = stravaOptions;
        }

        /// <inheritdoc />
        public virtual RefreshTokenRequest GetRefreshTokenRequest()
        {
            var options = _stravaOptions.Get(AuthenticationDefaults.AuthenticationScheme);

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
            var options = _stravaOptions.Get(AuthenticationDefaults.AuthenticationScheme);
            
            var requestDetails = new TokenRevocationRequest
            {
                Address = options.DeauthorizationEndpoint,
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret
            };

            return requestDetails;
        }
    }
}