using Edumination.Api.Features.Speaking;
using Edumination.Api.Features.Speaking.Interfaces;
using Edumination.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Edumination.Api.Features.Speaking.Services
{
    public class GroqSpeakingGradingService : ISpeakingGradingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GroqSpeakingGradingService> _logger;
        private readonly AppDbContext _context;
        private readonly string _groqApiKey;
        private readonly string _whisperEndpoint;
        private readonly string _llmEndpoint;
        private readonly long _maxAudioSizeBytes;

        private static readonly string[] AllowedAudioFormats = 
        {
            "audio/mpeg", "audio/mp3", "audio/wav", 
            "audio/m4a", "audio/webm", "audio/ogg"
        };

        public GroqSpeakingGradingService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<GroqSpeakingGradingService> logger,
            AppDbContext context)
        {
            _httpClient = httpClient;
            _logger = logger;
            _context = context;
            
            _groqApiKey = configuration["Groq:ApiKey"] 
                ?? throw new InvalidOperationException("Groq API key not configured in appsettings.json");
            
            _whisperEndpoint = configuration["Groq:WhisperEndpoint"] 
                ?? "https://api.groq.com/openai/v1/audio/transcriptions";
            
            _llmEndpoint = configuration["Groq:LLMEndpoint"] 
                ?? "https://api.groq.com/openai/v1/chat/completions";
            
            _maxAudioSizeBytes = configuration.GetValue<long>("Audio:MaxSizeMB", 50) * 1024 * 1024;
            
            _logger.LogInformation("GroqSpeakingGradingService initialized with endpoints: Whisper={Whisper}, LLM={LLM}", 
                _whisperEndpoint, _llmEndpoint);
        }

        public async Task<GroqGradingResponse> GradeSubmissionAsync(
            Stream audioStream, 
            string promptText, 
            string? questionContext = null,
            CancellationToken cancellationToken = default)
        {
            var startTime = DateTime.UtcNow;
            _logger.LogInformation("Starting Groq grading process. Prompt: {Prompt}", promptText);

            try
            {
                // Step 1: Validate audio
                var validation = await ValidateAudioAsync(audioStream);
                if (!validation.IsValid)
                {
                    return new GroqGradingResponse
                    {
                        Success = false,
                        ErrorMessage = validation.ErrorMessage
                    };
                }

                // Step 2: Transcribe audio using Whisper
                var transcript = await TranscribeAudioAsync(audioStream, cancellationToken);
                
                if (string.IsNullOrWhiteSpace(transcript))
                {
                    return new GroqGradingResponse
                    {
                        Success = false,
                        ErrorMessage = "Failed to transcribe audio. The audio may be too short or unclear."
                    };
                }

                var wordCount = CountWords(transcript);
                _logger.LogInformation("Audio transcribed successfully. Length: {Length} characters, Words: {Words}", 
                    transcript.Length, wordCount);

                // Step 3: Grade transcript using LLM
                var gradingResult = await GradeTranscriptAsync(
                    transcript, 
                    promptText, 
                    questionContext, 
                    cancellationToken);

                // Step 4: Populate complete response
                gradingResult.Transcript = transcript;
                gradingResult.WordsCount = wordCount;
                gradingResult.Provider = "Groq";

                var duration = (DateTime.UtcNow - startTime).TotalSeconds;
                _logger.LogInformation(
                    "Grading completed in {Duration:F2}s. Success: {Success}, Score: {Score}", 
                    duration, gradingResult.Success, gradingResult.Scores?.Overall);

                return gradingResult;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Grading operation was cancelled");
                return new GroqGradingResponse
                {
                    Success = false,
                    ErrorMessage = "Grading operation was cancelled"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Groq grading process");
                return new GroqGradingResponse
                {
                    Success = false,
                    ErrorMessage = $"Grading error: {ex.Message}"
                };
            }
        }

        private async Task<string> TranscribeAudioAsync(
            Stream audioStream, 
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting audio transcription with Groq Whisper");

            using var content = new MultipartFormDataContent();
            
            // Reset stream position if seekable
            if (audioStream.CanSeek)
                audioStream.Position = 0;

            // Copy stream to memory to get accurate length
            var memoryStream = new MemoryStream();
            await audioStream.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;

            var streamContent = new StreamContent(memoryStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");
            
            content.Add(streamContent, "file", "audio.mp3");
            content.Add(new StringContent("whisper-large-v3"), "model");
            content.Add(new StringContent("en"), "language");
            content.Add(new StringContent("0.3"), "temperature"); // Lower temperature for more accurate transcription

            var request = new HttpRequestMessage(HttpMethod.Post, _whisperEndpoint)
            {
                Content = content
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _groqApiKey);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Whisper API error. Status: {Status}, Response: {Response}", 
                    response.StatusCode, errorContent);
                throw new HttpRequestException(
                    $"Whisper API returned {response.StatusCode}: {errorContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<JsonElement>(responseJson);
            
            var transcriptText = result.GetProperty("text").GetString() ?? string.Empty;
            return transcriptText.Trim();
        }

        private async Task<GroqGradingResponse> GradeTranscriptAsync(
            string transcript, 
            string promptText, 
            string? questionContext,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting transcript grading with Groq LLM");

            var systemPrompt = BuildGradingSystemPrompt();
            var userPrompt = BuildUserPrompt(transcript, promptText, questionContext);

            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                },
                temperature = 0.3, // Lower for more consistent scoring
                max_tokens = 2000,
                response_format = new { type = "json_object" }
            };

            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, _llmEndpoint)
            {
                Content = content
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _groqApiKey);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("LLM API error. Status: {Status}, Response: {Response}", 
                    response.StatusCode, errorContent);
                throw new HttpRequestException(
                    $"LLM API returned {response.StatusCode}: {errorContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<JsonElement>(responseJson);
            
            var assistantMessage = result
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return ParseGradingResult(assistantMessage);
        }

        private string BuildGradingSystemPrompt()
        {
            return @"You are an expert IELTS Speaking examiner with 10+ years of experience. Evaluate the given transcript based on official IELTS Speaking band descriptors.

Return a JSON response with this EXACT structure (no additional fields):
{
  ""scores"": {
    ""overall"": 7.0,
    ""fluency"": 7.5,
    ""grammar"": 7.0,
    ""vocabulary"": 7.0,
    ""pronunciation"": 6.5
  },
  ""feedback"": ""Overall evaluation summary (2-3 sentences)..."",
  ""strengths"": [""specific strength 1"", ""specific strength 2"", ""specific strength 3""],
  ""improvements"": [""specific improvement 1"", ""specific improvement 2"", ""specific improvement 3""]
}

SCORING CRITERIA (0.0 to 9.0, in 0.5 increments):
- Fluency & Coherence: speech rate, hesitations, self-correction, logical flow
- Lexical Resource: vocabulary range, precision, paraphrasing ability, collocations
- Grammatical Range & Accuracy: sentence structures, complexity, error frequency
- Pronunciation: individual sounds, word/sentence stress, intonation, intelligibility
- Overall: weighted average of the four criteria, rounded to nearest 0.5

BE SPECIFIC in feedback. Reference actual words/phrases from the transcript.
For improvements, suggest concrete strategies the speaker can practice.";
        }

        private string BuildUserPrompt(string transcript, string promptText, string? questionContext)
        {
            var context = string.IsNullOrWhiteSpace(questionContext) 
                ? "" 
                : $"\n\nAdditional Context: {questionContext}";

            return $@"Evaluate this IELTS Speaking response:

Question/Prompt: ""{promptText}""{context}

Candidate's Response Transcript:
""{transcript}""

Provide detailed scoring and feedback in the JSON format specified. Be fair but rigorous in assessment.";
        }

        private GroqGradingResponse ParseGradingResult(string? jsonResponse)
        {
            if (string.IsNullOrWhiteSpace(jsonResponse))
            {
                _logger.LogError("Empty JSON response from LLM");
                return new GroqGradingResponse
                {
                    Success = false,
                    ErrorMessage = "Empty response from AI grading service"
                };
            }

            try
            {
                var result = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
                var scores = result.GetProperty("scores");

                return new GroqGradingResponse
                {
                    Success = true,
                    Scores = new GroqScoresDto
                    {
                        Overall = scores.GetProperty("overall").GetDecimal(),
                        Fluency = scores.GetProperty("fluency").GetDecimal(),
                        Grammar = scores.GetProperty("grammar").GetDecimal(),
                        Vocabulary = scores.GetProperty("vocabulary").GetDecimal(),
                        Pronunciation = scores.GetProperty("pronunciation").GetDecimal()
                    },
                    FeedbackSummary = result.GetProperty("feedback").GetString(),
                    Strengths = JsonSerializer.Deserialize<string[]>(
                        result.GetProperty("strengths").GetRawText()) ?? Array.Empty<string>(),
                    Improvements = JsonSerializer.Deserialize<string[]>(
                        result.GetProperty("improvements").GetRawText()) ?? Array.Empty<string>(),
                    Model = "llama-3.3-70b-versatile"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse grading result. Response: {Response}", jsonResponse);
                return new GroqGradingResponse
                {
                    Success = false,
                    ErrorMessage = $"Failed to parse AI response: {ex.Message}"
                };
            }
        }

        public async Task<string?> TranscribeOnlyAsync(
            Stream audioStream, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await TranscribeAudioAsync(audioStream, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during transcription-only operation");
                return null;
            }
        }

        public async Task<(bool IsValid, string? ErrorMessage)> ValidateAudioAsync(Stream audioStream)
        {
            if (audioStream == null || !audioStream.CanRead)
            {
                return (false, "Invalid audio stream");
            }

            if (audioStream.CanSeek && audioStream.Length == 0)
            {
                return (false, "Audio file is empty");
            }

            if (audioStream.CanSeek && audioStream.Length > _maxAudioSizeBytes)
            {
                var maxSizeMB = _maxAudioSizeBytes / 1024 / 1024;
                return (false, $"Audio file exceeds maximum size of {maxSizeMB}MB");
            }

            return await Task.FromResult((true, (string?)null));
        }

        public async Task<GroqGradingResponse> RetryGradingAsync(
            long submissionId, 
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrying grading for submission {SubmissionId}", submissionId);

            // Get existing submission with audio
            var submission = await _context.SpeakingSubmissions
                .Include(s => s.AudioAsset)
                .FirstOrDefaultAsync(s => s.Id == submissionId, cancellationToken);

            if (submission == null)
            {
                return new GroqGradingResponse
                {
                    Success = false,
                    ErrorMessage = "Submission not found"
                };
            }

            // Re-download audio from storage and re-grade
            // This requires implementation of IAssetService.GetStreamAsync()
            throw new NotImplementedException(
                "Retry grading requires implementation of audio stream retrieval from storage");
        }

        private static int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            return text.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', '!', '?' }, 
                StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}