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
    
    public partial class tUserHealthGoal
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public int UserID { get; set; }
        public decimal Value { get; set; }
        public int GoalTypeID { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    
        public virtual tHealthGoalType tHealthGoalType { get; set; }
        public virtual tSystemStatus tSystemStatus { get; set; }
    }
}
