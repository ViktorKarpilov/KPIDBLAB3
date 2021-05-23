namespace KPI.DB.Domain.Models
{
    public class Result : BaseEntity
    {
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public bool? IsPassed { get; set; }
        public int? Score { get; set; }
        public int Year { get; set; }
    }
}
