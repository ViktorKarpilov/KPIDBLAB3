using KPI.DB.Persistance.Repositories;
using KPI.DB.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainLesson = KPI.DB.Domain.Models.Lesson;

namespace KPI.DB.Server.Controllers
{
    public class LessonsController : BaseController
    {
        public LessonsController(ILessonsRepository lessonsRepository)
        {
            LessonsRepository = lessonsRepository;
        }

        public ILessonsRepository LessonsRepository { get; }

        [HttpPost("group")]
        public async Task<StatusCodeResult> CreateGroupLesson([FromBody] GroupLesson lesson)
        {
            try
            {
                DomainLesson domain = new DomainLesson()
                {
                    Theme = lesson.Lesson.Theme,
                    Time = lesson.Lesson.Time
                };
                await LessonsRepository.AssignGroupLesson(domain, lesson.GroupName);
                return StatusCode(201);
            }
            catch (System.Exception)
            {
                return StatusCode(400);
            }
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<DomainLesson>> GetPersonLessons(int id)
        {
            return await LessonsRepository.GetPersonLessons(id);
        }

        [HttpPut]
        public async Task UpdateLesson(DomainLesson lesson)
        {
            await LessonsRepository.UpdateLesson(lesson);
        }
    }
}
