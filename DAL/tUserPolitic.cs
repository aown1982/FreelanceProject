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
    
    public partial class tUserPolitic
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public string SourceObjectID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> UserSourceServiceID { get; set; }
        public string PoliticalAffiliation { get; set; }
        public Nullable<System.DateTimeOffset> StartDateTime { get; set; }
        public Nullable<System.DateTimeOffset> EndDateTime { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    
        public virtual tSystemStatus tSystemStatus { get; set; }
        public virtual tUserID tUserID { get; set; }
        public virtual tUserSourceService tUserSourceService { get; set; }
    }
}
