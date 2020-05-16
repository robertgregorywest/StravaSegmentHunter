using System.Threading.Tasks;
using IdentityModel.Client;

namespace StravaSegmentHunter.OAuth.AccessTokenManagement
{
    /// <summary>
    /// Abstraction for token endpoint operations
    /// </summary>
    public interface ITokenEndpointService
    {
        /// <summary>
        /// Refreshes a user access token.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<TokenResponse> RefreshUserAccessTokenAsync(string refreshToken);
        
        /// <summary>
        /// Revokes an access token.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<TokenRevocationResponse> RevokeAccessTokenAsync(string accessToken);
    }
}