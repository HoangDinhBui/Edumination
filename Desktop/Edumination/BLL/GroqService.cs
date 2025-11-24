using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using DotNetEnv;

namespace IELTS.BLL
{
    public class GroqService
    {
        private readonly string _apiKey;
        private readonly string _apiUrl = "https://api.groq.com/openai/v1/chat/completions";
        private readonly HttpClient _httpClient;

        public GroqService()
        {
            // Load .env
            string envPath = null;
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
            while (currentDir != null)
            {
                string path = System.IO.Path.Combine(currentDir, ".env");
                if (System.IO.File.Exists(path))
                {
                    envPath = path;
                    break;
                }
                var parent = System.IO.Directory.GetParent(currentDir);
                if (parent == null) break;
                currentDir = parent.FullName;
            }

            if (!string.IsNullOrEmpty(envPath))
            {
                Env.Load(envPath);
            }

            _apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY");
            _httpClient = new HttpClient();
        }

        public async Task<WritingGradeResult> GradeWritingAsync(string taskPrompt, string userEssay)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("GROQ_API_KEY not found in .env file.");
            }

            if (string.IsNullOrWhiteSpace(userEssay))
            {
                 return new WritingGradeResult 
                 { 
                     BandScore = 0, 
                     Feedback = "No essay submitted.", 
                     Correction = "" 
                 };
            }

            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new { role = "system", content = "You are an expert IELTS Writing examiner. Grade the following essay based on IELTS criteria (Task Achievement/Response, Coherence & Cohesion, Lexical Resource, Grammatical Range & Accuracy). Provide the output in JSON format with keys: 'band_score' (number), 'feedback' (string), 'correction' (string - rewritten version or specific corrections)." },
                    new { role = "user", content = $"Task Prompt: {taskPrompt}\n\nUser Essay:\n{userEssay}" }
                },
                temperature = 0.3,
                response_format = new { type = "json_object" }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            try
            {
                var response = await _httpClient.PostAsync(_apiUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Groq API Error: {response.StatusCode} - {responseString}");
                }

                dynamic result = JsonConvert.DeserializeObject(responseString);
                string contentStr = result.choices[0].message.content;

                return JsonConvert.DeserializeObject<WritingGradeResult>(contentStr);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to grade essay: {ex.Message}");
            }
        }
    }

    public class WritingGradeResult
    {
        [JsonProperty("band_score")]
        public double BandScore { get; set; }

        [JsonProperty("feedback")]
        public string Feedback { get; set; }

        [JsonProperty("correction")]
        public string Correction { get; set; }
    }
}
