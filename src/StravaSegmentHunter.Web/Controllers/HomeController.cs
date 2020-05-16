using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StravaSegmentHunter.Web.Models;

namespace StravaSegmentHunter.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.AccessToken = await HttpContext.GetTokenAsync("access_token");
            ViewBag.ExpiresAt = DateTimeOffset.Parse(await HttpContext.GetTokenAsync("expires_at"));
            return View();
        }
        
        public async Task<IActionResult> Test([FromServices] StravaClient client)
        {
            var response = await client.GetStarredSegments();
            ViewBag.Json = JArray.Parse(response).ToString();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}