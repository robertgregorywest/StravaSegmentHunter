using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StravaSegmentHunter.OAuth.AccessTokenManagement
{
    /// <summary>
    /// Implements basic token management logic
    /// </summary>
    public class AccessTokenManagementService : IAccessTokenManagementService
    {
        static readonly ConcurrentDictionary<string, Lazy<Task<string>>> UserRefreshDictionary =
            new ConcurrentDictionary<string, Lazy<Task<string>>>();

        private readonly IUserTokenStore _userTokenStore;
        private readonly ISystemClock _clock;
        private readonly AccessTokenManagementOptions _options;
        private readonly ITokenEndpointService _tokenEndpointService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AccessTokenManagementService> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="userTokenStore"></param>
        /// <param name="clock"></param>
        /// <param name="options"></param>
        /// <param name="tokenEndpointService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="logger"></param>
        public AccessTokenManagementService(
            IUserTokenStore userTokenStore,
            ISystemClock clock,
            IOptions<AccessTokenManagementOptions> options,
            ITokenEndpointService tokenEndpointService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AccessTokenManagementService> logger)
        {
            _userTokenStore = userTokenStore;
            _clock = clock;
            _options = options.Value;
            _tokenEndpointService = tokenEndpointService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<string> GetUserAccessTokenAsync(bool forceRenewal = false)
        {
            var user = _httpContextAccessor.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                return null;
            }

            var userName = user.FindFirst(ClaimTypes.Name)?.Value ?? "unknown";
            var userToken = await _userTokenStore.GetTokenAsync(_httpContextAccessor.HttpContext.User);

            if (userToken == null)
            {
                _logger.LogDebug("No token data found in user token store.");
                return null;
            }

            var dtRefresh = userToken.Expiration.Subtract(_options.User.RefreshBeforeExpiration);
            if (dtRefresh < _clock.UtcNow || forceRenewal == true)
            {
                _logger.LogDebug("Token for user {user} needs refreshing.", userName);

                try
                {
                    return await UserRefreshDictionary.GetOrAdd(userToken.RefreshToken, _ =>
                    {
                        return new Lazy<Task<string>>(async () =>
                        {
                            var refreshed = await RefreshUserAccessTokenAsync();
                            return refreshed.AccessToken;
                        });
                    }).Value;
                }
                finally
                {
                    UserRefreshDictionary.TryRemove(userToken.RefreshToken, out _);
                }
            }

            return userToken.AccessToken;
        }

        /// <inheritdoc/>
        public async Task RevokeAccessTokenAsync()
        {
            var userToken = await _userTokenStore.GetTokenAsync(_httpContextAccessor.HttpContext.User);

            if (!string.IsNullOrEmpty(userToken?.AccessToken))
            {
                var response = await _tokenEndpointService.RevokeAccessTokenAsync(userToken.AccessToken);

                if (response.IsError)
                {
                    _logger.LogError("Error revoking access token. Error = {error}", response.Error);
                }
            }
        }

        internal async Task<TokenResponse> RefreshUserAccessTokenAsync()
        {
            var userToken = await _userTokenStore.GetTokenAsync(_httpContextAccessor.HttpContext.User);
            var response = await _tokenEndpointService.RefreshUserAccessTokenAsync(userToken.RefreshToken);

            if (!response.IsError)
            {
                await _userTokenStore.StoreTokenAsync(_httpContextAccessor.HttpContext.User, response.AccessToken, response.ExpiresIn, response.RefreshToken);
            }
            else
            {
                _logger.LogError("Error refreshing access token. Error = {error}", response.Error);
            }

            return response;
        }
    }
}