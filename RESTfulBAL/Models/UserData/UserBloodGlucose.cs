﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Models.UserData
{
    public class UserBloodGlucose
    {
        public string Note { get; set; }
        public int ID { get; set; }
        public DateTimeOffset ResultDateTime { get; set; }
        public string Value { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public string Source { get; set; }
        public int? UOMID { get; set; }
        public int? StatusID { get; set; }
        public int SourceID { get; set; }
        public int UserID { get; set; }
        public string SourceObjectID { get; set; }
        public int? SourceOrganizationID { get; set; }
        public int? UserSourceServiceID { get; set; }
        public int SystemStatusID { get; set; }
        public string UnitOfMeasure { get; set; }
        public int? NoteID { get; set; }
        public int TestResultID { get; set; }
        public int TestResultComponentID { get; set; }
        public int Count { get;  set; }
        public string HighValue { get; set; }
        public string LowValue { get; set; }
        public string RefRange { get; set; }
        public int? CodeID { get; set; }
        public string Name { get; set; }

    }
}