using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Models
{
    public class context:IdentityDbContext<IdentityUser>

    {
        public DbSet<students> students {  get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Teachers> teachers { get; set; }
        public DbSet<Users>users { get; set; }
        public DbSet<Chapters>chapters { get; set; }
        public DbSet<Grades>grades { get; set; }
        public DbSet<Subjects>subjects { get; set; }
        public DbSet<GradeSubject>gradeSubject { get; set; }
        public DbSet<EducationQuestions> educationQuestions { get; set; }
        public DbSet<Hints> hints { get; set; }
        public context(DbContextOptions<context> options) : base(options) { }
    }
}
