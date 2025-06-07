using Graduation_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Graduation_Project.Controllers.Education
{
    [Route("api/education/[controller]")]
    [ApiController]
    public class SixQuestionsController : ControllerBase
    {
        context context;
        public SixQuestionsController(context _context)
        {
            context = _context;
        }


        [HttpPost("question")]
        public IActionResult GetQuestionsList([FromBody] GetQuestionsDTO getQuestionsDTO)
        {
            var gradeSubject = context.gradeSubject
                .Include(gs => gs.Grades)
                .Include(gs => gs.Subjects)
                .FirstOrDefault(gs =>
                    gs.Grades.GradeId == getQuestionsDTO.grade &&
                    gs.Subjects.SubjectName == getQuestionsDTO.subject);

            if (gradeSubject == null)
                return NotFound("Grade-Subject combination not found");


            var chapter = context.chapters
                .FirstOrDefault(c =>
                    c.GradeSubjectId == gradeSubject.GradeSubjectId &&
                    c.ChapterNumber == getQuestionsDTO.chapter);

            if (chapter == null)
                return NotFound("Chapter not found");


            var questions = context.educationQuestions.Include(q => q.Hints)
                .Where(q => q.GradeSubjectId == gradeSubject.GradeSubjectId &&
                            q.ChapterId == chapter.ChapterId && q.game == "DifficultyLevel")
                .OrderBy(q => q.DifficultyLevel)
                .Take(6)
                 .AsEnumerable()
                .Select(q => new GetInformationDTO
                {

                    information = q.Hints.Select(h => h.hint).ToList(),
                    correctAnswer = q.answer.Select(c => int.Parse(c.ToString())).ToList()
                })
                .ToList();

            if (!questions.Any())
                return NotFound("No questions found.");

            return Ok(questions);
        }

    }
}