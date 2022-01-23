using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.DTO;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<UserDTOwithID> users = await _userService.ReadAll();
            return Json(users);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("username/{id}")]
        public async Task<IActionResult> GetByUsername(string id)
        {
            UserDTOwithID? user = await _userService.Read(id);
            if (user == null)
                return NotFound();
            return Json(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDTO user)
        {
            if (user.Username==null||user.Email==null||user.Password==null)
                return BadRequest();
            try
            {
                await _userService.Create(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDTO user)
        {
            if (user.Username == null && user.Email == null && user.Password == null)
                return BadRequest();
            try
            {
                await _userService.Update(id, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.Delete(id);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            try
            {
                string? token = await _userService.Login(login);
                if (token == null)
                    return Unauthorized("Wrong password");
                return Ok(token);
            }
            catch (NullReferenceException)
            {
                return NotFound("User does not exist");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Sorry, your logins are temporarily locked out. This happens when you enter a wrong password too many times." +
                    " The lockout should automatically subside within 3 minutes.");
            }
        }

        [HttpGet("bysession/{id}")]
        public async Task<IActionResult> GetBySession(Guid id)
        {
            string? username = await _userService.UsernameFromSession(id);
            return Json(username);
        }

        [HttpDelete("session/{id}")]
        public async Task<IActionResult> DeleteSession(Guid id)
        {
            try
            {
                await _userService.EndSession(id);
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            return Ok();
        }
    }
}
