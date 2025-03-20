using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Models
{
    public class Subjects
    {
        [Key]
        public int SubjectId {  get; set; }
        public string SubjectName { get; set; }
    }
}
