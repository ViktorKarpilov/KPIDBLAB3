using KPI.DB.Domain.Models;
using KPI.DB.Persistance.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KPI.DB.Server.Controllers
{
    public class PersonsController : BaseController
    {
        public PersonsController(IPersonsRepository personsRepository)
        {
            PersonsRepository = personsRepository;
        }

        public IPersonsRepository PersonsRepository { get; }

        [HttpPost]
        public async Task<Person> GetOrCreateUser([FromBody] Person person)
        {
            if ((await PersonsRepository.Get(person.Email)) is null)
                await PersonsRepository.Create(person);

            return await PersonsRepository.Get(person.Email);
        }

        [HttpPost("{id}")]
        public async Task<StatusCodeResult> LinkUserToGroup([FromQuery] string groupName, [FromRoute] int id)
        {
            try
            {
                await PersonsRepository.LinkToGroup(new Person() { Id = id }, groupName);
                return StatusCode(201);
            }
            catch (System.Exception)
            {
                return StatusCode(403);
            }
        }
    }
}
