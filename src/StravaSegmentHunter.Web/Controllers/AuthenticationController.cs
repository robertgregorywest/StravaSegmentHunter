using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StravaSegmentHunter.OAuth;

namespace StravaSegmentHunter.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpGet("~/signin")]
        public IActionResult Index()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, AuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet("~/signout")]
        [HttpPost("~/signout")]
        public IActionResult SignOut()
        {
            // Instruct the cookies middleware to delete the local cookie created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            return SignOut(new AuthenticationProperties { RedirectUri = "/" }, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}