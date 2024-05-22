using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public async Task<ActionResult> GetAllClientProjects()
        {
            try
            {
                var clientProjects = await _clientProjectService.GetAllClientProject();
                return Ok(clientProjects);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
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
        public async Task<IActionResult> PostClientProject(ClientProjectsDto clientProjectsDto)
        {
            var createdClientProject = await _clientProjectService.CreateClientProject(clientProjectsDto);

            if (createdClientProject == null)
            {
                return BadRequest("Unable to create the project.");
            }
            return CreatedAtAction(nameof(GetClientProject), new { id = createdClientProject.Id }, createdClientProject);
        }

        [HttpPut]
        public async Task<IActionResult> PutClientProject(int id, ClientProjectsDto clientProjectsDto)
        {
            if (id != clientProjectsDto.Id)
            {
                return BadRequest("Mismatched project ID.");
            }

            try
            {
                var clientProjectObject = await _clientProjectService.UpdateClientProject(clientProjectsDto);
                return Ok(clientProjectObject);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the project.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClientProject(int id)
        {
            var clientProject = await _clientProjectService.GetClientProject(id);
            if (clientProject == null)
            {
                return NotFound("Project not found");
            }

            var clientProjectObject = await _clientProjectService.DeleteClientProject(id);
            return Ok(clientProjectObject);
        }
    }
}
