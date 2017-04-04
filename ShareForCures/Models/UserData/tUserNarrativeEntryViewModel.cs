using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class tUserNarrativeEntryViewModel
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public int NarrativeID { get; set; }
        public int SectionSeqNum { get; set; }
        public string SectionTitle { get; set; }
        public string SectionText { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
        public int SystemStatusID { get; set; }
    }
}