using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class Hints
    {
        [Key]
        public int HintID {  get; set; }
        public string hint {  get; set; }
        [ForeignKey("EducationQuestions")]
        public int QuestionID {  get; set; }
        public EducationQuestions EducationQuestions { get; set; }
    }
}
