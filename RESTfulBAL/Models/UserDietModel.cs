namespace RESTfulBAL.Models
{
    public class UserDietModel
    {
        public System.DateTimeOffset EnteredDateTime { get; set; }
        public decimal? Value { get; set; }
        public string Day => EnteredDateTime.Month + "/" + EnteredDateTime.Day;
        public float Goal { get; set; }
    }
}