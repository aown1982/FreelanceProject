using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Wellness
{
    [DynamoDBTable("HAPI_wLocations")]
    public class Locations : BaseHAPIWellness
    {
        [JsonProperty("startTime")] //The start time of the activity in UTC time
        public DateTime startTime { get; set; }

        [JsonProperty("endTime")] //The end time of the activity in UTC time
        public DateTime endTime { get; set; }

        [JsonProperty("tzOffset")] //The offset from UTC time in +/-hh:mm (e.g. -04:00)
        public string tzOffset { get; set; }

        [JsonProperty("name")] //The name of the place
        public string name { get; set; }

        [JsonProperty("source")] //The name of the originating service
        public string source { get; set; }

        [JsonProperty("location")] //The coordinate point with a lat/lon value
        public location location { get; set; }

        [JsonProperty("createdAt")] //The time the activity was created on the Human API server
        public DateTime createdAt { get; set; }

        [JsonProperty("updatedAt")] //The time the activity was updated on the Human API server
        public DateTime updatedAt { get; set; }
    }

    [JsonObject]
    public class location
    {
        [JsonProperty("lat")] //Latitude
        public decimal? lat { get; set; }

        [JsonProperty("lon")] //Longitude
        public decimal? lon { get; set; }
    }
}
