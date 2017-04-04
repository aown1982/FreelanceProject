using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [JsonObject("organization")]
    public class Organization
    {
        [JsonProperty("id")] // String  The Id of the organization
        public string id { get; set; }

        [JsonProperty("name")] // String  The name of the organization
        public string name { get; set; }

        [JsonProperty("href")] // String  Human API organizations endpoint URL to retrieve full details
        public string href { get; set; }

    }
}
