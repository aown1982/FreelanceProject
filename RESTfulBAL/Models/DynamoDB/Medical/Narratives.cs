using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [DynamoDBTable("HAPI_mNarratives")]
    public class Narratives : BaseHAPIMedical
    {
        [JsonProperty("author")] // String  The name of the author
        public string author { get; set; }

        [JsonProperty("dateTime")]  //Date The date of the narrative
        public DateTimeOffset dateTime { get; set; }

        [JsonProperty("entries")] // Array of entry objects (See below)
        public entries[] entries { get; set; }

        [JsonProperty("organization")] // Object  Hospital information(See organizations)
        public Organization organization { get; set; }

    }

    //ENTRY OBJECT
    [JsonObject]
    public class entries
    {
        [JsonProperty("title")] // String  The title of the entry
        public string title { get; set; }

        [JsonProperty("text")] // String  Text content of the entry
        public string text { get; set; }
    }
}