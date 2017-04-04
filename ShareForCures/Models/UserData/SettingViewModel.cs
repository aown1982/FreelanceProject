using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class SettingViewModel
    {
        public string TimeZone { get; set; }
        public int? TimeZoneId { get; set; }
        public int? LocationId { get; set; }
        public string Location { get; set; }
        public int? LanguageId { get; set; }
        public string Language { get; set; }
    }
}