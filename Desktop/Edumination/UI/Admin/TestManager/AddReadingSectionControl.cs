using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.Admin.TestManager
{
    public partial class AddReadingSectionControl : UserControl
    {
        private string? testPaperTitle;
        private string? testPaperDescription;
        private string? testPaperPdfFileName;
        private string? testPaperPdfFilePath;
        private int testMonth;
        private int mockTestId;

        public enum QuestionType
        {
            MCQ,
            MULTI_SELECT,
            FILL_BLANK,
            MATCHING,
            ORDERING,
            SHORT_ANSWER,
            ESSAY,
            SPEAK_PROMPT
        }

        class QuestionDraft
        {
            public int QuestionNumber { get; set; }
            public QuestionType Type { get; set; }

            public string QuestionText { get; set; } = "";
            // option content
            public List<string> Options { get; set; } = new();

            // đáp án đúng (index: 0=A, 1=B,...)
            //public List<int> CorrectAnswers { get; set; } = new();
            public string CorrectAnswer { get; set; } = "";

            public int NumberOfChoices { get; set; }
            public int End { get; set; }
        }

        Dictionary<int, QuestionDraft> questionDrafts = new();

        // ===== Constructors =====

        public AddReadingSectionControl()
        {
            InitializeComponent();
            RegisterQuestionButtonEvents();
            LoadQuestionTypes();
            InitUIState();
        }

        public AddReadingSectionControl(
            string? title,
            string? description,
            string? pdfFileName,
            string? pdfFilePath,
            int testMonth,
            int mockTestId
        ) : this()
        {
            this.testPaperTitle = title;
            this.testPaperDescription = description;
            this.testPaperPdfFileName = pdfFileName;
            this.testPaperPdfFilePath = pdfFilePath;
            this.testMonth = testMonth;
            this.mockTestId = mockTestId;
            InitializeComponent();
            RegisterQuestionButtonEvents();
            LoadQuestionTypes();
            InitUIState();

        }


        public string? GetTestPaperTitle() => testPaperTitle;
        public void SetTestPaperTitle(string? title) => testPaperTitle = title;

        public string? GetTestPaperDescription() => testPaperDescription;
        public void SetTestPaperDescription(string? desc) => testPaperDescription = desc;

        public string? GetTestPaperPdfFileName() => testPaperPdfFileName;
        public void SetTestPaperPdfFileName(string? fileName) => testPaperPdfFileName = fileName;

        public string? GetTestPaperPdfFilePath() => testPaperPdfFilePath;
        public void SetTestPaperPdfFilePath(string? path) => testPaperPdfFilePath = path;

        public int GetTestMonth() => testMonth;
        public void SetTestMonth(int month) => testMonth = month;

        public int GetMockTestId() => mockTestId;
        public void SetMockTestId(int id) => mockTestId = id;


        private void LoadQuestionTypes()
        {
            cboQuestionType.DataSource = Enum.GetValues(typeof(QuestionType));
        }


        private void InitUIState()
        {
            SetQuestionButtonsEnabled(0);
            nmEnd.Visible = false;
            cboQuestionType.Enabled = false;
            cboQuestionType.Visible = false;
            nmNumberOfChoices.Visible = false;
            label4.Visible = false;
            label2.Visible = false;
        }

        private void EnableQuestionButtonsByTotal(int total)
        {
            for (int i = 1; i <= 40; i++)
            {
                Button? btn = Controls.Find($"btnQ{i}", true).FirstOrDefault() as Button;
                if (btn != null)
                    btn.Enabled = i <= total;
            }
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(txtTotal.Text, out int total) || total <= 0 || total > 40)
            {
                SetQuestionButtonsEnabled(0);
                return;
            }

            EnableQuestionButtonsByTotal(total);
        }

        private void RegisterQuestionButtonEvents()
        {
            for (int i = 1; i <= 40; i++)
            {
                Button? btn = Controls.Find($"btnQ{i}", true).FirstOrDefault() as Button;
                if (btn != null)
                    btn.Click += QuestionButton_Click;
            }
        }

        private void QuestionButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int selected = int.Parse(btn.Text);
            txtSelectedButton.Text = selected.ToString();
            cboQuestionType.Visible = true;
            cboQuestionType.Enabled = true;

            UpdateEndAndChoices();

            //ShowDynamic(draft);
            if (questionDrafts.ContainsKey(selected))
            {
                LoadQuestionDraft(questionDrafts[selected]);
            }
        }

        private void cboQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RenderDynamicOptions();
            RenderDynamicOptions();
            QuestionType type = (QuestionType)cboQuestionType.SelectedItem;

            bool showExtra =
                type == QuestionType.MULTI_SELECT ||
                type == QuestionType.MATCHING ||
                type == QuestionType.ORDERING;

            nmEnd.Visible = showExtra;
            nmNumberOfChoices.Visible = showExtra;
            label4.Visible = showExtra;
            label2.Visible = showExtra;

            if (showExtra)
                UpdateEndAndChoices();
        }

        private void UpdateEndAndChoices()
        {
            if (!int.TryParse(txtSelectedButton.Text, out int selected))
                return;

            QuestionType type = (QuestionType)cboQuestionType.SelectedItem;

            nmEnd.Minimum = selected + 1;
            nmEnd.Value = selected + 1;

            if (type == QuestionType.ORDERING)
            {
                nmNumberOfChoices.Enabled = false;
                nmNumberOfChoices.Value = nmEnd.Value - selected + 1;
            }
            else
            {
                nmNumberOfChoices.Enabled = true;
                nmNumberOfChoices.Minimum = 2;
                nmNumberOfChoices.Value = 2;
            }
        }

        private void nmEnd_ValueChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSelectedButton.Text, out int selected))
                return;

            if (nmEnd.Value <= selected)
                nmEnd.Value = selected + 1;

            if ((QuestionType)cboQuestionType.SelectedItem == QuestionType.MATCHING)
                RenderDynamicOptions();

            QuestionType type = (QuestionType)cboQuestionType.SelectedItem;

            if ((type == QuestionType.ORDERING) || (type == QuestionType.MULTI_SELECT))
            {
                nmNumberOfChoices.Value = nmEnd.Value - selected + 1;
            }

        }

        private void nmNumberOfChoices_ValueChanged(object sender, EventArgs e)
        {
            if ((QuestionType)cboQuestionType.SelectedItem == QuestionType.MULTI_SELECT)
                RenderDynamicOptions();

            if ((QuestionType)cboQuestionType.SelectedItem == QuestionType.MATCHING)
                RenderDynamicOptions();
        }

        private void SetQuestionButtonsEnabled(int total)
        {
            // Danh sách tất cả button câu hỏi
            List<Button> questionButtons = new List<Button>()
    {
        btnQ1, btnQ2, btnQ3, btnQ4, btnQ5,
        btnQ6, btnQ7, btnQ8, btnQ9, btnQ10,
        btnQ11, btnQ12, btnQ13, btnQ14, btnQ15,
        btnQ16, btnQ17, btnQ18, btnQ19, btnQ20,
        btnQ21, btnQ22, btnQ23, btnQ24, btnQ25,
        btnQ26, btnQ27, btnQ28, btnQ29, btnQ30,
        btnQ31, btnQ32, btnQ33, btnQ34, btnQ35,
        btnQ36, btnQ37, btnQ38, btnQ39, btnQ40
    };

            foreach (Button btn in questionButtons)
            {
                int questionNumber = int.Parse(btn.Text);

                if (questionNumber <= total)
                {
                    btn.Enabled = true;
                    btn.BackColor = Color.White;
                }
                else
                {
                    btn.Enabled = false;
                    btn.BackColor = Color.LightGray;
                }
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
                Location = new Point(5, 5)
            };

            TextBox txtQuestion = new TextBox
            {
                Name = "txtQuestionContent",
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Width = pnlDynamic.Width - 20,
                Height = 80,
                Location = new Point(5, 25)
            };

            pnlDynamic.Controls.Add(lbl);
            pnlDynamic.Controls.Add(txtQuestion);
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
            else if (type == QuestionType.MATCHING)
            {
                RenderMatching();
            }

        }

        private void RenderMCQ()
        {
            InitDynamicPanelBase();

            int top = 120;

            for (int i = 0; i < 4; i++)
            {
                RadioButton rb = new RadioButton
                {
                    Name = $"rb_{i}",
                    Location = new Point(10, top + i * 30)
                };

                TextBox txt = new TextBox
                {
                    Name = $"txtOption_{i}",
                    Width = 300,
                    Location = new Point(35, top + i * 30)
                };

                pnlDynamic.Controls.Add(rb);
                pnlDynamic.Controls.Add(txt);
            }
        }

        private void RenderMultiChoice(int numberOfChoices)
        {
            InitDynamicPanelBase();

            int top = 120;

            for (int i = 0; i < numberOfChoices; i++)
            {
                CheckBox cb = new CheckBox
                {
                    Name = $"cb_{i}",
                    Location = new Point(10, top + i * 30),
                    Text = ((char)('A' + i)).ToString()
                };

                TextBox txt = new TextBox
                {
                    Name = $"txtOption_{i}",
                    Width = 300,
                    Location = new Point(35, top + i * 30)
                };

                pnlDynamic.Controls.Add(cb);
                pnlDynamic.Controls.Add(txt);
            }
        }

        private void EnforceMultiChoiceLimit(int maxSelected)
        {
            var checkedBoxes = pnlDynamic.Controls
                .OfType<CheckBox>()
                .Where(c => c.Checked)
                .ToList();

            if (checkedBoxes.Count > maxSelected)
            {
                checkedBoxes.Last().Checked = false;
                MessageBox.Show(
                    $"Bạn chỉ được chọn tối đa {maxSelected} đáp án.",
                    "Giới hạn đáp án",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private int GetMaxSelectableAnswers()
        {
            int selected = int.Parse(txtSelectedButton.Text);
            int end = (int)nmEnd.Value;

            return end - selected + 1;
        }

        private void RenderMatching()
        {
            pnlDynamic.Controls.Clear();

            int selected = int.Parse(txtSelectedButton.Text);
            int end = (int)nmEnd.Value;

            int leftCount = end - selected + 1;
            int rightCount = (int)nmNumberOfChoices.Value;

            int startY = 10;
            int rowHeight = 35;

            int leftX = 10;
            int leftTextX = 40;

            int rightX = 420;
            int rightTextX = 450;

            // ===== CỘT TRÁI: SỐ =====
            for (int i = 0; i < leftCount; i++)
            {
                int number = selected + i;

                Label lblNumber = new Label()
                {
                    Text = number.ToString(),
                    Location = new Point(leftX, startY + i * rowHeight),
                    Width = 25
                };

                TextBox txtNumber = new TextBox()
                {
                    Name = $"txtMatchLeft_{number}",
                    Location = new Point(leftTextX, startY + i * rowHeight),
                    Width = 300
                };

                pnlDynamic.Controls.Add(lblNumber);
                pnlDynamic.Controls.Add(txtNumber);
            }

            // ===== CỘT PHẢI: A, B, C... =====
            for (int i = 0; i < rightCount; i++)
            {
                char letter = (char)('A' + i);

                Label lblLetter = new Label()
                {
                    Text = letter.ToString(),
                    Location = new Point(rightX, startY + i * rowHeight),
                    Width = 25
                };

                TextBox txtLetter = new TextBox()
                {
                    Name = $"txtMatchRight_{letter}",
                    Location = new Point(rightTextX, startY + i * rowHeight),
                    Width = 300
                };

                pnlDynamic.Controls.Add(lblLetter);
                pnlDynamic.Controls.Add(txtLetter);
            }
        }

        private void btnSaveQuestion_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSelectedButton.Text, out int qNum))
            {
                MessageBox.Show("Chưa chọn câu hỏi");
                return;
            }

            QuestionType type = (QuestionType)cboQuestionType.SelectedItem;

            QuestionDraft draft = new()
            {
                QuestionNumber = qNum,
                Type = type,
                End = (int)nmEnd.Value
            };
            TextBox txtQ = pnlDynamic.Controls
                .Find("txtQuestionContent", true)
                .FirstOrDefault() as TextBox;

            if (txtQ == null || string.IsNullOrWhiteSpace(txtQ.Text))
            {
                MessageBox.Show("Nội dung câu hỏi không được để trống");
                return;
            }

            draft.QuestionText = txtQ.Text;



            bool success = false;

            if (type == QuestionType.MULTI_SELECT)
            {
                draft.NumberOfChoices = (int)nmNumberOfChoices.Value;
                success = SaveMultiChoice(draft);
            }
            else if (type == QuestionType.MCQ)
            {
                success = SaveMCQ(draft);
            }

            if (!success)
                return; // ⛔ dừng nhẹ, không làm gì thêm

            questionDrafts[qNum] = draft;
            MarkQuestionButtonSaved(qNum);
        }

        private bool SaveMCQ(QuestionDraft draft)
        {
            var radios = pnlDynamic.Controls.OfType<RadioButton>().ToList();
            var selected = radios.FirstOrDefault(r => r.Checked);

            if (selected == null)
            {
                MessageBox.Show("MCQ phải chọn đúng 1 đáp án");
                return false;
            }

            draft.Options.Clear();
            draft.CorrectAnswer = "";

            for (int i = 0; i < radios.Count; i++)
            {
                TextBox txt = pnlDynamic.Controls
                    .Find($"txtOption_{i}", true)
                    .FirstOrDefault() as TextBox;

                draft.Options.Add(txt?.Text ?? "");

                if (radios[i].Checked)
                {
                    draft.CorrectAnswer = ((char)('A' + i)).ToString();
                }
            }

            return true;
        }




        private bool SaveMultiChoice(QuestionDraft draft)
        {
            if (!int.TryParse(txtSelectedButton.Text, out int selected))
            {
                MessageBox.Show("Chưa nhập selected");
                return false;
            }

            int required = draft.End - selected + 1;

            var checks = pnlDynamic.Controls.OfType<CheckBox>().ToList();
            var checkedIndexes = checks
                .Select((c, i) => new { c.Checked, Index = i })
                .Where(x => x.Checked)
                .Select(x => x.Index)
                .ToList();

            if (checkedIndexes.Count != required)
            {
                MessageBox.Show($"Phải chọn đúng {required} đáp án");
                return false;
            }

            draft.Options.Clear();
            for (int i = 0; i < checks.Count; i++)
            {
                TextBox txt = pnlDynamic.Controls
                    .Find($"txtOption_{i}", true)
                    .FirstOrDefault() as TextBox;

                draft.Options.Add(txt?.Text ?? "");
            }

            draft.NumberOfChoices = checks.Count;

            // JSON insert DB được
            draft.CorrectAnswer = JsonConvert.SerializeObject(checkedIndexes);

            return true;
        }




        private void LoadQuestionDraft(QuestionDraft draft)
        {
            cboQuestionType.SelectedItem = draft.Type;
            nmEnd.Value = draft.End;

            if (draft.Type == QuestionType.MCQ)
            {
                LoadMCQDraft(draft);
            }
            else if (draft.Type == QuestionType.MULTI_SELECT)
            {
                LoadMultiChoiceDraft(draft);
            }
        }



        private void LoadMCQDraft(QuestionDraft draft)
        {
            // render lại UI trước
            RenderDynamicOptions();

            // load nội dung câu hỏi
            TextBox txtQ = pnlDynamic.Controls
                .Find("txtQuestionContent", true)
                .FirstOrDefault() as TextBox;

            if (txtQ != null)
                txtQ.Text = draft.QuestionText;

            // load options
            for (int i = 0; i < draft.Options.Count; i++)
            {
                TextBox txt = pnlDynamic.Controls
                    .Find($"txtOption_{i}", true)
                    .FirstOrDefault() as TextBox;

                if (txt != null)
                    txt.Text = draft.Options[i];
            }

            // load đáp án đúng
            if (!string.IsNullOrEmpty(draft.CorrectAnswer))
            {
                int index = draft.CorrectAnswer[0] - 'A';

                var radios = pnlDynamic.Controls.OfType<RadioButton>().ToList();

                if (index >= 0 && index < radios.Count)
                    radios[index].Checked = true;
            }
        }



        private void RenderMultiChoiceOptions(int numberOfChoices)
        {
            pnlDynamic.Controls.Clear();

            // ==== Câu hỏi ====
            TextBox txtQ = new TextBox
            {
                Name = "txtQuestionContent",
                Multiline = true,
                Width = pnlDynamic.Width - 20,
                Height = 80,
                ScrollBars = ScrollBars.Vertical
            };
            pnlDynamic.Controls.Add(txtQ);

            // ==== Options ====
            for (int i = 0; i < numberOfChoices; i++)
            {
                CheckBox chk = new CheckBox
                {
                    Name = $"chkOption_{i}",
                    Text = ((char)('A' + i)).ToString(),
                    Left = 10,
                    Top = 100 + i * 35
                };

                TextBox txt = new TextBox
                {
                    Name = $"txtOption_{i}",
                    Left = 50,
                    Top = chk.Top - 3,
                    Width = pnlDynamic.Width - 70
                };

                pnlDynamic.Controls.Add(chk);
                pnlDynamic.Controls.Add(txt);
            }
        }

        private void LoadMultiChoiceDraft(QuestionDraft draft)
        {
            // 1. Render lại đúng số option
            RenderMultiChoiceOptions(draft.NumberOfChoices);

            // 2. Load nội dung câu hỏi
            TextBox txtQ = pnlDynamic.Controls
                .Find("txtQuestionContent", true)
                .FirstOrDefault() as TextBox;

            if (txtQ != null)
                txtQ.Text = draft.QuestionText;

            // 3. Load nội dung các lựa chọn
            for (int i = 0; i < draft.Options.Count; i++)
            {
                TextBox txt = pnlDynamic.Controls
                    .Find($"txtOption_{i}", true)
                    .FirstOrDefault() as TextBox;

                if (txt != null)
                    txt.Text = draft.Options[i];
            }

            // 4. Parse đáp án đúng (JSON)
            var correctIndexes = JsonConvert
                .DeserializeObject<List<int>>(draft.CorrectAnswer);

            // 5. Tích lại checkbox
            foreach (int i in correctIndexes)
            {
                CheckBox chk = pnlDynamic.Controls
                    .Find($"chkOption_{i}", true)
                    .FirstOrDefault() as CheckBox;

                if (chk != null)
                    chk.Checked = true;
            }
        }

        private void MarkQuestionButtonSaved(int qNum)
        {
            Button? btn = Controls.Find($"btnQ{qNum}", true).FirstOrDefault() as Button;

            if (btn != null)
            {
                btn.BackColor = Color.LightGreen;
                btn.ForeColor = Color.Black;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {

        }
    }
}
