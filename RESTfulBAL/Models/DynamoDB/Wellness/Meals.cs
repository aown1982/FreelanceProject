using System;
using Newtonsoft.Json;
using Amazon.DynamoDBv2.DataModel;

namespace RESTfulBAL.Models.DynamoDB.Wellness
{
    [DynamoDBTable("HAPI_wMeals")]
    public class Meals : BaseHAPIWellness
    {
        [JsonProperty("timestamp")] //The original date of the meal
        public DateTime timestamp { get; set; }

        [JsonProperty("tzOffset")] //The offset from UTC time in +/-hh:mm (e.g. -04:00)
        public string tzOffset { get; set; }

        [JsonProperty("source")] //The name of the originating service
        public string source { get; set; }

        [JsonProperty("type")] //Type of the meal: breakfast/lunch/dinner/other
        public string type { get; set; }

        [JsonProperty("name")] //Descriptive name of the meal
        public string name { get; set; }
       
        [JsonProperty("calories")] //The amount of consumed calories
        public decimal? calories { get; set; }
        
        [JsonProperty("carbohydrate")] //The amount of consumed carbohydrate
        public decimal? carbohydrate { get; set; }

        [JsonProperty("fat")] //The amount of consumed fat
        public decimal? fat { get; set; }

        [JsonProperty("protein")] //The amount of consumed protein
        public decimal? protein { get; set; }

        [JsonProperty("sodium")] //The amount of consumed sodium
        public decimal? sodium { get; set; }

        [JsonProperty("sugar")] //The amount of consumed sugar
        public decimal? sugar { get; set; }

        [JsonProperty("fiber")] //The amount of consumed fiber(optional)
        public decimal? fiber { get; set; }

        [JsonProperty("saturatedFat")] //The amount of consumed saturated fat(optional)
        public decimal? saturatedFat { get; set; }

        [JsonProperty("monounsaturatedFat")] //The amount of consumed monounsaturated fat(optional)
        public decimal? monounsaturatedFat { get; set; }

        [JsonProperty("polyunsaturatedFat")] //The amount of consumed polyunsaturated fat(optional)
        public decimal? polyunsaturatedFat { get; set; }

        [JsonProperty("cholesterol")] //The amount of consumed cholesterol(optional)
        public decimal? cholesterol { get; set; }

        [JsonProperty("vitaminA")] //The amount of consumed vitamin A(optional)
        public decimal? vitaminA { get; set; }

        [JsonProperty("vitaminC")] //The amount of consumed vitamin C(optional)
        public decimal? vitaminC { get; set; }

        [JsonProperty("calcium")] //The amount of consumed calcium(optional)
        public decimal? calcium { get; set; }

        [JsonProperty("iron")] //The amount of consumed iron(optional)
        public decimal? iron { get; set; }

        [JsonProperty("potassium")] //The amount of consumed potassium(optional)
        public decimal? potassium { get; set; }

        [JsonProperty("amount")] //The coordinate point with a lat/lon value
        public Amount amount { get; set; }

        [JsonProperty("createdAt")] //The time the activity was created on the Human API server
        public DateTime createdAt { get; set; }

        [JsonProperty("updatedAt")] //The time the activity was updated on the Human API server
        public DateTime updatedAt { get; set; }

    }


    [JsonObject]
    public class Amount
    {
        [JsonProperty("value")] //Quantity
        public decimal? value { get; set; }

        [JsonProperty("unit")] //Unit of measure
        public string unit { get; set; }
    }
}