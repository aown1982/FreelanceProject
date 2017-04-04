using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Models
{
    public class SurveyQuestionMCAnswersViewModel
    {
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public string AnswerText { get; set; }
        public int SequenceOrder { get; set; }
    }
}