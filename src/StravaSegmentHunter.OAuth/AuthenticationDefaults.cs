using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace StravaSegmentHunter.OAuth
{
    /// <summary>
    /// Default values used by the Strava authentication middleware.
    /// </summary>
    public static class AuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="Microsoft.AspNetCore.Authentication.AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "Strava";

        /// <summary>
        /// Default value for <see cref="Microsoft.AspNetCore.Authentication.AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "Strava";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Strava";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-strava";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://www.strava.com/oauth/authorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://www.strava.com/oauth/token";
        
        /// <summary>
        /// Default value for Deauthorization endpoint/>.
        /// </summary>
        public const string DeauthorizationEndpoint = "https://www.strava.com/oauth/deauthorize";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "https://www.strava.com/api/v3/athlete";

        /// <summary>
        /// Base address for API calls
        /// </summary>
        public const string BaseAddress = "https://www.strava.com/api/v3/";
        
        /// <summary>
        /// Name of the back-channel HTTP client
        /// </summary>
        public const string BackChannelHttpClientName = "StravaSegmentHunter.OAuth.AccessTokenManagement.TokenEndpointService";
    }
}
