using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class tUserEncounterViewModel
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> UserSourceServiceID { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public Nullable<int> VisitTypeID { get; set; }
        public System.DateTimeOffset EncounterDateTime { get; set; }
        public Nullable<int> FollowUpInstructionID { get; set; }
        public Nullable<int> PatientInstructionID { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public System.DateTime LastUpdateDateTime { get; set; }

    }
}