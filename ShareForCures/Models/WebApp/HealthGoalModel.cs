using System;

namespace ShareForCures.Models.WebApp
{
    public class HealthGoalModel
    {
        public int UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Last5 { get; set; }
    }
}

