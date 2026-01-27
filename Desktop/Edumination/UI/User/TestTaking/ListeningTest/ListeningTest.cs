using IELTS.BLL;
using IELTS.DAL;
using IELTS.DTO;
using IELTS.UI.User.TestTaking.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using IELTS.UI.User.Results;

namespace IELTS.UI.User.TestTaking.ListeningTest
{
    public partial class ListeningTest : Form
    {
        // --- Fields ---
        private readonly long _paperId;
        private readonly long _sectionId;

        private int _remainingSeconds;
        private readonly System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();

        private string _audioPath = null;
        private string _pdfPath = null;

        // Service AI
        private readonly GroqService _aiService;

        // --- Constructor ---
        public ListeningTest(long paperId, long sectionId)
        {
            _paperId = paperId;
            _sectionId = sectionId;

            InitializeComponent();

            // Cấu hình Form
            WindowState = FormWindowState.Maximized;

            // Khởi tạo AI Service
            _aiService = new GroqService();

            // Cấu hình Timer
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
        }

        // --- Events ---
        private void ListeningTest_Load(object sender, EventArgs e)
        {
            // Gán sự kiện cho NavBar
            testNavBar.OnExitRequested += TestNavBar_OnExitRequested;
            testNavBar.OnSubmitRequested += TestNavBar_OnSubmitRequested;

            if (_sectionId <= 0)
            {
                MessageBox.Show("Lỗi: Không tìm thấy ID phần thi Listening.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            // 1. Load thông tin cơ bản (Audio, PDF, Thời gian)
            if (!LoadSectionMetadata())
            {
                Close();
                return;
            }

            // 2. Load danh sách câu hỏi lên AnswerPanel (Để người dùng nhập)
            LoadQuestionsIntoAnswerPanel();

            // 3. Hiển thị Audio và PDF
            audioPanel.LoadAudio(_audioPath);
            pdfViewer.ShowPdf(
                _pdfPath,
                "Listening Question Sheet",
                "Không tìm thấy file đề bài PDF."
            );

            // 4. Bắt đầu tính giờ
            UpdateTimeLabel();
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _remainingSeconds--;

            if (_remainingSeconds <= 0)
            {
                _timer.Stop();
                UpdateTimeLabel();
                MessageBox.Show("Hết giờ làm bài!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Tự động nộp bài khi hết giờ
                SubmitTestWithAI();
            }
            else
            {
                UpdateTimeLabel();
            }
        }

        private void UpdateTimeLabel()
        {
            int min = _remainingSeconds / 60;
            int sec = _remainingSeconds % 60;
            testNavBar.SetTimeText($"{min:D2}:{sec:D2} remaining");
        }

        // --- Navigation Handlers ---
        private void TestNavBar_OnExitRequested()
        {
            var confirm = MessageBox.Show(
                "Bạn có chắc muốn thoát bài thi? Kết quả sẽ không được lưu.",
                "Thoát bài thi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                _timer.Stop();
                if (audioPanel != null) audioPanel.StopAudio(); // Dừng nhạc
                this.Close();
            }
        }

        // --- XỬ LÝ NỘP BÀI VÀ CHẤM ĐIỂM AI (QUAN TRỌNG) ---
        private void TestNavBar_OnSubmitRequested()
        {
            var confirm = MessageBox.Show(
                "Bạn có muốn nộp bài và để AI chấm điểm ngay lập tức không?",
                "Nộp bài",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                SubmitTestWithAI();
            }
        }

		private async void SubmitTestWithAI()
		{
			_timer.Stop();
			if (audioPanel != null) audioPanel.StopAudio();

			// Khóa giao diện để tránh bấm lung tung
			this.Enabled = false;
			testNavBar.SetTimeText("AI đang chấm điểm... Vui lòng chờ...");

			try
			{
				// 1. Lấy đáp án người dùng nhập từ UI
				Dictionary<int, string> userAnswers = answerPanel.CollectAnswers();

				// 2. Lấy đáp án đúng (Answer Key) từ Database
				Dictionary<int, string> correctKeys = GetCorrectKeysFromDB(_sectionId);

				if (correctKeys.Count == 0)
				{
					throw new Exception("Không tìm thấy đáp án trong CSDL để chấm điểm.");
				}

				// 3. Gọi Groq AI Service để chấm điểm
				ListeningGradeResult aiResult = await _aiService.GradeListeningAsync(userAnswers, correctKeys);

				// 4. CHUYỂN ĐỔI SANG ĐỐI TƯỢNG ExamResult ĐỂ HIỂN THỊ TRANG ANSWER
				string currentName = IELTS.UI.User.SessionManager.FullName;
				if (string.IsNullOrEmpty(currentName) || currentName == "Student") currentName = "Trần Tú";

				var finalResult = new ExamResult
				{
					Skill = "Listening",
					UserName = currentName,
					TimeTakenSeconds = (30 * 60) - _remainingSeconds, // Giả sử limit là 30p, bạn có thể lấy từ biến limit
					TotalQuestions = aiResult.TotalQuestions,
					CorrectCount = aiResult.TotalCorrect,
					Band = aiResult.BandScore,
					Parts = new List<PartReview>()
				};

				// Gom nhóm kết quả vào các Part (IELTS Listening thường có 4 Part)
				for (int p = 1; p <= 4; p++)
				{
					var part = new PartReview { PartName = $"Part {p}", Questions = new List<QuestionReview>() };

					// Lấy các câu hỏi thuộc Part này (ví dụ mỗi Part 10 câu)
					var partDetails = aiResult.Details
						.Where(d => d.QuestionNumber > (p - 1) * 10 && d.QuestionNumber <= p * 10)
						.ToList();

					foreach (var d in partDetails)
					{
						part.Questions.Add(new QuestionReview
						{
							Number = d.QuestionNumber,
							UserAnswer = d.UserAnswer,
							CorrectAnswer = d.CorrectKey,
							IsCorrect = d.IsCorrect
							// Bạn có thể gán thêm d.Explanation vào một field Ghi chú nếu QuestionReview có hỗ trợ
						});
					}
					if (part.Questions.Count > 0) finalResult.Parts.Add(part);
				}

				// 5. HIỂN THỊ FORM KẾT QUẢ GIỐNG READING
				new AnswerResultForm(finalResult).Show();

				// Đóng form thi
				this.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi trong quá trình chấm điểm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Enabled = true;
				_timer.Start();
			}
		}
		// --- DATABASE HELPERS ---

		// 1. Load thông tin bài thi (Metadata)
		private bool LoadSectionMetadata()
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT TimeLimitMinutes, AudioFilePath, PdfFilePath FROM TestSections WHERE Id = @Id";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", _sectionId);
                        using (var r = cmd.ExecuteReader())
                        {
                            if (!r.Read()) return false;

                            int limit = r["TimeLimitMinutes"] != DBNull.Value ? Convert.ToInt32(r["TimeLimitMinutes"]) : 30;
                            _remainingSeconds = limit * 60;

							if (r["AudioFilePath"] != DBNull.Value)
							{
								// 1. Lấy tên file gốc từ DB
								string fileName = Path.GetFileName(r["AudioFilePath"].ToString());

								// 2. Lùi 2 cấp từ thư mục thực thi (net8.0-windows -> Debug -> bin)
								// Cách này an toàn hơn dùng chuỗi ".." vì nó lấy đường dẫn vật lý thực tế
								DirectoryInfo binDir = Directory.GetParent(Application.StartupPath); // Lùi 1 cấp ra khỏi net8.0-windows
								DirectoryInfo debugDir = binDir?.Parent; // Lùi thêm 1 cấp nữa ra khỏi Debug (vào thư mục bin)

								if (debugDir != null)
								{
									// 3. Kết hợp với cấu trúc UI/assets/audios mà bạn đang có
									_audioPath = Path.Combine(debugDir.FullName, "UI", "assets", "audios", fileName);

									// Kiểm tra xem file có thực sự tồn tại ở đó không
									if (!File.Exists(_audioPath))
									{
										// THỬ NGHIỆM DỰ PHÒNG: Nếu folder UI nằm ngang hàng với bin
										DirectoryInfo projectRootDir = debugDir.Parent;
										if (projectRootDir != null)
										{
											string fallbackPath = Path.Combine(projectRootDir.FullName, "UI", "assets", "audios", fileName);
											if (File.Exists(fallbackPath)) _audioPath = fallbackPath;
										}
									}
								}

								// Hiển thị thông báo nếu vẫn không tìm thấy sau khi lùi 2 cấp
								if (!File.Exists(_audioPath))
								{
									MessageBox.Show($"❌ Vẫn không tìm thấy file sau khi lùi 2 cấp tại:\n{_audioPath}", "Lỗi đường dẫn");
								}
							}
							if (r["PdfFilePath"] != DBNull.Value) _pdfPath = r["PdfFilePath"].ToString();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB Error: " + ex.Message);
                return false;
            }
        }

        // 2. Load câu hỏi để tạo ô nhập liệu (Dynamic UI)
        private void LoadQuestionsIntoAnswerPanel()
        {
            // Giả lập tạo đối tượng ReadingPart/ListeningPart để tái sử dụng hàm LoadPart của AnswerPanel
            // Vì AnswerPanel của bạn cần một object chứa danh sách câu hỏi

            var questionsList = new List<ReadingTest.ReadingQuestion>(); // Tạm dùng class ReadingQuestion hoặc tạo class ListeningQuestion tương đương

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    // Lấy danh sách câu hỏi sắp xếp theo thứ tự
                    string sql = "SELECT Position, QuestionText, QuestionType FROM Questions WHERE SectionId = @Id ORDER BY Position";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", _sectionId);
                        using (var r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                // Map data từ DB sang Object
                                var q = new ReadingTest.ReadingQuestion
                                {
                                    Number = Convert.ToInt32(r["Position"]),
                                    Prompt = r["QuestionText"].ToString(),
                                    // Mapping kiểu câu hỏi (Giản lược)
                                    Type = r["QuestionType"].ToString() == "TRUE_FALSE" ?
                                           ReadingTest.QuestionType.TrueFalse :
                                           ReadingTest.QuestionType.ShortAnswer
                                };
                                questionsList.Add(q);
                            }
                        }
                    }
                }

                // Tạo một object Part giả để truyền vào AnswerPanel
                var partWrapper = new ReadingTest.ReadingPart { Questions = questionsList };

                // Gọi hàm LoadPart của AnswerPanel
                // (Tham số thứ 2 là userAnswers, ban đầu để null)
                answerPanel.LoadPart(partWrapper, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải câu hỏi: " + ex.Message);
            }
        }

        // 3. Lấy đáp án đúng từ DB để gửi cho AI
        private Dictionary<int, string> GetCorrectKeysFromDB(long sectionId)
        {
            var keys = new Dictionary<int, string>();
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                // Join bảng Questions và QuestionAnswerKeys để lấy đáp án theo số thứ tự câu hỏi (Position)
                string sql = @"
                    SELECT q.Position, k.AnswerData 
                    FROM Questions q
                    JOIN QuestionAnswerKeys k ON q.Id = k.QuestionId
                    WHERE q.SectionId = @SectionId
                    ORDER BY q.Position";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@SectionId", sectionId);
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            int number = Convert.ToInt32(r["Position"]);
                            string jsonAns = r["AnswerData"].ToString();
                            // Gửi nguyên chuỗi JSON (ví dụ: ["Sunday", "Sun"]) cho AI tự xử lý
                            keys[number] = jsonAns;
                        }
                    }
                }
            }
            return keys;
        }

        // --- HIỂN THỊ KẾT QUẢ ---
        private void ShowResultDialog(ListeningGradeResult result)
        {
            // Format chuỗi hiển thị chi tiết
            string detailStr = "";
            foreach (var d in result.Details)
            {
                string icon = d.IsCorrect ? "✅" : "❌";
                detailStr += $"{icon} Câu {d.QuestionNumber}: Bạn chọn '{d.UserAnswer}' | Đáp án đúng: {d.CorrectKey}\n";
                if (!d.IsCorrect && !string.IsNullOrEmpty(d.Explanation))
                {
                    detailStr += $"   -> Giải thích: {d.Explanation}\n";
                }
                detailStr += "--------------------------------------------------\n";
            }

            string summaryMsg = $"🎉 KẾT QUẢ BÀI THI\n\n" +
                                $"✅ Số câu đúng: {result.TotalCorrect} / {result.TotalQuestions}\n" +
                                $"🏆 Band Score ước tính: {result.BandScore}\n\n" +
                                $"💡 Nhận xét chung của AI:\n{result.Feedback}\n\n" +
                                $"Bạn có muốn xem chi tiết từng câu sai không?";

            var dialogResult = MessageBox.Show(summaryMsg, "Kết quả AI Chấm", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                // Hiển thị form chi tiết (Tạm thời dùng MessageBox, tốt nhất là tạo 1 Form mới có GridView)
                // Vì MessageBox có giới hạn chiều dài, nếu dài quá nên lưu ra file text hoặc hiện Form mới.
                MessageBox.Show(detailStr, "Chi tiết đáp án", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }
    }
}