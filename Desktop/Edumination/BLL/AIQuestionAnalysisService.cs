using IELTS.DAL;
using IELTS.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.BLL
{
	public class AIQuestionAnalysisService
	{
		private readonly TestSectionBLL _testSectionBLL;
		private static readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(120) };

		// Rate limiting
		private static readonly SemaphoreSlim _rateLimiter = new SemaphoreSlim(1, 1);
		private static DateTime _lastRequestTime = DateTime.MinValue;
		private const int MIN_REQUEST_INTERVAL_MS = 4000; // 4 giây giữa các request (15 requests/phút)

		// Giới hạn kích thước file
		private const long MAX_FILE_SIZE_BYTES = 15 * 1024 * 1024; // 15MB (để an toàn với giới hạn 20MB)

		public AIQuestionAnalysisService()
		{
			_testSectionBLL = new TestSectionBLL();
		}

		/// <summary>
		/// Phân tích PDF bằng AI và lưu câu hỏi vào database
		/// </summary>
		public async Task<bool> AnalyzePdfAndSaveQuestions(string pdfPath, long testPaperId)
		{
			try
			{
				// 1. Kiểm tra file
				if (!File.Exists(pdfPath))
				{
					MessageBox.Show($"❌ Lỗi: Không tìm thấy file PDF tại:\n{pdfPath}",
						"Lỗi File", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				FileInfo fileInfo = new FileInfo(pdfPath);
				if (fileInfo.Length > MAX_FILE_SIZE_BYTES)
				{
					MessageBox.Show($"❌ File PDF quá lớn ({fileInfo.Length / 1024 / 1024:F2} MB).\n" +
						$"Giới hạn: {MAX_FILE_SIZE_BYTES / 1024 / 1024} MB.\n\n" +
						"Vui lòng nén hoặc chia nhỏ file PDF.",
						"File Quá Lớn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return false;
				}

				// 2. Đọc và chuyển đổi PDF
				byte[] pdfBytes = await Task.Run(() => File.ReadAllBytes(pdfPath));
				string base64Pdf = Convert.ToBase64String(pdfBytes);

				// 3. Gọi Gemini API với rate limiting
				var analysisResult = await CallGeminiAPIWithRetry(base64Pdf, Path.GetFileName(pdfPath));

				if (analysisResult == null || analysisResult.Questions == null || !analysisResult.Questions.Any())
				{
					MessageBox.Show("⚠️ AI không tìm thấy câu hỏi IELTS hợp lệ trong file PDF.\n\n" +
						"Nguyên nhân có thể:\n" +
						"• File không chứa đề thi IELTS Reading\n" +
						"• Định dạng PDF không chuẩn\n" +
						"• Ảnh scan chất lượng kém\n\n" +
						"Bạn có thể thêm câu hỏi thủ công sau.",
						"Không Tìm Thấy Câu Hỏi", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return false;
				}

				// 4. Tạo TestSection cho READING
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
					MessageBox.Show("❌ Lỗi: Không thể tạo TestSection trong Database.",
						"Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				// 5. Chuẩn bị dữ liệu câu hỏi
				var questionTypes = new Dictionary<int, string>();
				var answers = new Dictionary<int, string>();
				var questionRanges = new Dictionary<int, int>();

				foreach (var q in analysisResult.Questions)
				{
					questionTypes[q.Position] = q.QuestionType;
					answers[q.Position] = q.Answer ?? "";

					if (q.EndPosition.HasValue && q.EndPosition > q.Position)
					{
						questionRanges[q.Position] = q.EndPosition.Value;
					}
				}

				// 6. Lưu vào database
				bool saveSuccess = _testSectionBLL.SaveQuestionsToSection(
					sectionId,
					questionTypes,
					answers,
					questionRanges
				);

				if (saveSuccess)
				{
					MessageBox.Show($"✅ Thành công!\n\n" +
						$"📊 Tổng số câu hỏi: {analysisResult.Questions.Count}\n" +
						$"📝 Đã lưu vào Section ID: {sectionId}",
						"Phân Tích Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

				return saveSuccess;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"❌ Lỗi hệ thống:\n{ex.Message}\n\n{ex.StackTrace}",
					"Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		/// <summary>
		/// Gọi Gemini API với retry mechanism và rate limiting
		/// </summary>
		private async Task<AIAnalysisResult> CallGeminiAPIWithRetry(string base64Pdf, string fileName, int maxRetries = 3)
		{
			int attempt = 0;
			Exception lastException = null;

			while (attempt < maxRetries)
			{
				try
				{
					attempt++;

					// Rate limiting
					await _rateLimiter.WaitAsync();
					try
					{
						var timeSinceLastRequest = DateTime.Now - _lastRequestTime;
						if (timeSinceLastRequest.TotalMilliseconds < MIN_REQUEST_INTERVAL_MS)
						{
							int waitTime = MIN_REQUEST_INTERVAL_MS - (int)timeSinceLastRequest.TotalMilliseconds;
							await Task.Delay(waitTime);
						}
						_lastRequestTime = DateTime.Now;
					}
					finally
					{
						_rateLimiter.Release();
					}

					return await CallGeminiAPI(base64Pdf, fileName);
				}
				catch (HttpRequestException ex) when (ex.Message.Contains("429"))
				{
					lastException = ex;
					int waitSeconds = (int)Math.Pow(2, attempt) * 10; // Exponential backoff: 10s, 20s, 40s
					MessageBox.Show($"⚠️ Đã đạt giới hạn API (lần {attempt}/{maxRetries}).\n" +
						$"Đang chờ {waitSeconds} giây...",
						"Rate Limit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					await Task.Delay(waitSeconds * 1000);
				}
				catch (Exception ex)
				{
					lastException = ex;
					if (attempt >= maxRetries) break;

					await Task.Delay(2000 * attempt); // 2s, 4s, 6s
				}
			}

			MessageBox.Show($"❌ Không thể kết nối API sau {maxRetries} lần thử.\n\n" +
				$"Lỗi: {lastException?.Message}",
				"Lỗi API", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return null;
		}

		/// <summary>
		/// Gọi Gemini API (Core method)
		private async Task<AIAnalysisResult> CallGeminiAPI(string base64Pdf, string fileName)
		{
			string geminiApiKey = ConfigurationManager.AppSettings["GeminiApiKey"];

			if (string.IsNullOrEmpty(geminiApiKey))
			{
				throw new Exception("Thiếu GeminiApiKey trong App.config!");
			}

			// ✅ Danh sách models theo thứ tự ưu tiên (từ danh sách của bạn)
			var modelsToTry = new[]
			{
		"gemini-2.5-flash",              // Mới nhất, nhanh nhất
        "gemini-2.0-flash",              // Ổn định
        "gemini-flash-latest",           // Luôn cập nhật
        "gemini-2.5-pro",                // Chính xác cao hơn
        "gemini-pro-latest",             // Fallback
        "gemini-2.0-flash-lite"          // Nhanh cho file nhỏ
    };

			Exception lastException = null;

			foreach (var modelName in modelsToTry)
			{
				try
				{
					string url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelName}:generateContent?key={geminiApiKey}";

					string prompt = @"You are a specialized Data Extraction Tool for IELTS documents. 

### YOUR MISSION:
Your primary goal is to find the 'ANSWER KEY' or 'CORRECT ANSWERS' section within the provided PDF and extract the answers for questions 1 to 40.

### INSTRUCTIONS:
1. SCANNED DATA FIRST: Search the entire document for a table or list containing the correct answers. 
2. NO REASONING REQUIRED: Do not try to solve the questions by reading the passages. Only extract the answers that are explicitly written in the answer key section of the PDF.
3. DATA MAPPING: Map each answer to its corresponding question number (position 1-40).

### DATA STRUCTURE:
- position: (int) 1-40.
- questionType: (string) Identify the type based on the question section (MCQ, FILL_BLANK, TRUE_FALSE_NOT_GIVEN, etc.).
- answer: (string) The exact value found in the answer key.
- endPosition: (int|null) For grouped questions.

### IF NO ANSWER KEY IS FOUND:
If you absolutely cannot find an answer key section in the PDF, return an empty JSON array [] so the system can notify the user.

### OUTPUT FORMAT:
Return ONLY a valid JSON array. No text, no markdown.
[
  { ""position"": 1, ""questionType"": ""MCQ"", ""answer"": ""A"", ""endPosition"": null }
]";

					var requestBody = new
					{
						contents = new[] {
					new {
						parts = new object[] {
							new { text = prompt },
							new {
								inline_data = new {
									mime_type = "application/pdf",
									data = base64Pdf
								}
							}
						}
					}
				},
						generationConfig = new
						{
							temperature = 0.1,      // Giảm tính ngẫu nhiên
							topK = 40,
							topP = 0.95,
							maxOutputTokens = 8192,
							responseMimeType = "application/json"  // Bắt buộc trả về JSON
						},
						safetySettings = new[]
						{
					new { category = "HARM_CATEGORY_HARASSMENT", threshold = "BLOCK_NONE" },
					new { category = "HARM_CATEGORY_HATE_SPEECH", threshold = "BLOCK_NONE" },
					new { category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_NONE" },
					new { category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_NONE" }
				}
					};

					string jsonContent = JsonConvert.SerializeObject(requestBody);
					var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

					System.Diagnostics.Debug.WriteLine($"🔄 Trying model: {modelName}");

					var response = await _httpClient.PostAsync(url, httpContent);
					var responseJson = await response.Content.ReadAsStringAsync();

					System.Diagnostics.Debug.WriteLine($"📊 Response ({response.StatusCode}): {responseJson.Substring(0, Math.Min(300, responseJson.Length))}...");

					if (!response.IsSuccessStatusCode)
					{
						// Kiểm tra lỗi cụ thể
						dynamic errorObj = JsonConvert.DeserializeObject(responseJson);
						string errorMsg = errorObj?.error?.message?.ToString() ?? "Unknown error";
						int errorCode = errorObj?.error?.code ?? 0;

						// Rate limit - đợi và thử model tiếp theo
						if (errorCode == 429)
						{
							System.Diagnostics.Debug.WriteLine($"⚠️ Rate limit hit for {modelName}, trying next model...");
							lastException = new Exception($"Rate limit: {modelName}");
							await Task.Delay(2000); // Đợi 2 giây
							continue;
						}

						// Model không tồn tại - thử model tiếp theo
						if (errorCode == 404)
						{
							System.Diagnostics.Debug.WriteLine($"⚠️ Model {modelName} not found, trying next...");
							lastException = new Exception($"Not found: {modelName}");
							continue;
						}

						throw new HttpRequestException($"API Error ({response.StatusCode}): {errorMsg}");
					}

					dynamic result = JsonConvert.DeserializeObject(responseJson);

					// Kiểm tra response structure
					if (result?.candidates == null || result.candidates.Count == 0)
					{
						System.Diagnostics.Debug.WriteLine($"⚠️ {modelName} returned no candidates");
						lastException = new Exception($"No candidates: {modelName}");
						continue;
					}

					// Kiểm tra finishReason
					string finishReason = result.candidates[0].finishReason?.ToString();
					if (finishReason == "SAFETY")
					{
						System.Diagnostics.Debug.WriteLine($"⚠️ {modelName} blocked by safety filters");
						lastException = new Exception($"Safety block: {modelName}");
						continue;
					}

					var parts = result.candidates[0].content?.parts;
					if (parts == null || parts.Count == 0)
					{
						System.Diagnostics.Debug.WriteLine($"⚠️ {modelName} returned no content parts");
						lastException = new Exception($"No parts: {modelName}");
						continue;
					}

					string aiResponse = parts[0].text?.ToString();

					if (string.IsNullOrWhiteSpace(aiResponse))
					{
						System.Diagnostics.Debug.WriteLine($"⚠️ {modelName} returned empty text");
						lastException = new Exception($"Empty text: {modelName}");
						continue;
					}

					System.Diagnostics.Debug.WriteLine($"✅ {modelName} returned response: {aiResponse.Substring(0, Math.Min(200, aiResponse.Length))}...");

					// Parse JSON
					string cleanedJson = ExtractJsonContent(aiResponse);

					if (string.IsNullOrWhiteSpace(cleanedJson) || cleanedJson == "[]")
					{
						System.Diagnostics.Debug.WriteLine($"⚠️ {modelName} returned empty JSON");
						lastException = new Exception($"Empty JSON: {modelName}");
						continue;
					}

					List<AIQuestionDTO> questions = null;

					try
					{
						questions = JsonConvert.DeserializeObject<List<AIQuestionDTO>>(cleanedJson);
					}
					catch (JsonException jsonEx)
					{
						System.Diagnostics.Debug.WriteLine($"⚠️ JSON parse error for {modelName}: {jsonEx.Message}");
						System.Diagnostics.Debug.WriteLine($"Raw JSON: {cleanedJson}");
						lastException = jsonEx;
						continue;
					}

					if (questions == null || questions.Count == 0)
					{
						System.Diagnostics.Debug.WriteLine($"⚠️ {modelName} parsed to empty list");
						lastException = new Exception($"Empty question list: {modelName}");
						continue;
					}

					// ✅ SUCCESS!
					System.Diagnostics.Debug.WriteLine($"✅✅✅ SUCCESS with {modelName}: {questions.Count} questions found");

					MessageBox.Show(
						$"✅ Phân tích thành công!\n\n" +
						$"🤖 Model: {modelName}\n" +
						$"📊 Số câu hỏi: {questions.Count}\n" +
						$"📄 File: {fileName}",
						"Thành Công",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);

					ValidateQuestions(questions);
					return new AIAnalysisResult { Questions = questions };
				}
				catch (HttpRequestException httpEx)
				{
					System.Diagnostics.Debug.WriteLine($"❌ HTTP error for {modelName}: {httpEx.Message}");
					lastException = httpEx;

					// Nếu là lỗi network, không thử model khác
					if (httpEx.Message.Contains("Unable to connect"))
					{
						throw;
					}

					continue;
				}
				catch (TaskCanceledException)
				{
					System.Diagnostics.Debug.WriteLine($"⏱️ Timeout for {modelName}");
					lastException = new Exception($"Timeout: {modelName}");
					continue;
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"❌ Unexpected error for {modelName}: {ex.Message}");
					lastException = ex;
					continue;
				}
			}

			// ❌ Tất cả models đều thất bại
			string errorMessage = lastException != null
				? $"Lỗi cuối cùng: {lastException.Message}"
				: "Không có lỗi cụ thể";

			throw new Exception(
				$"❌ Không thể phân tích PDF sau khi thử {modelsToTry.Length} models.\n\n" +
				$"{errorMessage}\n\n" +
				"Các nguyên nhân có thể:\n" +
				"1. File PDF không chứa đề thi IELTS Reading\n" +
				"2. PDF bị mã hóa hoặc lỗi format\n" +
				"3. Đã vượt quota API (chờ 1 giờ và thử lại)\n" +
				"4. Kết nối Internet không ổn định\n\n" +
				"Giải pháp:\n" +
				"- Thử file PDF khác\n" +
				"- Kiểm tra Console Output (Debug window)\n" +
				"- Nhập câu hỏi thủ công"
			);
		}
		/// Validate kết quả từ AI
		/// </summary>
		private void ValidateQuestions(List<AIQuestionDTO> questions)
		{
			// Kiểm tra số lượng câu hỏi IELTS chuẩn
			if (questions.Count > 40)
			{
				MessageBox.Show($"⚠️ Cảnh báo: AI tìm thấy {questions.Count} câu hỏi (IELTS chuẩn có 40).\n\n" +
					"Có thể file PDF chứa thêm nội dung bổ sung.",
					"Số Lượng Bất Thường", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			// Kiểm tra position trùng lặp
			var duplicates = questions.GroupBy(q => q.Position)
									.Where(g => g.Count() > 1)
									.Select(g => g.Key);

			if (duplicates.Any())
			{
				throw new Exception($"Lỗi: Có câu hỏi trùng position: {string.Join(", ", duplicates)}");
			}

			// Kiểm tra question type hợp lệ
			var validTypes = new[] {
				"MCQ", "MULTI_SELECT", "FILL_BLANK", "TRUE_FALSE_NOT_GIVEN",
				"MATCHING_HEADINGS", "MATCHING_INFORMATION", "MATCHING_FEATURES",
				"SENTENCE_COMPLETION", "SUMMARY_COMPLETION", "SHORT_ANSWER"
			};

			var invalidTypes = questions.Where(q => !validTypes.Contains(q.QuestionType?.ToUpperInvariant()))
									   .Select(q => $"Q{q.Position}: {q.QuestionType}");

			if (invalidTypes.Any())
			{
				MessageBox.Show($"⚠️ Cảnh báo: Một số question type không chuẩn:\n\n{string.Join("\n", invalidTypes)}\n\n" +
					"Hệ thống sẽ cố gắng xử lý, nhưng bạn nên kiểm tra lại.",
					"Question Type Không Chuẩn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// Trích xuất JSON từ response của AI
		/// </summary>
		private string ExtractJsonContent(string input)
		{
			if (string.IsNullOrEmpty(input)) return "[]";

			// Loại bỏ markdown code block
			input = input.Replace("```json", "").Replace("```", "").Trim();

			// Tìm array JSON
			int start = input.IndexOf('[');
			int end = input.LastIndexOf(']');

			if (start != -1 && end != -1 && end > start)
			{
				return input.Substring(start, end - start + 1);
			}

			return input;
		}
	}

	// DTO Classes
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