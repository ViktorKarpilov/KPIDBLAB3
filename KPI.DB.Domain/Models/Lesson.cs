using NodaTime;

namespace KPI.DB.Domain.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public LocalDateTime Time { get; set; }
        public string Theme { get; set; }
    }
}
