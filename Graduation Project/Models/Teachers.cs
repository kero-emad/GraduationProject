using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Models
{
    public class Teachers
    {
        [Key]
        public int TeacherID { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string subject { get; set; }
    }
}
