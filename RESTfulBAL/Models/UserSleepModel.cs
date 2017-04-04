using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Models
{
    public class UserSleepModel
    {
        public System.DateTimeOffset StartDateTime { get; set; }
        public Nullable<decimal> TimeAsleep { get; set; }
        public string Day => StartDateTime.Month + "/" + StartDateTime.Day;
        public float Goal { get; set; }

    }
}