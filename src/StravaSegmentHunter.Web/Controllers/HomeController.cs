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
        private readonly IWriteRepository<Segment, long> _segmentRepository;

        public HomeController(ILogger<HomeController> logger, IWriteRepository<Segment, long> segmentRepository)
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
                var segment = JsonSerializer.Deserialize<Segment>(element.GetRawText());
                
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