using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class Teachers
    {
        [Key]
        public string TeacherID { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        [ForeignKey("Subjects")]
        public int SubjectId { get; set; }
        public Subjects Subjects { get; set; }
    }
}
