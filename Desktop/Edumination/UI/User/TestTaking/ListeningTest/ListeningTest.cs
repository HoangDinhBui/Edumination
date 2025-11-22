using IELTS.UI.User.TestTaking.ReadingTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.User.TestTaking.ListeningTest
{
    public partial class ListeningTest : Form
    {
        private readonly List<ReadingPart> _parts;
        private int _currentPartIndex = 0;
        private int _remainingSeconds;

        private readonly System.Windows.Forms.Timer _timer;
        private readonly Dictionary<int, string> _userAnswers = new();

     
        public ListeningTest()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            _parts = ListeningMockData.GetParts();
            _remainingSeconds = ListeningMockData.TotalTimeSeconds;

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
        }

        private void ListeningTest_Load(object sender, EventArgs e)
        {
            testNavBar.OnExitRequested += TestNavBar_OnExitRequested;
            testNavBar.OnSubmitRequested += TestNavBar_OnSubmitRequested;

            testFooter.OnPartSelected += TestFooter_OnPartSelected;
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
                MessageBox.Show("Time is up! The listening test will be submitted.",
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

        private void ShowPart(int index)
        {
            if (index < 0 || index >= _parts.Count)
                return;

            SaveAnswersFromCurrentView();

            _currentPartIndex = index;
            var part = _parts[_currentPartIndex];

            audioPanel.DisplayPart(part);
            answerPanel.LoadPart(part, _userAnswers);
            testFooter.SetActivePart(part.PartName);
        }

        private void SaveAnswersFromCurrentView()
        {
            var partAnswers = answerPanel.CollectAnswers();
            foreach (var kv in partAnswers)
                _userAnswers[kv.Key] = kv.Value;
        }

        private void TestNavBar_OnExitRequested()
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to exit this listening test?",
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

        private void TestFooter_OnPartSelected(string partName)
        {
            int index = _parts.FindIndex(p => p.PartName == partName);
            if (index >= 0)
                ShowPart(index);
        }

        private void SubmitTest()
        {
            SaveAnswersFromCurrentView(); // nếu có

            var allQuestions = _parts.SelectMany(p => p.Questions).ToList();
            int total = allQuestions.Count;
            int correct = 0;

            foreach (var q in allQuestions)
            {
                _userAnswers.TryGetValue(q.Number, out var ansRaw);
                string userAns = (ansRaw ?? "").Trim().ToUpperInvariant();
                string correctAns = (q.CorrectAnswer ?? "").Trim().ToUpperInvariant();

                if (!string.IsNullOrEmpty(correctAns) && userAns == correctAns)
                    correct++;
            }

            int timeTaken = ListeningMockData.TotalTimeSeconds - _remainingSeconds;

            var exam = new IELTS.UI.User.Results.ExamResult
            {
                Skill = "Listening",
                UserName = "Tran Dung",    // TODO
                AvatarPath = null,
                CorrectCount = correct,
                TotalQuestions = total,
                TimeTakenSeconds = timeTaken
            };

            foreach (var part in _parts)
            {
                var partReview = new IELTS.UI.User.Results.PartReview
                {
                    PartName = part.PartName
                };

                foreach (var q in part.Questions)
                {
                    _userAnswers.TryGetValue(q.Number, out var ansRaw);

                    var review = new IELTS.UI.User.Results.QuestionReview
                    {
                        Number = q.Number,
                        PartName = part.PartName,
                        QuestionText = q.Prompt, // nếu có
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

            var resultForm = new IELTS.UI.User.Results.AnswerResultForm(exam);
            resultForm.Show();

            this.Hide();
        }
    }
}
