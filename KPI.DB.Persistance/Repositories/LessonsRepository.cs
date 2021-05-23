using Dapper;
using KPI.DB.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KPI.DB.Persistance.Repositories
{
    public class LessonsRepository : RepositoryBase, ILessonsRepository
    {
        public LessonsRepository(Configurations.IConnectionAccessor accessor) : base(accessor)
        {
        }

        public async Task<IEnumerable<Lesson>> GetGroupLessons(string groupName)
        {
            try
            {
                return await Accessor.Connection.QueryAsync<Lesson>($"select * from lessons l join person_lesson pl ON pl.lesson_id = l.id join persons_groups pg on pg.person_id = pl.person_id where pg.group_name = '{groupName}';");
            }
            catch (Exception)
            {
                return new List<Lesson>();
            }
        }

        public async Task<IEnumerable<Lesson>> GetPersonLessons(int id)
        {
            try
            {
                return await Accessor.Connection.QueryAsync<Lesson>($"select * from lessons l join person_lesson pl ON pl.lesson_id = l.id where pl.person_id = {id};");
            }
            catch (Exception)
            {
                return new List<Lesson>();
            }
        }

        public async Task<int> CreateLesson(Lesson lesson)
        {
            return await Accessor.Connection.QuerySingleAsync<int>($"INSERT INTO public.lessons (time,theme) VALUES(@time,@theme) RETURNING id; ", lesson);
        }

        public async Task AssignGroupLesson(Lesson lesson, string groupName)
        {
            int lessonId = await CreateLesson(lesson);

            await Accessor.Connection.QueryAsync<int>($"INSERT INTO person_lesson (person_id,lesson_id) select pg.person_id, {lessonId} from persons_groups pg where pg.group_name = '{groupName}'");
        }

        public async Task UpdateLesson(Lesson newLesson)
        {
            await Accessor.Connection.QueryAsync<int>($"UPDATE public.lessons SET time = '{newLesson.Time}' WHERE id = {newLesson.Id}; ");
        }
    }
}
