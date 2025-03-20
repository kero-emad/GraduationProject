using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class GradeSubject
    {
        public int GradeSubjectId {  get; set; }

        [ForeignKey("Grades")]
        public int GradeId {  get; set; }

        [ForeignKey("Subjects")]
        public int SubjectId {  get; set; }
        public Grades Grades { get; set; }
        public Subjects Subjects { get; set; }

        public List<Chapters> Chapters { get; set; }

        public List<EducationQuestions> EducationQuestions { get; set; }
    }
}
