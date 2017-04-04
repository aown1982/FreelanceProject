using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;


namespace RESTfulBAL.Models.DynamoDB
{
    public class BaseHAPIWellness : IHAPI
    {
        [DynamoDBHashKey]
        [JsonProperty("id")] //The Id of the resource
        public string id { get; set; }

        [JsonProperty("userId")] //[deprecated - use humanId]
        public string userId { get; set; }

        [JsonProperty("humanId")] //Unique user identifier
        public string humanId { get; set; }

        [JsonProperty("access_token")] //The user access token
        public string access_token { get; set; }

        public override string ToString()
        {
            return Utilities.ToString(this, Environment.NewLine);
        }
    }
}
