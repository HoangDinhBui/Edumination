using IELTS.DAL;
using IELTS.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms; // Thêm để hiển thị MessageBox thông báo lỗi

namespace IELTS.BLL
{
    public class AIQuestionAnalysisService
    {
        private readonly TestSectionBLL _testSectionBLL;
        // Sử dụng static HttpClient để tối ưu hiệu năng và tránh cạn kiệt socket
        private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };

        public AIQuestionAnalysisService()
        {
            _testSectionBLL = new TestSectionBLL();
        }

        /// <summary>
        /// Phân tích PDF bằng AI và lưu câu hỏi vào database EDUMINATION
        /// </summary>
        public async Task<bool> AnalyzePdfAndSaveQuestions(string pdfPath, long testPaperId)
        {
            try
            {
                if (!File.Exists(pdfPath))
                {
                    MessageBox.Show("Lỗi: Không tìm thấy file PDF tại: " + pdfPath);
                    return false;
                }

                // 1. Đọc PDF và chuyển sang Base64 trên luồng phụ để tránh treo UI
                byte[] pdfBytes = await Task.Run(() => File.ReadAllBytes(pdfPath));
                string base64Pdf = await Task.Run(() => Convert.ToBase64String(pdfBytes));

                // 2. Gọi Gemini API (Sử dụng model 1.5 Flash miễn phí)
                var analysisResult = await CallGeminiAPI(base64Pdf);

                if (analysisResult == null || analysisResult.Questions == null || !analysisResult.Questions.Any())
                {
                    return false; // Thông báo lỗi chi tiết đã có trong CallGeminiAPI
                }

                // 3. Tạo TestSection cho READING
                var section = new TestSectionDTO
                {
                    PaperId = testPaperId,
                    Skill = "READING",
                    TimeLimitMinutes = 60,
                    PdfFileName = Path.GetFileName(pdfPath),
                    PdfFilePath = pdfPath
                };

                long sectionId = _testSectionBLL.CreateTestSection(section);
                if (sectionId <= 0)
                {
                    MessageBox.Show("Lỗi: Không thể tạo TestSection trong Database.");
                    return false;
                }

                // 4. Chuẩn bị dữ liệu câu hỏi để lưu vào bảng Questions và QuestionAnswerKeys
                var questionTypes = new Dictionary<int, string>();
                var answers = new Dictionary<int, string>();
                var questionRanges = new Dictionary<int, int>();

                foreach (var q in analysisResult.Questions)
                {
                    questionTypes[q.Position] = q.QuestionType;
                    answers[q.Position] = q.Answer;

                    if (q.EndPosition.HasValue && q.EndPosition > q.Position)
                    {
                        questionRanges[q.Position] = q.EndPosition.Value;
                    }
                }

                // 5. Thực hiện lưu hàng loạt vào database qua BLL
                return _testSectionBLL.SaveQuestionsToSection(
                    sectionId,
                    questionTypes,
                    answers,
                    questionRanges
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống khi phân tích: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gọi Gemini API để xử lý file PDF
        /// </summary>
        private async Task<AIAnalysisResult> CallGeminiAPI(string base64Pdf)
        {
            try
            {
                // API Key lấy từ tài khoản của bạn trên Google AI Studio
                string geminiApiKey = "AIzaSyCqLOXrfEFRz84dEoMqlQA488wnwq6Bi7E";

                // URL đã sửa lỗi định dạng (không chứa ký tự Markdown)
                string url = $"https://generativelanguage.googleapis.com/v1/models/gemini-1.5-flash:generateContent?key={geminiApiKey}";

                var requestBody = new
                {
                    contents = new[] {
                        new {
                            parts = new object[] {
                                new { text = "Analyze this IELTS Reading PDF. Extract all questions. Return a JSON array of objects. Each object must have: 'position' (int), 'questionType' (string), 'answer' (string), 'endPosition' (int or null). Use types: MCQ, FILL_BLANK, MATCHING. Return ONLY raw JSON." },
                                new { inline_data = new { mime_type = "application/pdf", data = base64Pdf } }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        responseMimeType = "application/json",
                        temperature = 0.1 // Giảm độ sáng tạo để AI trích xuất chính xác hơn
                    }
                };

                string jsonContent = JsonConvert.SerializeObject(requestBody);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Thực hiện gửi yêu cầu
                var response = await _httpClient.PostAsync(url, httpContent);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Lỗi API ({response.StatusCode}): {responseJson}");
                    return null;
                }

                dynamic result = JsonConvert.DeserializeObject(responseJson);

                // Kiểm tra xem AI có trả về nội dung không
                if (result?.candidates == null || result.candidates.Count == 0)
                {
                    MessageBox.Show("AI không tìm thấy nội dung câu hỏi trong file PDF này.");
                    return null;
                }

                string aiResponse = result.candidates[0].content.parts[0].text.ToString();

                // Làm sạch chuỗi JSON (loại bỏ markdown nếu có)
                string cleanedJson = ExtractJsonContent(aiResponse);

                var questions = JsonConvert.DeserializeObject<List<AIQuestionDTO>>(cleanedJson);

                return new AIAnalysisResult { Questions = questions };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối API: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Bóc tách khối JSON chính xác từ phản hồi của AI
        /// </summary>
        private string ExtractJsonContent(string input)
        {
            if (string.IsNullOrEmpty(input)) return "[]";

            int start = input.IndexOf('[');
            int end = input.LastIndexOf(']');

            if (start != -1 && end != -1 && end > start)
            {
                return input.Substring(start, end - start + 1);
            }

            return input.Replace("```json", "").Replace("```", "").Trim();
        }
    }

    // --- DTO Classes ---
    public class AIAnalysisResult
    {
        public List<AIQuestionDTO> Questions { get; set; }
    }

    public class AIQuestionDTO
    {
        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("questionType")]
        public string QuestionType { get; set; }

        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("endPosition")]
        public int? EndPosition { get; set; }
    }
}