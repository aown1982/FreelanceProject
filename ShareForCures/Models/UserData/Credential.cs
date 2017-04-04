using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class Credential
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string SourceUserID { get; set; }
        public string AccessToken { get; set; }
        public string PublicToken { get; set; }
        public int SourceID { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public System.DateTime LastUpdatedDateTime { get; set; }
        public string SourceUserIDToken { get; set; }
        public string RefreshToken { get; set; }
        public Nullable<System.DateTime> AccessTokenExpiration { get; set; }
    }
}