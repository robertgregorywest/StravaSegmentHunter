namespace StravaSegmentHunter.OAuth
{
    /// <summary>
    /// Contains constants specific to the <see cref="AuthenticationHandler"/>.
    /// </summary>
    public static class AuthenticationConstants
    {
        public static class Claims
        {
            public const string City = "urn:strava:city";
            public const string CreatedAt = "urn:strava:created-at";
            public const string Premium = "urn:strava:premium";
            public const string Profile = "urn:strava:profile";
            public const string ProfileMedium = "urn:strava:profile-medium";
            public const string UpdatedAt = "urn:strava:updated-at";
        }
    }
}
