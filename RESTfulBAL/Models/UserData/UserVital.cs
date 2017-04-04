using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Models.UserData
{
    public class UserVital
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public string SourceObjectID { get; set; }
        public int UserID { get; set; }
        public int? SourceOrganizationID { get; set; }
        public int? UserSourceServiceID { get; set; }
        public int? ProviderID { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int UOMID { get; set; }
        public System.DateTimeOffset ResultDateTime { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
        public int? NoteID { get; set; }
        public string Note { get; set; }
        public string Source { get; set; }
        public string UnitOfMeasure { get; set; }
    }
}