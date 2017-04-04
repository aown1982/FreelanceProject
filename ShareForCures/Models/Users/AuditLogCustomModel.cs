using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.Users
{
    public class AuditLogCustomModel
    {
        public int Id { get; set; }
        public Nullable<int> ApplicationID { get; set; }
        public Nullable<int> UserID { get; set; }
        public int EventID { get; set; }
        public Nullable<int> ObjectID { get; set; }
        public string Description { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
        public Nullable<System.DateTime> DateTimeStamp { get; set; }

        public virtual UsersAuditObject tUsersAuditObject { get; set; }

    }
}