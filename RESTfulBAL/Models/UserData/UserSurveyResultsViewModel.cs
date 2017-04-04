using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulBAL.Models.UserData
{
    public class UserSurveyResultsViewModel : tUserSurveyResult
    {
        public string QuestionDescription { get; set; }
        public List<string> MCAnswers { get; set; }
    }
}