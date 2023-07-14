using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;
using RGO.Repository;
using RGO.Repository.Entities;
using RGO.Repository.Interfaces;

namespace RGO_Backend.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public ITestService _testSevice;
        public IUserGroupsRepository _userGroupsRepository;

        public DatabaseContext _dbContext;
        public TestController(ITestService testService, DatabaseContext dbContext, IUserGroupsRepository userGroupsRepository)
        {
            _testSevice = testService;
            _dbContext = dbContext;
            _userGroupsRepository = userGroupsRepository;
        }

        [HttpGet]
        [Route("/get-value")]
        public async Task<UserGroup[]> Get()
        {
           return await _userGroupsRepository.getUserGroups();
        }
    }
}
