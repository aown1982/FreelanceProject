using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class tOrganizationViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string PhoneNumber { get; set; }
        public System.DateTime CreateDateTime { get; set; }
    }
}