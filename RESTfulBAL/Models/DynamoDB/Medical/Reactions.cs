using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [JsonObject]
    public class Reaction
    {

        [JsonProperty("name")] // String  The name of the allergen
        public string name { get; set; }

        [JsonProperty("codes")] // 
        public Codes[] codes { get; set; }

        [JsonProperty("reactionType")] // Reaction type information
        public ReactionType reactionType { get; set; }

    }
}
