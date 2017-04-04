using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [JsonObject("results")]
    public class Results
    {

        [JsonProperty("name")] // String  The name of the test component (e.g. ‘HEIGHT’, 'WEIGHT’, 'BODY TEMPERATURE’)
        public string name { get; set; }

        [JsonProperty("value")] // String  The test result value
        public string value { get; set; }

        [JsonProperty("unit")] // String  The unit of the value - if provided(e.g. 'kg’, ’%’, 'Cel’)
        public string unit { get; set; }

        [JsonProperty("codes")] // 
        public Codes[] codes { get; set; }

    }
}
