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
    
    public partial class tAllergySeverity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tAllergySeverity()
        {
            this.tUserAllergies = new HashSet<tUserAllergy>();
            this.tAllergySeverities1 = new HashSet<tAllergySeverity>();
        }
    
        public int ID { get; set; }
        public string Severity { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<int> ParentID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserAllergy> tUserAllergies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tAllergySeverity> tAllergySeverities1 { get; set; }
        public virtual tAllergySeverity tAllergySeverity1 { get; set; }
    }
}