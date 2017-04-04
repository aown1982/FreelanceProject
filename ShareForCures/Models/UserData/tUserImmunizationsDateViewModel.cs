using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class tUserImmunizationsDateViewModel
    {
        public int ID { get; set; }
        public int UserImmunizationID { get; set; }
        public System.DateTimeOffset DateTime { get; set; }
        public int SystemStatusID { get; set; }
    }
}