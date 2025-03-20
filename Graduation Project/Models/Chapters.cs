using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class Chapters
    {
        [Key]
        public int ChapterId {  get; set; }
        public int ChapterNumber { get; set; }
        public string ChapterName { get; set; }

        [ForeignKey("GradeSubject")]
        public int GradeSubjectId {  get; set; }
        public GradeSubject GradeSubject { get; set; }
    }
}
