using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using StravaSegmentHunter.OAuth.AccessTokenManagement;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Authentication
{
    /// <summary>
    /// Extensions methods for HttpContext for token management
    /// </summary>
    public static class TokenManagementHttpContextExtensions
    {
        /// <summary>
        /// Returns (and refreshes if needed) the current access token for the logged on user
        /// </summary>
        /// <param name="context"></param>
        /// <param name="forceRenewal">If set to true, the cached user token is ignored, and a new one gets requested. Default to false.</param>
        /// <returns></returns>
        public static async Task<string> GetUserAccessTokenAsync(this HttpContext context, bool forceRenewal = false)
        {
            var service = context.RequestServices.GetRequiredService<IAccessTokenManagementService>();

            return await service.GetUserAccessTokenAsync(forceRenewal);
        }

        /// <summary>
        /// Revokes the current user refresh token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task RevokeUserAccessTokenAsync(this HttpContext context)
        {
            var service = context.RequestServices.GetRequiredService<IAccessTokenManagementService>();
            var store = context.RequestServices.GetRequiredService<IUserTokenStore>();

            await service.RevokeAccessTokenAsync();
            await store.ClearTokenAsync(context.User);
        }
    }
}