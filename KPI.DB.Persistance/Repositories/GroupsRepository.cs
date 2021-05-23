using Dapper;
using KPI.DB.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KPI.DB.Persistance.Repositories
{
    public class GroupsRepository : RepositoryBase, IGroupsRepository
    {
        public GroupsRepository(Configurations.IConnectionAccessor accessor) : base(accessor)
        {
        }

        public async Task AssignPersonToGroup(Person person, string group)
        {
            await Accessor.Connection.QueryAsync($"insert into public.groups(name) values('{group}') on conflict do nothing; insert into persons_groups(person_id, group_name) select {person.Id}, '{group}' ; ");
        }

        public async Task<IEnumerable<Group>> GetAllGroups()
        {
            return await Accessor.Connection.QueryAsync<Group>($"select * from groups;");
        }

        public async Task<IEnumerable<Group>> GetAllPersonGroups(Person person)
        {
            try
            {
                return await Accessor.Connection.QueryAsync<Group>($"select pg.group_name as name from public.persons_groups pg where person_id = {person.Id}");
            }
            catch (Exception)
            {
                return new List<Group>();
            }
        }
    }
}
