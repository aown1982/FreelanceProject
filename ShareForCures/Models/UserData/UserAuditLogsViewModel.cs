using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class UserAuditLogsViewModel
    {
        public int Id { get; set; }
        public int UserSourceServiceId { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public long TotalCount { get; set; }
    }
}