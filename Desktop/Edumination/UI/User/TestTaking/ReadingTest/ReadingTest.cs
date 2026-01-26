using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Edumination.Api.Domain.Entities;
using IELTS.DAL;
using IELTS.UI.User.Results;
using IELTS.UI.User.TestTaking.Controls;

namespace IELTS.UI.User.TestTaking.ReadingTest
{
    public partial class ReadingTest : Form
    {
        private List<Question> _questions = new List<Question>();
        private Dictionary<long, string> _userAnswers = new Dictionary<long, string>();
        private readonly long _paperId;
        private readonly long _sectionId;

        private int _remainingSeconds;
        private readonly System.Windows.Forms.Timer _timer;

        private FlowLayoutPanel questionsPanel; // host các câu hỏi hiển thị

        public ReadingTest(long paperId, long sectionId)
        {
            _paperId = paperId;
            _sectionId = sectionId;

            InitializeComponent();

            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
        }

        public ReadingTest() : this(0, 0) { }


        private void ReadingTest_Load(object sender, EventArgs e)
        {
            // kiểm tra controls cơ bản
            if (pdfViewer == null || answerPanel == null || testNavBar == null)
            {
                MessageBox.Show("UI controls are not initialized properly.", "Error");
                Close();
                return;
            }

            // Thêm event Exit + Submit giống Listening
            testNavBar.OnExitRequested += TestNavBar_OnExitRequested;
            testNavBar.OnSubmitRequested += TestNavBar_OnSubmitRequested;

            if (_sectionId <= 0)
            {
                MessageBox.Show(
                    "No Reading section specified. Please open this test from Test Library.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Close();
                return;
            }

            if (!LoadSectionFromDatabase())
            {
                Close();
                return;
            }

            // tải câu hỏi và hiển thị
            LoadQuestionsFromDatabase();
            DisplayQuestions();

            UpdateTimeLabel();
            _timer.Start();
        }

        // ================================
        // EXIT — GIỐNG LISTENING
        // ================================
        private void TestNavBar_OnExitRequested()
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to exit this Reading test?",
                "Exit Test",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                _timer.Stop();
                Hide();
                new IELTS.UI.User.TestLibrary.TestLibrary().Show();
            }
        }

        // ================================
        // SUBMIT — GIỐNG LISTENING
        // ================================
        private void TestNavBar_OnSubmitRequested()
        {
            var confirm = MessageBox.Show(
                "Do you want to submit your answers now?",
                "Submit Test",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                _timer.Stop();
                SubmitTest();
                Hide();
                new IELTS.UI.User.TestLibrary.TestLibrary().Show();
            }
        }

        // ================================
        // LOAD SECTION
        // ================================
        private bool LoadSectionFromDatabase()
        {
            string skill = "READING";
            int? timeLimitMinutes = null;
            string pdfPath = null;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Skill, TimeLimitMinutes, PdfFilePath
                                        FROM TestSections
                                        WHERE Id = @Id";

                    var p = cmd.CreateParameter();
                    p.ParameterName = "@Id";
                    p.Value = _sectionId;
                    cmd.Parameters.Add(p);

                    conn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        if (!r.Read())
                        {
                            MessageBox.Show(
                                "Reading section not found in database.",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return false;
                        }

                        if (r["Skill"] != DBNull.Value)
                            skill = r["Skill"].ToString();

                        if (r["TimeLimitMinutes"] != DBNull.Value)
                            timeLimitMinutes = Convert.ToInt32(r["TimeLimitMinutes"]);

                        if (r["PdfFilePath"] != DBNull.Value)
                            pdfPath = r["PdfFilePath"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading section from database:\r\n" + ex.Message,
                    "DB Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            if (!timeLimitMinutes.HasValue || timeLimitMinutes.Value <= 0)
                timeLimitMinutes = 60;

            _remainingSeconds = timeLimitMinutes.Value * 60;

            this.Text = $"IELTS Reading – {skill}";

            // Gọi method có sẵn của PdfViewerPanel (giả sử có)
            try
            {
                pdfViewer.ShowPdf(
                    pdfPath,
                    $"{skill} Section",
                    "No PDF is configured for this section in the database."
                );
            }
            catch
            {
                // Nếu ShowPdf không ném exception thì ổn, nếu ném thì vẫn tiếp tục (tùy implement)
            }

            return true;
        }

        // ================================
        // TIMER
        // ================================
        private void Timer_Tick(object sender, EventArgs e)
        {
            _remainingSeconds--;

            if (_remainingSeconds <= 0)
            {
                _timer.Stop();
                UpdateTimeLabel();

                MessageBox.Show(
                    "Time is up! The test will be submitted.",
                    "Time up", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SubmitTest();
                return;
            }

            UpdateTimeLabel();
        }

        private void UpdateTimeLabel()
        {
            int minutes = Math.Max(0, _remainingSeconds / 60);
            int seconds = Math.Max(0, _remainingSeconds % 60);

            string t = $"{minutes:D2}:{seconds:D2} minutes remaining";

            this.Text = $"IELTS Reading – {t}";
        }

        // ================================
        // SUBMIT
        // ================================
        private void SubmitTest()
        {
			string currentName = IELTS.UI.User.SessionManager.FullName;
			// DEBUG: Nếu vẫn ra "Student", hãy gán cứng tên từ DB để kiểm tra giao diện trước
			if (currentName == "Student")
			{
				currentName = "Trần Tú"; // Thay bằng logic lấy từ DB nếu cần
			}
			var finalResult = new ExamResult
            {
                Skill = "Reading",
                UserName = currentName, // ✅ Đảm bảo có giá trị
                TimeTakenSeconds = 3600 - _remainingSeconds,
                TotalQuestions = _questions.Count,
                Parts = new List<PartReview>()
            };

            // ✅ KIỂM TRA: In ra console để debug
            System.Diagnostics.Debug.WriteLine($"Total Questions: {_questions.Count}");
            System.Diagnostics.Debug.WriteLine($"User Answers Count: {_userAnswers.Count}");

            // Gom nhóm theo Part
            for (int p = 1; p <= 4; p++)
            {
                var part = new PartReview
                {
                    PartName = $"Part {p}",
                    Questions = new List<QuestionReview>()
                };

                var partQuestions = _questions.Skip((p - 1) * 10).Take(10).ToList();

                foreach (var q in partQuestions)
                {
                    _userAnswers.TryGetValue(q.Id, out string userAns);
                    string correctAns = GetCorrectAnswerFromDB(q.Id);

                    bool isCorrect = string.Equals(
                        userAns?.Trim(),
                        correctAns?.Trim(),
                        StringComparison.OrdinalIgnoreCase
                    );

                    if (isCorrect)
                        finalResult.CorrectCount++;

                    // ✅ DEBUG: In từng câu hỏi
                    System.Diagnostics.Debug.WriteLine(
                        $"Q{_questions.IndexOf(q) + 1}: User={userAns ?? "EMPTY"} | " +
                        $"Correct={correctAns ?? "EMPTY"} | Match={isCorrect}"
                    );

                    part.Questions.Add(new QuestionReview
                    {
                        Number = _questions.IndexOf(q) + 1,
                        UserAnswer = string.IsNullOrEmpty(userAns) ? "" : userAns, // ✅ Hiển thị rõ
                        CorrectAnswer = correctAns ?? "N/A",
                        IsCorrect = isCorrect
                    });
                }

                finalResult.Parts.Add(part);
            }

            // Tính Band Score
            finalResult.Band = CalculateBandScore(finalResult.CorrectCount);

            // ✅ DEBUG: In kết quả cuối
            System.Diagnostics.Debug.WriteLine($"=== FINAL RESULT ===");
            System.Diagnostics.Debug.WriteLine($"Correct: {finalResult.CorrectCount}/{finalResult.TotalQuestions}");
            System.Diagnostics.Debug.WriteLine($"Band: {finalResult.Band}");
            System.Diagnostics.Debug.WriteLine($"Time: {finalResult.TimeTakenSeconds}s");

            // Mở form kết quả
            AnswerResultForm resultForm = new AnswerResultForm(finalResult);
            resultForm.Show();
            this.Close();
        }
        private double CalculateBandScore(int correctAnswers)
        {
            if (correctAnswers >= 39) return 9.0;
            if (correctAnswers >= 37) return 8.5;
            if (correctAnswers >= 35) return 8.0;
            if (correctAnswers >= 33) return 7.5;
            if (correctAnswers >= 30) return 7.0;
            if (correctAnswers >= 27) return 6.5;
            if (correctAnswers >= 23) return 6.0;
            if (correctAnswers >= 19) return 5.5;
            if (correctAnswers >= 15) return 5.0;
            if (correctAnswers >= 13) return 4.5;
            if (correctAnswers >= 10) return 4.0;
            if (correctAnswers >= 8) return 3.5;
            if (correctAnswers >= 6) return 3.0;
            if (correctAnswers >= 4) return 2.5;
            return 0.0;
        }

        private void LoadQuestionsFromDatabase()
        {
            _questions.Clear();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, QuestionText, QuestionType FROM Questions WHERE SectionId = @SectionId ORDER BY Position";
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@SectionId";
                    p.Value = _sectionId;
                    cmd.Parameters.Add(p);

                    conn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var q = new Question
                            {
                                Id = Convert.ToInt64(r["Id"]),
                                Text = r["QuestionText"] != DBNull.Value ? r["QuestionText"].ToString() : string.Empty,
                                Type = r["QuestionType"] != DBNull.Value ? r["QuestionType"].ToString() : string.Empty
                            };
                            _questions.Add(q);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading questions:\r\n" + ex.Message, "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayQuestions()
        {   
            // nếu questionsPanel chưa có thì tạo và thêm vào answerPanel
            if (questionsPanel == null)
            {
                questionsPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    Padding = new Padding(10)
                };
                answerPanel.Controls.Clear();
                answerPanel.Controls.Add(questionsPanel);
            }

            questionsPanel.Controls.Clear();

            int questionNumber = 1;
            foreach (var q in _questions)
            {
                // Tạo TableLayoutPanel cho mỗi câu hỏi
                var table = new TableLayoutPanel
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    ColumnCount = 1,
                    RowCount = 3,
                    Width = questionsPanel.ClientSize.Width - 25,
                    Margin = new Padding(5, 10, 5, 10),
                    Padding = new Padding(10)
                };

                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Question number
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Question text
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Answer textbox

                // Question number
                var lblNumber = new Label
                {
                    Text = $"Question {questionNumber}",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(0, 102, 204),
                    Margin = new Padding(0, 0, 0, 5)
                };
                table.Controls.Add(lblNumber, 0, 0);

                // Question text
                var lbl = new Label
                {
                    Text = q.Text,
                    AutoSize = true,
                    MaximumSize = new Size(table.Width - 30, 0),
                    Font = new Font("Segoe UI", 10, FontStyle.Regular),
                    Margin = new Padding(0, 0, 0, 5)
                };
                table.Controls.Add(lbl, 0, 1);

                // Answer textbox với label
                var answerContainer = new FlowLayoutPanel
                {
                    AutoSize = true,
                    FlowDirection = FlowDirection.LeftToRight,
                    WrapContents = false,
                    Margin = new Padding(0)
                };

                var lblAnswer = new Label
                {
                    Text = "Your answer:",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    Margin = new Padding(0, 5, 10, 0)
                };
                answerContainer.Controls.Add(lblAnswer);

                var txt = new TextBox
                {
                    Width = 300,
                    Tag = q.Id,
                    Font = new Font("Segoe UI", 10),
                    Margin = new Padding(0)
                };

                // Load previous answer if exists
                if (_userAnswers.TryGetValue(q.Id, out var prev) && prev != null)
                    txt.Text = prev;

                txt.TextChanged += (s, e) =>
                {
                    var tb = s as TextBox;
                    if (tb == null) return;
                    if (!(tb.Tag is long qid)) return;
                    _userAnswers[qid] = tb.Text;
                };
                answerContainer.Controls.Add(txt);

                table.Controls.Add(answerContainer, 0, 2);

                questionsPanel.Controls.Add(table);
                questionNumber++;
            }
        }

        private int GradeReading()
        {
            int correct = 0;
            foreach (var q in _questions)
            {
                string userAns = "";
                _userAnswers.TryGetValue(q.Id, out userAns);
                userAns = (userAns ?? "").Trim().ToUpperInvariant();

                string correctAns = (GetCorrectAnswerFromDB(q.Id) ?? "").Trim().ToUpperInvariant();
                if (!string.IsNullOrEmpty(correctAns) && userAns == correctAns)
                    correct++;
            }
            return correct;
        }

        private string GetCorrectAnswerFromDB(long questionId)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT ChoiceText FROM QuestionChoices WHERE QuestionId = @QId AND IsCorrect = 1";
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@QId";
                    p.Value = questionId;
                    cmd.Parameters.Add(p);

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return result.ToString();
                }

                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT AnswerData FROM QuestionAnswerKeys WHERE QuestionId = @QId";
                    var p = cmd.CreateParameter();
                    p.ParameterName = "@QId";
                    p.Value = questionId;
                    cmd.Parameters.Add(p);

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return result.ToString();
                }
            }
            catch (Exception ex)
            {
                // không ném exception ra UI để không phá flow, nhưng log nếu bạn có logging
                MessageBox.Show("Error retrieving correct answer:\r\n" + ex.Message, "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return string.Empty;
        }
    }
}
