using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Models
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="please enter your Email")]
        public string email { get; set; }

        [Required(ErrorMessage ="please enter your password")]
        public string password { get; set; }
    }
}
