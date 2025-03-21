using Graduation_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Graduation_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IConfiguration config;
        context context;
        public AccountsController(context _context, IConfiguration config) //to read from appsettings 
        {
            context = _context;
            this.config = config;
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
                var grade=context.grades
                    .FirstOrDefault(g=>g.GradeId == model.grade);
                var student = new students
                {
                    Name = model.Name,
                    email = model.email,
                    password = model.password,
                    GradeId = grade.GradeId,
                    totalPoints = 0
                };
                context.students.Add(student);
            }
            else if (model.type == RegisterDTO.UserType.Teacher)
            {
                var subject = context.subjects
                .FirstOrDefault(s => s.SubjectName == model.subject);
                var teacher = new Teachers
                {
                    Name = model.Name,
                    email = model.email,
                    password = model.password,
                    SubjectId = subject.SubjectId
                };
                context.teachers.Add(teacher);
            }
            else if (model.type == RegisterDTO.UserType.User)
            {
                var user = new Users
                {
                    Name = model.Name,
                    email = model.email,
                    password = model.password
                };
                context.users.Add(user);
            }
            context.SaveChanges();
            return Ok("Created");

        }
        [HttpPost("Login")]
        public IActionResult login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if student exists
            var student = context.students
                .FirstOrDefault(s => s.email == model.email && s.password == model.password);
            if (student != null)
            {
                // Generate JWT for the student
                var userClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, student.StudentID.ToString()),
            new Claim(ClaimTypes.Name, student.Name),
            new Claim(ClaimTypes.Role, "Student") // Assign role 'Student'
        };

                var token = GenerateJwtToken(userClaims);
                return Ok(new { Token = token, expiration = DateTime.Now.AddHours(1), Message = "Student logged in successfully.",email=student.email,name=student.Name,grade=student.GradeId });
            }

            // Check if teacher exists
            var teacher = context.teachers
                .FirstOrDefault(t => t.email == model.email && t.password == model.password);
            if (teacher != null)
            {
                // Generate JWT for the teacher
                var userClaims = new List<Claim>
        {
                    //token Generated id change(JWT predfined claims)
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, teacher.TeacherID.ToString()),
            new Claim(ClaimTypes.Name, teacher.Name),
            new Claim(ClaimTypes.Role, "Teacher") // Assign role Teacher
        };

                var token = GenerateJwtToken(userClaims);
                return Ok(new { Token = token, expiration = DateTime.Now.AddHours(1), Message = "Teacher logged in successfully.",email=teacher.email,name=teacher.Name,subject=teacher.Subjects.SubjectName });
            }

            // Check if other user exists
            var user = context.users
                .FirstOrDefault(u => u.email == model.email && u.password == model.password);
            if (user != null)
            {
                // Generate JWT for the other user
                var userClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, "User") // Assign role 'OtherUser'
        };

                var token = GenerateJwtToken(userClaims);
                return Ok(new { Token = token, expiration = DateTime.Now.AddHours(1), Message = " User logged in successfully." });
            }
            // Check if teacher exists
            var admin = context.Admins
                .FirstOrDefault(t => t.Email == model.email && t.Password == model.password);
            if (admin != null)
            {
                // Generate JWT for the teacher
                var userClaims = new List<Claim>
        {
                    //token Generated id change(JWT predefined claims)
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier,admin .AdminID.ToString()),
            new Claim(ClaimTypes.Name, admin.FirstName),
            new Claim(ClaimTypes.Role, "Admin") // Assign role 'Admin'
        };

                var token = GenerateJwtToken(userClaims);
                return Ok(new { Token = token, expiration = DateTime.Now.AddHours(1) });
            }

            return Unauthorized("Invalid email or password.");
        }

        private string GenerateJwtToken(List<Claim> claims)
        {
            //design token
            var MyToken = new JwtSecurityToken(
                audience: config["jwt:AudienceIP"],//to read from appsetting 
                issuer: config["jwt:IssuerIP"],
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:secretKey"])),
                    SecurityAlgorithms.HmacSha256)
            );
            //generate token response
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(MyToken);

        }
        [HttpGet("AllStudents")]
        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult getStudents()
        {
            var u = context.students.ToList();
            return Ok(u);
        }
        [HttpGet("AllTeachers")]
        [Authorize(Roles = "Admin")]
        public IActionResult getTeachers()
        {
            var u = context.teachers.ToList();
            return Ok(u);
        }
        [HttpGet("AllUser")]
        [Authorize(Roles = "Admin")]
        public IActionResult getUsers()
        {
            var u = context.users.ToList();
            return Ok(u);
        }

        [HttpPost]
        [Route("subjects")]
        public IActionResult subjects([FromBody] int grade)
        {
            var subjectsIDs=context.gradeSubject.Where(s=>s.GradeId==grade)
                .Select(s=>s.SubjectId)
                .ToList();
            var subjects = context.subjects.Where(s => subjectsIDs.Contains(s.SubjectId))
                .Select(s => s.SubjectName)
                .ToList();
            return Ok(subjects);
        }
        [HttpPost("chapters")]        
        public IActionResult chapters([FromBody] GetChaptersDTO model)
        {
            var subjectID=context.subjects.Where(s=>s.SubjectName==model.subject)
                .Select(s=>s.SubjectId).FirstOrDefault();
            var gradesubject=context.gradeSubject
                .Where(s=>s.GradeId==model.grade&&s.SubjectId==subjectID)
                .Select(gs=>gs.GradeSubjectId).FirstOrDefault();
            var chapters = context.chapters.Where(c => c.GradeSubjectId == gradesubject)
                .ToDictionary(c => c.ChapterNumber, c => c.ChapterName);
            return Ok(chapters);
        }

    }
}