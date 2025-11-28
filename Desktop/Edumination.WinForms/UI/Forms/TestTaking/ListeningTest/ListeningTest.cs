using Edumination.WinForms.UI.Forms.TestTaking.ReadingTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.TestTaking.ListeningTest
{
    public partial class ListeningTest : Form
    {
        private readonly List<ReadingPart> _parts;
        private long _sectionId;
        private int _currentPartIndex = 0;
        private int _remainingSeconds;

        private readonly System.Windows.Forms.Timer _timer;
        private readonly Dictionary<int, string> _userAnswers = new();

     
        public ListeningTest(long sectionId)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            _sectionId = sectionId;

            // Load real data from BLL
            var passageBll = new IELTS.BLL.PassageBLL();
            var questionBll = new IELTS.BLL.QuestionBLL();
            _parts = new List<ReadingPart>();

            // Load passages for this section
            var passageTable = passageBll.GetPassagesBySectionId(sectionId);
            if (passageTable.Rows.Count == 0)
            {
                MessageBox.Show("No passages found for this section.");
            }
            foreach (DataRow pRow in passageTable.Rows)
            {
                var part = new ReadingPart
                {
                    PartId = Convert.ToInt32(pRow["Id"]),
                    PartName = "Passage " + pRow["Position"].ToString(),
                    PassageTitle = pRow["Title"].ToString(),
                    PassageText = pRow["ContentText"].ToString()
                };
                // Get questions for this passage
                var questionsTable = questionBll.GetQuestionsByPassageId(part.PartId);
                foreach (DataRow qRow in questionsTable.Rows)
                {
                    var question = new ReadingQuestion
                    {
                        Number = Convert.ToInt32(qRow["Id"]),
                        Prompt = qRow["QuestionText"].ToString(),
                        Type = qRow["QuestionType"].ToString() == "MCQ" ? QuestionType.ShortAnswer : QuestionType.TrueFalse,
                        CorrectAnswer = GetCorrectAnswer(qRow["Id"]),
                        Choices = new List<string>()
                    };
                    if (qRow["QuestionType"].ToString() == "MCQ")
                    {
                        var choiceBll = new IELTS.BLL.QuestionBLL();
                        var choicesTable = choiceBll.GetChoicesByQuestionId(question.Number);
                        foreach (DataRow cRow in choicesTable.Rows)
                        {
                            question.Choices.Add(cRow["ChoiceText"].ToString());
                        }
                    }
                    part.Questions.Add(question);
                }
                _parts.Add(part);
            }

            _remainingSeconds = 40 * 60; // Default 40 min, or get from section info

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
        }

        private string GetCorrectAnswer(object questionId)
        {
            var choiceBll = new IELTS.BLL.QuestionChoiceBLL();
            var choices = choiceBll.GetChoicesByQuestionId(Convert.ToInt64(questionId));
            foreach (DataRow cRow in choices.Rows)
            {
                if (Convert.ToBoolean(cRow["IsCorrect"]))
                    return cRow["ChoiceText"].ToString();
            }
            return "";
        }

            _remainingSeconds = 40 * 60; // Default 40 min, or get from section info

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
                new Edumination.WinForms.UI.Forms.TestLibrary.TestLibrary().Show();
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
                new Edumination.WinForms.UI.Forms.TestLibrary.TestLibrary().Show();
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
                new Edumination.WinForms.UI.Forms.TestLibrary.TestLibrary().Show();
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

            var exam = new Edumination.WinForms.UI.Forms.Results.ExamResult
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

            var resultForm = new Edumination.WinForms.UI.Forms.Results.AnswerResultForm(exam);
            resultForm.Show();

            this.Hide();
        }
    }
}
