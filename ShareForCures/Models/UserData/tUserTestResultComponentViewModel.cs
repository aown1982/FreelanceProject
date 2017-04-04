using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class tUserTestResultComponentViewModel
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public int TestResultID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Nullable<int> UOMID { get; set; }
        public string LowValue { get; set; }
        public string HighValue { get; set; }
        public string RefRange { get; set; }
        public string Comments { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }

    }
}