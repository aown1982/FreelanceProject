using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Models
{
    public class UserVitalModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public decimal? Value { get; set; }
        public System.DateTimeOffset ResultDateTime { get; set; }
        public decimal? Systolic { get; set; }
        public decimal? Diastolic { get; set; }
        public decimal? HeartRate { get; set; }
    }
}