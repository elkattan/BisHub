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
    public class InstructorsController : ControllerBase
    {
        private IUserService _userService;

        public InstructorsController(IUserService userService)
        {
            _userService = userService;
        }

        private InstructorRepository repo = new InstructorRepository();

        // GET api/instructors
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(repo.GetInstructors());
        }

        // GET api/instructors/{ID}
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(repo.GetInstructorById(id));
        }

        // POST api/instructors/{ID}/add/{SID}
        [HttpPost("{id}/add/{sid}")]
        public ActionResult Post(int id, int sid)
        {
            return Ok(repo.AddStudent(id, sid));
        }

    }
}
