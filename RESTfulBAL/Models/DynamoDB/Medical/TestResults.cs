using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [DynamoDBTable("HAPI_mTestResults")]
    public class TestResults : BaseHAPIMedical
    {

        [JsonProperty("components")] // Array[Object]   A list of components of the test
        public Components[] components { get; set; }

        [JsonProperty("name")] // String  The name of the test preformed (e.g. ‘COMPREHENSIVE METABOLIC PANEL’, 'LIPID PANEL’, 'URINE CULTURE’)
        public string name { get; set; }

        [JsonProperty("patient")] // Object  Patient information(“name” and other optional attributes)
        public Patient patient { get; set; }

        [JsonProperty("orderedBy")] // String The name of the ordering physician or entity
        public string orderedBy { get; set; }

        [JsonProperty("recipients")] // Array[Object]   A list of recipients of the test - if provided
        public Recipients[] recipients { get; set; }

        [JsonProperty("resultDateTime")] // Object  The date of the test result
        public DateTime resultDateTime { get; set; }

        [JsonProperty("status")] // String  The status of the test result (e.g. 'Final result’, 'Edited’, 'Edited Result - FINAL’)
        public string status { get; set; }

        [JsonProperty("allResults")] // Array[Object]	[Deprecated] Use components instead.
        public object allResults { get; set; }

        [JsonProperty("comments")] // String  Optional comments for the test result
        public string comments { get; set; }

        [JsonProperty("narrative")] // String Optional narrative for the test result
        public string narrative { get; set; }

        [JsonProperty("impression")] // String Optional impression for the test result
        public string impression { get; set; }

        [JsonProperty("transcriptions")] // String Optional transcriptions for the test result
        public string transcriptions { get; set; }

        [JsonProperty("organization")] // Object  Hospital information(See organizations)
        public Organization organization { get; set; }

        [JsonProperty("codes")] // Object  See codes object
        public Codes[] codes { get; set; }
        
    }
}