using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Models.UserData
{
    public class UserDiet
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public string SourceObjectID { get; set; }
        public int UserID { get; set; }
        public int? UserSourceServiceID { get; set; }
        public int? ProviderID { get; set; }
        public string Name { get; set; }
        public decimal? Servings { get; set; }
        public int? ServingUOMID { get; set; }
        public int? DietCategoryID { get; set; }
        public System.DateTimeOffset EnteredDateTime { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
        public int? NoteID { get; set; }
        public string Note { get; set; }
        public string Source { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Category { get; set; }
    }
}