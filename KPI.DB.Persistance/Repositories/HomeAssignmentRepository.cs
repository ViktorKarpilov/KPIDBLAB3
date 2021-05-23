using Dapper;
using KPI.DB.Domain.Models;
using KPI.DB.Persistance.Configurations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KPI.DB.Persistance.Repositories
{
    public class HomeAssignmentRepository : IHomeAssignmentRepository
    {
        public HomeAssignmentRepository(IConnectionAccessor accessor)
        {
            Accessor = accessor;
        }

        private IConnectionAccessor Accessor { get; }

        public async Task<HomeAssignment> Get(int id)
        {
            return await Accessor.Connection.QuerySingleAsync<HomeAssignment>($"SELECT * FROM public.home_assignments ha WHERE ha.id = {id}");
        }

        public async Task<IEnumerable<HomeAssignment>> GetAllAsignmentsForPerson(int id)
        {
            return await Accessor.Connection.QueryAsync<HomeAssignment>($"select * from home_assignments ha join person_assignments pa on ha.id = pa.home_assignment where pa.person_id = {id}");
        }

        public async Task AssignToGroup(HomeAssignment newAssignment, string groupName)
        {
            int assignment = await Create(newAssignment);
            await Accessor.Connection.QueryAsync($"INSERT INTO person_assignments(person_id,home_assignment) select pg.person_id, {assignment} from persons_groups pg where pg.group_name = '{groupName}'; ");
        }

        public async Task<int> Create(HomeAssignment assignment)
        {
            return await Accessor.Connection.QuerySingleAsync<int>($"INSERT INTO public.home_assignments (deadline,points) VALUES (@Deadline,@Points) RETURNING id", assignment);
        }

        public async Task AssignToPerson(HomeAssignment newAssignment, int personId)
        {
            int assignment = await Create(newAssignment);
            await Accessor.Connection.QueryAsync($"INSERT INTO person_assignments(person_id,home_assignment) VALUES({personId},{assignment}); ");
        }
    }
}
