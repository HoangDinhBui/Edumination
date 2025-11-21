using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Edumination.Api.Domain.Entities;
using Edumination.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Edumination.Api.Features.Answers
{
    [ApiController]
    [Route("api/v1/writing")]
    public class WritingController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _cfg;
        private readonly ILogger<WritingController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public WritingController(
            AppDbContext db,
            IConfiguration cfg,
            ILogger<WritingController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _cfg = cfg;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitWriting([FromBody] WritingSubmissionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ContentText))
            {
                return BadRequest(new { error = "Writing content cannot be empty." });
            }

            var apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY"); ;
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogError("Groq API Key is not configured.");
                return StatusCode(500, new { error = "Groq API Key is not configured." });
            }

            var wordsCount = CountWords(request.ContentText);

            try
            {
                var (score, feedback) = await GradeWritingWithGroq(apiKey, request.ContentText);

                var submission = new WritingSubmission
                {
                    SectionAttemptId = request.SectionAttemptId,
                    PromptText = request.PromptText,
                    ContentText = request.ContentText,
                    WordsCount = wordsCount,
                    CreatedAt = DateTime.UtcNow
                };

                _db.WritingSubmissions.Add(submission);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Writing submission saved. ID: {Id}, Score: {Score}", submission.Id, score);

                return Ok(new
                {
                    submissionId = submission.Id,
                    score,
                    feedback,
                    wordsCount,
                    createdAt = submission.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing writing submission");
                return StatusCode(500, new { error = "An error occurred.", details = ex.Message });
            }
        }

        private int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            return text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        private async Task<(int score, string feedback)> GradeWritingWithGroq(string apiKey, string contentText)
        {
            var prompt = BuildGradingPrompt(contentText);

            _logger.LogInformation("Sending request to Groq API...");

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new { role = "system", content = "You are a professional English writing evaluator." },
                    new { role = "user", content = prompt }
                },
                temperature = 0.7,
                max_tokens = 500
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var json = JsonSerializer.Serialize(requestBody, jsonOptions);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);

            var responseJson = await response.Content.ReadAsStringAsync();

            // LOG ĐỂ DEBUG
            _logger.LogInformation("Groq API Status: {StatusCode}", response.StatusCode);
            _logger.LogInformation("Groq API Response: {Response}", responseJson);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Groq API error: {StatusCode} - {Error}", response.StatusCode, responseJson);
                throw new Exception($"Groq API error: {response.StatusCode} - {responseJson}");
            }

            var groqResponse = JsonSerializer.Deserialize<GroqResponse>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var aiResponse = groqResponse?.Choices?.FirstOrDefault()?.Message?.Content?.Trim();

            if (string.IsNullOrWhiteSpace(aiResponse))
            {
                _logger.LogError("Empty AI response. Full response: {Response}", responseJson);
                throw new Exception($"Failed to get a response from Groq. Response: {responseJson}");
            }

            _logger.LogInformation("Groq AI Response: {Response}", aiResponse);

            return ParseAIResponse(aiResponse);
        }

        private string BuildGradingPrompt(string contentText)
        {
            return $@"You are a professional English writing evaluator. Grade the following writing based on:
1. Grammar and spelling (0-3 points)
2. Coherence and organization (0-3 points)
3. Vocabulary and word choice (0-2 points)
4. Overall quality (0-2 points)

Provide your response in this EXACT format:
Score: [number from 0-10]
Feedback: [Your detailed feedback here]

Writing to evaluate:
{contentText}";
        }

        private (int score, string feedback) ParseAIResponse(string aiResponse)
        {
            var scoreMatch = Regex.Match(aiResponse, @"Score:\s*(\d+)", RegexOptions.IgnoreCase);
            if (!scoreMatch.Success || !int.TryParse(scoreMatch.Groups[1].Value, out var score))
            {
                _logger.LogWarning("Failed to parse score. Response: {Response}", aiResponse);
                throw new Exception($"Failed to parse AI response. Response: {aiResponse}");
            }

            score = Math.Clamp(score, 0, 10);

            var feedbackMatch = Regex.Match(aiResponse, @"Feedback:\s*(.+)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var feedback = feedbackMatch.Success
                ? feedbackMatch.Groups[1].Value.Trim()
                : aiResponse.Replace(scoreMatch.Value, "").Trim();

            if (string.IsNullOrWhiteSpace(feedback))
            {
                feedback = "No detailed feedback provided.";
            }

            return (score, feedback);
        }

        public class WritingSubmissionRequest
        {
            public long SectionAttemptId { get; set; }
            public string PromptText { get; set; } = string.Empty;
            public string ContentText { get; set; } = string.Empty;
        }

        // Groq API Response Models
        private class GroqResponse
        {
            [JsonPropertyName("choices")]
            public List<GroqChoice>? Choices { get; set; }
        }

        private class GroqChoice
        {
            [JsonPropertyName("message")]
            public GroqMessage? Message { get; set; }
        }

        private class GroqMessage
        {
            [JsonPropertyName("content")]
            public string? Content { get; set; }
        }
    }
}