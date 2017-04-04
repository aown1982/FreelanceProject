using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.Users
{
    public class tUsersAddressHistory
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> TimeZoneID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public Nullable<int> StateID { get; set; }
        public string Zip { get; set; }
        public System.DateTime CreateDateTime { get; set; }

    }
}