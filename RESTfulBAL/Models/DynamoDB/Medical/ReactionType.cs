using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [JsonObject]
    public class ReactionType
    {

        [JsonProperty("name")] // String  The name of the reaction
        public string name { get; set; }

        [JsonProperty("codes")] // 
        public Codes[] codes { get; set; }

    }
}
