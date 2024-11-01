using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Models
{
    public class students
    {
        [Key]
        public int StudentID {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string email {  get; set; }
        public string password { get; set; }
        public string grade { get; set; }
        public int totalPoints {  get; set; }

    }
}
