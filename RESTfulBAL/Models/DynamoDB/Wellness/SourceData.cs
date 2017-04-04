using Newtonsoft.Json;

namespace RESTfulBAL.Models.DynamoDB.Wellness
{
    [JsonObject]
    public class SourceData
    {
        [JsonProperty("activityCalories")] //
        public int activityCalories { get; set; }

        [JsonProperty("caloriesBMR")] //
        public int caloriesBMR { get; set; }

        [JsonProperty("tracker")] //
        public trackerObj tracker { get; set; }

        [JsonObject]
        public class trackerObj
        {
            [JsonProperty("elevation")] //
            public decimal? elevation { get; set; }

            [JsonProperty("floors")] //
            public decimal? floors { get; set; }

            [JsonProperty("distance")] //
            public decimal? distance { get; set; }

            [JsonProperty("steps")] //
            public decimal? steps { get; set; }

            [JsonProperty("calories")] //
            public decimal? calories { get; set; }

        }

    }
}