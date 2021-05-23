using KPI.DB.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KPI.DB.Persistance.Repositories
{
    public interface ILessonsRepository
    {
        Task AssignGroupLesson(Lesson lesson, string groupName);
        Task<int> CreateLesson(Lesson lesson);
        Task<IEnumerable<Lesson>> GetGroupLessons(string groupName);
        Task<IEnumerable<Lesson>> GetPersonLessons(int id);
    }
}