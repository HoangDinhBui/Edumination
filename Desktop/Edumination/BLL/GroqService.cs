using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using DotNetEnv;
using System.IO;

namespace IELTS.BLL
{
    public class GroqService
    {
        private readonly string _apiKey;
        private readonly string _chatApiUrl = "https://api.groq.com/openai/v1/chat/completions";
        private readonly string _whisperApiUrl = "https://api.groq.com/openai/v1/audio/transcriptions";
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
            _httpClient.Timeout = TimeSpan.FromMinutes(5); // Tăng timeout cho audio
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
                var response = await _httpClient.PostAsync(_chatApiUrl, content);
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

        public async Task<SpeakingGradeResult> GradeSpeakingAsync(string taskPrompt, string audioFilePath)
        {
            if (string.IsNullOrEmpty(_apiKey))
                throw new Exception("GROQ_API_KEY not found in .env file.");

            if (string.IsNullOrWhiteSpace(audioFilePath) || !System.IO.File.Exists(audioFilePath))
            {
                return new SpeakingGradeResult
                {
                    BandScore = 0,
                    Feedback = "No audio submitted.",
                    Transcript = ""
                };
            }

            try
            {
                // BƯỚC 1: Transcribe audio bằng Whisper API
                string transcript = await TranscribeAudioAsync(audioFilePath);

                if (string.IsNullOrWhiteSpace(transcript))
                {
                    return new SpeakingGradeResult
                    {
                        BandScore = 0,
                        Feedback = "Could not transcribe audio. Please check audio quality.",
                        Transcript = ""
                    };
                }

                // Log transcript để debug
                System.Diagnostics.Debug.WriteLine($"Transcript: {transcript}");

                // BƯỚC 2: Chấm điểm dựa trên transcript
                var gradeResult = await GradeSpeakingFromTranscriptAsync(taskPrompt, transcript);
                
                // Đảm bảo transcript được gán
                if (gradeResult != null)
                {
                    gradeResult.Transcript = transcript;
                }
                else
                {
                    // Fallback nếu gradeResult null
                    gradeResult = new SpeakingGradeResult
                    {
                        BandScore = 5.0,
                        Feedback = "Could not grade the response properly.",
                        Transcript = transcript
                    };
                }

                return gradeResult;
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi
                System.Diagnostics.Debug.WriteLine($"Error in GradeSpeakingAsync: {ex}");
                throw new Exception($"Failed to grade speaking: {ex.Message}\n\nInner: {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// Transcribe audio file sang text bằng Whisper API
        /// </summary>
        private async Task<string> TranscribeAudioAsync(string audioFilePath)
        {
            try
            {
                // Đọc file audio
                byte[] audioBytes = System.IO.File.ReadAllBytes(audioFilePath);

                // Tạo multipart form data
                using (var form = new MultipartFormDataContent())
                {
                    // Add file audio
                    var fileContent = new ByteArrayContent(audioBytes);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/wav");
                    form.Add(fileContent, "file", Path.GetFileName(audioFilePath));

                    // Add model
                    form.Add(new StringContent("whisper-large-v3"), "model");

                    // Add language (optional, để auto-detect)
                    form.Add(new StringContent("en"), "language");

                    // Add response format
                    form.Add(new StringContent("json"), "response_format");

                    // Set headers
                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                    // Gọi API
                    var response = await _httpClient.PostAsync(_whisperApiUrl, form);
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Whisper API Error: {response.StatusCode} - {responseString}");
                    }

                    // Parse response
                    dynamic result = JsonConvert.DeserializeObject(responseString);
                    return result.text?.ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to transcribe audio: {ex.Message}");
            }
        }

        /// <summary>
        /// Chấm điểm Speaking dựa trên transcript
        /// </summary>
        private async Task<SpeakingGradeResult> GradeSpeakingFromTranscriptAsync(string taskPrompt, string transcript)
        {
            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new { 
                        role = "system", 
                        content = @"You are an expert IELTS Speaking examiner. Grade the following speaking response based on IELTS criteria:
- Fluency and Coherence
- Lexical Resource
- Grammatical Range and Accuracy
- Pronunciation (inferred from transcript quality)

You MUST respond with ONLY valid JSON, no markdown, no code blocks, no extra text.
Use this exact structure:
{
  ""band_score"": 6.5,
  ""feedback"": ""Your detailed feedback here"",
  ""transcript"": """"
}"
                    },
                    new { 
                        role = "user", 
                        content = $"Task/Question: {taskPrompt}\n\nCandidate's Response (Transcript):\n{transcript}"
                    }
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
                var response = await _httpClient.PostAsync(_chatApiUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Groq API Error: {response.StatusCode} - {responseString}");
                }

                dynamic result = JsonConvert.DeserializeObject(responseString);
                string contentStr = result.choices[0].message.content?.ToString() ?? "";

                // Làm sạch JSON response (loại bỏ markdown code blocks nếu có)
                contentStr = contentStr.Trim();
                if (contentStr.StartsWith("```json"))
                {
                    contentStr = contentStr.Substring(7); // Bỏ ```json
                }
                if (contentStr.StartsWith("```"))
                {
                    contentStr = contentStr.Substring(3); // Bỏ ```
                }
                if (contentStr.EndsWith("```"))
                {
                    contentStr = contentStr.Substring(0, contentStr.Length - 3); // Bỏ ```
                }
                contentStr = contentStr.Trim();

                // Parse JSON với error handling tốt hơn
                SpeakingGradeResult gradeResult;
                try
                {
                    gradeResult = JsonConvert.DeserializeObject<SpeakingGradeResult>(contentStr);
                }
                catch (JsonException jsonEx)
                {
                    // Nếu JSON parse fail, trả về kết quả mặc định với thông tin debug
                    return new SpeakingGradeResult
                    {
                        BandScore = 5.0,
                        Feedback = $"Unable to parse AI response properly. Raw response:\n\n{contentStr}\n\nError: {jsonEx.Message}",
                        Transcript = ""
                    };
                }

                return gradeResult;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to grade speaking from transcript: {ex.Message}");
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

    public class SpeakingGradeResult
    {
        [JsonProperty("band_score")]
        public double BandScore { get; set; }

        [JsonProperty("feedback")]
        public string Feedback { get; set; }

        [JsonProperty("transcript")]
        public string Transcript { get; set; }
    }
}