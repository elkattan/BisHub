using System;
using Microsoft.AspNetCore.Mvc;
using BisHub.Models;

namespace BisHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private UserRepository repo = new UserRepository();

        // GET api/users
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(repo.GetUsers());
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(repo.GetUserById(id));
        }

        // POST api/users
        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            return Ok(repo.InsertUser(user));
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
