using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class UserDietNutrientViewModel
    {
        public int ID { get; set; }
        public int UserDietID { get; set; }
        public int NutrientID { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<int> UOMID { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasure { get; set; }
        public int? SystemStatusID { get; set; }


    }
}