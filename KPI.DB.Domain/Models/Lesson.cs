using System;

namespace KPI.DB.Domain.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Theme { get; set; }
    }
}
