using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [DynamoDBTable("HAPI_mFunctionalStatuses")]
    public class FunctionalStatuses : BaseHAPIMedical
    {
        [JsonProperty("name")] // String  The name of the functional status
        public string name { get; set; }

        [JsonProperty("dateTime")] // Object  The date of the functional status
        public DateTime dateTime { get; set; }

        [JsonProperty("codes")] // Object  See codes object
        public Codes[] codes { get; set; }
    }
}