using System;

namespace StravaSegmentHunter.OAuth.AccessTokenManagement
{
    /// <summary>
    /// Options for the token management services
    /// </summary>
    public class AccessTokenManagementOptions
    {
        /// <summary>
        /// Options for user access tokens
        /// </summary>
        public UserOptions User { get; set; } = new UserOptions();
        
        /// <summary>
        /// User access token options
        /// </summary>
        public class UserOptions
        {
            /// <summary>
            /// Name of the authentication scheme to use for the token operations
            /// </summary>
            public string Scheme { get; set; } = AuthenticationDefaults.AuthenticationScheme;

            /// <summary>
            /// Timespan that specifies how long before expiration, the token should be refreshed
            /// </summary>
            public TimeSpan RefreshBeforeExpiration { get; set; } = TimeSpan.FromMinutes(1);
        }
    }
}