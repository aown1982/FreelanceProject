using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [JsonObject]
    public class Components
    {

        [JsonProperty("name")] // String  The name of the test component (e.g. 'Potassium’, 'MCHC’, 'MCV’)
        public string name { get; set; }

        [JsonProperty("value")] // String  The test result value
        public string value { get; set; }

        [JsonProperty("unit")] //String  The unit of the value - if provided(e.g. 'mg/dL’, ’%’, 'mmol/L’)
        public string unit { get; set; }

        [JsonProperty("low")] //String  The low value for the component - if provided
        public string low { get; set; }

        [JsonProperty("high")] //String The high value for the component - if provided
        public string high { get; set; }

        [JsonProperty("refRange")] //String The reference range for the value - if provided(e.g. 'Negative’, ’>60’, ’<150’)
        public string refRange { get; set; }

        [JsonProperty("componentComments")] //String  Optional comments provided for the component
        public string componentComments { get; set; }

        [JsonProperty("codes")] // 
        public Codes[] codes { get; set; }
        

    }
}
