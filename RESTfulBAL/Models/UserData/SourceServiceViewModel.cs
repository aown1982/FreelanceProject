using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Models.UserData
{
    public class SourceServiceViewModel
    {
        public int ID { get; set; }
        public string ServiceName { get; set; }
        public int SourceID { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int? TypeID { get; set; }
    }
}