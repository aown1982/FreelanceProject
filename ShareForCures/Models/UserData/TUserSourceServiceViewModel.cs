using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class TUserSourceServiceViewModel
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public int SourceServiceID { get; set; }
        public int UserID { get; set; }
        public int StatusID { get; set; }
    }
}