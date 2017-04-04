using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Wellness
{
    [DynamoDBTable("HAPI_wGeneticTraits")]
    public class GeneticTrait : BaseHAPIWellness
    {
        [JsonProperty("trait")] //The most likely present trait
        public string trait { get; set; }

        [JsonProperty("possibleTraits")] //A list of all the possible values for a specific trait, for easy comparison
        public string possibleTraits { get; set; }

        [JsonProperty("description")] //A description/name of the trait
        public string description { get; set; }


    }
}