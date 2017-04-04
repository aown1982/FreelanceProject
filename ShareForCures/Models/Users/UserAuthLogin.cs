using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.Users
{
    public class UserAuthLogin
    {
        public System.Guid ID { get; set; }
        public int UserID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public System.DateTime ExpirationDate { get; set; }

        public virtual UsersModel tUser { get; set; }
    }
}