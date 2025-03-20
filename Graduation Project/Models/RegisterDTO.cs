using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Models
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Name is required")]
        [MinLength(3,ErrorMessage ="Name can't be less than 3 letters")]
        public string Name {  get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }
        public UserType type { get; set; }

        [Required ]
        public int? grade {  get; set; }
        public string? subject {  get; set; }

        public enum UserType
        {
            Student,
            Teacher,
            User
        }
    }
}
