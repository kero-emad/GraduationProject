using Graduation_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Controllers.Entertainment
{
    [Route("api/entertainment/[controller]")]
    [ApiController]
    public class HintGameController : ControllerBase
    {
        context context;
        public HintGameController(context _context)
        {
            context = _context;
        }
        [HttpGet("question/{section}")]
        public IActionResult GetQuestionBySection(int section)
        {
            // Ensure section is valid enum value
            if (!Enum.IsDefined(typeof(EntertainmentQuestions.Section), section))
                return BadRequest("Invalid section number.");

            var question = context.entertainmentQuestions
                .Include(q => q.Hints)
                .Where(q => q.section == (EntertainmentQuestions.Section)section && q.game == "five hints")
                .Select(q => new GetHintsDTO
                {
                    hints = q.Hints
                        .Where(h => h.QuestionType == QuestionType.Entertainment)
                        .Select(h => h.hint)
                        .ToList(),
                    correctAnswer = q.answer
                })
                .FirstOrDefault();

            if (question == null)
                return NotFound("No question found for this section.");

            return Ok(question);
        }
    }
}
