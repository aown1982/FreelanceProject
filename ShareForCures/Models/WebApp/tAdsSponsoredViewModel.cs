using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.WebApp
{
    public partial class tAdsSponsoredViewModel
    {
        public int ID { get; set; }
        public int AdTypeID { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public Nullable<int> UserFilterID { get; set; }
        public Nullable<int> IconID { get; set; }
        public string ImagePath { get; set; }
        public string ClickPath { get; set; }
        public int SponsorID { get; set; }
        public System.DateTime StartDateTime { get; set; }
        public System.DateTime EndDateTime { get; set; }
        public System.DateTime CreateDateTime { get; set; }

        //public virtual ICollection<tAdsLog> tAdsLogs { get; set; }
        //public virtual tAdType tAdType { get; set; }
        //public virtual tIcon tIcon { get; set; }
        //public virtual tSponsor tSponsor { get; set; }
        //public virtual tUserFilter tUserFilter { get; set; }
    }
}