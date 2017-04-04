using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.WebApp
{
    public class tUserDiet
    {
        //public int ID { get; set; }
        //public System.Guid ObjectID { get; set; }
        //public string SourceObjectID { get; set; }
        //public int UserID { get; set; }
        //public Nullable<int> UserSourceServiceID { get; set; }
        //public string Name { get; set; }
        //public Nullable<decimal> Servings { get; set; }
        //public Nullable<int> ServingUOMID { get; set; }
        //public Nullable<int> DietCategoryID { get; set; }
        //public System.DateTimeOffset EnteredDateTime { get; set; }
        //public int SystemStatusID { get; set; }
        //public System.DateTime CreateDateTime { get; set; }
        //public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
        //public ICollection<tXrefUserDietNutrient> tXrefUserDietNutrients { get; set; }
        //public string Day => EnteredDateTime.Month + "/" + EnteredDateTime.Day;
        //public float Goal { get; set; }
        //public decimal? Value
        //{
        //    get
        //    {
        //        return tXrefUserDietNutrients.Where(x => x.NutrientID == 170)?.Sum(x => x.Value);
        //    }
        //}


        public System.DateTimeOffset EnteredDateTime { get; set; }
        public decimal? Value { get; set; }
        public string Day => EnteredDateTime.Month + "/" + EnteredDateTime.Day + "/" + EnteredDateTime.Year.ToString().Substring(2);
        public float Goal { get; set; }

    }
}