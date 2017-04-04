using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [JsonObject("allergen")]
    public class Allergen
    {

        [JsonProperty("name")] // String  The name of the allergen
        public string name { get; set; }

        [JsonProperty("codes")] // 
        public Codes[] codes { get; set; }

    }
}
