using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Models.WebApp
{
    public class SurveyQuestionsViewModel
    {

        public int ID { get; set; }
        public int SurveyID { get; set; }
        public string Question { get; set; }
        public int QuestionTypeID { get; set; }
        public byte IsValid { get; set; }
        public byte CanSkip { get; set; }
        public bool IsLast { get; set; }
        public List<SurveyQuestionMCAnswersViewModel> tMCAnswers { get; set; }
    }
}