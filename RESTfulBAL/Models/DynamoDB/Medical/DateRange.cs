using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [JsonObject]
    public class DateRange
    {
        [JsonProperty("start")] // Start date
        public DateTime start { get; set; }

        [JsonProperty("end")] // End date
        public DateTime end { get; set; }

    }
}
