using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Wellness
{
    [DynamoDBTable("HAPI_wSleepSummaries")]
    public class SleepSummaries : BaseHAPIWellness
    {       
        [JsonProperty("endTime")] //The date of the sleep
        public DateTime endTime { get; set; }

        [JsonProperty("source")] //The name of the originating service
        public string source { get; set; }

        [JsonProperty("mainSleep")] //A boolean value indicating if this sleep was the main sleep of the day (default true when not specified by source)
        public bool mainSleep { get; set; }

        [JsonProperty("timeAsleep")] //The time asleep during the segment (in minutes)
        public decimal? timeAsleep { get; set; }

        [JsonProperty("timeAwake")] //The time awake during the segment (in minutes)
        public decimal? timeAwake { get; set; }

        [JsonProperty("createdAt")] //The time the activity was created on the Human API server
        public DateTime createdAt { get; set; }

        [JsonProperty("updatedAt")] //The time the activity was updated on the Human API server
        public DateTime updatedAt { get; set; }

    }
}