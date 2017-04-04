using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace RESTfulBAL.Models
{
    public class SurveyQuestionViewModel
    {

        public bool IsLast { get; set; }
        public int ID { get; set; }
        public int SurveyID { get; set; }
        public string Question { get; set; }
        public int QuestionTypeID { get; set; }
        public byte IsValid { get; set; }
        public byte CanSkip { get; set; }

        public List<SurveyQuestionMCAnswersViewModel> tMCAnswers { get; set; }
    }
}