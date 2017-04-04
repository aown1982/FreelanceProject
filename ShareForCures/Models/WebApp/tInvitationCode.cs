using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.WebApp
{
    public class tInvitationCode
    {
        public int ID { get; set; }
        public System.Guid InvitationCode { get; set; }
        public string AssignedFirstName { get; set; }
        public string AssignedLastName { get; set; }
        public string AssignedCompany { get; set; }
        public string AssignedEmailAddress { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> UsedDateTime { get; set; }
    }
}