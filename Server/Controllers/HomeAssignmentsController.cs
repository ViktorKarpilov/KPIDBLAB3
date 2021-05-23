using KPI.DB.Domain.Models;
using KPI.DB.Persistance.Repositories;
using KPI.DB.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KPI.DB.Server.Controllers
{
    public class HomeAssignmentsController : BaseController
    {
        public HomeAssignmentsController(IHomeAssignmentRepository homeAssignmentsRepository)
        {
            HomeAssignmentsRepository = homeAssignmentsRepository;
        }

        private IHomeAssignmentRepository HomeAssignmentsRepository { get; }

        [HttpGet("{personId}")]
        public async Task<IEnumerable<Domain.Models.HomeAssignment>> GetPersonAssingments([FromRoute] int personId)
        {
            return await HomeAssignmentsRepository.GetAllAsignmentsForPerson(personId);
        }

        [HttpPost]
        public async Task<StatusCodeResult> CreateAssignment([FromBody] CreateHomeAssignment createAssignment)
        {
            try
            {
                Domain.Models.HomeAssignment assignment = new Domain.Models.HomeAssignment()
                {
                    Deadline = createAssignment.assignment.Deadline,
                    Points = createAssignment.assignment.Points,
                };
                await HomeAssignmentsRepository.AssignToPerson(assignment, createAssignment.PersonId);
                return StatusCode(201);
            }
            catch (System.Exception)
            {
                return StatusCode(400);
            }
        }

        [HttpPost("group")]
        public async Task<StatusCodeResult> CreateGroupAssignment([FromBody] GroupAssignment assignment)
        {
            try
            {
                Domain.Models.HomeAssignment domainModel = new Domain.Models.HomeAssignment()
                {
                    Deadline = assignment.Assignment.Deadline,
                    Points = assignment.Assignment.Points
                };
                await HomeAssignmentsRepository.AssignToGroup(domainModel, assignment.GroupName);
                return StatusCode(201);
            }
            catch (System.Exception)
            {
                return StatusCode(400);
            }
        }

    }
}
