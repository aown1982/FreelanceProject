﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class UserVitalViewModel
    {

        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public string SourceObjectID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> SourceOrganizationID { get; set; }
        public Nullable<int> UserSourceServiceID { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int UOMID { get; set; }
        public System.DateTimeOffset ResultDate{ get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdateDateTime { get; set; }

        public string Note { get; set; }
        public DateTime ResultDateTime { get; set; }
        public System.Guid fkObjectID { get; set; }


        public string Source { get; set; }
        public string UnitOfMeasure { get; set; }

        public int NoteID { get; set; }



    }
}