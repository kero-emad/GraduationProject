using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class EducationQuestions
    {
        [Key]
        public int QuestionID {  get; set; }
        //[ForeignKey("Subjects")]
        //public int SubjectId { get; set; }

        //[ForeignKey("Grades")]
        //public int GradeId {  get; set; }

        [ForeignKey("Chapters")]
        public int ChapterId {  get; set; }

        [ForeignKey("GradeSubject")]
        public int GradeSubjectId {  get; set; }
        public string answer {  get; set; }
        public string summary { get; set; }
        public string game { get; set; }
        public Status status{  get; set; }

        public List<Hints> Hints {  get; set; }
        //public Grades Grades { get; set; }
        public Chapters Chapters { get; set; }
        //public Subjects Subjects { get; set; }

        public GradeSubject GradeSubject { get; set; }
        public enum Status
        {
            underreview,
            approved,
            rejected
        }
            
    }
}
