using System;

namespace StravaSegmentHunter.OAuth.AccessTokenManagement
{
    /// <summary>
    /// Models a user access token
    /// </summary>
    public class UserAccessToken
    {
        /// <summary>
        /// The access token
        /// </summary>
        public string AccessToken { get; set; }
        
        /// <summary>
        /// The access token expiration
        /// </summary>
        public DateTimeOffset Expiration { get; set; }
        
        /// <summary>
        /// The refresh token
        /// </summary>
        public string RefreshToken { get; set; }
    }
}