

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
        private long sectionId;
        public long SectionId
        {
            get => sectionId;
            set { sectionId = value; }
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