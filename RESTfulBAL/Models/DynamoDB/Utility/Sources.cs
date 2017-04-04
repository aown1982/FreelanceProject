using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;
using RESTfulBAL.Models.DynamoDB.Medical;

namespace RESTfulBAL.Models.DynamoDB.Utility
{
    [DynamoDBTable("HAPI_Sources")]
    public class Sources : BaseHAPIWellness
    {
        [JsonProperty("source")] //String  Name of the connected source(all lowercase, no spaces)
        public String source { get; set; }

        [JsonProperty("supportedDataTypes")] //Array[String]   List of data types supported by the source(does not indicate data is available for each type)
        public String[] supportedDataTypes { get; set; }

        [JsonProperty("devices")] //Array[String]   List of devices(when available)
        public string[] devices { get; set; }

        [JsonProperty("connectedSince")] //Date    Date source was connected to Human API
        public DateTime connectedSince { get; set; }

        [JsonProperty("externalId")] //String External userID from the source(when available)
        public string externalId { get; set; }

        [JsonProperty("historySync")] //Object  See HistorySync object below
        public historySync historySync { get; set; }

        [JsonProperty("syncStatus")] //Object  See SyncStatus object below
        public syncStatus syncStatus { get; set; }

        [JsonProperty("organization")] //Object  organization
        public Organization organization { get; set; }

        [JsonProperty("sourceName")] //String  Organization Name
        public String sourceName { get; set; }

        [JsonProperty("timeZone")] //String  Timezone of source
        public String timeZone { get; set; }
    }

    //HISTORYSYNC OBJECT
    [JsonObject]
    public class historySync
    {
        [JsonProperty("status")] //String Status of the historical sync
        public string status { get; set; }

        [JsonProperty("oldestDate")] //Date    Oldest date of the data retrieved from the service
        public DateTime oldestDate { get; set; }
    }

    //SYNCSTATUS OBJECT
    [JsonObject]
    public class syncStatus
    {
        [JsonProperty("status")] //String  Status of the most recent sync
        public string status { get; set; }

        [JsonProperty("synchedAt")] //Date    The last time the synchronization task ran for this user
        public DateTime synchedAt { get; set; }

        [JsonProperty("newestDate")] //Date The timestamp of the most recently retrieved data*/
        public DateTime newestDate { get; set; }
    }
}