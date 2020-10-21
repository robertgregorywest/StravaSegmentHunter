using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SimpleDatastore.Interfaces;
using StravaSegmentHunter.Domain;
using StravaSegmentHunter.Web.Models;

namespace StravaSegmentHunter.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Segment> _segmentRepository;

        public HomeController(ILogger<HomeController> logger, IRepository<Segment> segmentRepository)
        {
            _logger = logger;
            _segmentRepository = segmentRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.AccessToken = await HttpContext.GetTokenAsync("access_token");
            return View();
        }
        
        public async Task<IActionResult> Test([FromServices] StravaClient client)
        {
            var response = await client.GetStarredSegments();

            using var document = JsonDocument.Parse(response);

            foreach (var element in document.RootElement.EnumerateArray())
            {
                var segment = new Segment {Name = element.GetString("name")};
                
                if (element.TryGetProperty("id", out var idElement))
                {
                    if (idElement.TryGetInt64(out var segmentStravaId))
                    {
                        segment.StravaId = segmentStravaId;
                    }
                }
                
                if (element.TryGetProperty("distance", out var distanceElement))
                {
                    if (distanceElement.TryGetDouble(out var distance))
                    {
                        segment.Distance = distance;
                    }
                }
                
                await _segmentRepository.SaveAsync(segment);
            }
            
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