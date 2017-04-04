using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [DynamoDBTable("HAPI_mInstructions")]
    public class Instructions : BaseHAPIMedical
    {
        [JsonProperty("name")] // String  The name of the instruction (ex. “instructions”, or “discharge instructions”, etc.)
        public string name { get; set; }

        [JsonProperty("dateTime")]  //Date The date of the instruction
        public DateTimeOffset dateTime { get; set; }

        [JsonProperty("text")] // String  Text content of the instruction
        public string text { get; set; }
        
        [JsonProperty("codes")] // Object  See codes object
        public Codes[] codes { get; set; }

    }
}