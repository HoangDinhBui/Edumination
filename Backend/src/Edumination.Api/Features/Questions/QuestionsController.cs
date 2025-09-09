// Edumination.Api.Controllers/QuestionsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Edumination.Api.Dtos;
using Edumination.Api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Edumination.Api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IQuestionChoiceService _questionChoiceService; // Thêm service
        private readonly ILogger<QuestionsController> _logger;

        public QuestionsController(
            IQuestionService questionService,
            IQuestionChoiceService questionChoiceService, // Thêm dependency
            ILogger<QuestionsController> logger)
        {
            _questionService = questionService;
            _questionChoiceService = questionChoiceService;
            _logger = logger;
        }

        [HttpPost("sections/{sid}/questions")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<IActionResult> CreateQuestion(long sid, [FromBody] QuestionCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid: {@Errors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(ModelState);
            }

            try
            {
                var createdQuestion = await _questionService.CreateQuestionAsync(sid, dto);
                return CreatedAtAction(nameof(GetQuestion), new { sid, qid = createdQuestion.Id }, createdQuestion);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Section not found for ID: {Sid}", sid);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Conflict error for section ID: {Sid}", sid);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error for section ID: {Sid}", sid);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("sections/{sid}/questions/{qid}")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<IActionResult> GetQuestion(long sid, long qid)
        {
            return NotFound();
        }

        [HttpPost("questions/{qid}/choices")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<IActionResult> CreateChoice(long qid, [FromBody] QuestionChoiceCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid: {@Errors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(ModelState);
            }

            try
            {
                var createdChoice = await _questionChoiceService.CreateChoiceAsync(qid, dto);
                return CreatedAtAction(nameof(GetChoice), new { qid, cid = createdChoice.Id }, createdChoice);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Question not found for ID: {Qid}", qid);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Conflict error for question ID: {Qid}", qid);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error for question ID: {Qid}", qid);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("questions/{qid}/choices/{cid}")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<IActionResult> GetChoice(long qid, long cid)
        {
            return NotFound();
        }
    }
}