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
    
    public partial class tUserProcedureDevice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tUserProcedureDevice()
        {
            this.tUserProcedureDeviceCodes = new HashSet<tUserProcedureDeviceCode>();
            this.tUserProcedures = new HashSet<tUserProcedure>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserProcedureDeviceCode> tUserProcedureDeviceCodes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserProcedure> tUserProcedures { get; set; }
    }
}