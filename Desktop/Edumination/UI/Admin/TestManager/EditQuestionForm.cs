using Edumination.Api.Domain.Entities;
using IELTS.BLL;
using IELTS.DTO;
using Sprache;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IELTS.BLL.QuestionBLL;

namespace IELTS.UI.Admin.TestManager
{
    public partial class EditQuestionForm : Form
    {
        private readonly QuestionBLL _bll = new QuestionBLL();
        public long QuestionId { get; set; }
        public long SectionId { get; set; }
        public long PassageId { get; set; }
        public int Position { get; set; }


        public enum QuestionType
        {
            MCQ,
            MULTI_SELECT,
            FILL_BLANK
        }

        public EditQuestionForm(long QuestionId, long passageId, int position)
        {
            InitializeComponent();
            LoadQuestionTypes();

            this.QuestionId = QuestionId;
            PassageId = passageId;
            Position = position;

            var question = _bll.GetQuestion(passageId, position);

            if (question != null)
            {
                txtSelectedButton.Text = position.ToString();
                cboQuestionType.SelectedItem =
                    (QuestionType)Enum.Parse(typeof(QuestionType), question.QuestionType);

                nmEnd.Value = question.EndIndex;
                RenderQuestion(question);
            }
        }

        public EditQuestionForm(long passageId, int position)
        {
            InitializeComponent();
            this.PassageId = passageId;
            this.Position = position;

            LoadQuestionTypes();
            InitializeNewQuestion();
        }
        private void InitializeNewQuestion()
        {
            txtSelectedButton.Text = Position.ToString();  
            nmEnd.Value = Position;
            RenderDynamicOptions();
        }

        private void LoadQuestionTypes()
        {
            cboQuestionType.DataSource = Enum.GetValues(typeof(QuestionType));
            cboQuestionType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboQuestionType.FlatStyle = FlatStyle.Popup;
        }

        private void RenderQuestion(QuestionDTO question)
        {
            pnlDynamic.Controls.Clear();

            int y = 15;

            pnlDynamic.Controls.Add(new Label
            {
                Text = "Question:",
                Location = new Point(15, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            });

            TextBox txtQuestion = new TextBox
            {
                Name = "txtQuestion",
                Text = question.QuestionText,
                Location = new Point(110, y - 3),
                Width = 720,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlDynamic.Controls.Add(txtQuestion);
            y += 45;

            cboQuestionType.SelectedItem = question.QuestionType;

            if (question.QuestionType == "MCQ"
                || question.QuestionType == "MULTI_CHOICES")
            {
                var choices = _bll.GetChoices(question.Id);
                nmNumberOfChoices.Value = choices.Count;

                for (int i = 0; i < choices.Count; i++)
                {
                    Control selector;

                    if (question.QuestionType == "MCQ")
                    {
                        selector = new RadioButton
                        {
                            Name = $"rb_{i}",
                            Checked = choices[i].IsCorrect,
                            AutoSize = false,
                            Width = 20
                        };
                    }
                    else
                    {
                        selector = new CheckBox
                        {
                            Name = $"cb_{i}",
                            Checked = choices[i].IsCorrect,
                            AutoSize = false,
                            Width = 20
                        };
                    }

                    selector.Location = new Point(15, y + 4);
                    pnlDynamic.Controls.Add(selector);

                    pnlDynamic.Controls.Add(new Label
                    {
                        Text = $"{(char)('A' + i)}.",
                        Location = new Point(45, y + 6),
                        AutoSize = true
                    });

                    pnlDynamic.Controls.Add(new TextBox
                    {
                        Name = $"txtOpt_{i}",
                        Text = choices[i].ChoiceText,
                        Location = new Point(75, y),
                        Width = 720,
                        Font = new Font("Segoe UI", 10),
                        BorderStyle = BorderStyle.FixedSingle
                    });

                    y += 38;
                }
            }
            else if (question.QuestionType == "FILL_BLANK")
            {
                nmNumberOfChoices.Value = 0;

                string answer = _bll.GetAnswerKey(question.Id);

                pnlDynamic.Controls.Add(new Label
                {
                    Text = "Correct answer:",
                    Location = new Point(15, y),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9)
                });

                pnlDynamic.Controls.Add(new TextBox
                {
                    Name = "txtAnswer",
                    Text = answer,
                    Location = new Point(130, y - 3),
                    Width = 300,
                    Font = new Font("Segoe UI", 10),
                    BorderStyle = BorderStyle.FixedSingle
                });
            }
        }

        private void cboQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RenderDynamicOptions();
            RenderDynamicOptions();

            QuestionType type = (QuestionType)cboQuestionType.SelectedItem;

            bool showExtra = type == QuestionType.MULTI_SELECT;

            nmEnd.Visible = showExtra;
            nmNumberOfChoices.Visible = showExtra;
            label4.Visible = showExtra;
            label2.Visible = showExtra;

            if (showExtra)
                UpdateEndAndChoices();
        }

        private void RenderDynamicOptions()
        {
            pnlDynamic.Controls.Clear();

            QuestionType type = (QuestionType)cboQuestionType.SelectedItem;

            if (type == QuestionType.MCQ)
            {
                RenderMCQ();
            }
            else if (type == QuestionType.MULTI_SELECT)
            {
                RenderMultiChoice((int)nmNumberOfChoices.Value);
            }
        }

        private void RenderMCQ()
        {
            InitDynamicPanelBase();

            int top = 130;

            for (int i = 0; i < 4; i++)
            {
                RadioButton rb = new RadioButton
                {
                    Name = $"rb_{i}",
                    Location = new Point(15, top),
                    AutoSize = false,
                    Width = 20
                };

                Label lbl = new Label
                {
                    Text = $"{(char)('A' + i)}.",
                    Location = new Point(45, top + 4),
                    AutoSize = true
                };

                TextBox txt = new TextBox
                {
                    Name = $"txtOption_{i}",
                    Width = 680,
                    Location = new Point(80, top),
                    Font = new Font("Segoe UI", 10),
                    BorderStyle = BorderStyle.FixedSingle
                };

                pnlDynamic.Controls.Add(rb);
                pnlDynamic.Controls.Add(lbl);
                pnlDynamic.Controls.Add(txt);

                txt.BringToFront(); // đảm bảo không bị che

                top += 38;
            }
        }


        private void RenderMultiChoice(int numberOfChoices)
        {
            InitDynamicPanelBase();

            int top = 130;

            for (int i = 0; i < numberOfChoices; i++)
            {
                CheckBox cb = new CheckBox
                {
                    Name = $"cb_{i}",
                    Location = new Point(15, top),
                    AutoSize = false,
                    Width = 20
                };

                Label lbl = new Label
                {
                    Text = $"{(char)('A' + i)}.",
                    Location = new Point(45, top + 4),
                    AutoSize = true
                };

                TextBox txt = new TextBox
                {
                    Name = $"txtOption_{i}",
                    Width = 680,
                    Location = new Point(80, top),
                    Font = new Font("Segoe UI", 10),
                    BorderStyle = BorderStyle.FixedSingle
                };

                pnlDynamic.Controls.Add(cb);
                pnlDynamic.Controls.Add(lbl);
                pnlDynamic.Controls.Add(txt);

                txt.BringToFront();

                top += 38;
            }
        }


        private void UpdateEndAndChoices()
        {
            if (!int.TryParse(txtSelectedButton.Text, out int selected))
                return;

            nmEnd.Minimum = selected + 1;
            nmEnd.Value = selected + 1;

            nmNumberOfChoices.Enabled = true;
            nmNumberOfChoices.Minimum = 2;
            nmNumberOfChoices.Value = 2;
        }

        private void nmEnd_ValueChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSelectedButton.Text, out int selected))
                return;

            if (nmEnd.Value <= selected)
                nmEnd.Value = selected + 1;

            if ((QuestionType)cboQuestionType.SelectedItem == QuestionType.MULTI_SELECT)
            {
                nmNumberOfChoices.Value = nmEnd.Value - selected + 1;
            }
        }

        private void InitDynamicPanelBase()
        {
            pnlDynamic.Controls.Clear();

            Label lbl = new Label
            {
                Text = "Question content",
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(10, 10)
            };

            TextBox txtQuestion = new TextBox
            {
                Name = "txtQuestion",
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Width = pnlDynamic.Width - 30,
                Height = 80,
                Location = new Point(10, 35),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlDynamic.Controls.Add(lbl);
            pnlDynamic.Controls.Add(txtQuestion);
        }

        private void nmNumberOfChoices_ValueChanged(object sender, EventArgs e)
        {
            if ((QuestionType)cboQuestionType.SelectedItem == QuestionType.MULTI_SELECT)
                RenderDynamicOptions();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveQuestion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving question: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveQuestion()
        {
            var txtQuestion = pnlDynamic.Controls
                .Find("txtQuestion", true)
                .FirstOrDefault() as TextBox;

            if (txtQuestion == null || string.IsNullOrWhiteSpace(txtQuestion.Text))
            {
                MessageBox.Show("Chưa nhập nội dung câu hỏi");
                return;
            }

            QuestionType type = (QuestionType)cboQuestionType.SelectedItem;

            // 1️⃣ Nếu đang edit → xóa câu cũ
            // 1️⃣ Nếu edit → lấy section từ question cũ
            if (QuestionId > 0)
            {
                SectionId = _bll.GetSectionIdByQuestionId(QuestionId);
                _bll.DeleteQuestion(QuestionId);
                QuestionId = 0;
            }
            else
            {
                // 2️⃣ Nếu tạo mới → lấy section từ passage
                SectionId = _bll.GetSectionIdByPassageId(PassageId);
            }


            // 2️⃣ Insert câu hỏi mới
            QuestionId = _bll.CreateQuestion(
                SectionId,
                PassageId,
                type.ToString(),
                txtQuestion.Text.Trim(),
                1.0m,
                Position

            );

            // 3️⃣ Insert đáp án
            if (type == QuestionType.MCQ || type == QuestionType.MULTI_SELECT)
                SaveChoices(QuestionId);
            else
                SaveFillBlankAnswer(QuestionId);

            MessageBox.Show("Lưu câu hỏi thành công!");
            DialogResult = DialogResult.OK;
            Close();
        }



        private void SaveChoices(long questionId)
        {
            var optionTextBoxes = pnlDynamic.Controls
                .OfType<TextBox>()
                .Where(t => t.Name.StartsWith("txtOption_"))
                .OrderBy(t => t.Name)
                .ToList();

            if (!optionTextBoxes.Any())
            {
                MessageBox.Show("Chưa có đáp án nào");
                return;
            }

            bool hasCorrect = false;
            int position = 1;

            foreach (var txt in optionTextBoxes)
            {
                int index = int.Parse(txt.Name.Split('_')[1]);

                var rb = pnlDynamic.Controls.Find($"rb_{index}", true)
                    .FirstOrDefault() as RadioButton;

                var cb = pnlDynamic.Controls.Find($"cb_{index}", true)
                    .FirstOrDefault() as CheckBox;

                bool isCorrect =
                    (rb != null && rb.Checked) ||
                    (cb != null && cb.Checked);

                if (string.IsNullOrWhiteSpace(txt.Text))
                    continue;

                if (isCorrect) hasCorrect = true;

                _bll.InsertChoice(
                    questionId,
                    txt.Text.Trim(),
                    isCorrect,
                    position++
                );
            }

            if (!hasCorrect)
            {
                MessageBox.Show("Phải chọn ít nhất 1 đáp án đúng");
                throw new Exception("No correct choice");
            }
        }




        private void SaveFillBlankAnswer(long questionId)
        {
            var txtAnswer = pnlDynamic.Controls
                .Find("txtAnswer", true)
                .FirstOrDefault() as TextBox;

            if (txtAnswer == null || string.IsNullOrWhiteSpace(txtAnswer.Text))
            {
                MessageBox.Show("Chưa nhập đáp án");
                return;
            }

            _bll.SaveAnswerKey(questionId, txtAnswer.Text.Trim());
        }


    }
}
