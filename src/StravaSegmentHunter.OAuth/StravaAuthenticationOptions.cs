using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static StravaSegmentHunter.OAuth.StravaAuthenticationConstants;

namespace StravaSegmentHunter.OAuth
{
    /// <summary>
    /// Defines a set of options used by <see cref="StravaAuthenticationHandler"/>.
    /// </summary>
    public class StravaAuthenticationOptions : OAuthOptions
    {
        public StravaAuthenticationOptions()
        {
            ClaimsIssuer = StravaAuthenticationDefaults.Issuer;

            CallbackPath = StravaAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = StravaAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = StravaAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = StravaAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "firstname");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "lastname");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.StateOrProvince, "state");
            ClaimActions.MapJsonKey(ClaimTypes.Country, "country");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "sex");
            ClaimActions.MapJsonKey(Claims.City, "city");
            ClaimActions.MapJsonKey(Claims.Profile, "profile");
            ClaimActions.MapJsonKey(Claims.ProfileMedium, "profile_medium");
            ClaimActions.MapJsonKey(Claims.CreatedAt, "created_at");
            ClaimActions.MapJsonKey(Claims.UpdatedAt, "updated_at");
            ClaimActions.MapJsonKey(Claims.Premium, "premium");
        }
    }
}
