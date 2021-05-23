using KPI.DB.Domain.Models;
using System.Threading.Tasks;

namespace KPI.DB.Persistance.Repositories
{
    public interface IPersonsRepository
    {
        Task Create(Person person);
        Task<Person> Get(string email);
        Task LinkToGroup(Person person, string groupName);
    }
}