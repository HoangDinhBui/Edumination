////using Edumination.WinForms.UI.Admin.TestManager;
////using IELTS.BLL;
////using IELTS.DTO;
////using System;
////using System.Collections.Generic;
////using System.Drawing;
////using System.Linq;
////using System.Windows.Forms;
////using static IELTS.BLL.QuestionBLL;

////namespace IELTS.UI.Admin.TestManager
////{
////    public partial class ShowQuestionControl : UserControl
////    {
////        private long _passageId;
////        private readonly QuestionService _service = new QuestionService();
////        private TestManagerControl _testManagerControl;
////        private List<QuestionDTO> _questions = new List<QuestionDTO>();
////        private Dictionary<long, string> _userAnswers = new Dictionary<long, string>(); // QuestionId -> Answer
////        private bool _isSubmitted = false;

////        // ===== PROPERTY =====
////        public long PassageId
////        {
////            get => _passageId;
////            set
////            {
////                _passageId = value;
////                LoadQuestions();
////            }
////        }

////        // ===== CONSTRUCTOR =====
////        public ShowQuestionControl()
////        {
////            InitializeComponent();

////            // Thêm sự kiện này để các câu hỏi tự giãn rộng khi bung to cửa sổ
////            this.Load += (s, e) => {
////                flpQuestions.Resize += FlpQuestions_Resize;
////            };
////        }

////        private void FlpQuestions_Resize(object sender, EventArgs e)
////        {
////            foreach (Panel pnl in flpQuestions.Controls.OfType<Panel>())
////            {
////                pnl.Width = flpQuestions.ClientSize.Width - 25;

////                foreach (Label lbl in pnl.Controls.OfType<Label>())
////                {
////                    lbl.MaximumSize = new Size(pnl.Width - 45, 0);
////                }

////                // Recalculate height
////                int bottom = 0;
////                foreach (Control c in pnl.Controls)
////                    bottom = Math.Max(bottom, c.Bottom);

////                pnl.Height = bottom + 20;
////            }
////        }


////        public ShowQuestionControl(TestManagerControl testManagerControl)
////        {
////            _testManagerControl = testManagerControl;
////            // flpQuestions


////            InitializeComponent();
////            this.Load += (s, e) => {
////                flpQuestions.Resize += FlpQuestions_Resize;
////            };
////        }

////        public ShowQuestionControl(long passageId) : this()
////        {
////            PassageId = passageId;
////        }

////        // ===== LOAD =====
////        private void LoadQuestions()
////        {
////            if (flpQuestions == null) return;

////            flpQuestions.SuspendLayout();
////            flpQuestions.Controls.Clear();
////            _userAnswers.Clear();
////            _isSubmitted = false;

////            _questions = _service.GetQuestionsByPassage(_passageId);

////            int questionNumber = 1;
////            foreach (var q in _questions)
////            {
////                flpQuestions.Controls.Add(RenderQuestion(q, questionNumber));
////                questionNumber++;
////            }

////            // Add Submit Button
////            AddSubmitButton();

////            flpQuestions.ResumeLayout();
////        }

////        // ===== RENDER QUESTION =====
////        private Control RenderQuestion(QuestionDTO q, int number)
////        {
////            int currentWidth = flpQuestions.ClientSize.Width - 40;
////            Panel pnlMain = new Panel
////            {
////                Width = flpQuestions.ClientSize.Width - 25,
////                AutoSize = false,
////                Height = 10, // sẽ set lại sau
////                Margin = new Padding(10, 10, 10, 20),
////                BackColor = Color.White,
////                Padding = new Padding(20),
////                Tag = q.Id
////            };

////            // ===== HEADER =====
////            Panel pnlHeader = new Panel
////            {

////                Height = 45,
////                BackColor = GetColorByType(q.QuestionType)
////            };

////            Label lblNumber = new Label
////            {
////                Text = $"Question {number}",
////                Font = new Font("Segoe UI", 12, FontStyle.Bold),
////                ForeColor = Color.White,
////                AutoSize = true,
////                Location = new Point(15, 12)
////            };

////            Label lblType = new Label
////            {
////                Text = GetTypeDisplayName(q.QuestionType),
////                Font = new Font("Segoe UI", 9),
////                ForeColor = Color.White,
////                AutoSize = true,
////                Location = new Point(150, 14)
////            };

////            Label lblPoints = new Label
////            {
////                Text = $"{q.Points} pts",
////                Font = new Font("Segoe UI", 10, FontStyle.Bold),
////                ForeColor = Color.White,
////                AutoSize = true
////            };
////            lblPoints.Location = new Point(pnlMain.Width - lblPoints.Width - 55, 12);

////            pnlHeader.Controls.AddRange(new Control[] { lblNumber, lblType, lblPoints });

////            // ===== QUESTION TEXT =====
////            Label lblQuestion = new Label
////            {
////                Text = q.QuestionText,
////                Font = new Font("Segoe UI", 10),
////                AutoSize = true,
////                MaximumSize = new Size(pnlMain.Width - 40, 0),

////                Padding = new Padding(0, 15, 0, 15)
////            };

////            // ===== ANSWER AREA =====
////            Panel pnlAnswer = new Panel
////            {
////                AutoSize = true,
////                   // ⭐
////                Padding = new Padding(0, 10, 0, 10)
////            };

////            switch (q.QuestionType)
////            {
////                case "MCQ":
////                    RenderMCQ(pnlAnswer, q);
////                    break;
////                case "MULTI_SELECT":
////                    RenderMulti(pnlAnswer, q);
////                    break;
////                case "MATCHING":
////                    RenderMatching(pnlAnswer, q);
////                    break;
////                case "ORDER":
////                    RenderOrder(pnlAnswer, q);
////                    break;
////                case "FILL_BLANK":
////                    RenderFillBlank(pnlAnswer, q);
////                    break;
////                case "SHORT_ANSWER":
////                    RenderShortAnswer(pnlAnswer, q);
////                    break;
////            }

////            // ===== RESULT LABEL (Hidden initially) =====
////            Label lblResult = new Label
////            {
////                Name = $"lblResult_{q.Id}",
////                AutoSize = true,
////                Font = new Font("Segoe UI", 10, FontStyle.Bold),
////                Padding = new Padding(0, 10, 0, 0),
////                Visible = false

////            };

////            // Add controls in reverse order (Dock.Top stacks from bottom)
////            pnlMain.Controls.Add(lblResult);
////            pnlMain.Controls.Add(pnlAnswer);
////            pnlMain.Controls.Add(lblQuestion);
////            pnlMain.Controls.Add(pnlHeader);

////            // Border
////            pnlMain.Paint += (s, e) =>
////            {
////                ControlPaint.DrawBorder(e.Graphics, pnlMain.ClientRectangle,
////                    Color.LightGray, 1, ButtonBorderStyle.Solid,
////                    Color.LightGray, 1, ButtonBorderStyle.Solid,
////                    Color.LightGray, 1, ButtonBorderStyle.Solid,
////                    Color.LightGray, 1, ButtonBorderStyle.Solid);
////            };

////            int bottom = 0;
////            foreach (Control c in pnlMain.Controls)
////            {
////                bottom = Math.Max(bottom, c.Bottom);
////            }

////            pnlMain.Height = bottom + 20;

////            return pnlMain;

////        }

////        // ===== MCQ =====
////        private void RenderMCQ(Panel parent, QuestionDTO q)
////        {
////            FlowLayoutPanel flp = new FlowLayoutPanel
////            {
////                FlowDirection = FlowDirection.TopDown,
////                AutoSize = true,
////                Dock = DockStyle.Top,   // ⭐
////                WrapContents = false
////            };

////            foreach (var opt in q.Options.OrderBy(o => o.OptionKey))
////            {
////                RadioButton rb = new RadioButton
////                {
////                    Text = $"{opt.OptionKey}. {opt.OptionText}",
////                    Font = new Font("Segoe UI", 10),
////                    AutoSize = true,
////                    Margin = new Padding(0, 5, 0, 5),
////                    Tag = new { QuestionId = q.Id, Answer = opt.OptionKey }
////                };

////                rb.CheckedChanged += (s, e) =>
////                {
////                    if (rb.Checked && !_isSubmitted)
////                    {
////                        dynamic tag = rb.Tag;
////                        _userAnswers[(long)tag.QuestionId] = tag.Answer;
////                    }
////                };

////                flp.Controls.Add(rb);
////            }

////            parent.Controls.Add(flp);
////        }

////        // ===== MULTI_SELECT =====
////        private void RenderMulti(Panel parent, QuestionDTO q)
////        {
////            FlowLayoutPanel flp = new FlowLayoutPanel
////            {
////                FlowDirection = FlowDirection.TopDown,
////                AutoSize = true,
////                Dock = DockStyle.Top,   // ⭐
////                WrapContents = false
////            };

////            foreach (var opt in q.Options.OrderBy(o => o.OptionKey))
////            {
////                CheckBox cb = new CheckBox
////                {
////                    Text = $"{opt.OptionKey}. {opt.OptionText}",
////                    Font = new Font("Segoe UI", 10),
////                    AutoSize = true,
////                    Margin = new Padding(0, 5, 0, 5),
////                    Tag = opt.OptionKey
////                };

////                cb.CheckedChanged += (s, e) =>
////                {
////                    if (!_isSubmitted)
////                        UpdateMultiAnswer(q.Id, flp);
////                };

////                flp.Controls.Add(cb);
////            }

////            parent.Controls.Add(flp);
////        }

////        private void UpdateMultiAnswer(long questionId, FlowLayoutPanel flp)
////        {
////            var selected = flp.Controls.OfType<CheckBox>()
////                .Where(cb => cb.Checked)
////                .Select(cb => cb.Tag.ToString())
////                .OrderBy(k => k)
////                .ToList();

////            if (selected.Any())
////                _userAnswers[questionId] = string.Join("", selected);
////            else
////                _userAnswers.Remove(questionId);
////        }

////        // ===== MATCHING =====
////        private void RenderMatching(Panel parent, QuestionDTO q)
////        {
////            TableLayoutPanel table = new TableLayoutPanel
////            {
////                ColumnCount = 3,
////                AutoSize = true,
////                Dock = DockStyle.Fill,
////                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
////            };

////            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
////            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
////            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));

////            // Header
////            table.Controls.Add(new Label
////            {
////                Text = "Items",
////                Font = new Font("Segoe UI", 10, FontStyle.Bold),
////                Dock = DockStyle.Fill,
////                TextAlign = ContentAlignment.MiddleLeft,
////                Padding = new Padding(5)
////            }, 0, 0);

////            table.Controls.Add(new Label
////            {
////                Text = "Match",
////                Font = new Font("Segoe UI", 10, FontStyle.Bold),
////                Dock = DockStyle.Fill,
////                TextAlign = ContentAlignment.MiddleCenter,
////                Padding = new Padding(5)
////            }, 1, 0);

////            table.Controls.Add(new Label
////            {
////                Text = "Choices",
////                Font = new Font("Segoe UI", 10, FontStyle.Bold),
////                Dock = DockStyle.Fill,
////                TextAlign = ContentAlignment.MiddleLeft,
////                Padding = new Padding(5)
////            }, 2, 0);

////            // Get left items and right choices
////            var leftItems = q.MatchPairs
////                .Where(m => string.IsNullOrEmpty(m.RightKey))
////                .OrderBy(m => m.LeftKey)
////                .ToList();

////            var rightChoices = q.Options.OrderBy(o => o.OptionKey).ToList();

////            int row = 1;
////            foreach (var item in leftItems)
////            {
////                // Left item
////                Label lblLeft = new Label
////                {
////                    Text = $"{item.LeftKey}. {item.LeftText}",
////                    Dock = DockStyle.Fill,
////                    Padding = new Padding(5),
////                    Font = new Font("Segoe UI", 9)
////                };

////                // ComboBox for matching
////                ComboBox cbo = new ComboBox
////                {
////                    DropDownStyle = ComboBoxStyle.DropDownList,
////                    Dock = DockStyle.Fill,
////                    Font = new Font("Segoe UI", 9),
////                    Tag = new { QuestionId = q.Id, LeftKey = item.LeftKey }
////                };

////                cbo.Items.Add("--");
////                foreach (var choice in rightChoices)
////                {
////                    cbo.Items.Add(choice.OptionKey);
////                }
////                cbo.SelectedIndex = 0;

////                cbo.SelectedIndexChanged += (s, e) =>
////                {
////                    if (!_isSubmitted)
////                        UpdateMatchingAnswer(q.Id, table);
////                };

////                // Right choice display
////                Label lblRight = new Label
////                {
////                    Text = row - 1 < rightChoices.Count ?
////                        $"{rightChoices[row - 1].OptionKey}. {rightChoices[row - 1].OptionText}" : "",
////                    Dock = DockStyle.Fill,
////                    Padding = new Padding(5),
////                    Font = new Font("Segoe UI", 9)
////                };

////                table.Controls.Add(lblLeft, 0, row);
////                table.Controls.Add(cbo, 1, row);
////                table.Controls.Add(lblRight, 2, row);

////                row++;
////            }

////            parent.Controls.Add(table);
////        }

////        private void UpdateMatchingAnswer(long questionId, TableLayoutPanel table)
////        {
////            List<string> matches = new List<string>();

////            for (int row = 1; row < table.RowCount; row++)
////            {
////                var cbo = table.GetControlFromPosition(1, row) as ComboBox;
////                if (cbo != null && cbo.SelectedIndex > 0)
////                {
////                    dynamic tag = cbo.Tag;
////                    matches.Add($"{tag.LeftKey}{cbo.SelectedItem}");
////                }
////            }

////            if (matches.Any())
////                _userAnswers[questionId] = string.Join(",", matches);
////            else
////                _userAnswers.Remove(questionId);
////        }

////        // ===== ORDER =====
////        private void RenderOrder(Panel parent, QuestionDTO q)
////        {
////            Label lblInstruction = new Label
////            {
////                Text = "Select the order number for each item:",
////                Font = new Font("Segoe UI", 9, FontStyle.Italic),
////                AutoSize = true,
////                Dock = DockStyle.Top,
////                Padding = new Padding(0, 0, 0, 10)
////            };

////            TableLayoutPanel table = new TableLayoutPanel
////            {
////                ColumnCount = 2,
////                AutoSize = true,
////                Dock = DockStyle.Top,
////                Tag = q.Id
////            };

////            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
////            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

////            int max = q.Options.Count;

////            for (int i = 0; i < q.Options.Count; i++)
////            {
////                ComboBox cbo = new ComboBox
////                {
////                    DropDownStyle = ComboBoxStyle.DropDownList,
////                    Width = 60,
////                    Tag = q.Options[i].OptionKey
////                };

////                cbo.Items.Add("--");
////                for (int n = 1; n <= max; n++)
////                    cbo.Items.Add(n);
////                cbo.SelectedIndex = 0;

////                cbo.SelectedIndexChanged += (s, e) =>
////                {
////                    if (!_isSubmitted)
////                        UpdateOrderAnswer(q.Id, table);
////                };

////                table.Controls.Add(cbo, 0, i);

////                table.Controls.Add(new Label
////                {
////                    Text = $"{q.Options[i].OptionKey}. {q.Options[i].OptionText}",
////                    AutoSize = true,
////                    Dock = DockStyle.Fill,
////                    TextAlign = ContentAlignment.MiddleLeft,
////                    Padding = new Padding(5)
////                }, 1, i);
////            }

////            parent.Controls.Add(table);
////            parent.Controls.Add(lblInstruction);
////        }

////        private void UpdateOrderAnswer(long questionId, TableLayoutPanel table)
////        {
////            // Build answer string based on order selected
////            var orderMap = new Dictionary<int, string>(); // position -> optionKey

////            for (int row = 0; row < table.RowCount; row++)
////            {
////                var cbo = table.GetControlFromPosition(0, row) as ComboBox;
////                if (cbo != null && cbo.SelectedIndex > 0)
////                {
////                    int position = (int)cbo.SelectedItem;
////                    string optionKey = cbo.Tag.ToString();
////                    orderMap[position] = optionKey;
////                }
////            }

////            if (orderMap.Count == table.RowCount)
////            {
////                var orderedKeys = orderMap.OrderBy(kvp => kvp.Key)
////                    .Select(kvp => kvp.Value)
////                    .ToList();
////                _userAnswers[questionId] = string.Join("", orderedKeys);
////            }
////            else
////            {
////                _userAnswers.Remove(questionId);
////            }
////        }

////        // ===== FILL_BLANK =====
////        private void RenderFillBlank(Panel parent, QuestionDTO q)
////        {
////            int blankCount = q.AnswerData?.Split('|').Length ?? 1;

////            Label lblInstruction = new Label
////            {
////                Text = $"Fill in {blankCount} blank(s), separated by '|':",
////                Font = new Font("Segoe UI", 9, FontStyle.Italic),
////                AutoSize = true,
////                Dock = DockStyle.Top,
////                Padding = new Padding(0, 0, 0, 10)
////            };

////            TextBox txt = new TextBox
////            {
////                Width = 500,
////                Font = new Font("Segoe UI", 10),
////                Dock = DockStyle.Top,
////                Tag = q.Id
////            };

////            if (blankCount > 1)
////                txt.PlaceholderText = "answer1|answer2|answer3";

////            txt.TextChanged += (s, e) =>
////            {
////                if (!_isSubmitted)
////                {
////                    if (!string.IsNullOrWhiteSpace(txt.Text))
////                        _userAnswers[(long)txt.Tag] = txt.Text.Trim();
////                    else
////                        _userAnswers.Remove((long)txt.Tag);
////                }
////            };

////            parent.Controls.Add(txt);
////            parent.Controls.Add(lblInstruction);
////        }

////        // ===== SHORT_ANSWER =====
////        private void RenderShortAnswer(Panel parent, QuestionDTO q)
////        {
////            Label lblInstruction = new Label
////            {
////                Text = "Enter your answer:",
////                Font = new Font("Segoe UI", 9, FontStyle.Italic),
////                AutoSize = true,
////                Dock = DockStyle.Top,
////                Padding = new Padding(0, 0, 0, 5)
////            };

////            TextBox txt = new TextBox
////            {
////                Width = 500,
////                Height = 80,
////                Multiline = true,
////                Font = new Font("Segoe UI", 10),
////                Dock = DockStyle.Top,
////                Tag = q.Id,
////                ScrollBars = ScrollBars.Vertical
////            };

////            txt.TextChanged += (s, e) =>
////            {
////                if (!_isSubmitted)
////                {
////                    if (!string.IsNullOrWhiteSpace(txt.Text))
////                        _userAnswers[(long)txt.Tag] = txt.Text.Trim();
////                    else
////                        _userAnswers.Remove((long)txt.Tag);
////                }
////            };

////            parent.Controls.Add(txt);
////            parent.Controls.Add(lblInstruction);
////        }

////        // ===== SUBMIT BUTTON =====
////        private void AddSubmitButton()
////        {
////            Panel pnlSubmit = new Panel
////            {
////                Width = flpQuestions.Width - 40,
////                Height = 80,
////                Margin = new Padding(10)
////            };

////            Button btnSubmit = new Button
////            {
////                Text = "Submit Test",
////                Width = 200,
////                Height = 50,
////                Font = new Font("Segoe UI", 12, FontStyle.Bold),
////                BackColor = Color.FromArgb(0, 122, 204),
////                ForeColor = Color.White,
////                FlatStyle = FlatStyle.Flat,
////                Cursor = Cursors.Hand
////            };
////            btnSubmit.Location = new Point((pnlSubmit.Width - btnSubmit.Width) / 2, 25);
////            btnSubmit.Anchor = AnchorStyles.None; // Để nó không bị kéo dãn theo panel

////            btnSubmit.FlatAppearance.BorderSize = 0;

////            btnSubmit.Click += BtnSubmit_Click;

////            pnlSubmit.Controls.Add(btnSubmit);
////            flpQuestions.Controls.Add(pnlSubmit);
////        }

////        // ===== SUBMIT & GRADING =====
////        private void BtnSubmit_Click(object sender, EventArgs e)
////        {
////            if (_isSubmitted)
////            {
////                MessageBox.Show("Test already submitted!", "Info",
////                    MessageBoxButtons.OK, MessageBoxIcon.Information);
////                return;
////            }

////            var result = MessageBox.Show("Are you sure you want to submit your test?",
////                "Confirm Submission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

////            if (result != DialogResult.Yes) return;

////            _isSubmitted = true;
////            ((Button)sender).Enabled = false;

////            GradeTest();
////        }

////        private void GradeTest()
////        {
////            int correctCount = 0;
////            int totalQuestions = _questions.Count;
////            decimal totalScore = 0;
////            decimal maxScore = 0;

////            for (int i = 0; i < _questions.Count; i++)
////            {
////                var question = _questions[i];
////                maxScore += question.Points;

////                bool isCorrect = CheckAnswer(question);
////                if (isCorrect)
////                {
////                    correctCount++;
////                    totalScore += question.Points;
////                }

////                ShowQuestionResult(question, isCorrect);
////            }

////            // Show summary
////            string summary = $"Test Completed!\n\n" +
////                           $"Correct Answers: {correctCount}/{totalQuestions}\n" +
////                           $"Score: {totalScore:F2}/{maxScore:F2} points\n" +
////                           $"Percentage: {(maxScore > 0 ? (totalScore / maxScore * 100) : 0):F1}%";

////            MessageBox.Show(summary, "Test Results",
////                MessageBoxButtons.OK, MessageBoxIcon.Information);
////        }

////        private bool CheckAnswer(QuestionDTO question)
////        {
////            if (!_userAnswers.ContainsKey(question.Id))
////                return false;

////            string userAnswer = _userAnswers[question.Id].Trim().ToUpper();
////            string correctAnswer = question.AnswerData?.Trim().ToUpper() ?? "";

////            switch (question.QuestionType)
////            {
////                case "MCQ":
////                case "ORDER":
////                    return userAnswer == correctAnswer;

////                case "MULTI_SELECT":
////                    var userChars = userAnswer.OrderBy(c => c).ToArray();
////                    var correctChars = correctAnswer.OrderBy(c => c).ToArray();
////                    return new string(userChars) == new string(correctChars);

////                case "MATCHING":
////                    var userMatches = userAnswer.Split(',').OrderBy(m => m).ToList();
////                    var correctMatches = correctAnswer.Split(',').OrderBy(m => m).ToList();
////                    return string.Join(",", userMatches) == string.Join(",", correctMatches);

////                case "FILL_BLANK":
////                    var userBlanks = userAnswer.Split('|').Select(b => b.Trim()).ToArray();
////                    var correctBlanks = correctAnswer.Split('|').Select(b => b.Trim()).ToArray();

////                    if (userBlanks.Length != correctBlanks.Length)
////                        return false;

////                    for (int i = 0; i < userBlanks.Length; i++)
////                    {
////                        if (userBlanks[i] != correctBlanks[i])
////                            return false;
////                    }
////                    return true;

////                case "SHORT_ANSWER":
////                    return userAnswer.Contains(correctAnswer) ||
////                           correctAnswer.Contains(userAnswer);

////                default:
////                    return false;
////            }
////        }

////        private void ShowQuestionResult(QuestionDTO question, bool isCorrect)
////        {
////            var questionPanel = flpQuestions.Controls.OfType<Panel>()
////                .FirstOrDefault(p => (long)p.Tag == question.Id);

////            if (questionPanel == null) return;

////            var lblResult = questionPanel.Controls.OfType<Label>()
////                .FirstOrDefault(l => l.Name == $"lblResult_{question.Id}");

////            if (lblResult != null)
////            {
////                string userAnswer = _userAnswers.ContainsKey(question.Id) ?
////                    _userAnswers[question.Id] : "(Not answered)";

////                lblResult.Text = $"Your Answer: {userAnswer}\n" +
////                               $"Correct Answer: {question.AnswerKey}\n" +
////                               $"Result: {(isCorrect ? "✓ CORRECT" : "✗ WRONG")}";

////                lblResult.ForeColor = isCorrect ? Color.Green : Color.Red;
////                lblResult.Visible = true;
////            }
////        }

////        // ===== HELPER METHODS =====
////        private Color GetColorByType(string type)
////        {
////            return type switch
////            {
////                "MCQ" => Color.FromArgb(52, 152, 219),
////                "MULTI_SELECT" => Color.FromArgb(155, 89, 182),
////                "MATCHING" => Color.FromArgb(230, 126, 34),
////                "ORDER" => Color.FromArgb(231, 76, 60),
////                "FILL_BLANK" => Color.FromArgb(26, 188, 156),
////                "SHORT_ANSWER" => Color.FromArgb(46, 204, 113),
////                _ => Color.Gray
////            };
////        }

////        private string GetTypeDisplayName(string type)
////        {
////            return type switch
////            {
////                "MCQ" => "Multiple Choice",
////                "MULTI_SELECT" => "Multiple Select",
////                "MATCHING" => "Matching",
////                "ORDER" => "Ordering",
////                "FILL_BLANK" => "Fill in the Blanks",
////                "SHORT_ANSWER" => "Short Answer",
////                _ => type
////            };
////        }
////    }
////}

//using Edumination.WinForms.UI.Admin.TestManager;
//using IELTS.BLL;
//using IELTS.DTO;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using static IELTS.BLL.QuestionBLL;

//namespace IELTS.UI.Admin.TestManager
//{
//    public partial class ShowQuestionControl : UserControl
//    {
//        private long _passageId;
//        private readonly QuestionService _service = new QuestionService();
//        private TestManagerControl _testManagerControl;
//        private List<QuestionDTO> _questions = new List<QuestionDTO>();
//        private Dictionary<long, string> _userAnswers = new Dictionary<long, string>(); // QuestionId -> Answer
//        private bool _isSubmitted = false;

//        // ===== PROPERTY =====
//        public long PassageId
//        {
//            get => _passageId;
//            set
//            {
//                _passageId = value;
//                LoadQuestions();
//            }
//        }

//        // ===== CONSTRUCTOR =====
//        public ShowQuestionControl()
//        {
//            InitializeComponent();

//            // Thêm sự kiện này để các câu hỏi tự giãn rộng khi bung to cửa sổ
//            this.Load += (s, e) => {
//                flpQuestions.Resize += FlpQuestions_Resize;
//            };
//        }

//        private void FlpQuestions_Resize(object sender, EventArgs e)
//        {
//            foreach (Panel pnl in flpQuestions.Controls.OfType<Panel>())
//            {
//                pnl.Width = flpQuestions.ClientSize.Width - 25;

//                foreach (Label lbl in pnl.Controls.OfType<Label>())
//                {
//                    lbl.MaximumSize = new Size(pnl.Width - 45, 0);
//                }

//                // Recalculate height
//                int bottom = 0;
//                foreach (Control c in pnl.Controls)
//                    bottom = Math.Max(bottom, c.Bottom);

//                pnl.Height = bottom + 20;
//            }
//        }


//        public ShowQuestionControl(TestManagerControl testManagerControl)
//        {
//            _testManagerControl = testManagerControl;
//            // flpQuestions


//            InitializeComponent();
//            this.Load += (s, e) => {
//                flpQuestions.Resize += FlpQuestions_Resize;
//            };
//        }

//        public ShowQuestionControl(long passageId) : this()
//        {
//            PassageId = passageId;
//        }

//        // ===== LOAD =====
//        private void LoadQuestions()
//        {
//            if (flpQuestions == null) return;

//            flpQuestions.SuspendLayout();
//            flpQuestions.Controls.Clear();
//            _userAnswers.Clear();
//            _isSubmitted = false;

//            _questions = _service.GetQuestionsByPassage(_passageId);

//            int questionNumber = 1;
//            foreach (var q in _questions)
//            {
//                flpQuestions.Controls.Add(RenderQuestion(q, questionNumber));
//                questionNumber++;
//            }

//            // Add Submit Button
//            AddSubmitButton();

//            flpQuestions.ResumeLayout();
//        }

//        // ===== RENDER QUESTION =====
//        private Control RenderQuestion(QuestionDTO q, int number)
//        {
//            int currentWidth = flpQuestions.ClientSize.Width - 40;
//            Panel pnlMain = new Panel
//            {
//                Width = flpQuestions.ClientSize.Width - 25,
//                AutoSize = false,
//                Height = 10, // sẽ set lại sau
//                Margin = new Padding(10, 10, 10, 20),
//                BackColor = Color.White,
//                Padding = new Padding(20),
//                Tag = q.Id
//            };

//            // ===== HEADER =====
//            Panel pnlHeader = new Panel
//            {

//                Height = 45,
//                BackColor = GetColorByType(q.QuestionType)
//            };

//            Label lblNumber = new Label
//            {
//                Text = $"Question {number}",
//                Font = new Font("Segoe UI", 12, FontStyle.Bold),
//                ForeColor = Color.White,
//                AutoSize = true,
//                Location = new Point(15, 12)
//            };

//            Label lblType = new Label
//            {
//                Text = GetTypeDisplayName(q.QuestionType),
//                Font = new Font("Segoe UI", 9),
//                ForeColor = Color.White,
//                AutoSize = true,
//                Location = new Point(150, 14)
//            };

//            Label lblPoints = new Label
//            {
//                Text = $"{q.Points} pts",
//                Font = new Font("Segoe UI", 10, FontStyle.Bold),
//                ForeColor = Color.White,
//                AutoSize = true
//            };
//            lblPoints.Location = new Point(pnlMain.Width - lblPoints.Width - 55, 12);

//            pnlHeader.Controls.AddRange(new Control[] { lblNumber, lblType, lblPoints });

//            // ===== QUESTION TEXT =====
//            Label lblQuestion = new Label
//            {
//                Text = q.QuestionText,
//                Font = new Font("Segoe UI", 10),
//                AutoSize = true,
//                MaximumSize = new Size(pnlMain.Width - 40, 0),

//                Padding = new Padding(0, 15, 0, 15)
//            };

//            // ===== ANSWER AREA =====
//            Panel pnlAnswer = new Panel
//            {
//                AutoSize = true,
//                // ⭐
//                Padding = new Padding(0, 10, 0, 10)
//            };

//            switch (q.QuestionType)
//            {
//                case "MCQ":
//                    RenderMCQ(pnlAnswer, q);
//                    break;
//                case "MULTI_SELECT":
//                    RenderMulti(pnlAnswer, q);
//                    break;
//                case "MATCHING":
//                    RenderMatching(pnlAnswer, q);
//                    break;
//                case "ORDER":
//                    RenderOrder(pnlAnswer, q);
//                    break;
//                case "FILL_BLANK":
//                    RenderFillBlank(pnlAnswer, q);
//                    break;
//                case "SHORT_ANSWER":
//                    RenderShortAnswer(pnlAnswer, q);
//                    break;
//            }

//            // ===== RESULT LABEL (Hidden initially) =====
//            Label lblResult = new Label
//            {
//                Name = $"lblResult_{q.Id}",
//                AutoSize = true,
//                Font = new Font("Segoe UI", 10, FontStyle.Bold),
//                Padding = new Padding(0, 10, 0, 0),
//                Visible = false

//            };

//            // Add controls in reverse order (Dock.Top stacks from bottom)
//            pnlMain.Controls.Add(lblResult);
//            pnlMain.Controls.Add(pnlAnswer);
//            pnlMain.Controls.Add(lblQuestion);
//            pnlMain.Controls.Add(pnlHeader);

//            // Border
//            pnlMain.Paint += (s, e) =>
//            {
//                ControlPaint.DrawBorder(e.Graphics, pnlMain.ClientRectangle,
//                    Color.LightGray, 1, ButtonBorderStyle.Solid,
//                    Color.LightGray, 1, ButtonBorderStyle.Solid,
//                    Color.LightGray, 1, ButtonBorderStyle.Solid,
//                    Color.LightGray, 1, ButtonBorderStyle.Solid);
//            };

//            int bottom = 0;
//            foreach (Control c in pnlMain.Controls)
//            {
//                bottom = Math.Max(bottom, c.Bottom);
//            }

//            pnlMain.Height = bottom + 20;

//            return pnlMain;

//        }

//        // ===== MCQ =====
//        private void RenderMCQ(Panel parent, QuestionDTO q)
//        {
//            FlowLayoutPanel flp = new FlowLayoutPanel
//            {
//                FlowDirection = FlowDirection.TopDown,
//                AutoSize = true,
//                Dock = DockStyle.Top,   // ⭐
//                WrapContents = false
//            };

//            foreach (var opt in q.Options.OrderBy(o => o.OptionKey))
//            {
//                RadioButton rb = new RadioButton
//                {
//                    Text = $"{opt.OptionKey}. {opt.OptionText}",
//                    Font = new Font("Segoe UI", 10),
//                    AutoSize = true,
//                    Margin = new Padding(0, 5, 0, 5),
//                    Tag = new { QuestionId = q.Id, Answer = opt.OptionKey }
//                };

//                rb.CheckedChanged += (s, e) =>
//                {
//                    if (rb.Checked && !_isSubmitted)
//                    {
//                        dynamic tag = rb.Tag;
//                        _userAnswers[(long)tag.QuestionId] = tag.Answer;
//                    }
//                };

//                flp.Controls.Add(rb);
//            }

//            parent.Controls.Add(flp);
//        }

//        // ===== MULTI_SELECT =====
//        private void RenderMulti(Panel parent, QuestionDTO q)
//        {
//            FlowLayoutPanel flp = new FlowLayoutPanel
//            {
//                FlowDirection = FlowDirection.TopDown,
//                AutoSize = true,
//                Dock = DockStyle.Top,   // ⭐
//                WrapContents = false
//            };

//            foreach (var opt in q.Options.OrderBy(o => o.OptionKey))
//            {
//                CheckBox cb = new CheckBox
//                {
//                    Text = $"{opt.OptionKey}. {opt.OptionText}",
//                    Font = new Font("Segoe UI", 10),
//                    AutoSize = true,
//                    Margin = new Padding(0, 5, 0, 5),
//                    Tag = opt.OptionKey
//                };

//                cb.CheckedChanged += (s, e) =>
//                {
//                    if (!_isSubmitted)
//                        UpdateMultiAnswer(q.Id, flp);
//                };

//                flp.Controls.Add(cb);
//            }

//            parent.Controls.Add(flp);
//        }

//        private void UpdateMultiAnswer(long questionId, FlowLayoutPanel flp)
//        {
//            var selected = flp.Controls.OfType<CheckBox>()
//                .Where(cb => cb.Checked)
//                .Select(cb => cb.Tag.ToString())
//                .OrderBy(k => k)
//                .ToList();

//            if (selected.Any())
//                _userAnswers[questionId] = string.Join("", selected);
//            else
//                _userAnswers.Remove(questionId);
//        }

//        // ===== MATCHING =====
//        private void RenderMatching(Panel parent, QuestionDTO q)
//        {
//            TableLayoutPanel table = new TableLayoutPanel
//            {
//                ColumnCount = 3,
//                AutoSize = true,
//                Dock = DockStyle.Fill,
//                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
//            };

//            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
//            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
//            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));

//            // Header
//            table.Controls.Add(new Label
//            {
//                Text = "Items",
//                Font = new Font("Segoe UI", 10, FontStyle.Bold),
//                Dock = DockStyle.Fill,
//                TextAlign = ContentAlignment.MiddleLeft,
//                Padding = new Padding(5)
//            }, 0, 0);

//            table.Controls.Add(new Label
//            {
//                Text = "Match",
//                Font = new Font("Segoe UI", 10, FontStyle.Bold),
//                Dock = DockStyle.Fill,
//                TextAlign = ContentAlignment.MiddleCenter,
//                Padding = new Padding(5)
//            }, 1, 0);

//            table.Controls.Add(new Label
//            {
//                Text = "Choices",
//                Font = new Font("Segoe UI", 10, FontStyle.Bold),
//                Dock = DockStyle.Fill,
//                TextAlign = ContentAlignment.MiddleLeft,
//                Padding = new Padding(5)
//            }, 2, 0);

//            // Get left items and right choices
//            var leftItems = q.MatchPairs
//                .Where(m => string.IsNullOrEmpty(m.RightKey))
//                .OrderBy(m => m.LeftKey)
//                .ToList();

//            var rightChoices = q.Options.OrderBy(o => o.OptionKey).ToList();

//            int row = 1;
//            foreach (var item in leftItems)
//            {
//                // Left item
//                Label lblLeft = new Label
//                {
//                    Text = $"{item.LeftKey}. {item.LeftText}",
//                    Dock = DockStyle.Fill,
//                    Padding = new Padding(5),
//                    Font = new Font("Segoe UI", 9)
//                };

//                // ComboBox for matching
//                ComboBox cbo = new ComboBox
//                {
//                    DropDownStyle = ComboBoxStyle.DropDownList,
//                    Dock = DockStyle.Fill,
//                    Font = new Font("Segoe UI", 9),
//                    Tag = new { QuestionId = q.Id, LeftKey = item.LeftKey }
//                };

//                cbo.Items.Add("--");
//                foreach (var choice in rightChoices)
//                {
//                    cbo.Items.Add(choice.OptionKey);
//                }
//                cbo.SelectedIndex = 0;

//                cbo.SelectedIndexChanged += (s, e) =>
//                {
//                    if (!_isSubmitted)
//                        UpdateMatchingAnswer(q.Id, table);
//                };

//                // Right choice display
//                Label lblRight = new Label
//                {
//                    Text = row - 1 < rightChoices.Count ?
//                        $"{rightChoices[row - 1].OptionKey}. {rightChoices[row - 1].OptionText}" : "",
//                    Dock = DockStyle.Fill,
//                    Padding = new Padding(5),
//                    Font = new Font("Segoe UI", 9)
//                };

//                table.Controls.Add(lblLeft, 0, row);
//                table.Controls.Add(cbo, 1, row);
//                table.Controls.Add(lblRight, 2, row);

//                row++;
//            }

//            parent.Controls.Add(table);
//        }

//        private void UpdateMatchingAnswer(long questionId, TableLayoutPanel table)
//        {
//            List<string> matches = new List<string>();

//            for (int row = 1; row < table.RowCount; row++)
//            {
//                var cbo = table.GetControlFromPosition(1, row) as ComboBox;
//                if (cbo != null && cbo.SelectedIndex > 0)
//                {
//                    dynamic tag = cbo.Tag;
//                    matches.Add($"{tag.LeftKey}{cbo.SelectedItem}");
//                }
//            }

//            if (matches.Any())
//                _userAnswers[questionId] = string.Join(",", matches);
//            else
//                _userAnswers.Remove(questionId);
//        }

//        // ===== ORDER =====
//        private void RenderOrder(Panel parent, QuestionDTO q)
//        {
//            Label lblInstruction = new Label
//            {
//                Text = "Select the order number for each item:",
//                Font = new Font("Segoe UI", 9, FontStyle.Italic),
//                AutoSize = true,
//                Dock = DockStyle.Top,
//                Padding = new Padding(0, 0, 0, 10)
//            };

//            TableLayoutPanel table = new TableLayoutPanel
//            {
//                ColumnCount = 2,
//                AutoSize = true,
//                Dock = DockStyle.Top,
//                Tag = q.Id
//            };

//            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
//            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

//            int max = q.Options.Count;

//            for (int i = 0; i < q.Options.Count; i++)
//            {
//                ComboBox cbo = new ComboBox
//                {
//                    DropDownStyle = ComboBoxStyle.DropDownList,
//                    Width = 60,
//                    Tag = q.Options[i].OptionKey
//                };

//                cbo.Items.Add("--");
//                for (int n = 1; n <= max; n++)
//                    cbo.Items.Add(n);
//                cbo.SelectedIndex = 0;

//                cbo.SelectedIndexChanged += (s, e) =>
//                {
//                    if (!_isSubmitted)
//                        UpdateOrderAnswer(q.Id, table);
//                };

//                table.Controls.Add(cbo, 0, i);

//                table.Controls.Add(new Label
//                {
//                    Text = $"{q.Options[i].OptionKey}. {q.Options[i].OptionText}",
//                    AutoSize = true,
//                    Dock = DockStyle.Fill,
//                    TextAlign = ContentAlignment.MiddleLeft,
//                    Padding = new Padding(5)
//                }, 1, i);
//            }

//            parent.Controls.Add(table);
//            parent.Controls.Add(lblInstruction);
//        }

//        private void UpdateOrderAnswer(long questionId, TableLayoutPanel table)
//        {
//            // Build answer string based on order selected
//            var orderMap = new Dictionary<int, string>(); // position -> optionKey

//            for (int row = 0; row < table.RowCount; row++)
//            {
//                var cbo = table.GetControlFromPosition(0, row) as ComboBox;
//                if (cbo != null && cbo.SelectedIndex > 0)
//                {
//                    int position = (int)cbo.SelectedItem;
//                    string optionKey = cbo.Tag.ToString();
//                    orderMap[position] = optionKey;
//                }
//            }

//            if (orderMap.Count == table.RowCount)
//            {
//                var orderedKeys = orderMap.OrderBy(kvp => kvp.Key)
//                    .Select(kvp => kvp.Value)
//                    .ToList();
//                _userAnswers[questionId] = string.Join("", orderedKeys);
//            }
//            else
//            {
//                _userAnswers.Remove(questionId);
//            }
//        }

//        // ===== FILL_BLANK =====
//        private void RenderFillBlank(Panel parent, QuestionDTO q)
//        {
//            int blankCount = q.AnswerData?.Split('|').Length ?? 1;

//            Label lblInstruction = new Label
//            {
//                Text = $"Fill in {blankCount} blank(s), separated by '|':",
//                Font = new Font("Segoe UI", 9, FontStyle.Italic),
//                AutoSize = true,
//                Dock = DockStyle.Top,
//                Padding = new Padding(0, 0, 0, 10)
//            };

//            TextBox txt = new TextBox
//            {
//                Width = 500,
//                Font = new Font("Segoe UI", 10),
//                Dock = DockStyle.Top,
//                Tag = q.Id
//            };

//            if (blankCount > 1)
//                txt.PlaceholderText = "answer1|answer2|answer3";

//            txt.TextChanged += (s, e) =>
//            {
//                if (!_isSubmitted)
//                {
//                    if (!string.IsNullOrWhiteSpace(txt.Text))
//                        _userAnswers[(long)txt.Tag] = txt.Text.Trim();
//                    else
//                        _userAnswers.Remove((long)txt.Tag);
//                }
//            };

//            parent.Controls.Add(txt);
//            parent.Controls.Add(lblInstruction);
//        }

//        // ===== SHORT_ANSWER =====
//        private void RenderShortAnswer(Panel parent, QuestionDTO q)
//        {
//            Label lblInstruction = new Label
//            {
//                Text = "Enter your answer:",
//                Font = new Font("Segoe UI", 9, FontStyle.Italic),
//                AutoSize = true,
//                Dock = DockStyle.Top,
//                Padding = new Padding(0, 0, 0, 5)
//            };

//            TextBox txt = new TextBox
//            {
//                Width = 500,
//                Height = 80,
//                Multiline = true,
//                Font = new Font("Segoe UI", 10),
//                Dock = DockStyle.Top,
//                Tag = q.Id,
//                ScrollBars = ScrollBars.Vertical
//            };

//            txt.TextChanged += (s, e) =>
//            {
//                if (!_isSubmitted)
//                {
//                    if (!string.IsNullOrWhiteSpace(txt.Text))
//                        _userAnswers[(long)txt.Tag] = txt.Text.Trim();
//                    else
//                        _userAnswers.Remove((long)txt.Tag);
//                }
//            };

//            parent.Controls.Add(txt);
//            parent.Controls.Add(lblInstruction);
//        }

//        // ===== SUBMIT BUTTON =====
//        private void AddSubmitButton()
//        {
//            Panel pnlSubmit = new Panel
//            {
//                Width = flpQuestions.Width - 40,
//                Height = 80,
//                Margin = new Padding(10)
//            };

//            Button btnSubmit = new Button
//            {
//                Text = "Submit Test",
//                Width = 200,
//                Height = 50,
//                Font = new Font("Segoe UI", 12, FontStyle.Bold),
//                BackColor = Color.FromArgb(0, 122, 204),
//                ForeColor = Color.White,
//                FlatStyle = FlatStyle.Flat,
//                Cursor = Cursors.Hand
//            };
//            btnSubmit.Location = new Point((pnlSubmit.Width - btnSubmit.Width) / 2, 25);
//            btnSubmit.Anchor = AnchorStyles.None; // Để nó không bị kéo dãn theo panel

//            btnSubmit.FlatAppearance.BorderSize = 0;

//            btnSubmit.Click += BtnSubmit_Click;

//            pnlSubmit.Controls.Add(btnSubmit);
//            flpQuestions.Controls.Add(pnlSubmit);
//        }

//        // ===== SUBMIT & GRADING =====
//        private void BtnSubmit_Click(object sender, EventArgs e)
//        {
//            if (_isSubmitted)
//            {
//                MessageBox.Show("Test already submitted!", "Info",
//                    MessageBoxButtons.OK, MessageBoxIcon.Information);
//                return;
//            }

//            var result = MessageBox.Show("Are you sure you want to submit your test?",
//                "Confirm Submission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

//            if (result != DialogResult.Yes) return;

//            _isSubmitted = true;
//            ((Button)sender).Enabled = false;

//            GradeTest();
//        }

//        private void GradeTest()
//        {
//            int correctCount = 0;
//            int totalQuestions = _questions.Count;
//            decimal totalScore = 0;
//            decimal maxScore = 0;

//            for (int i = 0; i < _questions.Count; i++)
//            {
//                var question = _questions[i];
//                maxScore += question.Points;

//                bool isCorrect = CheckAnswer(question);
//                if (isCorrect)
//                {
//                    correctCount++;
//                    totalScore += question.Points;
//                }

//                ShowQuestionResult(question, isCorrect);
//            }

//            // Show summary
//            string summary = $"Test Completed!\n\n" +
//                           $"Correct Answers: {correctCount}/{totalQuestions}\n" +
//                           $"Score: {totalScore:F2}/{maxScore:F2} points\n" +
//                           $"Percentage: {(maxScore > 0 ? (totalScore / maxScore * 100) : 0):F1}%";

//            MessageBox.Show(summary, "Test Results",
//                MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }

//        private bool CheckAnswer(QuestionDTO question)
//        {
//            if (!_userAnswers.ContainsKey(question.Id))
//                return false;

//            string userAnswer = _userAnswers[question.Id].Trim().ToUpper();
//            string correctAnswer = question.AnswerData?.Trim().ToUpper() ?? "";

//            switch (question.QuestionType)
//            {
//                case "MCQ":
//                case "ORDER":
//                    return userAnswer == correctAnswer;

//                case "MULTI_SELECT":
//                    var userChars = userAnswer.OrderBy(c => c).ToArray();
//                    var correctChars = correctAnswer.OrderBy(c => c).ToArray();
//                    return new string(userChars) == new string(correctChars);

//                case "MATCHING":
//                    var userMatches = userAnswer.Split(',').OrderBy(m => m).ToList();
//                    var correctMatches = correctAnswer.Split(',').OrderBy(m => m).ToList();
//                    return string.Join(",", userMatches) == string.Join(",", correctMatches);

//                case "FILL_BLANK":
//                    var userBlanks = userAnswer.Split('|').Select(b => b.Trim()).ToArray();
//                    var correctBlanks = correctAnswer.Split('|').Select(b => b.Trim()).ToArray();

//                    if (userBlanks.Length != correctBlanks.Length)
//                        return false;

//                    for (int i = 0; i < userBlanks.Length; i++)
//                    {
//                        if (userBlanks[i] != correctBlanks[i])
//                            return false;
//                    }
//                    return true;

//                case "SHORT_ANSWER":
//                    return userAnswer.Contains(correctAnswer) ||
//                           correctAnswer.Contains(userAnswer);

//                default:
//                    return false;
//            }
//        }

//        private void ShowQuestionResult(QuestionDTO question, bool isCorrect)
//        {
//            var questionPanel = flpQuestions.Controls.OfType<Panel>()
//                .FirstOrDefault(p => (long)p.Tag == question.Id);

//            if (questionPanel == null) return;

//            var lblResult = questionPanel.Controls.OfType<Label>()
//                .FirstOrDefault(l => l.Name == $"lblResult_{question.Id}");

//            if (lblResult != null)
//            {
//                string userAnswer = _userAnswers.ContainsKey(question.Id) ?
//                    _userAnswers[question.Id] : "(Not answered)";

//                lblResult.Text = $"Your Answer: {userAnswer}\n" +
//                               $"Correct Answer: {question.AnswerKey}\n" +
//                               $"Result: {(isCorrect ? "✓ CORRECT" : "✗ WRONG")}";

//                lblResult.ForeColor = isCorrect ? Color.Green : Color.Red;
//                lblResult.Visible = true;
//            }
//        }

//        // ===== HELPER METHODS =====
//        private Color GetColorByType(string type)
//        {
//            return type switch
//            {
//                "MCQ" => Color.FromArgb(52, 152, 219),
//                "MULTI_SELECT" => Color.FromArgb(155, 89, 182),
//                "MATCHING" => Color.FromArgb(230, 126, 34),
//                "ORDER" => Color.FromArgb(231, 76, 60),
//                "FILL_BLANK" => Color.FromArgb(26, 188, 156),
//                "SHORT_ANSWER" => Color.FromArgb(46, 204, 113),
//                _ => Color.Gray
//            };
//        }

//        private string GetTypeDisplayName(string type)
//        {
//            return type switch
//            {
//                "MCQ" => "Multiple Choice",
//                "MULTI_SELECT" => "Multiple Select",
//                "MATCHING" => "Matching",
//                "ORDER" => "Ordering",
//                "FILL_BLANK" => "Fill in the Blanks",
//                "SHORT_ANSWER" => "Short Answer",
//                _ => type
//            };
//        }
//    }
//}

using IELTS.BLL;
using IELTS.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Edumination.WinForms.UI.Admin.TestManager;

namespace IELTS.UI.Admin.TestManager
{
    public partial class ShowQuestionControl : UserControl
    {
        private long _passageId;
        private readonly QuestionBLL _bll;
        private TestManagerControl _testManagerControl;
        private long? _selectedQuestionId = null;
        private int _selectedPosition = 0;
        private int _passagePosition;
        private PassageBLL _passageBll = new PassageBLL();

        public TestManagerControl TestManagerControl
        {
            get { return _testManagerControl; }
            set { _testManagerControl = value; }
        }

        public long PassageId
        {
            get => _passageId;
            set
            {
                _passageId = value;
                _passagePosition=_passageBll.GetPassagePosition(_passageId);
                LoadQuestionButtons();
            }
        }

        public ShowQuestionControl()
        {
            InitializeComponent();
            _bll = new QuestionBLL();
        }

        public ShowQuestionControl(TestManagerControl testManagerControl) : this()
        {
            _testManagerControl = testManagerControl;
        }

        public ShowQuestionControl(long passageId, int passagePosition) : this()
        {
            _passageId = passageId;
            _passagePosition = passagePosition;
            LoadQuestionButtons();
        }

        private void LoadQuestionButtons()
        {
            flpQuestionButtons.Controls.Clear();

            var statuses = _bll.GetQuestionStatuses(_passageId, _passagePosition);

            foreach (var item in statuses)
            {
                Button btn = new Button
                {
                    Text = item.Position.ToString(),
                    Width = 55,
                    Height = 55,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    BackColor = item.HasData ? Color.FromArgb(46, 204, 113) : Color.FromArgb(189, 195, 199),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Tag = item.Position,
                    Margin = new Padding(5)
                };

                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = item.HasData ?
                    Color.FromArgb(39, 174, 96) : Color.FromArgb(149, 165, 166);

                btn.Click += QuestionButton_Click;
                flpQuestionButtons.Controls.Add(btn);
            }

            // Show instruction if no question selected
            if (_selectedQuestionId == null)
            {
                ShowNoQuestionMessage();
            }
        }

        private void QuestionButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            _selectedPosition = (int)btn.Tag;

            // Highlight selected button
            foreach (Button b in flpQuestionButtons.Controls.OfType<Button>())
            {
                if (b == btn)
                {
                    b.Font = new Font("Segoe UI", 12, FontStyle.Bold | FontStyle.Underline);
                }
                else
                {
                    b.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                }
            }

            LoadQuestionContent();
        }

        private void LoadQuestionContent()
        {
            flpQuestion.Controls.Clear();

            var question = _bll.GetQuestion(_passageId, _selectedPosition);

            if (question == null)
            {
                _selectedQuestionId = null;
                ShowNoDataMessage();
                return;
            }

            _selectedQuestionId = question.Id;

            // Question Header Card
            Panel pnlQuestionHeader = new Panel
            {
                Width = flpQuestion.Width - 40,
                Height = 80,
                BackColor = GetTypeColor(question.QuestionType),
                Margin = new Padding(0, 0, 0, 15)
            };

            Label lblQNumber = new Label
            {
                Text = $"Question {question.Position}",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 15)
            };

            Label lblQType = new Label
            {
                Text = GetQuestionTypeDisplay(question.QuestionType),
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 45)
            };

            pnlQuestionHeader.Controls.AddRange(new Control[] { lblQNumber, lblQType });
            flpQuestion.Controls.Add(pnlQuestionHeader);

            // Question Text
            Panel pnlQuestionText = new Panel
            {
                Width = flpQuestion.Width - 40,
                AutoSize = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(236, 240, 241),
                Padding = new Padding(20),
                Margin = new Padding(0, 0, 0, 20)
            };

            Label lblQuestion = new Label
            {
                Text = question.QuestionText,
                Font = new Font("Segoe UI", 11),
                AutoSize = true,
                MaximumSize = new Size(pnlQuestionText.Width - 40, 0),
                ForeColor = Color.FromArgb(52, 73, 94)
            };

            pnlQuestionText.Controls.Add(lblQuestion);
            flpQuestion.Controls.Add(pnlQuestionText);

            // Answer Section
            Label lblAnswerHeader = new Label
            {
                Text = "📋 Answer(s):",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            };
            flpQuestion.Controls.Add(lblAnswerHeader);

            // Render answers based on type
            Panel pnlAnswers = new Panel
            {
                Width = flpQuestion.Width - 40,
                AutoSize = true,
                Padding = new Padding(10)
            };

            switch (question.QuestionType)
            {
                case "MCQ":
                    RenderMCQ(pnlAnswers, question.Id);
                    break;
                case "MULTI_CHOICES":
                    RenderMultiChoices(pnlAnswers, question.Id);
                    break;
                case "FILL_BLANK":
                    RenderFillBlank(pnlAnswers, question.Id);
                    break;
            }

            flpQuestion.Controls.Add(pnlAnswers);
        }

        private void RenderMCQ(Panel parent, long questionId)
        {
            var choices = _bll.GetChoices(questionId);

            int y = 5;
            foreach (var choice in choices)
            {
                Panel pnlChoice = new Panel
                {
                    Width = parent.Width - 20,
                    Height = 40,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = choice.IsCorrect ? Color.FromArgb(212, 239, 223) : Color.White,
                    Location = new Point(5, y),
                    Margin = new Padding(0, 0, 0, 5)
                };

                RadioButton rb = new RadioButton
                {
                    Checked = choice.IsCorrect,
                    Enabled = false,
                    Location = new Point(10, 10),
                    AutoSize = true
                };

                Label lblChoice = new Label
                {
                    Text = choice.ChoiceText,
                    Font = new Font("Segoe UI", 10, choice.IsCorrect ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = choice.IsCorrect ? Color.FromArgb(39, 174, 96) : Color.Black,
                    Location = new Point(35, 11),
                    AutoSize = true,
                    MaximumSize = new Size(pnlChoice.Width - 50, 0)
                };

                if (choice.IsCorrect)
                {
                    Label lblCorrect = new Label
                    {
                        Text = "✓",
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        ForeColor = Color.FromArgb(39, 174, 96),
                        Location = new Point(pnlChoice.Width - 35, 8),
                        AutoSize = true
                    };
                    pnlChoice.Controls.Add(lblCorrect);
                }

                pnlChoice.Controls.AddRange(new Control[] { rb, lblChoice });
                parent.Controls.Add(pnlChoice);
                y += 45;
            }

            parent.Height = y + 5;
        }

        private void RenderMultiChoices(Panel parent, long questionId)
        {
            var choices = _bll.GetChoices(questionId);

            int y = 5;
            foreach (var choice in choices)
            {
                Panel pnlChoice = new Panel
                {
                    Width = parent.Width - 20,
                    Height = 40,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = choice.IsCorrect ? Color.FromArgb(212, 239, 223) : Color.White,
                    Location = new Point(5, y),
                    Margin = new Padding(0, 0, 0, 5)
                };

                CheckBox cb = new CheckBox
                {
                    Checked = choice.IsCorrect,
                    Enabled = false,
                    Location = new Point(10, 10),
                    AutoSize = true
                };

                Label lblChoice = new Label
                {
                    Text = choice.ChoiceText,
                    Font = new Font("Segoe UI", 10, choice.IsCorrect ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = choice.IsCorrect ? Color.FromArgb(39, 174, 96) : Color.Black,
                    Location = new Point(35, 11),
                    AutoSize = true,
                    MaximumSize = new Size(pnlChoice.Width - 50, 0)
                };

                if (choice.IsCorrect)
                {
                    Label lblCorrect = new Label
                    {
                        Text = "✓",
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        ForeColor = Color.FromArgb(39, 174, 96),
                        Location = new Point(pnlChoice.Width - 35, 8),
                        AutoSize = true
                    };
                    pnlChoice.Controls.Add(lblCorrect);
                }

                pnlChoice.Controls.AddRange(new Control[] { cb, lblChoice });
                parent.Controls.Add(pnlChoice);
                y += 45;
            }

            parent.Height = y + 5;
        }

        private void RenderFillBlank(Panel parent, long questionId)
        {
            string answer = _bll.GetAnswerKey(questionId);

            Panel pnlAnswer = new Panel
            {
                Width = parent.Width - 20,
                Height = 60,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(212, 239, 223),
                Location = new Point(5, 5),
                Padding = new Padding(15)
            };

            Label lblLabel = new Label
            {
                Text = "Correct Answer:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(39, 174, 96),
                AutoSize = true,
                Location = new Point(15, 10)
            };

            Label lblAnswer = new Label
            {
                Text = answer,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(39, 174, 96),
                AutoSize = true,
                Location = new Point(15, 30)
            };

            pnlAnswer.Controls.AddRange(new Control[] { lblLabel, lblAnswer });
            parent.Controls.Add(pnlAnswer);
            parent.Height = 70;
        }

        private void ShowNoQuestionMessage()
        {
            // Already handled in Designer with lblNoQuestion
        }

        private void ShowNoDataMessage()
        {
            flpQuestion.Controls.Clear();

            Label lblNoData = new Label
            {
                Text = "⚠️ This question has no data yet.\nClick 'Edit' button to create question content.",
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(20)
            };

            flpQuestion.Controls.Add(lblNoData);
        }

        private Color GetTypeColor(string type)
        {
            return type switch
            {
                "MCQ" => Color.FromArgb(52, 152, 219),
                "MULTI_CHOICES" => Color.FromArgb(155, 89, 182),
                "FILL_BLANK" => Color.FromArgb(26, 188, 156),
                _ => Color.FromArgb(149, 165, 166)
            };
        }

        private string GetQuestionTypeDisplay(string type)
        {
            return type switch
            {
                "MCQ" => "Multiple Choice (Single Answer)",
                "MULTI_CHOICES" => "Multiple Choice (Multiple Answers)",
                "FILL_BLANK" => "Fill in the Blank",
                _ => type
            };
        }

        private void btnEditQuestion_Click(object sender, EventArgs e)
        {
            if (_selectedQuestionId == null)
            {
                // Create new question
                var frm = new EditQuestionForm(_passageId, _selectedPosition);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadQuestionButtons();
                    LoadQuestionContent();
                }
            }
            else
            {
                // Edit existing question
                var frm = new EditQuestionForm(_selectedQuestionId.Value, _passageId, _selectedPosition);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadQuestionButtons();
                    LoadQuestionContent();
                }
            }
        }

    //    private void btnDelete_Click(object sender, EventArgs e)
    //    {
    //        if (_selectedQuestionId == null)
    //        {
    //            MessageBox.Show("Please select a question to delete.", "No Question Selected",
    //                MessageBoxButtons.OK, MessageBoxIcon.Information);
    //            return;
    //        }

    //        var result = MessageBox.Show(
    //            $"Are you sure you want to delete Question {_selectedPosition}?\n\n" +
    //            "This action cannot be undone!",
    //            "Confirm Delete",
    //            MessageBoxButtons.YesNo,
    //            MessageBoxIcon.Warning);

    //        if (result == DialogResult.Yes)
    //        {
    //            try
    //            {
    //                _bll.DeleteQuestion(_selectedQuestionId.Value);

    //                MessageBox.Show("Question deleted successfully!", "Success",
    //                    MessageBoxButtons.OK, MessageBoxIcon.Information);

    //                _selectedQuestionId = null;
    //                LoadQuestionButtons();
    //                ShowNoQuestionMessage();
    //            }
    //            catch (Exception ex)
    //            {
    //                MessageBox.Show($"Error deleting question: {ex.Message}", "Error",
    //                    MessageBoxButtons.OK, MessageBoxIcon.Error);
    //            }
    //        }
    //    }
    }
}