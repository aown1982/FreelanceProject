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
    
    public partial class tSHAREPurpose
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tSHAREPurpose()
        {
            this.tXrefUserSHARESettingsPurposes = new HashSet<tXrefUserSHARESettingsPurpos>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<int> ConsentID { get; set; }
        public Nullable<System.DateTimeOffset> StartDateTime { get; set; }
        public Nullable<System.DateTimeOffset> EndDateTime { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string PurposeDescription { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tXrefUserSHARESettingsPurpos> tXrefUserSHARESettingsPurposes { get; set; }
        public virtual tConsent tConsent { get; set; }
        public virtual tSHAREPurposeCategory tSHAREPurposeCategory { get; set; }
    }
}
