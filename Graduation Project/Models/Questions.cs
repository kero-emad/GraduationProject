using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Models
{
    public abstract class Questions
    {
        [Key]
        public int QuestionID { get; set; }
        public string answer { get; set; }
        public string summary { get; set; }
        public string game { get; set; }

        public List<Hints> Hints { get; set; }
        public Difficulty DifficultyLevel { get; set; }
        public enum Difficulty
        {
            easy,
            medium,
            difficult
        }
    }
}
