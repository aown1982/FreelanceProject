using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [JsonObject("ccd")]
    public class CCD
    {
        [JsonProperty("href")] // String  
        public string href { get; set; }

        [JsonProperty("id")] // String  The identifier of the ccd
        public string id { get; set; }
    }
}
