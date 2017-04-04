using System;
using System.Collections.Generic;
using Antlr.Runtime;

namespace ShareForCures.Models.WebApp
{
    public class tUserVital
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public decimal? Value { get; set; }
        public DateTime? ResultDateTime { get; set; }

        public string Day => ResultDateTime.HasValue ? ResultDateTime.Value.Month + "/" + ResultDateTime.Value.Day + "/" + ResultDateTime.Value.Year.ToString().Substring(2) : "" ;
        public decimal? Weight { get; set; }
        public float Goal { get; set; }
        public decimal? Systolic { get; set; }
        public decimal? Diastolic { get; set; }
        public decimal? HeartRate { get; set; }
        public int? UOMID { get; set; }
        public decimal? WeightLbs
        {
            get
            {
                switch (UOMID)
                {
                    case 128://kg
                        return (Value * (decimal)2.20462);
                        break;
                    case 139://lbs
                        return Value;
                        break;
                    default:
                        return null;
                }
            }
        }
        public decimal? CurrentWeightLbs
        {
            get
            {
                switch (UOMID)
                {
                    case 128://kg
                        return (Weight * (decimal)2.20462);
                        break;
                    case 139://lbs
                        return Weight;
                        break;
                    default:
                        return null;
                }
            }
        }
    }

}