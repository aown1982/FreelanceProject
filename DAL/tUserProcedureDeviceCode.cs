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
    
    public partial class tUserProcedureDeviceCode
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int DeviceID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
}
