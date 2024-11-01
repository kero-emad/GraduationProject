using Graduation_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        context context;
        public AccountsController(context _context) 
        { 
            context = _context;
        }
        [HttpPost("register")]
        public IActionResult register([FromBody] RegisterDTO model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            bool emailExists = context.students.Any(s => s.email == model.email) ||
                               context.teachers.Any(t => t.email == model.email) ||
                               context.users.Any(u => u.email == model.email);
            if (emailExists)
            {
                return BadRequest("Email already exists please login");
            }
            if (model.type == RegisterDTO.UserType.Student)
            {
                var student = new students
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    email = model.email,
                    password = model.password,
                    grade = model.grade,
                    totalPoints = 0
                };
                context.students.Add(student);
            }
            else if (model.type == RegisterDTO.UserType.Teacher)
            {
                var teacher = new Teachers
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    email = model.email,
                    password = model.password,
                    subject = model.subject
                };
                context.teachers.Add(teacher);
            }
            else if (model.type == RegisterDTO.UserType.User)
            {
                var user = new Users
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    email = model.email,
                    password = model.password
                };
                context.users.Add(user);
            }
            context.SaveChanges();
            return Created();

            }
        [HttpPost("Login")]
        public IActionResult login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var student = context.students
                .FirstOrDefault(s => s.email == model.email && s.password == model.password);
            if (student != null )
            {
                return Ok("Student logged in successfully.");
            }
            var teacher = context.teachers
                .FirstOrDefault(t => t.email == model.email && t.password == model.password);

            if (teacher != null)
            {
                return Ok("Teacher logged in successfully.");
            }
            var user = context.users
                .FirstOrDefault(u => u.email == model.email && u.password == model.password);

            if (user != null)
            {
                return Ok("Other User logged in successfully.");
            }
            return Unauthorized("Invalid email or password.");
        }
        [HttpGet]
        public IActionResult getusers() 
        {
            var u = context.users.ToList();
            return Ok(u);
        }
    }
    }

