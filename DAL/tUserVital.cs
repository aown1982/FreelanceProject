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
    
    public partial class tUserVital
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tUserVital()
        {
            this.tXrefUserEncountersVitals = new HashSet<tXrefUserEncountersVital>();
            this.tXrefUserVitalsCodes = new HashSet<tXrefUserVitalsCode>();
        }
    
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public string SourceObjectID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> SourceOrganizationID { get; set; }
        public Nullable<int> UserSourceServiceID { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int UOMID { get; set; }
        public System.DateTimeOffset ResultDateTime { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    
        public virtual tProvider tProvider { get; set; }
        public virtual tSourceOrganization tSourceOrganization { get; set; }
        public virtual tSystemStatus tSystemStatus { get; set; }
        public virtual tUnitsOfMeasure tUnitsOfMeasure { get; set; }
        public virtual tUserID tUserID { get; set; }
        public virtual tUserSourceService tUserSourceService { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tXrefUserEncountersVital> tXrefUserEncountersVitals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tXrefUserVitalsCode> tXrefUserVitalsCodes { get; set; }
    }
}
