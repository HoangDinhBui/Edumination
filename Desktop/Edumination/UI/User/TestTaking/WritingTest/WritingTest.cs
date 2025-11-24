using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using IELTS.BLL;
using IELTS.UI.User.TestTaking.Controls;

namespace IELTS.UI.User.TestTaking.WritingTest
{
    public partial class WritingTest : Form
    {
        private List<WritingTask> _tasks;
        private int _currentTaskIndex = 0;

        private int _remainingSeconds;

        // FIX: chỉ định rõ Timer để tránh ambiguous
        private readonly System.Windows.Forms.Timer _timer;

        // Lưu bài theo từng PartName (Task 1 / Task 2 ...)
        private readonly Dictionary<string, string> _userEssays = new();

        private readonly long _sectionId;
        private readonly QuestionBLL _questionBLL = new QuestionBLL();

        public WritingTest(long sectionId)
        {
            InitializeComponent();

            _sectionId = sectionId;
            LoadTaskFromDatabase();

            WindowState = FormWindowState.Maximized;


            _remainingSeconds = WritingMockData.TotalTimeSeconds;

            // FIX: dùng Forms.Timer
            _timer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            _timer.Tick += Timer_Tick;
        }

        private void WritingTest_Load(object sender, EventArgs e)
        {
            // Điều hướng nav
            testNavBar.OnExitRequested += TestNavBar_OnExitRequested;
            testNavBar.OnSubmitRequested += TestNavBar_OnSubmitRequested;

            // Footer
            testFooter.OnPartSelected += TestFooter_OnPartSelected;
            testFooter.LoadParts(_tasks.Select(t => t.PartName));

            // Hiển thị Task đầu
            ShowTask(0);

            UpdateTimeLabel();
            _timer.Start();
        }

        // ============================
        // TIMER
        // ============================
        private void Timer_Tick(object sender, EventArgs e)
        {
            _remainingSeconds--;
            if (_remainingSeconds <= 0)
            {
                _timer.Stop();
                UpdateTimeLabel();
                MessageBox.Show("Time is up! The writing test will be submitted.",
                    "Time up", MessageBoxButtons.OK, MessageBoxIcon.Information);

                SubmitTest();
                Hide();
                new IELTS.UI.User.TestLibrary.TestLibrary().Show();
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

        // ============================
        // HIỂN THỊ TASK
        // ============================
        private void ShowTask(int index)
        {
            if (index < 0 || index >= _tasks.Count)
                return;

            SaveCurrentEssay();

            _currentTaskIndex = index;
            var task = _tasks[index];

            promptPanel.DisplayTask(task); // hiển thị đề

            _userEssays.TryGetValue(task.PartName, out var essay);
            writingAnswerPanel.SetEssay(essay ?? "");

            testFooter.SetActivePart(task.PartName);
        }

        private void SaveCurrentEssay()
        {
            if (_currentTaskIndex < 0 || _currentTaskIndex >= _tasks.Count)
                return;

            var task = _tasks[_currentTaskIndex];
            _userEssays[task.PartName] = writingAnswerPanel.GetEssay();
        }

        private void TestFooter_OnPartSelected(string partName)
        {
            int index = _tasks.FindIndex(t => t.PartName == partName);
            if (index >= 0)
                ShowTask(index);
        }

        // ============================
        // EXIT
        // ============================
        private void TestNavBar_OnExitRequested()
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to exit this writing test?",
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

        // ============================
        // SUBMIT
        // ============================
        private void TestNavBar_OnSubmitRequested()
        {
            var confirm = MessageBox.Show(
                "Do you want to submit your essays now?",
                "Submit Test",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                _timer.Stop();
                SubmitTest();
                Hide();
                
            }
        }

        private async void SubmitTest()
        {
            SaveCurrentEssay();
            _timer.Stop();

            // Hiển thị loading đơn giản
            var loadingForm = new Form
            {
                Size = new Size(300, 100),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ControlBox = false,
                Text = "Please Wait"
            };
            var lbl = new Label
            {
                Text = "AI is grading your writing...\nThis may take a few seconds.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10)
            };
            loadingForm.Controls.Add(lbl);
            loadingForm.Show(); // Show non-modal để code tiếp tục chạy (nhưng await sẽ pause)
            // Thực tế nên dùng Task.Run để không block UI thread hoàn toàn nếu ShowDialog

            try
            {
                var groqService = new IELTS.BLL.GroqService();

                // Demo: Chấm bài của Task hiện tại
                // Trong thực tế nên chấm tất cả các task và tổng hợp lại
                var task = _tasks[_currentTaskIndex];
                string essay = "";
                _userEssays.TryGetValue(task.PartName, out essay);

                if (string.IsNullOrWhiteSpace(essay))
                {
                    loadingForm.Close();
                    MessageBox.Show("No essay content found to grade.", "Info");
                }
                else
                {
                    // Gọi AI
                    var result = await groqService.GradeWritingAsync(task.Prompt, essay);
                    
                    loadingForm.Close();

                    // Hiển thị kết quả
                    using (var resultForm = new WritingResultForm(result.BandScore, result.Feedback, result.Correction))
                    {
                        resultForm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                loadingForm.Close();
                MessageBox.Show($"Error grading essay: {ex.Message}\n\nPlease check your internet connection and GROQ_API_KEY in .env file.", "Grading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Sau khi xem kết quả xong thì thoát
            Hide();
            // Mở lại TestLibrary hoặc Home tùy logic
            // new IELTS.UI.User.TestLibrary.TestLibrary().Show(); 
        }

        private void LoadTaskFromDatabase()
            {
                var dt = _questionBLL.GetQuestionsBySectionId(_sectionId);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy đề Writing cho phần này.", "Error");
                    Close();
                return;
            }

            var row = dt.Rows[0];
            var task = new WritingTask
            {
                PartName = "Task 1",
                Title = "Writing Test",
                Prompt = row["QuestionText"].ToString()
            };
            _tasks = new List<WritingTask> { task };
            ShowTask(0);
        }
    }
}
