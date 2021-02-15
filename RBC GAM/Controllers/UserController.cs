using Microsoft.AspNetCore.Mvc;
using RBC_GAM.Model;
using RBC_GAM.Repositories;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RBC_GAM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // GET: api/<UserController>
        [HttpGet("{id}")]

        public async Task<IActionResult> Get(int id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user != null)
                return Ok(user);
            else
                return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _userRepository.GetAsync();
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            var result = await _userRepository.AddAsync(user);
            if (result)
                return Ok();
            else
                return BadRequest();
        }

    }
}
