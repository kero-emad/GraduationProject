using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class EducationQuestions:Questions
    {
        

        [ForeignKey("Chapters")]
        public int ChapterId {  get; set; }

        [ForeignKey("GradeSubject")]
        public int GradeSubjectId {  get; set; }
        
        public Status status{  get; set; }
        

       
       
        public Chapters Chapters { get; set; }
        

        public GradeSubject GradeSubject { get; set; }
        public enum Status
        {
            underreview,
            approved,
            rejected
        }

       

    }
}
