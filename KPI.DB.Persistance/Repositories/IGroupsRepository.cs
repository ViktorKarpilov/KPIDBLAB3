using KPI.DB.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KPI.DB.Persistance.Repositories
{
    public interface IGroupsRepository
    {
        Task AssignPersonToGroup(Person person, string group);
        Task<IEnumerable<Group>> GetAllGroups();
        Task<IEnumerable<Group>> GetAllPersonGroups(Person person);
    }
}