using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RGO_Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService profileService;

        public ProfileController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        // GET: api/<ProfileController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProfileController>/5
        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser([FromQuery]string email)
        {
            try
            {
            var user = profileService.GetUserByEmail(email);
                System.Diagnostics.Debug.WriteLine("user " + user);
                return Ok(user);

            }
            catch (Exception e)
            {

                await Console.Out.WriteLineAsync(e.Message);
                return BadRequest(e.Message);
            }
        }

        // POST api/<ProfileController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProfileController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProfileController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
