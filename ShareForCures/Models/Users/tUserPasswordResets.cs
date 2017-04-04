using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.Users
{
    public class tUserPasswordResets
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public System.Guid ExternalUserID { get; set; }
        public System.Guid ResetCodeID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> UsedDateTime { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

    }
}