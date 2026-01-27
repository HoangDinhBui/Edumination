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
		public async Task<bool> AnalyzePdfAndSaveQuestions(string pdfPath, long testPaperId, string audioPath = "")
		{
			try
			{
				// 1. Kiểm tra file (Giữ nguyên logic cũ)
				if (!File.Exists(pdfPath))
				{
					MessageBox.Show($"❌ Lỗi: Không tìm thấy file PDF tại:\n{pdfPath}", "Lỗi File", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				FileInfo fileInfo = new FileInfo(pdfPath);
				if (fileInfo.Length > MAX_FILE_SIZE_BYTES)
				{
					MessageBox.Show($"❌ File PDF quá lớn. Giới hạn: {MAX_FILE_SIZE_BYTES / 1024 / 1024} MB.", "File Quá Lớn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return false;
				}

				// 2. Đọc và chuyển đổi PDF (Giữ nguyên logic cũ)
				byte[] pdfBytes = await Task.Run(() => File.ReadAllBytes(pdfPath));
				string base64Pdf = Convert.ToBase64String(pdfBytes);

				// 3. Gọi Gemini API (Kết quả đã có Skill và TimeLimit)
				var analysisResult = await CallGeminiAPIWithRetry(base64Pdf, Path.GetFileName(pdfPath));

				// Kiểm tra dữ liệu hợp lệ (Giữ nguyên logic cũ)
				bool hasQuestions = analysisResult?.Questions != null && analysisResult.Questions.Any();
				bool hasPrompts = analysisResult?.Prompts != null && analysisResult.Prompts.Any();

				if (analysisResult == null || (!hasQuestions && !hasPrompts))
				{
					MessageBox.Show("⚠️ AI không tìm thấy dữ liệu IELTS hợp lệ (Câu hỏi hoặc Đề bài) trong file PDF.", "Không Tìm Thấy Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return false;
				}

				// 4. Tạo TestSection dựa trên nhận diện của AI
				string detectedSkill = (analysisResult.Skill ?? "READING").ToUpper();
				int detectedTime = analysisResult.TimeLimit > 0 ? analysisResult.TimeLimit : 60;

				var section = new TestSectionDTO
				{
					PaperId = testPaperId,
					Skill = detectedSkill,
					TimeLimitMinutes = detectedTime,
					PdfFileName = Path.GetFileName(pdfPath),
					PdfFilePath = pdfPath,
					// ĐỔI: Gán đường dẫn file âm thanh vào DTO để lưu xuống Database (Bảng TestSections)
					AudioFilePath = audioPath
				};

				long sectionId = _testSectionBLL.CreateTestSection(section);
				if (sectionId <= 0)
				{
					MessageBox.Show("❌ Lỗi: Không thể tạo TestSection trong Database.", "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				// 5. Logic lưu dữ liệu linh hoạt theo loại bài thi (Giữ nguyên logic cũ)
				// 5. Logic lưu dữ liệu linh hoạt theo loại bài thi
				bool saveSuccess = false;

				if (detectedSkill == "READING" || detectedSkill == "LISTENING")
				{
					// Logic cho bài thi trắc nghiệm (Giữ nguyên)
					var questionTypes = new Dictionary<int, string>();
					var answers = new Dictionary<int, string>();
					var questionRanges = new Dictionary<int, int>();

					if (analysisResult.Questions != null)
					{
						foreach (var q in analysisResult.Questions)
						{
							questionTypes[q.Position] = q.QuestionType;
							answers[q.Position] = q.Answer ?? "";
							if (q.EndPosition.HasValue && q.EndPosition > q.Position)
								questionRanges[q.Position] = q.EndPosition.Value;
						}
						saveSuccess = _testSectionBLL.SaveQuestionsToSection(sectionId, questionTypes, answers, questionRanges);
					}
				}
				else if (detectedSkill == "WRITING" || detectedSkill == "SPEAKING")
				{
					// ✅ PHẦN SỬA MỚI: Xử lý lưu nội dung đề bài Writing/Speaking
					if (analysisResult.Prompts != null && analysisResult.Prompts.Any())
					{
						var questionTypes = new Dictionary<int, string>();
						var contents = new Dictionary<int, string>();

						for (int i = 0; i < analysisResult.Prompts.Count; i++)
						{
							int position = i + 1;
							questionTypes[position] = "ESSAY"; // Loại câu hỏi tự luận
							contents[position] = analysisResult.Prompts[i]; // Nội dung đề bài trích xuất từ PDF
						}

						// Gọi hàm BLL để lưu nội dung đề vào bảng Questions
						// Bạn cần đảm bảo hàm này đã được định nghĩa trong TestSectionBLL
						saveSuccess = _testSectionBLL.SaveWritingPrompts(sectionId, questionTypes, contents);

						if (!saveSuccess)
						{
							MessageBox.Show($"❌ Lỗi: AI đã tìm thấy đề {detectedSkill} nhưng không thể lưu vào Database.", "Lỗi Lưu Trữ", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
					else
					{
						MessageBox.Show($"⚠️ AI xác định là {detectedSkill} nhưng không trích xuất được nội dung đề bài (prompts rỗng).", "Thiếu Nội Dung", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}

				if (saveSuccess)
				{
					MessageBox.Show($"✅ Thành công!\n\n" +
						$"📊 Loại bài thi: {detectedSkill}\n" +
						$"⏱️ Thời gian: {detectedTime} phút\n" +
						$"📝 Section ID: {sectionId}",
						"Phân Tích Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

				return saveSuccess;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"❌ Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

			// ✅ Danh sách models theo thứ tự ưu tiên
			var modelsToTry = new[]
			{
		"gemini-2.5-flash",
		"gemini-2.0-flash",
		"gemini-flash-latest",
		"gemini-2.5-pro",
		"gemini-pro-latest",
		"gemini-2.0-flash-lite"
	};

			Exception lastException = null;

			foreach (var modelName in modelsToTry)
			{
				try
				{
					string url = $"https://generativelanguage.googleapis.com/v1beta/models/{modelName}:generateContent?key={geminiApiKey}";

					// GIỮ NGUYÊN PROMPT CỦA BẠN
					string prompt = @"You are a specialized Data Extraction Tool for IELTS documents.

### YOUR MISSION:
1. IDENTIFY the test skill: LISTENING, READING, WRITING, or SPEAKING.
2. EXTRACT the answer keys (for Listening/Reading) or the prompt tasks (for Writing/Speaking).

### SKILL IDENTIFICATION RULES:
- LISTENING: Look for keywords like 'Section 1', 'Part 1', 'Recording', or answers containing words/short phrases.
- READING: Look for 'Passage 1', 'Reading Passage', and answers like 'TRUE/FALSE/NOT GIVEN' or letters A-D.
- WRITING: Look for 'Task 1', 'Task 2', 'Write at least 150/250 words'.
- SPEAKING: Look for 'Part 1', 'Cue Card', 'Discussion topics'.

### DATA STRUCTURE:
- skill: (string) Must be one of: 'LISTENING', 'READING', 'WRITING', 'SPEAKING'.
- timeLimit: (int) Default 60 for Reading/Writing, 30 for Listening, 15 for Speaking.
- questions: (array) 
    - position: (int) 1-40.
    - questionType: (string) MCQ, FILL_BLANK, TRUE_FALSE, etc.
    - answer: (string) The exact answer from the key.
    - endPosition: (int|null) For grouped questions.
- prompts: (array of strings) ONLY for WRITING or SPEAKING. Extract the full text of the essay tasks or speaking questions.

### OUTPUT FORMAT:
Return ONLY a valid JSON object. No markdown, no prose.
{
  ""skill"": ""READING"",
  ""timeLimit"": 60,
  ""questions"": [
    { ""position"": 1, ""questionType"": ""MCQ"", ""answer"": ""A"", ""endPosition"": null }
  ],
  ""prompts"": []
}";

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
							temperature = 0.1,
							topK = 40,
							topP = 0.95,
							maxOutputTokens = 8192,
							responseMimeType = "application/json"
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

					if (!response.IsSuccessStatusCode)
					{
						dynamic errorObj = JsonConvert.DeserializeObject(responseJson);
						string errorMsg = errorObj?.error?.message?.ToString() ?? "Unknown error";
						int errorCode = errorObj?.error?.code ?? 0;

						if (errorCode == 429) { await Task.Delay(2000); continue; }
						if (errorCode == 404) continue;
						throw new HttpRequestException($"API Error ({response.StatusCode}): {errorMsg}");
					}

					dynamic result = JsonConvert.DeserializeObject(responseJson);
					string aiResponse = result.candidates[0].content.parts[0].text?.ToString();

					if (string.IsNullOrWhiteSpace(aiResponse)) continue;

					// Parse JSON vào object AIAnalysisResult thay vì List
					string cleanedJson = ExtractJsonContent(aiResponse);

					// SỬA TẠI ĐÂY: Parse ra toàn bộ object kết quả
					AIAnalysisResult finalResult = JsonConvert.DeserializeObject<AIAnalysisResult>(cleanedJson);

					if (finalResult == null || (finalResult.Questions == null && finalResult.Prompts == null))
					{
						System.Diagnostics.Debug.WriteLine($"⚠️ {modelName} returned empty result");
						continue;
					}

					// Hiển thị thông báo thành công
					int qCount = finalResult.Questions?.Count ?? 0;
					MessageBox.Show(
						$"✅ Phân tích thành công!\n\n" +
						$"🤖 Model: {modelName}\n" +
						$"📊 Kỹ năng: {finalResult.Skill}\n" +
						$"📝 Số câu hỏi: {qCount}\n" +
						$"📄 File: {fileName}",
						"Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);

					if (finalResult.Questions != null) ValidateQuestions(finalResult.Questions);

					return finalResult; // Trả về object đã parse
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"❌ Error {modelName}: {ex.Message}");
					lastException = ex;
					continue;
				}
			}

			throw lastException ?? new Exception("❌ Tất cả models đều thất bại.");
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
			if (string.IsNullOrEmpty(input)) return "{}"; // Trả về object rỗng thay vì mảng rỗng

			input = input.Replace("```json", "").Replace("```", "").Trim();

			// Tìm vị trí của dấu ngoặc nhọn đầu tiên và cuối cùng
			int start = input.IndexOf('{');
			int end = input.LastIndexOf('}');

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
		[JsonProperty("skill")]
		public string Skill { get; set; }

		[JsonProperty("timeLimit")]
		public int TimeLimit { get; set; }

		[JsonProperty("questions")]
		public List<AIQuestionDTO> Questions { get; set; }

		[JsonProperty("prompts")]
		public List<string> Prompts { get; set; }
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