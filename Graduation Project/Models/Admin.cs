using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Models
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
