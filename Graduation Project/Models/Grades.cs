using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Models
{
    public class Grades
    {
        [Key]
        public int GradeId { get; set; }
        public string GradeName { get; set; }
    }
}
