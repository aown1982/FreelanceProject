using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.WebApp
{
    public class tUserHealthGoal
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public int UserID { get; set; }
        public decimal Value { get; set; }
        public int GoalTypeID { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
}