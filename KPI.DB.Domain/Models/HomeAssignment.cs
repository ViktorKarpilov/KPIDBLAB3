using NodaTime;
using System;

namespace KPI.DB.Domain.Models
{
    public class HomeAssignment
    {
        public int Id { get; set; }
        public DateTime Deadline { get; set; }
        public int Points { get; set; }
    }
}
