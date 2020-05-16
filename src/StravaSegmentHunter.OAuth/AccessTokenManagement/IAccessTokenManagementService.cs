using System.Threading.Tasks;

namespace StravaSegmentHunter.OAuth.AccessTokenManagement
{
    /// <summary>
    /// Abstraction for managing user and client accesss tokens
    /// </summary>
    public interface IAccessTokenManagementService
    {
        /// <summary>
        /// Returns the user access token. If the current token is expired, it will try to refresh it.
        /// </summary>
        /// <returns>An access token or null if refreshing did not work.</returns>
        Task<string> GetUserAccessTokenAsync(bool forceRenewal = false);

        /// <summary>
        /// Revokes the current access token
        /// </summary>
        /// <returns></returns>
        Task RevokeAccessTokenAsync();
    }
}