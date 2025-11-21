using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Edumination.WinForms.UI.Forms.TestTaking.Controls;


namespace Edumination.WinForms.UI.Forms.TestTaking.ReadingTest
{
    public partial class ReadingTest : Form
    {
        private readonly List<ReadingPart> _parts;
        private int _currentPartIndex = 0;

        private int _remainingSeconds;
        private readonly System.Windows.Forms.Timer _timer;

        // Lưu câu trả lời của user cho tất cả câu
        private readonly Dictionary<int, string> _userAnswers = new();


        public ReadingTest()
        {
            InitializeComponent();
            // Full screen
            this.WindowState = FormWindowState.Maximized;

            // Load mock data
            _parts = ReadingMockData.GetParts();
            _remainingSeconds = ReadingMockData.TotalTimeSeconds;

            // Timer
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
        }
        private void ReadingTest_Load(object sender, EventArgs e)
        {
            testNavBar.OnExitRequested += TestNavBar_OnExitRequested;
            testNavBar.OnSubmitRequested += TestNavBar_OnSubmitRequested;
            testFooter.OnPartSelected += TestFooter_OnPartSelected;

            // Load footer từ mockdata
            testFooter.LoadParts(_parts.Select(p => p.PartName));

            ShowPart(0);

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
                MessageBox.Show("Time is up! The test will be submitted.", "Time up",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                SubmitTest();
                return;
            }

            UpdateTimeLabel();
        }

        private void UpdateTimeLabel()
        {
            int minutes = _remainingSeconds / 60;
            int seconds = _remainingSeconds % 60;
            testNavBar.SetTimeText($"{minutes:D2}:{seconds:D2} minutes remaining");
        }

        private void ShowPart(int index)
        {
            if (index < 0 || index >= _parts.Count)
                return;

            // Lưu câu trả lời đang hiển thị trước khi chuyển part
            SaveAnswersFromCurrentView();

            _currentPartIndex = index;
            var part = _parts[_currentPartIndex];

            // Hiển thị passage
            pdfViewer.DisplayPart(part);

            // Hiển thị câu hỏi & điền lại các câu đã trả lời
            answerPanel.LoadPart(part, _userAnswers);

            // Cập nhật highlight footer
            testFooter.SetActivePart(part.PartName);
        }

        private void SaveAnswersFromCurrentView()
        {
            // merge answers từ view vào dictionary
            var partAnswers = answerPanel.CollectAnswers();
            foreach (var kv in partAnswers)
            {
                _userAnswers[kv.Key] = kv.Value;
            }
        }

        private void TestNavBar_OnExitRequested()
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to exit this test?",
                "Exit Test",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.Yes)
            {
                _timer.Stop();

                this.Hide();  // ẨN form ReadingTest, không đóng app

                var library = new Edumination.WinForms.UI.Forms.TestLibrary.TestLibrary();
                library.Show();
            }
        }

        private void TestNavBar_OnSubmitRequested()
        {
            var result = MessageBox.Show(
                "Do you want to submit your answers now?",
                "Submit test",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _timer.Stop();
                SubmitTest();

                this.Hide();  // ẨN form ReadingTest, không đóng app

                var library = new Edumination.WinForms.UI.Forms.TestLibrary.TestLibrary();
                library.Show();
            }
        }

        private void TestFooter_OnPartSelected(string partName)
        {
            int index = _parts.FindIndex(p => p.PartName == partName);
            if (index >= 0)
            {
                ShowPart(index);
            }
        }

        private void SubmitTest()
        {
            SaveAnswersFromCurrentView();

            var allQuestions = _parts.SelectMany(p => p.Questions).ToList();
            int total = allQuestions.Count;
            int correct = 0;

            foreach (var q in allQuestions)
            {
                _userAnswers.TryGetValue(q.Number, out var ansRaw);
                string userAns = (ansRaw ?? "").Trim().ToLower();
                string correctAns = (q.CorrectAnswer ?? "").Trim().ToLower();

                if (!string.IsNullOrEmpty(correctAns) && userAns == correctAns)
                    correct++;
            }

            int timeTaken = ReadingMockData.TotalTimeSeconds - _remainingSeconds;

            // Build ExamResult
            var exam = new Edumination.WinForms.UI.Forms.Results.ExamResult
            {
                Skill = "Reading",
                UserName = "Tran Dung",            // TODO: lấy từ Session nếu có
                AvatarPath = null,                 // hoặc đường dẫn avatar
                CorrectCount = correct,
                TotalQuestions = total,
                TimeTakenSeconds = timeTaken
            };

            // Build Parts
            foreach (var part in _parts)
            {
                var partReview = new Edumination.WinForms.UI.Forms.Results.PartReview
                {
                    PartName = part.PartName
                };

                foreach (var q in part.Questions)
                {
                    _userAnswers.TryGetValue(q.Number, out var ansRaw);
                    var review = new Edumination.WinForms.UI.Forms.Results.QuestionReview
                    {
                        Number = q.Number,
                        PartName = part.PartName,
                        CorrectAnswer = q.CorrectAnswer,
                        UserAnswer = ansRaw ?? "",
                        IsCorrect = string.Equals(
                            (ansRaw ?? "").Trim(),
                            (q.CorrectAnswer ?? "").Trim(),
                            StringComparison.OrdinalIgnoreCase)
                    };
                    partReview.Questions.Add(review);
                }

                exam.Parts.Add(partReview);
            }

            // Mở form kết quả
            var resultForm = new Edumination.WinForms.UI.Forms.Results.AnswerResultForm(exam);
            resultForm.Show();

            // Ẩn / đóng ReadingTest
            this.Hide();
        }
    }
}
