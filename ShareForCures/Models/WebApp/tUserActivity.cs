using System;
using static System.Decimal;

namespace ShareForCures.Models.WebApp
{
    public class tUserActivity
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public string SourceObjectID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> UserSourceServiceID { get; set; }
        public int ActivityID { get; set; }
        public Nullable<decimal> Duration { get; set; }
        public Nullable<int> DurationUOMID { get; set; }
        public Nullable<decimal> Distance { get; set; }
        public Nullable<int> DistanceUOMID { get; set; }
        public Nullable<decimal> Steps { get; set; }
        public Nullable<decimal> Calories { get; set; }
        public Nullable<decimal> LightActivityMin { get; set; }
        public Nullable<decimal> ModerateActivityMin { get; set; }
        public Nullable<decimal> VigorousActivityMin { get; set; }
        public Nullable<decimal> SedentaryActivityMin { get; set; }
        public System.DateTimeOffset StartDateTime { get; set; }
        public Nullable<System.DateTimeOffset> EndDateTime { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
        public float Goal { get; set; }
        public string Day => StartDateTime.Month + "/" + StartDateTime.Day + "/" + StartDateTime.Year.ToString().Substring(2);

        public decimal? DurationMinutes => Duration.HasValue ? Round(Convert.ToDecimal(Duration)/60,2) : 0;
        public decimal? DurationHours => Duration.HasValue ? Round(Convert.ToDecimal(Duration) / 3600, 2) : 0;

    }
}