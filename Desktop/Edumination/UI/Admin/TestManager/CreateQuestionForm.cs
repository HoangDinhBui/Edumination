using IELTS.BLL;
using IELTS.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace IELTS.UI.Admin.TestManager
{
    public partial class CreateQuestionForm : Form
    {
        private readonly TestPaperBLL _bll = new TestPaperBLL();
        private long _sectionId;
        private List<QuestionChoiceDTO> _choices = new List<QuestionChoiceDTO>();

        public CreateQuestionForm(long sectionId)
        {
            InitializeComponent();
            _sectionId = sectionId;
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Create Question";
            this.Size = new Size(800, 700);
            this.StartPosition = FormStartPosition.CenterParent;

            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };

            // Title
            Label lblTitle = new Label
            {
                Text = "Create New Question",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            // Question Type
            Label lblType = new Label
            {
                Text = "Question Type:*",
                Location = new Point(0, 50),
                AutoSize = true
            };
            ComboBox cboType = new ComboBox
            {
                Name = "cboType",
                Location = new Point(150, 47),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboType.Items.AddRange(new object[] {
                "MCQ",
                "MULTI_SELECT",
                "TRUE_FALSE",
                "FILL_BLANK",
                "SHORT_ANSWER",
                "MATCHING",
                "ORDER"
            });
            cboType.SelectedIndex = 0;
            cboType.SelectedIndexChanged += CboType_SelectedIndexChanged;

            // Position
            Label lblPosition = new Label
            {
                Text = "Position:*",
                Location = new Point(0, 85),
                AutoSize = true
            };
            NumericUpDown nudPosition = new NumericUpDown
            {
                Name = "nudPosition",
                Location = new Point(150, 82),
                Width = 100,
                Minimum = 1,
                Maximum = 1000,
                Value = 1
            };

            // Points
            Label lblPoints = new Label
            {
                Text = "Points:*",
                Location = new Point(260, 85),
                AutoSize = true
            };
            NumericUpDown nudPoints = new NumericUpDown
            {
                Name = "nudPoints",
                Location = new Point(330, 82),
                Width = 100,
                Minimum = 0.5M,
                Maximum = 10M,
                DecimalPlaces = 1,
                Increment = 0.5M,
                Value = 1M
            };

            // Question Text
            Label lblQuestion = new Label
            {
                Text = "Question Text:*",
                Location = new Point(0, 120),
                AutoSize = true
            };
            TextBox txtQuestion = new TextBox
            {
                Name = "txtQuestion",
                Location = new Point(150, 117),
                Width = 600,
                Height = 100,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            // Choices Section (for MCQ, MULTI_SELECT, TRUE_FALSE)
            Label lblChoices = new Label
            {
                Name = "lblChoices",
                Text = "Answer Choices:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(0, 230),
                AutoSize = true
            };

            FlowLayoutPanel flpChoices = new FlowLayoutPanel
            {
                Name = "flpChoices",
                Location = new Point(0, 260),
                Width = 750,
                Height = 200,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            Button btnAddChoice = new Button
            {
                Name = "btnAddChoice",
                Text = "Add Choice",
                Width = 120,
                Height = 30,
                Location = new Point(0, 470),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddChoice.Click += BtnAddChoice_Click;

            // Answer Key Section (for non-choice questions)
            Label lblAnswer = new Label
            {
                Name = "lblAnswer",
                Text = "Answer Key:",
                Location = new Point(0, 230),
                AutoSize = true,
                Visible = false
            };
            TextBox txtAnswer = new TextBox
            {
                Name = "txtAnswer",
                Location = new Point(150, 227),
                Width = 600,
                Height = 100,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Visible = false,
                PlaceholderText = "Enter the correct answer"
            };

            // Buttons
            Button btnSave = new Button
            {
                Text = "Save Question",
                Width = 130,
                Height = 40,
                Location = new Point(0, 520),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Width = 100,
                Height = 40,
                Location = new Point(140, 520),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            pnlMain.Controls.AddRange(new Control[]
            {
                lblTitle, lblType, cboType, lblPosition, nudPosition,
                lblPoints, nudPoints, lblQuestion, txtQuestion,
                lblChoices, flpChoices, btnAddChoice,
                lblAnswer, txtAnswer,
                btnSave, btnCancel
            });

            this.Controls.Add(pnlMain);

            // Add initial choices for MCQ
            AddChoiceControl();
        }

        private void CboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbo = sender as ComboBox;
            string type = cbo.SelectedItem.ToString();

            var lblChoices = this.Controls.Find("lblChoices", true)[0] as Label;
            var flpChoices = this.Controls.Find("flpChoices", true)[0] as FlowLayoutPanel;
            var btnAddChoice = this.Controls.Find("btnAddChoice", true)[0] as Button;
            var lblAnswer = this.Controls.Find("lblAnswer", true)[0] as Label;
            var txtAnswer = this.Controls.Find("txtAnswer", true)[0] as TextBox;

            bool showChoices = type == "MCQ" || type == "MULTI_SELECT" || type == "TRUE_FALSE";

            lblChoices.Visible = showChoices;
            flpChoices.Visible = showChoices;
            btnAddChoice.Visible = showChoices;

            lblAnswer.Visible = !showChoices;
            txtAnswer.Visible = !showChoices;

            if (type == "TRUE_FALSE" && flpChoices.Controls.Count == 0)
            {
                // Auto-add True/False choices
                AddChoiceControl("True");
                AddChoiceControl("False");
            }
        }

        private void BtnAddChoice_Click(object sender, EventArgs e)
        {
            AddChoiceControl();
        }

        private void AddChoiceControl(string defaultText = "")
        {
            var flpChoices = this.Controls.Find("flpChoices", true)[0] as FlowLayoutPanel;

            Panel pnlChoice = new Panel
            {
                Width = 720,
                Height = 50,
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            CheckBox chkCorrect = new CheckBox
            {
                Text = "Correct",
                Location = new Point(10, 15),
                Width = 80
            };

            TextBox txtChoice = new TextBox
            {
                Location = new Point(100, 12),
                Width = 500,
                Text = defaultText
            };

            Button btnRemove = new Button
            {
                Text = "✖",
                Width = 30,
                Height = 25,
                Location = new Point(620, 10),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRemove.Click += (s, e) =>
            {
                flpChoices.Controls.Remove(pnlChoice);
            };

            pnlChoice.Controls.AddRange(new Control[] { chkCorrect, txtChoice, btnRemove });
            flpChoices.Controls.Add(pnlChoice);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var cboType = this.Controls.Find("cboType", true)[0] as ComboBox;
                var nudPosition = this.Controls.Find("nudPosition", true)[0] as NumericUpDown;
                var nudPoints = this.Controls.Find("nudPoints", true)[0] as NumericUpDown;
                var txtQuestion = this.Controls.Find("txtQuestion", true)[0] as TextBox;

                if (string.IsNullOrWhiteSpace(txtQuestion.Text))
                {
                    MessageBox.Show("Please enter question text.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string questionType = cboType.SelectedItem.ToString();

                var question = new QuestionDTO
                {
                    SectionId = _sectionId,
                    QuestionType = questionType,
                    QuestionText = txtQuestion.Text.Trim(),
                    Points = nudPoints.Value,
                    Position = (int)nudPosition.Value,
                    Choices = new List<QuestionChoiceDTO>()
                };

                string answerKey = "";

                // Handle choices or answer key based on type
                if (questionType == "MCQ" || questionType == "MULTI_SELECT" || questionType == "TRUE_FALSE")
                {
                    var flpChoices = this.Controls.Find("flpChoices", true)[0] as FlowLayoutPanel;

                    int position = 1;
                    List<string> correctAnswers = new List<string>();

                    foreach (Panel pnlChoice in flpChoices.Controls.OfType<Panel>())
                    {
                        var chkCorrect = pnlChoice.Controls.OfType<CheckBox>().First();
                        var txtChoice = pnlChoice.Controls.OfType<TextBox>().First();

                        if (string.IsNullOrWhiteSpace(txtChoice.Text))
                            continue;

                        var choice = new QuestionChoiceDTO
                        {
                            ChoiceText = txtChoice.Text.Trim(),
                            IsCorrect = chkCorrect.Checked,
                            Position = position
                        };

                        question.Choices.Add(choice);

                        if (chkCorrect.Checked)
                        {
                            correctAnswers.Add(((char)('A' + position - 1)).ToString());
                        }

                        position++;
                    }

                    if (question.Choices.Count == 0)
                    {
                        MessageBox.Show("Please add at least one choice.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (correctAnswers.Count == 0)
                    {
                        MessageBox.Show("Please mark at least one correct answer.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    answerKey = string.Join("", correctAnswers);
                }
                else
                {
                    var txtAnswer = this.Controls.Find("txtAnswer", true)[0] as TextBox;

                    if (string.IsNullOrWhiteSpace(txtAnswer.Text))
                    {
                        MessageBox.Show("Please enter the answer key.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    answerKey = txtAnswer.Text.Trim();
                }

                question.AnswerData = answerKey;

                long questionId = _bll.CreateQuestion(question);

                MessageBox.Show($"Question created successfully! ID: {questionId}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving question: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}