using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Controllers.HRIS
{
    [Route("client-project")]
    [ApiController]
    public class ClientProjectsController : ControllerBase
    {
        private readonly IClientProjectService _clientProjectService;
        public ClientProjectsController(IClientProjectService clientProjectService)
        {
            _clientProjectService = clientProjectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClientProjects()
        {
            var clientProjects = await _clientProjectService.GetAllClientProject();
            return Ok(clientProjects);
        }

        [HttpGet]
        public async Task<ActionResult<ClientProjectsDto>> GetClientProject(int id)
        {
            var clientProjectDto = await _clientProjectService.GetClientProject(id);

            if (clientProjectDto == null)
            {
                return NotFound();
            }

            return Ok(clientProjectDto);
        }

        [HttpPost]
        public async Task<ActionResult<ClientProject>> PostClientProject(ClientProject clientProject)
        {
            var createdClientProject = await _clientProjectService.CreateClientProject(clientProject);
            return CreatedAtAction(nameof(GetClientProject), new { id = createdClientProject.Id }, createdClientProject);
        }

        [HttpPut]
        public async Task<IActionResult> PutClientProject(int id, ClientProject clientProject)
        {
            if (id != clientProject.Id)
            {
                return BadRequest();
            }

            await _clientProjectService.UpdateClientProject(clientProject);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClientProject(int id)
        {
            var clientProject = await _clientProjectService.GetClientProject(id);
            if (clientProject == null)
            {
                return NotFound();
            }

            await _clientProjectService.DeleteClientProject(id);
            return NoContent();
        }
    }
}
