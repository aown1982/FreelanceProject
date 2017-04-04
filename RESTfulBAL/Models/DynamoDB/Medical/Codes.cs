using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [JsonObject("codes")]
    public class Codes
    {
        [JsonProperty("code")] // String  Medical code
        public string code { get; set; }

        [JsonProperty("codeSystem")] // String  The identifier of the code system that the code belongs to
        public string codeSystem { get; set; }

        [JsonProperty("codeSystemName")] //  String The name of the code system that the code belongs to
        public string codeSystemName { get; set; }

        [JsonProperty("name")] // String  The name of the code
        public string name { get; set; }

    }
}
