using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class tSourceOrganizationViewModel
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public string SourceObjectID { get; set; }
        public int OrganizationID { get; set; }
        public string SourceOrgID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public int SourceServiceID { get; set; }
        public tOrganizationViewModel tOrganization { get; set; }
    }
}