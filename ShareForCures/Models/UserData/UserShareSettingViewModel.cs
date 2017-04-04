using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class UserShareSettingViewModel
    {
        
        public int ID { get; set; }
        public int UserID { get; set; }
        public Nullable<bool> AllData { get; set; }
        public Nullable<int> UserSourceServiceID { get; set; }
        public Nullable<int> SourceOrganizationID { get; set; }
        public Nullable<int> ObjectID { get; set; }
        public Nullable<int> EventID { get; set; }
        public int SHARESettingID { get; set; }
        public Nullable<System.DateTimeOffset> StartDateTime { get; set; }
        public Nullable<System.DateTimeOffset> EndDateTime { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }

        //public virtual tSHARESetting tSHARESetting { get; set; }
        //public virtual tSourceOrganization tSourceOrganization { get; set; }
        //public virtual tSystemStatus tSystemStatus { get; set; }
        //public virtual tUserEvent tUserEvent { get; set; }
        //public virtual tUserID tUserID { get; set; }
        //public virtual tUserSourceService tUserSourceService { get; set; }
       }
}