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
    
    public partial class tXrefUserFunctionalStatusesCode
    {
        public int ID { get; set; }
        public int UserFunctionalStatusID { get; set; }
        public int CodeID { get; set; }
    
        public virtual tCode tCode { get; set; }
        public virtual tUserFunctionalStatus tUserFunctionalStatus { get; set; }
    }
}
