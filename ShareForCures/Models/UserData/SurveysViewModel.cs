using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class SurveysViewModel
    {
        public int ID { get; set; }
        public int UserFilterID { get; set; }
        public string InternalName { get; set; }
        public string PublicName { get; set; }
        public string PublicDescription { get; set; }
        public string AvgLengthofTime { get; set; }
        public string MoreInfoLink { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime? CreateDateTime { get; set; }

        public bool IsAllCompleted { get; set; }
        public bool IsStarted { get; set; }


    }

    public class SurveysData
    {
        public List<SurveysViewModel> Surveys { get; set; }
        public int TotalSurveysCount { get; set; }
        public int SurveysCompletedCount { get; set; }

        public string Category { get; set; }
        public int SurveysCount { get; set; }
        public int MonthValue { get; set; }
    }
}