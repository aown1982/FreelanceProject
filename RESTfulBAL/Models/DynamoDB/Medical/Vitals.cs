using System;
using System.Collections;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [DynamoDBTable("HAPI_mVitals")]
    public class Vitals : BaseHAPIMedical
    {

        [JsonProperty("dateTime")] //  String  The date of the vitals reading
        public DateTime dateTime { get; set; }

        [JsonProperty("author")] //  String  The name of the vitals reading author(e.g.doctor)
        public string author { get; set; }

        [JsonProperty("results")] //  Array[Object]   A list of all test results(see results object below)
        public Results[] results { get; set; }
  
        [JsonProperty("organization")] // Object  Hospital information(See organizations)
        public Organization organization { get; set; }

        [JsonProperty("codes")] // Object  See codes object
        public Codes[] codes { get; set; }
       
    }
}