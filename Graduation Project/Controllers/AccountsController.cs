using Graduation_Project.Models;
using Graduation_Project.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
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
        UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountsController> _logger;
        private readonly IEmailService _emailService;

        public AccountsController(context _context, IConfiguration config, UserManager<IdentityUser> userManager, ILogger<AccountsController> logger,IEmailService emailService)
        {
            context = _context;
            this.config = config;
            _userManager = userManager;
            _logger = logger;
            _emailService = emailService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] RegisterDTO model)
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
                return BadRequest("Email already exists, please login.");
            }

            ///////////userManager
            

            var user = new IdentityUser
            {
                UserName = model.email,
                Email = model.email,
            };
            var result = await _userManager.CreateAsync(user, model.password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest($"Error creating user: {errors}");
            }

            

            try
            {
                if (model.type == RegisterDTO.UserType.Student)
                {
                    var grade = context.grades
                        .FirstOrDefault(g => g.GradeId == model.grade);
                    if (grade == null)
                        return BadRequest("Grade not found.");
                    var student = new students
                    {
                        StudentID = user.Id,
                        Name = model.Name,
                        email = model.email,
                        password = user.PasswordHash,
                        GradeId = grade.GradeId,
                        totalPoints = 0
                    };
                    context.students.Add(student);
                }
                else if (model.type == RegisterDTO.UserType.Teacher)
                {
                    var subject = context.subjects
                        .FirstOrDefault(s => s.SubjectName == model.subject);
                    if (subject == null)
                        return BadRequest("Subject not found.");
                    var teacher = new Teachers
                    {
                        TeacherID = user.Id,
                        Name = model.Name,
                        email = model.email,
                        password = user.PasswordHash,
                        SubjectId = subject.SubjectId
                    };
                    context.teachers.Add(teacher);
                }
                else if (model.type == RegisterDTO.UserType.User)
                {
                    var User = new Users
                    {
                        UserID = user.Id,
                        Name = model.Name,
                        email = model.email,
                        password = user.PasswordHash
                    };
                    context.users.Add(User);
                }

                context.SaveChanges();
                return Ok("Created");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while saving changes: {ex.Message}. Inner Exception: {ex.InnerException?.Message}");
                return StatusCode(500, $"An error occurred while saving changes: {ex.Message}");
            }
        }





        [HttpPost("Login")]
        public async Task<IActionResult> login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var user = await _userManager.FindByEmailAsync(model.email);
            if (user == null)
            {
                return Unauthorized("Email doesn't exist please register");
            }

         
            var result = await _userManager.CheckPasswordAsync(user, model.password);
            if (!result)
            {
                return Unauthorized("Invalid password.");
            }

            
            string role = "";
            int grade=0;
            var subject = "";
            var student = context.students.FirstOrDefault(s => s.email == model.email);
            if (student != null)
            {
                role = "Student";
                grade = context.students.Where(s=>s.GradeId==student.GradeId).Select(s=>s.GradeId).FirstOrDefault();
            }
            else
            {
                var teacher = context.teachers.FirstOrDefault(t => t.email == model.email);
                if (teacher != null)
                {
                    role = "Teacher";
                    subject=context.subjects.Where(s=>s.SubjectId==teacher.SubjectId).Select(s=>s.SubjectName).FirstOrDefault();
                }
                else
                {
                    var regularUser = context.users.FirstOrDefault(u => u.email == model.email);
                    if (regularUser != null)
                    {
                        role = "User";
                    }
                    else
                    {
                        var admin = context.Admins.FirstOrDefault(a => a.Email == model.email);
                        if (admin != null)
                        {
                            role = "Admin";
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(role))
            {
                return Unauthorized("Invalid email or password.");
            }

            
            var userClaims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Role, role) 
    };

            var token = GenerateJwtToken(userClaims);

            if (role== "Student")
            return Ok(new { Token = token, expiration = DateTime.Now.AddHours(1), Message = $"{role} logged in successfully.",role=role,grade=grade});
            else if (role=="Teacher")
            return Ok(new { Token = token, expiration = DateTime.Now.AddHours(1), Message = $"{role} logged in successfully.", role = role, subject = subject });
            else
            return Ok(new { Token = token, expiration = DateTime.Now.AddHours(1), Message = $"{role} logged in successfully.", role = role});

        }

        private string GenerateJwtToken(List<Claim> claims)
        {
            
            var MyToken = new JwtSecurityToken(
                audience: config["jwt:AudienceIP"],
                issuer: config["jwt:IssuerIP"],
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:secretKey"])),
                    SecurityAlgorithms.HmacSha256)
            );


            
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
        public IActionResult subjects([FromBody] GetSubjectsDTO model)
        {
            var subjectsIDs = context.gradeSubject.Where(s => s.GradeId == model.grade)
                .Select(s => s.SubjectId)
                .ToList();
            var subjects = context.subjects.Where(s => subjectsIDs.Contains(s.SubjectId))
                .Select(s => s.SubjectName)
                .ToList();
            return Ok(new { subjects = subjects });
        }
        [HttpPost("chapters")]
        public IActionResult chapters([FromBody] GetChaptersDTO model)
        {
            var subjectID = context.subjects.Where(s => s.SubjectName == model.subject)
                .Select(s => s.SubjectId).FirstOrDefault();
            var gradesubject = context.gradeSubject
                .Where(s => s.GradeId == model.grade && s.SubjectId == subjectID)
                .Select(gs => gs.GradeSubjectId).FirstOrDefault();
            var chapters = context.chapters.Where(c => c.GradeSubjectId == gradesubject)
                .Select(c => new
                {
                    number = c.ChapterNumber,
                    name = c.ChapterName
                })
                .ToList();

            return Ok(new { chapters = chapters });
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> forgetPassword(forgetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("UserNotFound");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Sorry,something went wrong");
            }

            var resetLink = Url.Action(
               "ResetPassword",
               "Accounts",
               new { token = token, email = user.Email },
                protocol: Request.Scheme
                  );

            var subject = "Password Reset Request";
            var message = $"Welcome in EDU Play Go to the link shown below to reset your password: {resetLink}";

            
            await _emailService.SendEmailAsync(user.Email, subject, message);

            return Ok(new
            {
                token = token,
                email = user.Email,
            }
            );
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("UserNotFound");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return Ok("Password reset successfully");
            }

            return BadRequest("Sorry, something went wrong");

        }
    }
}