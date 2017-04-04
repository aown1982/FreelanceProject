using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class tUserPrescriptionViewModel
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public string SourceObjectID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> SourceOrganizationID { get; set; }
        public Nullable<int> UserSourceServiceID { get; set; }
        public string Name { get; set; }
        public string Instructions { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string DosageText { get; set; }
        public string DosageValue { get; set; }
        public Nullable<int> MedFormID { get; set; }
        public string FrequencyValue { get; set; }
        public Nullable<int> FrequencyUOMID { get; set; }
        public Nullable<int> RouteID { get; set; }
        public string StrengthValue { get; set; }
        public Nullable<int> StrengthUOMID { get; set; }
        public Nullable<int> PharmacyID { get; set; }
        public Nullable<int> RefillsRemaining { get; set; }
        public Nullable<int> RefillsTotal { get; set; }
        public Nullable<System.DateTimeOffset> ExpirationDateTime { get; set; }
        public System.DateTimeOffset StartDateTime { get; set; }
        public Nullable<System.DateTimeOffset> EndDateTime { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
}