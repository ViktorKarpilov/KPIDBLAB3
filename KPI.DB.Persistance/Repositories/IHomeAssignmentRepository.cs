using KPI.DB.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KPI.DB.Persistance.Repositories
{
    public interface IHomeAssignmentRepository
    {
        Task<HomeAssignment> Get(int id);
        Task<IEnumerable<HomeAssignment>> GetAllAsignmentsForPerson(int id);
        Task<int> Create(HomeAssignment assignment);
        Task AssignToGroup(HomeAssignment newAssignment, string groupName);
        Task AssignToPerson(HomeAssignment newAssignment, int personId);
    }
}