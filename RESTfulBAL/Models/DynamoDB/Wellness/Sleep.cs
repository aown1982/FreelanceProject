using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Wellness
{
    [DynamoDBTable("HAPI_wSleep")]
    public class Sleep : BaseHAPIWellness
    {
        [JsonProperty("day")] //The day the sleep was recorded
        public DateTime day { get; set; }

        [JsonProperty("startTime")] //The start time of the activity in UTC time
        public DateTime startTime { get; set; }

        [JsonProperty("endTime")] //The end time of the activity in UTC time
        public DateTime endTime { get; set; }

        [JsonProperty("tzOffset")] //The offset from UTC time in +/-hh:mm (e.g. -04:00)
        public string tzOffset { get; set; }

        [JsonProperty("source")] //The name of the originating service
        public string source { get; set; }

        [JsonProperty("mainSleep")] //A boolean value indicating if this sleep was the main sleep of the day (default true when not specified by source)
        public bool mainSleep { get; set; }

        [JsonProperty("timeAsleep")] //The time asleep during the segment (in minutes)
        public decimal? timeAsleep { get; set; }

        [JsonProperty("timeAwake")] //The time awake during the segment (in minutes)
        public decimal? timeAwake { get; set; }

        [JsonProperty("efficiency")] //The efficiency score
        public int efficiency { get; set; }

        [JsonProperty("timeToFallAsleep")] //The number of minutes it took to fall asleep
        public decimal? timeToFallAsleep { get; set; }

        [JsonProperty("timeAfterWakeup")] //The number of minutes in bed after waking up
        public decimal? timeAfterWakeup { get; set; }

        [JsonProperty("timeInBed")] //The total number of minutes spend in bed
        public decimal? timeInBed { get; set; }

        [JsonProperty("numberOfWakeups")] //The number of times the user woke up while in bed
        public int numberOfWakeups { get; set; }

        [JsonProperty("timeSeries")] //Time series data for the sleep, such as quality
        public TimeSeries timeSeries { get; set; }

        [JsonProperty("createdAt")] //The time the activity was created on the Human API server
        public DateTime createdAt { get; set; }

        [JsonProperty("updatedAt")] //The time the activity was updated on the Human API server
        public DateTime updatedAt { get; set; }

    }
}