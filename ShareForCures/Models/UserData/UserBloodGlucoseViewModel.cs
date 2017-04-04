using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class UserBloodGlucoseViewModel
    {
        public int UserID { get; set; }
        public string Note { get; set; }
        public int UOMID { get; set; }
        public int StatusID { get; set; }
        public DateTime ResultDateTime { get; set; }
        public System.Guid fkObjectID { get; set; }
        public int SystemStatusID { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdateDateTime { get; set; }



        public DateTimeOffset ResultDate { get; set; }
        public string Comments { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Status { get; set; }
        public string Source { get; set; }
        public string UnitOfMeasure { get; set; }


        public string SourceObjectID { get; set; }
        public int? SourceOrganizationID { get; set; }
        public int? UserSourceServiceID { get; set; }
        public int NoteID { get; set; }
        public int TestResultID { get; set; }
        public int TestResultComponentID { get; set; }
        public int Count { get; set; }






    }
}