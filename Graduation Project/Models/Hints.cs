using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class Hints
    {
        [Key]
        public int HintID {  get; set; }
        public string hint {  get; set; }
        [ForeignKey("Questions")]
        public int QuestionID {  get; set; }
        public QuestionType QuestionType { get; set; }

        public Questions Questions { get; set; }

    }

    public enum QuestionType
    {
        Education,
        Entertainment
    }
}
