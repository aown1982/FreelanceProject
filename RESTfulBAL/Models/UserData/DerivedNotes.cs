using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Models.UserData
{
    public class UserDataWithNote
    {
        public string Note { get; set; }
        public int ID { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset? EndDateTime { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Allergen { get; set; }
        public string Severity { get; set; }
        public string Reaction { get; set; }
        public string Status { get; set; }
        public string Source { get; set; }
        public int AllergenID { get; set; }
        public Nullable<int> SeverityID { get; set; }
        public Nullable<int> ReactionID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public int SourceID { get; set; }
        public int ParentId { get; set; }
        public string CarePlanName { get; set; }
        public string CarePlanText { get; set; }
        public Nullable<int> CarePlanTypeID { get; set; }
        public Nullable<int> CarePlanSpecialtyID { get; set; }
        public string CarePlanTypeName { get; set; }
        public string CarePlanSpecialtyName { get; set; }
        public DateTimeOffset EncounterDateTime { get; set; }
        public string ProviderName { get; set; }
        public string VisitTypeName { get; set; }
        public int? ProviderID { get; set; }
        public int? VisitTypeID { get; set; }
        public DAL.tUserInstruction FollowUpInstruction { get; set; }
        public DAL.tUserInstruction PatientInstruction { get; set; }

        public string Name { get; set; }
        public string Text { get; set; }
        public Nullable<int> InstructionTypeID { get; set; }
        public string InstructionTypeName { get; set; }
        public Nullable<int> MedFormID { get; set; }
        public string MedFormName { get; set; }
        public Nullable<int> FrequencyUOMID { get; set; }
        public string FrequencyUOMName { get; set; }
        public Nullable<int> StrengthUOMID { get; set; }
        public string StrengthUOMName { get; set; }
        public Nullable<int> RouteID { get; set; }
        public string RouteName { get; set; }
        public Nullable<int> PharmacyID { get; set; }
        public string PharmacyName { get; set; }
        public string Instructions { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string DosageText { get; set; }
        public string DosageValue { get; set; }
        public string FrequencyValue { get; set; }
        public string StrengthValue { get; set; }
        public Nullable<int> RefillsRemaining { get; set; }
        public Nullable<int> RefillsTotal { get; set; }
        public Nullable<System.DateTimeOffset> ExpirationDateTime { get; set; }
        public Nullable<int> PerformingOrganizationID { get; set; }
        public Nullable<int> DeviceID { get; set; }
        public Nullable<int> SpecimenID { get; set; }
        public string PerformingOrganizationName { get; set; }
        public string DeviceName { get; set; }
        public string SpecimenName { get; set; }
        public string Comments { get; set; }
        public string Narrative { get; set; }
        public string Impression { get; set; }
        public string Transcriptions { get; set; }
        public DateTimeOffset ResultDateTime { get; set; }
        public int UOMID { get; set; }
        public string UOMName { get; set; }
        public decimal Value { get; set; }
        public string Title { get; set; }
        public int SeqNum { get; set; }
        public string ComponentsValue { get; set; }
        public string LowValue { get; set; }
        public string HighValue { get; set; }
        public string RefRange { get; set; }
    }
}