using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.WebApp
{
    public class AdsLogViewModel
    {
        public int ID { get; set; }
        public int SponsoredAdID { get; set; }
        public int UserID { get; set; }
        public byte ViewClick { get; set; }
        public System.DateTime CreateDateTime { get; set; }

    }
}