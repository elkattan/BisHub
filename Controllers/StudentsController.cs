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
    public class StudentsController : ControllerBase
    {
        private IUserService _userService;

        public StudentsController(IUserService userService)
        {
            _userService = userService;
        }

        private StudentRepository repo = new StudentRepository();

        // GET api/students
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(repo.GetStudents());
        }

        // GET api/students/{ID}
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(repo.GetStudentById(id));
        }

        // POST api/students/{ID}/add/{SID}
        [HttpPost("{id}/add/{iid}")]
        public ActionResult Post(int id, int iid)
        {
            return Ok(repo.AddInstructor(id, iid));
        }
    }
}
