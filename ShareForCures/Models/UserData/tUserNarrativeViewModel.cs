using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class tUserNarrativeViewModel
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public Nullable<int> SourceObjectID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> SourceOrganizationID { get; set; }
        public Nullable<int> UserSourceServiceID { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public System.DateTimeOffset StartDateTime { get; set; }
        public Nullable<System.DateTimeOffset> EndDateTime { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
}