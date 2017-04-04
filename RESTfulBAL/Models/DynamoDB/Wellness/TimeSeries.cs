using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using System;

namespace RESTfulBAL.Models.DynamoDB.Wellness
{
    [JsonObject]
    public class TimeSeries
    {
        [JsonProperty("type")] //
        public string type { get; set; }

        [JsonProperty("intervalInMillis")] //
        public decimal? intervalInMillis { get; set; }

        [JsonProperty("values")] //
        public string values { get; set; }       
        
    }
}