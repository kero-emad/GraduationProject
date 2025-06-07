using Graduation_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Controllers.Entertainment
{
    [Route("api/entertainment/[controller]")]
    [ApiController]
    public class OffsideGameController : ControllerBase
    {
        context context;
        public OffsideGameController(context _context)
        {
            context = _context;
        }
        [HttpGet("question/{section}")]
        public IActionResult GetOffsideQuestionBySection(int section)
        {
            if (!Enum.IsDefined(typeof(EntertainmentQuestions.Section), section))
                return BadRequest("Invalid section number.");

            var question = context.entertainmentQuestions
                .Include(q => q.Hints)
                .Where(q => q.section == (EntertainmentQuestions.Section)section && q.game == "offside")
                .AsEnumerable()
                .Select(q => new GetInformationDTO
                {
                    information = q.Hints
                        .Where(h => h.QuestionType == QuestionType.Entertainment)
                        .Select(h => h.hint)
                        .ToList(),

                    correctAnswer = q.answer
                        .Select(c => int.Parse(c.ToString()))
                        .ToList()
                })
                .FirstOrDefault();

            if (question == null)
                return NotFound("No question found for this section.");

            return Ok(question);
        }
    }
}
