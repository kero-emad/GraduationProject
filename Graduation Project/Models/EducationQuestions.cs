using System.ComponentModel.DataAnnotations;

namespace Graduation_Project.Models
{
    public class EducationQuestions
    {
        [Key]
        public int QuestionID {  get; set; }
        public string subject { get; set; }
        public string grade {  get; set; }
        public string chapter {  get; set; }
        public string answer {  get; set; }
        public string summary { get; set; }
        public Status status{  get; set; }

        public List<Hints> Hints {  get; set; }

        public enum Status
        {
            underreview,
            approved,
            rejected
        }
            
    }
}
