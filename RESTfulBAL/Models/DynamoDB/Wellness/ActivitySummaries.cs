using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Wellness
{
    [DynamoDBTable("HAPI_wActivitySummaries")]
    public class ActivitySummaries : BaseHAPIWellness
    {
        [JsonProperty("date")] //The date of the activity
        public DateTime date { get; set; }

        [JsonProperty("source")] //The name of the originating service
        public string source { get; set; }

        [JsonProperty("duration")] //The duration in seconds
        public decimal? duration { get; set; }

        [JsonProperty("distance")] //The distance in meters
        public decimal? distance { get; set; }

        [JsonProperty("steps")] //The number of steps taken during the activity
        public decimal? steps { get; set; }

        [JsonProperty("vigorous")] //The number of minutes of vigorous activity
        public decimal? vigorous { get; set; }

        [JsonProperty("moderate")] //The number of minutes of moderate activity
        public decimal? moderate { get; set; }

        [JsonProperty("light")] //The number of minutes of light activity
        public decimal? light { get; set; }

        [JsonProperty("sedentary")] //The number of minutes of sedentary activity
        public decimal? sedentary { get; set; }
        
        [JsonProperty("calories")] //The number of estimated calories burned during the activity
        public decimal? calories { get; set; }

        [JsonProperty("sourceData")] //Additional data from the source that does not fit into the Human API model.
        public SourceData sourceData { get; set; }

        [JsonProperty("createdAt")] //The time the activity was created on the Human API server
        public DateTime createdAt { get; set; }

        [JsonProperty("updatedAt")] //The time the activity was updated on the Human API server
        public DateTime updatedAt { get; set; }

    }
}