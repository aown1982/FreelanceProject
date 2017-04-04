using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.WebApp
{
    public class tUserTestResult
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public System.DateTimeOffset ResultDateTime { get; set; }
        public string Day => ResultDateTime.Month + "/" + ResultDateTime.Day + "/" + ResultDateTime.Year.ToString().Substring(2);
        public string Value { get; set; }
        public string Hdl { get; set; }
        public string Ldl { get; set; }
        public string Cholesterol { get; set; }

    }
}