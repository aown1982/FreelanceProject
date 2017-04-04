using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Models
{
    public class SurveyViewModel : tSurvey
    {
        //public List<int> UserIdsPermitted { get; set; }
        public string Category { get; set; }
        public int SurveysCount { get; set; }
        public int MonthValue { get; set; }

        public bool IsAllCompleted { get; set; }
        public bool IsStarted { get; set; }
    }
}