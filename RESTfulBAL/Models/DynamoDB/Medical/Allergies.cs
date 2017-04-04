using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [DynamoDBTable("HAPI_mAllergies")]
    public class Allergies : BaseHAPIMedical
    {
        [JsonProperty("name")] // String  The name of the allergy(e.g. ‘Etomidate’, 'Fluconazole’, 'Metaxalone’)
        public string name { get; set; }

        [JsonProperty("patient")] // Object  Patient information(“name” and other optional attributes)
        public Patient patient { get; set; }

        [JsonProperty("organization")] // Object  Hospital information(See organizations)
        public Organization organization { get; set; }

        [JsonProperty("reactionsFull")] // Array[Object]   Reactions information(see below)
        public Reaction[] reactionsFull { get; set; }

        [JsonProperty("allergen")] // Object  Allergen information(see below)
        public Allergen allergen { get; set; }

        [JsonProperty("status")] // String  The status of the allergy
        public string status { get; set; }

        [JsonProperty("severity")] // String The severity of the allergy
        public string severity { get; set; }

        [JsonProperty("dateRange")] // Object  Date Range information(see below)
        public DateRange dateRange { get; set; }

        [JsonProperty("codes")] // Object  See codes object
        public Codes[] codes { get; set; }




    }
}