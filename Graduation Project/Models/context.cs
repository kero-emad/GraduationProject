using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Models
{
    public class context:DbContext

    {
        public DbSet<students> students {  get; set; }
        public DbSet<Teachers> teachers { get; set; }
        public DbSet<Users>users { get; set; }
        public DbSet<EducationQuestions> educationQuestions { get; set; }
        public DbSet<Hints> hints { get; set; }
        public context(DbContextOptions<context> options) : base(options) { }
    }
}
