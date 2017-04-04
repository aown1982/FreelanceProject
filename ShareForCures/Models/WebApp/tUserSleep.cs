using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.WebApp
{
    public class tUserSleep
    {
        public System.DateTimeOffset StartDateTime { get; set; }
        public Nullable<decimal> TimeAsleep { get; set; }
        public string Day => StartDateTime.Month + "/" + StartDateTime.Day + "/" + StartDateTime.Year.ToString().Substring(2);
        public float Goal { get; set; }

    }
}