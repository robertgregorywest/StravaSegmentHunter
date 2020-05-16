using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;

namespace StravaSegmentHunter.OAuth.AccessTokenManagement
{
    /// <summary>
    /// Implements token endpoint operations using IdentityModel
    /// </summary>
    public class TokenEndpointService : ITokenEndpointService
    {
        private readonly ITokenClientConfigurationService _configService;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configService"></param>
        /// <param name="httpClientFactory"></param>
        public TokenEndpointService(
            ITokenClientConfigurationService configService,
            IHttpClientFactory httpClientFactory)
        {
            _configService = configService;
            _httpClient = httpClientFactory.CreateClient(AccessTokenManagementDefaults.BackChannelHttpClientName);
        }

        /// <inheritdoc/>
        public async Task<TokenResponse> RefreshUserAccessTokenAsync(string refreshToken)
        {
            var requestDetails = _configService.GetRefreshTokenRequest();
            requestDetails.RefreshToken = refreshToken;

            return await _httpClient.RequestRefreshTokenAsync(requestDetails);
        }

        /// <inheritdoc/>
        public async Task<TokenRevocationResponse> RevokeAccessTokenAsync(string accessToken)
        {
            var requestDetails = _configService.GetTokenRevocationRequest();
            requestDetails.Token = accessToken;

            return await _httpClient.RevokeTokenAsync(requestDetails);
        }
    }
}