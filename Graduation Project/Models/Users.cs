using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Models
{
    public class Users
    {
        [Key]
        public string UserID { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
