using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [JsonObject("patient")]
    public class Patient
    {
        [JsonProperty("name")] // String  The name of the patient
        public string name { get; set; }

    }
}
