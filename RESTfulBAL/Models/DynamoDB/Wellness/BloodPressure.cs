using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Wellness
{
    [DynamoDBTable("HAPI_wBloodPressure")]
    public class BloodPressure : BaseHAPIWellness
    {
        [JsonProperty("timestamp")] //The original date and time of the measurement
        public DateTime timestamp { get; set; }

        [JsonProperty("tzOffset")] //The offset from UTC time in +/-hh:mm (e.g. -04:00)
        public string tzOffset { get; set; }

        [JsonProperty("source")] //The name of the originating service
        public string source { get; set; }

        [JsonProperty("unit")] //The unit of the measurement value
        public string unit { get; set; }

        [JsonProperty("heartRate")] //The heart rate in BPM captured at the time of measurement
        public string heartRate { get; set; }

        [JsonProperty("systolic")] //The systolic value captured at the time of measurement
        public string systolic { get; set; }

        [JsonProperty("diastolic")] //The diastolic value captured at the time of measurement
        public string diastolic { get; set; }

        [JsonProperty("createdAt")] //The time the activity was created on the Human API server
        public DateTime createdAt { get; set; }

        [JsonProperty("updatedAt")] //The time the activity was updated on the Human API server
        public DateTime updatedAt { get; set; }
    }
}