using IdentityModel.Client;

namespace StravaSegmentHunter.OAuth.AccessTokenManagement
{
    /// <summary>
    /// Retrieves request details for refresh and revocation requests
    /// </summary>
    public interface ITokenClientConfigurationService
    {
        /// <summary>
        /// Returns the request details for a refresh token request
        /// </summary>
        /// <returns></returns>
        RefreshTokenRequest GetRefreshTokenRequest();

        /// <summary>
        /// Returns the request details for a token revocation request
        /// </summary>
        /// <returns></returns>
        TokenRevocationRequest GetTokenRevocationRequest();
    }
}