using KPI.DB.Domain.Models;
using KPI.DB.Persistance.Repositories;
using KPI.DB.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Group = KPI.DB.Domain.Models.Group;
using Person = KPI.DB.Domain.Models.Person;

namespace KPI.DB.Server.Controllers
{
    public class GroupsController : BaseController
    {
        public GroupsController(IGroupsRepository repository)
        {
            Repository = repository;
        }

        public IGroupsRepository Repository { get; }

        [HttpPost]
        public async Task<StatusCodeResult> AssignPersonToGroup([FromBody] PersonGroup createGroup)
        {
            try
            {
                await Repository.AssignPersonToGroup(new Person() { Id = createGroup.Person.Id }, createGroup.GroupName);
                return StatusCode(201);
            }
            catch (System.Exception)
            {
                return StatusCode(400);
            }
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<Group>> GetAllPersonGroups(int id)
        {
            return await Repository.GetAllPersonGroups(new Person() { Id=id});
        }

        [HttpGet]
        public async Task<IEnumerable<Group>> GetAllGroups()
        {
            return await Repository.GetAllGroups();
        }

    }
}
