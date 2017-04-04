using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShareForCures.Models.WebApp;

namespace ShareForCures.Models.UserData
{
    public class UserDataViewModel
    {
        public List<tAdsSponsoredViewModel> AdsList { get; set; }
        public IEnumerable<UserAuditLogsViewModel> RecentActivities { get; set; }
        public List<SourceServiceTypeViewModel> SourceServiceTypeData { get; set; }
    }
}