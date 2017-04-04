using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.WebApp
{
    public class UserBodyComposition
    {
        public double Weight { get; set; }
        public double Height { get; set; }
        public double Bmi { get; set; }
        public double AdultBodyFatPercent { get; set; }
        public double BodyFat { get; set; }
        public double LeanTissue { get; set; }
        public DateTime? ResultDateTime { get; set; }
        public string Source { get; set; }
        public double Value { get; set; }
    }
}