using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class UserAcitivityViewModel
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
        public string Activity { get; set; }
        public int SourceID { get; set; }
        public int? SourceOrganizationID { get; set; }
        public string DurationUOM { get; set; }
        public string DistanceUOM { get; set; }
        public string Source { get; set; }
        public string Note { get; set; }
        public int? NoteID { get; set; }
        public System.Guid fkObjectID { get; set; }
        public Nullable<System.DateTime> LastUpdateDateTime { get; set; }

    }
}