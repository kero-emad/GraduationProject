using Azure.Core;
using Graduation_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Controllers.Education
{
    [Route("api/education/[controller]")]
    [ApiController]
    public class HintGameController : ControllerBase
    {
        context context;
        public HintGameController(context _context)
        {
            context = _context;
        }
        [HttpPost("question")]
        public IActionResult getQuestion([FromBody] GetQuestionsDTO getQuestionsDTO)
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



            var question = context.educationQuestions.Include(q => q.Hints)
                .Where
                (q => q.GradeSubjectId == gradeSubject.GradeSubjectId &&
                 q.ChapterId == chapter.ChapterId &&
                 q.game == "five hints" &&
                 q.Hints.Any(h => h.QuestionType == QuestionType.Education))
                .Select(q => new GetHintsDTO
                {
                    hints = q.Hints
                    .Where(h => h.QuestionType == QuestionType.Education)
                    .Select(h => h.hint).ToList(),
                    correctAnswer = q.answer
                }).FirstOrDefault();
            if (question == null)
            {
                return NotFound("Question not found.");
            }
            //var cookieOptions = new CookieOptions
            //{
            //  HttpOnly = true, 
            // Expires = DateTime.Now.AddMinutes(20)
            //};
            //Response.Cookies.Append("correctAnswer", question.correctAnswer, cookieOptions);
            //HttpContext.Session.SetString("correctAnswer", question.correctAnswer);
            return Ok(question);
        }

        [HttpPost("ans")]
        public IActionResult getAnswer(GetAnswerDTO getAnswerDTO)
        {
            string correctans = getAnswerDTO.correctanswer;
            //string correctans = Request.Cookies["correctAnswer"];
            if (string.IsNullOrEmpty(correctans))
            {
                return BadRequest("Correct answer not found in cookie.");
            }
            //string correctans = HttpContext.Session.GetString("correctAnswer");
            //Console.WriteLine($"Correct answer retrieved from session: {correctans}");
            //if (string.IsNullOrEmpty(correctans))
            // {
            //  return BadRequest("Correct answer not found in session.");
            //}
            bool iscorrect = false;
            int hintsused = getAnswerDTO.hintsused;
            while (hintsused <= 5)
            {
                double similarity = calculate(getAnswerDTO.answer, correctans);
                if (similarity >= 0.7)
                {
                    iscorrect = true;
                    if (hintsused == 1) return Ok(25);
                    else if (hintsused == 2) return Ok(20);
                    else if (hintsused == 3) return Ok(15);
                    else if (hintsused == 4) return Ok(10);
                    else if (hintsused == 5) return Ok(5);
                }
                else
                {
                    return Ok(0);
                }
            }
            return Ok(0);
        }
        private double calculate(string str1, string str2)
        {
            var words1 = str1.Split(' ').GroupBy(w => w).ToDictionary(g => g.Key, g => g.Count());
            var words2 = str2.Split(' ').GroupBy(w => w).ToDictionary(g => g.Key, g => g.Count());

            var allWords = words1.Keys.Union(words2.Keys).ToList();

            double dotProduct = 0;
            double magnitude1 = 0;
            double magnitude2 = 0;

            foreach (var word in allWords)
            {
                int count1 = words1.ContainsKey(word) ? words1[word] : 0;
                int count2 = words2.ContainsKey(word) ? words2[word] : 0;

                dotProduct += count1 * count2;
                magnitude1 += count1 * count1;
                magnitude2 += count2 * count2;
            }

            if (magnitude1 == 0 || magnitude2 == 0) return 0;

            return dotProduct / (Math.Sqrt(magnitude1) * Math.Sqrt(magnitude2));
        }


        /*
        [HttpPost("makequestion")]
        public IActionResult makequestion([FromBody] MakeQuestionDTO makeQuestionDTO)
        {
            if (makeQuestionDTO == null)
            {
                return BadRequest("No question to added");
            }
            var question = new EducationQuestions
            {
                subject = makeQuestionDTO.subject,
                grade =  makeQuestionDTO.grade,
                chapter = makeQuestionDTO.chapter,
                Hints = makeQuestionDTO.hints.Select(h=>new Hints { hint = h }).ToList(),
                answer = makeQuestionDTO.answer,
                summary= makeQuestionDTO.summary,
                status=0
            };
            context.educationQuestions.Add(question);
            context.SaveChanges();
            return Ok("Question submitted succesfully and will send to the admin");
        }
        */

    }

}
