using System.Text.Json;
using Edumination.Api.Features.Attempts.Dtos;
using Microsoft.Extensions.Options;

namespace Edumination.Api.Infrastructure.Services;

public class GroqApiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.groq.com/v1";
    public string WhisperModel { get; set; } = "whisper-large-v3-turbo";
    public string GradingModel { get; set; } = "llama-3.3-70b-versatile";
    public int MaxTokens { get; set; } = 2000;
    public decimal Temperature { get; set; } = 0.3m;
}

public class GroqSpeakingGradingService : Edumination.Api.Features.Attempts.Services.ISpeakingGradingService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly GroqApiSettings _settings;
    private readonly ILogger<GroqSpeakingGradingService> _logger;

    public GroqSpeakingGradingService(IHttpClientFactory httpFactory, IOptions<GroqApiSettings> opts, ILogger<GroqSpeakingGradingService> logger)
    {
        _httpFactory = httpFactory;
        _settings = opts.Value;
        _logger = logger;
    }

    public async Task<string> TranscribeAudioAsync(string audioUrl, CancellationToken ct = default)
    {
        // Download the audio bytes from the provided URL, then POST as multipart/form-data
        var client = _httpFactory.CreateClient();
        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.ApiKey}");

        // 1) Download audio file
        byte[] audioBytes;
        try
        {
            using var downloadClient = _httpFactory.CreateClient();
            using var dlResp = await downloadClient.GetAsync(audioUrl, ct);
            dlResp.EnsureSuccessStatusCode();
            audioBytes = await dlResp.Content.ReadAsByteArrayAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to download audio from URL: {Url}", audioUrl);
            throw;
        }

        // 2) Build multipart/form-data body
        using var formData = new MultipartFormDataContent();
        using var audioContent = new ByteArrayContent(audioBytes);
        audioContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/webm");
        formData.Add(audioContent, "file", "audio.webm");
        formData.Add(new StringContent(_settings.WhisperModel ?? string.Empty), "model");

        // 3) Post to Groq Whisper transcription endpoint
        var resp = await client.PostAsync(new Uri(new Uri(_settings.BaseUrl), "audio/transcriptions"), formData, ct);
        var respJson = await resp.Content.ReadAsStringAsync(ct);
        _logger.LogInformation("Groq whisper response status: {Status}; body: {Body}", resp.StatusCode, respJson);

        if (!resp.IsSuccessStatusCode)
            throw new Exception($"Groq whisper error: {resp.StatusCode} - {respJson}");

        using var doc = JsonDocument.Parse(respJson);
        if (doc.RootElement.TryGetProperty("text", out var textEl))
            return textEl.GetString() ?? string.Empty;

        // fallback: try choices[0].text
        if (doc.RootElement.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
        {
            var first = choices[0];
            if (first.TryGetProperty("text", out var t)) return t.GetString() ?? string.Empty;
        }

        return string.Empty;
    }

    public async Task<SpeakingGradingResult> GradeSpeakingAsync(string transcript, CancellationToken ct = default)
    {
        var client = _httpFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.ApiKey}");

        var prompt = BuildGradingPrompt(transcript);

        var requestBody = new
        {
            model = _settings.GradingModel,
            messages = new[] {
                new { role = "system", content = "You are a professional IELTS speaking examiner. Grade the candidate based on the four IELTS speaking criteria and return a JSON with keys: fluency, lexical, grammar, pronunciation, overall, feedback." },
                new { role = "user", content = prompt }
            },
            temperature = (double)_settings.Temperature,
            max_tokens = _settings.MaxTokens
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var resp = await client.PostAsync(new Uri(new Uri(_settings.BaseUrl), "chat/completions"), content, ct);
        var respJson = await resp.Content.ReadAsStringAsync(ct);
        _logger.LogInformation("Groq grading response status: {Status}; body: {Body}", resp.StatusCode, respJson);

        if (!resp.IsSuccessStatusCode)
            throw new Exception($"Groq grading error: {resp.StatusCode} - {respJson}");

        // Parse response assuming assistant returns JSON in content
        using var doc = JsonDocument.Parse(respJson);
        var aiText = string.Empty;
        try
        {
            if (doc.RootElement.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
            {
                var msg = choices[0].GetProperty("message").GetProperty("content").GetString();
                aiText = msg ?? string.Empty;
            }
        }
        catch { aiText = string.Empty; }

        // Try to extract JSON object from aiText
        var jsonStart = aiText.IndexOf('{');
        var jsonObj = jsonStart >= 0 ? aiText.Substring(jsonStart) : aiText;

        try
        {
            var parsed = JsonDocument.Parse(jsonObj).RootElement;
            decimal fluency = parsed.GetProperty("fluency").GetDecimal();
            decimal lexical = parsed.GetProperty("lexical").GetDecimal();
            decimal grammar = parsed.GetProperty("grammar").GetDecimal();
            decimal pronunciation = parsed.GetProperty("pronunciation").GetDecimal();
            decimal overall = parsed.GetProperty("overall").GetDecimal();
            string feedback = parsed.TryGetProperty("feedback", out var f) ? f.GetString() ?? string.Empty : aiText;

            return new SpeakingGradingResult
            {
                TranscribedText = transcript,
                WordCount = transcript.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length,
                FluencyScore = fluency,
                LexicalScore = lexical,
                GrammarScore = grammar,
                PronunciationScore = pronunciation,
                OverallBand = overall,
                Feedback = feedback
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to parse grading JSON, returning fallback.");
            // fallback: return overall 0 and include aiText as feedback
            return new SpeakingGradingResult
            {
                TranscribedText = transcript,
                WordCount = transcript.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length,
                FluencyScore = 0,
                LexicalScore = 0,
                GrammarScore = 0,
                PronunciationScore = 0,
                OverallBand = 0,
                Feedback = aiText
            };
        }
    }

    public async Task<SpeakingGradingResult> ProcessSpeakingSubmissionAsync(string audioUrl, CancellationToken ct = default)
    {
        var transcript = await TranscribeAudioAsync(audioUrl, ct);
        var result = await GradeSpeakingAsync(transcript, ct);
        return result;
    }

    private string BuildGradingPrompt(string transcript)
    {
                return $@"Candidate transcript:
{transcript}

Please evaluate the candidate on each IELTS Speaking criterion on a scale 0-9 (decimal allowed, one decimal place), and return a JSON object exactly like:
{{
    ""fluency"": 0.0,
    ""lexical"": 0.0,
    ""grammar"": 0.0,
    ""pronunciation"": 0.0,
    ""overall"": 0.0,
    ""feedback"": ""detailed feedback here""
}}

Explain your scoring briefly in the feedback.";
        }
}
