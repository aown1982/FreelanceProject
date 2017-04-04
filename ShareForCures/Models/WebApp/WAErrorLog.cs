using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.WebApp
{
    public class WAErrorLog
    {
        public Nullable<int> Id { get; set; }
        public int ErrTypeID { get; set; }
        public int ErrSourceID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Trace { get; set; }
        public Nullable<int> UserID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
    }
}