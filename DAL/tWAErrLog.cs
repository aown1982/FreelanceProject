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
    
    public partial class tWAErrLog
    {
        public int Id { get; set; }
        public int ErrTypeID { get; set; }
        public int ErrSourceID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Trace { get; set; }
        public Nullable<int> UserID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
    }
}
