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
    
    public partial class tXrefUserAllergiesCode
    {
        public int ID { get; set; }
        public int UserAllergiesID { get; set; }
        public int CodeID { get; set; }
    
        public virtual tCode tCode { get; set; }
        public virtual tUserAllergy tUserAllergy { get; set; }
    }
}