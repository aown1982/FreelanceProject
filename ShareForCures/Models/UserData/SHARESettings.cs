using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class allUserSHARESettings
    {
        public List<SHARESettings> allows { get; set; }
        public List<SHARESettings> denies { get; set; }
        public List<SourceServiceTypeViewModel> SourceServiceTypeData { get; set; }

    }


    public class SHARESettings
    {
        public Nullable<int> ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<bool> AllData { get; set; }
        public Nullable<int> UserSourceServiceID { get; set; }
        public Nullable<int> SourceOrganizationID { get; set; }
        public Nullable<int> ObjectID { get; set; }
        public Nullable<int> EventID { get; set; }
        public Nullable<int> SHARESettingID { get; set; }
        public Nullable<System.DateTimeOffset> StartDateTime { get; set; }
        public Nullable<System.DateTimeOffset> EndDateTime { get; set; }
        public Nullable<int> SystemStatusID { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
        public string SHAREPurpose { get; set; }
        public string SHAREPurposeDescription { get; set; }
        public string ConsentDesc { get; set; }
        public string ConsentPath { get; set; }
        public string OrgName { get; set; }
        public string ServiceName { get; set; }

        public xrefUserSHARESettingsPurposes[] xrefSHAREPurposes { get; set; }

    }
    
    public class xrefUserSHARESettingsPurposes
    {
        public int ID { get; set; }
        public int SHAREPurposeID { get; set; }
        public int SHARESettingID { get; set; }

        public SHAREPurposes[] userSHAREPurposes { get; set; }
    }

    public class SHAREPurposes
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<int> ConsentID { get; set; }
        public Nullable<System.DateTimeOffset> StartDateTime { get; set; }
        public Nullable<System.DateTimeOffset> EndDateTime { get; set; }
        public Nullable<int> CategoryID { get; set; }
        
        public SHAREConsents userConsents { get; set; }
    }

    public class SHAREConsents
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string NavPath { get; set; }
        public System.DateTime CreateDateTime { get; set; }
    }
}
