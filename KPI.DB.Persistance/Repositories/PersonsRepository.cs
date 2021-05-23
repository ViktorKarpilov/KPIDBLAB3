using Dapper;
using KPI.DB.Domain.Models;
using System;
using System.Threading.Tasks;

namespace KPI.DB.Persistance.Repositories
{
    public class PersonsRepository : RepositoryBase, IPersonsRepository
    {
        public PersonsRepository(Configurations.IConnectionAccessor accessor) : base(accessor)
        {
        }

        public async Task Create(Person person)
        {
            string query = $"INSERT INTO public.persons (name,email,type) VALUES('{person.Name}','{person.Email}','{person.Type}'::person_enum_type);";
            var command = new CommandDefinition(query);
            await Accessor.Connection.QueryAsync<Person>(command);
        }
        public async Task<Person> Get(string email)
        {
            try
            {
                return await Accessor.Connection.QuerySingleAsync<Person>($"SELECT * FROM public.persons p WHERE p.email = '{email}' LIMIT 1 ");
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public async Task LinkToGroup(Person person, string groupName)
        {
            await Accessor.Connection.QuerySingleAsync<Person>($"INSERT INTO public.persons_groups (person_id,group_name) VALUES('{person.Id}','{groupName}'); ");
        }
    }
}
