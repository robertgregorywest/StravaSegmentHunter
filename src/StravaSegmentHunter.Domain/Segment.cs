using System;
using System.Text.Json.Serialization;
using SimpleDatastore;

namespace StravaSegmentHunter.Domain
{
    public class Segment : PersistentObject<long>
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("distance")]
        public double Distance { get; set; }
    }
}