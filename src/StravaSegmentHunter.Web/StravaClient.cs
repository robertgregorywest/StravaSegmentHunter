using System.Net.Http;
using System.Threading.Tasks;

namespace StravaSegmentHunter.Web
{
    public class StravaClient
    {
        private readonly HttpClient _client;

        public StravaClient(HttpClient client)
        {
            _client = client;
        }

        public virtual async Task<string> GetStarredSegments()
        {
            return await _client.GetStringAsync("segments/starred");
        }
    }
}