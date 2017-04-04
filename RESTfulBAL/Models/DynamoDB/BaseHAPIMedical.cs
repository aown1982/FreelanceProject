using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;
using RESTfulBAL.Models.DynamoDB.Medical;

namespace RESTfulBAL.Models.DynamoDB
{
    public class BaseHAPIMedical : IHAPI
    {
        [DynamoDBHashKey]
        [JsonProperty("id")] //The Id of the resource
        public string Id { get; set; }

        [JsonProperty("source")] //The name of the originating service
        public string source { get; set; }

        [JsonProperty("humanId")] //Unique user identifier
        public string humanId { get; set; }

        [JsonProperty("access_token")] //The user access token
        public string access_token { get; set; }

        [JsonProperty("ccd")] //The HAPI location of the CCD
        public CCD ccd { get; set; }

        [JsonProperty("updatedAt")] // Date    The time the record was updated on the Human API server
        public DateTime updatedAt { get; set; }

        [JsonProperty("createdAt")] //   Date The time the record was created on the Human API server
        public DateTime createdAt { get; set; }

        public override string ToString()
        {
            return Utilities.ToString(this, Environment.NewLine);
        }
    }
}
