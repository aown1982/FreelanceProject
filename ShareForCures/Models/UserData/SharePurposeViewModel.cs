using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class SharePurposeViewModel
    {
       
        public int ID { get; set; }
        public string Name { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<int> ConsentID { get; set; }
        public Nullable<System.DateTimeOffset> StartDateTime { get; set; }
        public Nullable<System.DateTimeOffset> EndDateTime { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string PurposeDescription { get; set; }
    }
}