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
    
    public partial class tUserSpecimen
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tUserSpecimen()
        {
            this.tUserProcedures = new HashSet<tUserProcedure>();
            this.tUserSpecimenCodes = new HashSet<tUserSpecimenCode>();
        }
    
        public int ID { get; set; }
        public string Specimen { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserProcedure> tUserProcedures { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserSpecimenCode> tUserSpecimenCodes { get; set; }
    }
}