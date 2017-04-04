using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Wellness
{
    [DynamoDBTable("HAPI_wActivities")]
    public class Activities : BaseHAPIWellness
    {
        [JsonProperty("startTime")] //The start time of the activity in UTC time
        public DateTime startTime { get; set; }

        [JsonProperty("endTime")] //The end time of the activity in UTC time
        public DateTime endTime { get; set; }

        [JsonProperty("tzOffset")] //The offset from UTC time in +/-hh:mm (e.g. -04:00)
        public string tzOffset { get; set; }

        [JsonProperty("type")] //The type of activity, such as walking, running, cycling
        public string type { get; set; }

        [JsonProperty("source")] //The name of the originating service
        public string source { get; set; }

        [JsonProperty("duration")] //The duration in seconds
        public decimal? duration { get; set; }

        [JsonProperty("distance")] //The distance in meters
        public decimal? distance { get; set; }

        [JsonProperty("steps")] //The number of steps taken during the activity
        public decimal? steps { get; set; }
       
        [JsonProperty("calories")] //The number of estimated calories burned during the activity
        public decimal? calories { get; set; }

        [JsonProperty("sourceData")] //Additional data from the source that does not fit into the Human API model.
        public SourceData sourceData { get; set; }

        [JsonProperty("timeSeries")] //Time series of data such as heart rate, gps location, distance, etc.
        public TimeSeries timeSeries { get; set; }

        [JsonProperty("createdAt")] //The time the activity was created on the Human API server
        public DateTime createdAt { get; set; }

        [JsonProperty("updatedAt")] //The time the activity was updated on the Human API server
        public DateTime updatedAt { get; set; }

    }
}