using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class students
    {
        [Key]
        public string StudentID {  get; set; }
        public string Name { get; set; }
        public string email {  get; set; }
        public string password { get; set; }
        [ForeignKey("Grades")]
        public int GradeId { get; set; }
        public int totalPoints {  get; set; }
        public Grades Grades { get; set; }

    }
}
