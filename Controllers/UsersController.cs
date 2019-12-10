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
            // Checking username and password
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

        // POST api/users
        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult Register([FromBody] User user)
        {
            // Validation
            if (
                user.id != 0 ||
                user.username == null ||
                user.username.Length < 4 ||
                !user.isPasswordOkay() ||
                user.email == null ||
                !user.email.Contains(".") ||
                !user.email.Contains("@") ||
                user.citizen_id == null ||
                user.citizen_id.Length < 14 ||
                user.citizen_id.Length > 14
            )
            {
                return BadRequest(new { message = "Invalid User Data" });
            }
            // Checking for duplicates
            if (repo.GetUserByUsername(user.username) != null)
            {
                return BadRequest(new { message = "Username Already Taken" });
            }
            else if (repo.GetUserByField("email", user.email) != null)
            {
                return BadRequest(new { message = "Email Was Used Before" });
            }
            else if (repo.GetUserByField("citizen_id", user.citizen_id) != null)
            {
                return BadRequest(new { message = "Citizen ID Was Registered Before" });
            }
            return Ok(repo.InsertUser(user));
        }

        // GET api/users
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(repo.GetUsers());
        }

        // GET api/users/{ID}
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(repo.GetUserById(id));
        }

        // GET api/users/username/{username}
        [HttpGet("username/{username}")]
        public ActionResult Get(string username)
        {
            return Ok(repo.GetUserByUsername(username));
        }
    }

    public class Cred
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
