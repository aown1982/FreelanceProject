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
    
    public partial class tMedicationForm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tMedicationForm()
        {
            this.tXrefMedicationFormsCodes = new HashSet<tXrefMedicationFormsCode>();
        }
    
        public int ID { get; set; }
        public string Form { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDateTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tXrefMedicationFormsCode> tXrefMedicationFormsCodes { get; set; }
    }
}