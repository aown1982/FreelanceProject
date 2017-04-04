using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class tUserTestResultViewModel
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public string SourceObjectID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> SourceOrganizationID { get; set; }
        public Nullable<int> UserSourceServiceID { get; set; }
        public string Name { get; set; }
        public Nullable<int> OrderingProviderID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string Comments { get; set; }
        public string Narrative { get; set; }
        public string Impression { get; set; }
        public string Transcriptions { get; set; }
        public System.DateTimeOffset ResultDateTime { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }

    }
}