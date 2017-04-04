namespace RESTfulBAL.Models.DynamoDB.Wellness
{
    public class BloodPressureValue
    {
        public decimal? diastolic { get; set; }
        public decimal? systolic { get; set; }
        public string unit { get; set; }
    }
}