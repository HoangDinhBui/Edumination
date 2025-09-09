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
        private readonly IQuestionAnswerKeyService _questionAnswerKeyService;

        public QuestionsController(
            IQuestionService questionService,
            IQuestionChoiceService questionChoiceService, // Thêm dependency
            IQuestionAnswerKeyService questionAnswerKeyService,
            ILogger<QuestionsController> logger)
        {
            _questionService = questionService;
            _questionChoiceService = questionChoiceService;
            _questionAnswerKeyService = questionAnswerKeyService;
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
            // Triển khai logic lấy question (placeholder)
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
            // Triển khai logic lấy choice (placeholder)
            return NotFound();
        }

        [HttpPost("questions/{qid}/answer-key")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<IActionResult> CreateAnswerKey(long qid, [FromBody] QuestionAnswerKeyCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState không hợp lệ: {@Errors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(ModelState);
            }

            try
            {
                var createdAnswerKey = await _questionAnswerKeyService.CreateAnswerKeyAsync(qid, dto);
                return CreatedAtAction(nameof(GetAnswerKey), new { qid }, createdAnswerKey);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Không tìm thấy câu hỏi hoặc khóa đáp án cho ID: {Qid}", qid);
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Lỗi xung đột cho ID câu hỏi: {Qid}", qid);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi máy chủ nội bộ cho ID câu hỏi: {Qid}", qid);
                return StatusCode(500, "Lỗi máy chủ nội bộ");
            }
        }

        [HttpPatch("questions/{qid}/answer-key")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<IActionResult> UpdateAnswerKey(long qid, [FromBody] QuestionAnswerKeyUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState không hợp lệ: {@Errors}", ModelState.Values.SelectMany(v => v.Errors));
                return BadRequest(ModelState);
            }

            try
            {
                var updatedAnswerKey = await _questionAnswerKeyService.UpdateAnswerKeyAsync(qid, dto);
                return Ok(updatedAnswerKey);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Không tìm thấy khóa đáp án cho ID: {Qid}", qid);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi máy chủ nội bộ cho ID câu hỏi: {Qid}", qid);
                return StatusCode(500, "Lỗi máy chủ nội bộ");
            }
        }

        [HttpDelete("questions/{qid}/answer-key")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<IActionResult> DeleteAnswerKey(long qid)
        {
            try
            {
                await _questionAnswerKeyService.DeleteAnswerKeyAsync(qid);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Không tìm thấy khóa đáp án cho ID: {Qid}", qid);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi máy chủ nội bộ cho ID câu hỏi: {Qid}", qid);
                return StatusCode(500, "Lỗi máy chủ nội bộ");
            }
        }

        // Thêm endpoint GET nếu cần (tùy chọn)
        [HttpGet("questions/{qid}/answer-key")]
        [Authorize(Roles = "TEACHER,ADMIN")]
        public async Task<IActionResult> GetAnswerKey(long qid)
        {
            try
            {
                var answerKey = await _questionAnswerKeyService.GetAnswerKeyAsync(qid); // Triển khai trong service
                return Ok(answerKey);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Không tìm thấy khóa đáp án cho ID: {Qid}", qid);
                return NotFound(ex.Message);
            }
        }
    }
}