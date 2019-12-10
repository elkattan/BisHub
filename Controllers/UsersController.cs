using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

using BisHub.Entities;
using BisHub.Models;
using BisHub.Services;

namespace BisHub.Controllers
{
    [Authorize]
    [ApiController]
    [EnableCors("AllowBisHub")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        private UserRepository repo = new UserRepository();

        // POST api/users/login
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Authenticate([FromBody] Cred cred)
        {
            // Checkng username and password
            if (cred.username == null || cred.password == null)
            {
                return null;
            }

            // Authenticating User
            User user = _userService.Authenticate(cred.username, cred.password);

            // Checking if Authentication Seccedded
            if (user == null) return BadRequest(new { message = "Invalid credentials" });
            return Ok(user);
        }

        // GET api/users
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(repo.GetUsers());
        }

        // POST api/users
        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            return Ok(repo.InsertUser(user));
        }

        // GET api/users/{ID}
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(repo.GetUserById(id));
        }
    }

    public class Cred
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
