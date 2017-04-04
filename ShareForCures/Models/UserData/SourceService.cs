using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class SourceService
    {
        public int ID { get; set; }
        public string ServiceName { get; set; }
        public int SourceID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<int> TypeID { get; set; }
    }
}