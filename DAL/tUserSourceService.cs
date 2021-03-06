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
    
    public partial class tUserSourceService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tUserSourceService()
        {
            this.tXrefUserSourceServiceDevices = new HashSet<tXrefUserSourceServiceDevice>();
            this.tUserSHARESettings = new HashSet<tUserSHARESetting>();
            this.tUserEducations = new HashSet<tUserEducation>();
            this.tUserEmployments = new HashSet<tUserEmployment>();
            this.tUserPolitics = new HashSet<tUserPolitic>();
            this.tUserRelationships = new HashSet<tUserRelationship>();
            this.tUserReligions = new HashSet<tUserReligion>();
            this.tUserSocialPosts = new HashSet<tUserSocialPost>();
        }
    
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public int SourceServiceID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> CredentialID { get; set; }
        public Nullable<System.DateTime> ConnectedOnDateTime { get; set; }
        public Nullable<System.DateTime> LastSyncDateTime { get; set; }
        public Nullable<System.DateTime> LatestDateTime { get; set; }
        public int StatusID { get; set; }
        public int SystemStatusID { get; set; }
    
        public virtual tCredential tCredential { get; set; }
        public virtual tSourceService tSourceService { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tXrefUserSourceServiceDevice> tXrefUserSourceServiceDevices { get; set; }
        public virtual tUserSourceServiceStatus tUserSourceServiceStatus { get; set; }
        public virtual tSystemStatus tSystemStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserSHARESetting> tUserSHARESettings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserEducation> tUserEducations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserEmployment> tUserEmployments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserPolitic> tUserPolitics { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserRelationship> tUserRelationships { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserReligion> tUserReligions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserSocialPost> tUserSocialPosts { get; set; }
    }
}
