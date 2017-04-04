using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [DynamoDBTable("HAPI_mImmunizations")]
    public class Immunizations : BaseHAPIMedical
    {
        [JsonProperty("name")] // String  The name of the immunization(e.g. ‘Tetanus+Dip ADULT (Td)’, 'Varicella’, 'Influenza Virus Vaccine’)
        public string name { get; set; }

        [JsonProperty("dates")]  //Array[Date] The dates immunization was givin
        public DateTimeOffset[] dates { get; set; }

        [JsonProperty("patient")] // Object  Patient information(“name” and other optional attributes)
        public Patient patient { get; set; }

        [JsonProperty("organization")] // Object  Hospital information(See organizations)
        public Organization organization { get; set; }

        [JsonProperty("codes")] // Object  See codes object
        public Codes[] codes { get; set; }

    }
}