using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Models
{
    public class UserTestModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public System.DateTimeOffset ResultDateTime { get; set; }
        public string Value { get; set; }
        public string Hdl { get; set; }
        public string Ldl { get; set; }
        public string Cholesterol { get; set; }


    }
}