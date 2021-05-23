using NodaTime;

namespace KPI.DB.Shared
{
    public class Lesson
    {
        public int Id { get; set; }
        public LocalDateTime Time { get; set; }
        public string Theme { get; set; }
    }
}
