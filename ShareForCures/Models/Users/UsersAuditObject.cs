using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.Users
{
    public class UsersAuditObject
    {
        public int ID { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
    }
}