using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Controllers.HRIS
{
    [Route("client-project")]
    [ApiController]
    public class ClientProjectsController : ControllerBase
    {
        private readonly IClientProjectService _clientProjectService;
        private readonly AuthorizeIdentity _identity;

        public ClientProjectsController(AuthorizeIdentity identity, IClientProjectService clientProjectService)
        {
            _clientProjectService = clientProjectService;
            _identity = identity;
        }

        [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
        [HttpGet]
        public async Task<ActionResult> GetAllClientProjects()
        {
                var clientProjects = await _clientProjectService.GetAllClientProjects();
                return Ok(clientProjects);
        }

        [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientProjectsDto>> GetClientProjectById(int id)
        {
            if (!_identity.IsSupport && id != _identity.EmployeeId)
                throw new CustomException("Unauthorized Access.");

            var clientProjectDto = await _clientProjectService.GetClientProjectById(id);
                return Ok(clientProjectDto);
        }

        [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> SaveClientProject(ClientProjectsDto clientProjectsDto)
        {
            if (!_identity.IsSupport && clientProjectsDto.EmployeeId != _identity.EmployeeId)
                throw new CustomException("Unauthorized Access.");

            var createdClientProject = await _clientProjectService.CreateClientProject(clientProjectsDto);
                return Ok(createdClientProject);
        }

        [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
        [HttpPut]
        public async Task<IActionResult> UpdateClientProject(ClientProjectsDto clientProjectsDto)
        {
            if (!_identity.IsSupport && clientProjectsDto.EmployeeId != _identity.EmployeeId)
                throw new CustomException("Unauthorized Access.");

            var clientProjectObject = await _clientProjectService.UpdateClientProject(clientProjectsDto);
                return Ok(clientProjectObject);
            
          
        }

        [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
        [HttpDelete]
        public async Task<IActionResult> DeleteClientProject(int id)
        {
            if (!_identity.IsSupport && id != _identity.EmployeeId)
                throw new CustomException("Unauthorized Access.");

            var clientProjectObject = await _clientProjectService.DeleteClientProject(id);
            return Ok(clientProjectObject);
        }
    }
}
