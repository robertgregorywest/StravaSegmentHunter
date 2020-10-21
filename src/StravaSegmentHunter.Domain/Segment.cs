using System;
using System.Text.Json.Serialization;
using SimpleDatastore;

namespace StravaSegmentHunter.Domain
{
    public class Segment : PersistentObject
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("stravaId")]
        public long StravaId { get; set; }
        
        [JsonPropertyName("distance")]
        public double Distance { get; set; }
    }
}