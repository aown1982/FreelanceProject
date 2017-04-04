using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.UserData
{
    public class SurveyResultViewModel
    {
        public int ID { get; set; }
        public System.Guid ObjectID { get; set; }
        public int UserID { get; set; }
        public int SurveyID { get; set; }
        public int QuestionID { get; set; }
        public string Answer { get; set; }
        public int SystemStatusID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
        public string QuestionIDsPassed { get; set; }

        public string QuestionDescription { get; set; }
        public List<string> MCAnswers { get; set; }
    }
}