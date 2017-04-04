using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Medical
{
    [JsonObject]
    public class Recipients
    {
        [JsonProperty("name")] // String  The name of the recipient
        public string name { get; set; }

        [JsonProperty("objectID")] // String  The id of the recipient
        public string objectID { get; set; }

        [JsonProperty("isPCP")] // Boolean Indicates if the provider is the Primary Care Provider of the user
        public bool isPCP { get; set; }

        [JsonProperty("recipTemplate")] // String The recipient template(e.g. 'WPMessageRecipientTemplateUnknown’)
        public string recipTemplate { get; set; }


    }
}
