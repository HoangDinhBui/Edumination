using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace IELTS.UI.User.TestTaking.WritingTest
{
    public partial class WritingTest : Form
    {
        private readonly List<WritingTask> _tasks;
        private int _currentTaskIndex = 0;

        private int _remainingSeconds;

        // FIX: chỉ định rõ Timer để tránh ambiguous
        private readonly System.Windows.Forms.Timer _timer;

        // Lưu bài theo từng PartName (Task 1 / Task 2 ...)
        private readonly Dictionary<string, string> _userEssays = new();

        public WritingTest()
        {
            InitializeComponent();

            WindowState = FormWindowState.Maximized;

            // Load mockdata
            _tasks = WritingMockData.GetTasks();
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

        private void SubmitTest()
        {
            SaveCurrentEssay();

            // MOCK SCORING: đếm số từ
            int totalWords = _userEssays.Values
                .Select(text => (text ?? "")
                .Split(' ', '\n', '\r', '\t')
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .Count())
                .Sum();

            MessageBox.Show(
                $"You have written approximately {totalWords} words across all tasks.\n(Mock scoring – no band given.)",
                "Writing Submitted",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
