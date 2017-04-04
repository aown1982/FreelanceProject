//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tUserEncounter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tUserEncounter()
        {
            this.tUserEncounterReasons = new HashSet<tUserEncounterReason>();
            this.tUserOrders = new HashSet<tUserOrder>();
            this.tXrefUserEncountersCodes = new HashSet<tXrefUserEncountersCode>();
            this.tXrefUserEncountersPrescriptions = new HashSet<tXrefUserEncountersPrescription>();
            this.tXrefUserEncountersProblems = new HashSet<tXrefUserEncountersProblem>();
            this.tXrefUserEncountersVitals = new HashSet<tXrefUserEncountersVital>();
        }
    
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public Nullable<int> SourceObjectID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> SourceOrganizationID { get; set; }
        public Nullable<int> UserSourceServiceID { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public Nullable<int> VisitTypeID { get; set; }
        public System.DateTimeOffset EncounterDateTime { get; set; }
        public Nullable<int> FollowUpInstructionID { get; set; }
        public Nullable<int> PatientInstructionID { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public System.DateTime LastUpdateDateTime { get; set; }
    
        public virtual tProvider tProvider { get; set; }
        public virtual tSourceOrganization tSourceOrganization { get; set; }
        public virtual tSystemStatus tSystemStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserEncounterReason> tUserEncounterReasons { get; set; }
        public virtual tUserSourceService tUserSourceService { get; set; }
        public virtual tUserID tUserID { get; set; }
        public virtual tUserInstruction tUserInstruction { get; set; }
        public virtual tUserInstruction tUserInstruction1 { get; set; }
        public virtual tVisitType tVisitType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserOrder> tUserOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tXrefUserEncountersCode> tXrefUserEncountersCodes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tXrefUserEncountersPrescription> tXrefUserEncountersPrescriptions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tXrefUserEncountersProblem> tXrefUserEncountersProblems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tXrefUserEncountersVital> tXrefUserEncountersVitals { get; set; }
    }
}