using AxAcroPDFLib;
using Edumination.WinForms.UI.Admin.TestManager;
using IELTS.DAL;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Design.AxImporter;

namespace IELTS.BLL
{
    public partial class AddTestSectionForm : Form
    {
        private int TestPaperId;
        // Index từ 1 → 40, index 0 không dùng
        private string[] answerTypes = new string[41];
        private string[] answers = new string[41];
        private int[] questionGroupStart = new int[41]; // Lưu câu bắt đầu của nhóm
        private int[] questionGroupEnd = new int[41];   // Lưu câu kết thúc của nhóm
        private string answer = "";
        private Button selectedButton = null;
        private UIPanel formCard;
        private int start, end;
        private string currentPdfPath = "";
        private TestSectionBLL _testSectionBLL;

        public AddTestSectionForm(int testPaperId)
        {
            InitializeComponent();
            TestPaperId = testPaperId;
            _testSectionBLL = new TestSectionBLL();
            LoadQuestionTypes();
            this.Controls.Add(scrollPanel);
            this.Controls.Add(pnlDynamic);
            pnlDynamic.AutoScroll = true;
            pnlDynamic.BackColor = Color.White;
        }

        private void LoadQuestionTypes()
        {
            cboQuestionType.Items.AddRange(new string[]
            {
                "MCQ",
                "MULTI_SELECT",
                "FILL_BLANK",
                "MATCHING",
                "ORDERING",
                "SHORT_ANSWER",
                "ESSAY",
                "SPEAK_PROMPT"
            });

            cboQuestionType.SelectedIndexChanged += CboQuestionType_SelectedIndexChanged;
        }

        private void CboQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlDynamic.Controls.Clear();

            if (!int.TryParse(txtSelectedButton.Text, out int questionNum))
                return;

            string selectedType = cboQuestionType.SelectedItem?.ToString();
            string questionText;

            // ⭐ Tạo tiêu đề phù hợp cho câu hỏi nhóm hoặc đơn
            if (IsGroupQuestionType(selectedType))
            {
                int groupEnd = (int)nmEnd.Value;
                questionText = $"Question {questionNum} - {groupEnd}";
            }
            else
            {
                questionText = "Question " + questionNum;
            }

            // ⭐ Lấy saved answer nếu có
            string savedAnswer = (questionNum >= 1 && questionNum <= 40) ? answers[questionNum] : null;

            switch (selectedType)
            {
                case "MCQ":
                    LoadUI_MCQ(questionText, savedAnswer);
                    break;

                case "MULTI_SELECT":
                    LoadUI_MultiSelect(questionText, savedAnswer);
                    break;

                case "FILL_BLANK":
                    LoadUI_FillBlank(questionText, savedAnswer);
                    break;

                case "MATCHING":
                    LoadUI_Matching(questionText, savedAnswer);
                    break;

                case "ORDERING":
                    LoadUI_Ordering(questionText, savedAnswer);
                    break;

                case "SHORT_ANSWER":
                    LoadUI_ShortAnswer(questionText, savedAnswer);
                    break;

                case "ESSAY":
                    LoadUI_Essay(questionText, savedAnswer);
                    break;

                case "SPEAK_PROMPT":
                    LoadUI_Speaking(questionText, savedAnswer);
                    break;
            }
        }

        private void QuestionButton_Click(object sender, EventArgs e)
        {
            Button clicked = sender as Button;

            // Bỏ viền nút trước đó
            if (selectedButton != null)
            {
                selectedButton.FlatStyle = FlatStyle.Standard;
                selectedButton.FlatAppearance.BorderSize = 1;
                selectedButton.FlatAppearance.BorderColor = SystemColors.ControlDark;
            }

            // Chọn nút mới
            selectedButton = clicked;

            // Tạo viền highlight cho nút hiện tại
            clicked.FlatStyle = FlatStyle.Flat;
            clicked.FlatAppearance.BorderSize = 1;
            clicked.FlatAppearance.BorderColor = Color.Blue;

            // Ghi số câu vào textbox
            txtSelectedButton.Text = clicked.Text;

            // Hiển thị giá trị combobox nếu đã lưu
            if (int.TryParse(clicked.Text, out int i))
            {
                if (i >= 1 && i <= 40 && !string.IsNullOrEmpty(answerTypes[i]))
                {
                    cboQuestionType.Text = answerTypes[i];
                    lblCorrect.Text = "Correct Answer: " + answers[i];

                    // ⭐ Nếu là câu hỏi nhóm, hiển thị range trong nmEnd
                    if (IsGroupQuestionType(answerTypes[i]))
                    {
                        int groupStart = questionGroupStart[i];
                        int groupEnd = questionGroupEnd[i];

                        if (groupStart > 0 && groupEnd > 0)
                        {
                            txtSelectedButton.Text = groupStart.ToString();
                            nmEnd.Value = groupEnd;
                        }
                    }

                    // ⭐ Trigger load UI với saved answer
                    CboQuestionType_SelectedIndexChanged(cboQuestionType, EventArgs.Empty);
                }
                else
                {
                    cboQuestionType.Text = "";
                    pnlDynamic.Controls.Clear();
                    lblCorrect.Text = "Correct";
                }
            }
        }

        // Helper method để check xem có phải loại câu hỏi nhóm không
        private bool IsGroupQuestionType(string questionType)
        {
            return questionType == "MULTI_SELECT" ||
                   questionType == "MATCHING" ||
                   questionType == "ORDERING";
        }

        #region Load UI Methods with SavedAnswer

        private void LoadUI_MCQ(string questionText, string savedAnswer = null)
        {
            formCard = QuestionCardGenerator.CreateQuestionCard(
                1,
                "MCQ",
                questionText,
                null,
                savedAnswer
            );
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private void LoadUI_MultiSelect(string questionText, string savedAnswer = null)
        {
            int.TryParse(txtSelectedButton.Text, out start);
            end = (int)nmEnd.Value;

            // Tạo danh sách options A, B, C, D...
            int count = 10;
            //List<string> options = new List<string>();
            //for (int i = 0; i < count; i++)
            //{
            //    options.Add(((char)('A' + i)).ToString());
            //}

            formCard = QuestionCardGenerator.CreateQuestionCard(
                1,
                "MULTI_SELECT",
                questionText,
                count,
                savedAnswer
            );
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private void LoadUI_FillBlank(string questionText, string savedAnswer = null)
        {
            formCard = QuestionCardGenerator.CreateQuestionCard(
                1,
                "FILL_BLANK",
                questionText,
                null,
                savedAnswer
            );
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private void LoadUI_Matching(string questionText, string savedAnswer = null)
        {
            int.TryParse(txtSelectedButton.Text, out start);
            end = (int)nmEnd.Value;

            Dictionary<string, List<string>> matchData = GenerateMatchData(start, end);

            formCard = QuestionCardGenerator.CreateQuestionCard(
                1,
                "MATCHING",
                questionText,
                matchData,
                savedAnswer
            );
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private void LoadUI_Ordering(string questionText, string savedAnswer = null)
        {
            int.TryParse(txtSelectedButton.Text, out start);
            end = (int)nmEnd.Value;

            int count = end - start + 1;
            List<string> options = new List<string>();
            for (int i = 0; i < count; i++)
            {
                options.Add(((char)('A' + i)).ToString());
            }

            formCard = QuestionCardGenerator.CreateQuestionCard(
                1,
                "ORDERING",
                questionText,
                options,
                savedAnswer
            );
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private void LoadUI_ShortAnswer(string questionText, string savedAnswer = null)
        {
            formCard = QuestionCardGenerator.CreateQuestionCard(
                1,
                "SHORT_ANSWER",
                questionText,
                null,
                savedAnswer
            );
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private void LoadUI_Essay(string questionText, string savedAnswer = null)
        {
            formCard = QuestionCardGenerator.CreateQuestionCard(
                1,
                "ESSAY",
                questionText,
                null,
                savedAnswer
            );
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private void LoadUI_Speaking(string questionText, string savedAnswer = null)
        {
            formCard = QuestionCardGenerator.CreateQuestionCard(
                1,
                "SPEAK_PROMPT",
                questionText,
                null,
                savedAnswer
            );
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        #endregion

        #region Helper Methods

        private static Dictionary<string, List<string>> GenerateMatchData(int x, int y)
        {
            int count = y - x + 1;

            // Tạo danh sách chữ cái từ 'A'
            List<string> options = new List<string>();
            for (int i = 0; i < count; i++)
            {
                options.Add(((char)('A' + i)).ToString());
            }

            // Tạo matchData
            Dictionary<string, List<string>> matchData = new Dictionary<string, List<string>>();

            for (int i = x; i <= y; i++)
            {
                matchData.Add(i.ToString(), new List<string>(options));
            }

            return matchData;
        }

        #endregion

        #region PDF Methods

        private void btnOpenPdf_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF Files|*.pdf";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    currentPdfPath = ofd.FileName;
                    txtFileName.Text = Path.GetFileName(currentPdfPath);
                    LoadPdf(currentPdfPath);
                }
            }
        }

        private void LoadPdf(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                bool ok = axAcroPDFViewer.LoadFile(path);

                if (ok)
                {
                    axAcroPDFViewer.setShowToolbar(true);
                    axAcroPDFViewer.setView("Fit");
                    MessageBox.Show("PDF loaded successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to load PDF!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading PDF: {ex.Message}");
            }
        }

        #endregion

        #region Save Logic

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Lấy số nút hiện tại từ txtSelectedButton
            if (!int.TryParse(txtSelectedButton.Text, out int i) || i < 1 || i > 40)
            {
                MessageBox.Show("Chưa chọn nút câu hỏi hoặc số câu không hợp lệ!");
                return;
            }

            string selectedValue = cboQuestionType.Text?.Trim();

            if (string.IsNullOrEmpty(selectedValue))
            {
                MessageBox.Show("Vui lòng chọn loại câu hỏi trong combobox!");
                return;
            }

            if (formCard == null)
            {
                MessageBox.Show("Chưa có form câu hỏi để lưu!");
                return;
            }

            try
            {
                // ⭐ Nếu là câu hỏi nhóm (MULTI_SELECT, MATCHING, ORDERING)
                if (IsGroupQuestionType(selectedValue))
                {
                    start = i; // start là số câu đã chọn
                    end = (int)nmEnd.Value;

                    if (end < start)
                    {
                        MessageBox.Show("Câu kết thúc phải lớn hơn hoặc bằng câu bắt đầu!");
                        return;
                    }

                    // Lấy đáp án từ form
                    answer = QuestionCardGenerator.GetStudentAnswer(formCard, selectedValue);

                    // Lưu cho tất cả các câu trong nhóm
                    for (int x = start; x <= end; x++)
                    {
                        answerTypes[x] = selectedValue;
                        answers[x] = answer;
                        questionGroupStart[x] = start;
                        questionGroupEnd[x] = end;

                        // Đổi màu button
                        string buttonName = "btnQ" + x;
                        Control[] found = this.Controls.Find(buttonName, true);

                        if (found.Length > 0 && found[0] is Button btn)
                        {
                            btn.BackColor = Color.LightGreen;
                        }
                    }

                    MessageBox.Show($"✅ Lưu câu {start} - {end} thành công!\n\nLoại: {selectedValue}\nĐáp án: {answer}",
                        "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // ⭐ Câu hỏi đơn lẻ
                    answerTypes[i] = selectedValue;
                    answer = QuestionCardGenerator.GetStudentAnswer(formCard, selectedValue);
                    answers[i] = answer;
                    questionGroupStart[i] = 0;
                    questionGroupEnd[i] = 0;

                    // Đổi màu nền nút thành xanh
                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = Color.LightGreen;
                    }

                    MessageBox.Show($"✅ Lưu câu {i} thành công!\n\nLoại: {selectedValue}\nĐáp án: {answer}",
                        "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Cập nhật label hiển thị đáp án
                lblCorrect.Text = "Correct Answer: " + answer;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Form Load

        private void AddTestSectionForm_Load(object sender, EventArgs e)
        {
            // Gán sự kiện click cho tất cả button câu hỏi
            foreach (Control c in this.Controls)
            {
                if (c is Button && c.Name.StartsWith("btnQ"))
                {
                    c.Click += QuestionButton_Click;
                }
            }
        }

        #endregion

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (TestPaperId <= 0)
                {
                    MessageBox.Show("Invalid Test Paper ID!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra xem có câu hỏi nào được lưu không
                bool hasQuestions = false;
                for (int i = 1; i <= 40; i++)
                {
                    if (!string.IsNullOrEmpty(answerTypes[i]))
                    {
                        hasQuestions = true;
                        break;
                    }
                }

                if (!hasQuestions)
                {
                    MessageBox.Show("Please add at least one question!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm dialog
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to save this Reading section to database?",
                    "Confirm Save",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                // Convert arrays to dictionaries
                Dictionary<int, string> questionTypesDict = new Dictionary<int, string>();
                Dictionary<int, string> answersDict = new Dictionary<int, string>();

                for (int i = 1; i <= 40; i++)
                {
                    if (!string.IsNullOrEmpty(answerTypes[i]))
                    {
                        questionTypesDict[i] = answerTypes[i];
                        answersDict[i] = answers[i] ?? "";
                    }
                }

                // Lấy thông tin PDF nếu có
                string pdfFileName = CheckPdf();
                if (pdfFileName != null)
                {
                    // Save to database
                    bool success = _testSectionBLL.SaveTestSection(
                        TestPaperId,
                        "READING",
                        60,
                        pdfFileName,
                        currentPdfPath,
                        questionTypesDict,
                        answersDict
                    );

                    if (success)
                    {
                        MessageBox.Show(
                            "✅ Test section saved successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(
                            "❌ Failed to save test section!",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Pdf File name is NULL!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private string CheckPdf()
        {
            // Kiểm tra file PDF
            if (string.IsNullOrWhiteSpace(currentPdfPath) || !File.Exists(currentPdfPath))
            {
                MessageBox.Show("Vui lòng chọn file PDF hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "";
            }

            try
            {
                // Thư mục assets trong project
                string assetsFolder = Path.Combine(Application.StartupPath, "..", "..", "UI", "assets");

                if (!Directory.Exists(assetsFolder))
                    Directory.CreateDirectory(assetsFolder);

                // Tạo tên file mới để tránh trùng lặp
                string newFileName = Guid.NewGuid().ToString() + ".pdf";
                string destPath = Path.Combine(assetsFolder, newFileName);
                axAcroPDFViewer.Dispose();
                MessageBox.Show("Copy vào: " + assetsFolder);
                // Copy file PDF vào folder assets
                File.Copy(currentPdfPath, destPath, true);

                return newFileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        private void LoadQuestionTypes()
        {
            cboQuestionType.Items.AddRange(new string[]
            {
        "MCQ",
        "FILL_BLANK",
        "MATCHING",
        "ESSAY",
        "SPEAKING"
            });

                cboQuestionType.SelectedIndexChanged += CboQuestionType_SelectedIndexChanged;
        }

        private void CboQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlDynamic.Controls.Clear();

            switch (cboQuestionType.SelectedItem.ToString())
            {
                case "MCQ":
                    LoadUI_MCQ();
                    break;

                case "FILL_BLANK":
                    LoadUI_FillBlank();
                    break;

                case "MATCHING":
                    LoadUI_Matching();
                    break;

                case "ESSAY":
                    LoadUI_Essay();
                    break;

                case "SPEAKING":
                    LoadUI_Speaking();
                    break;
            }
        }


        private FlowLayoutPanel flpChoices;

        private void LoadUI_MCQ()
        {
            flpChoices = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            Button btnAddChoice = new Button()
            {
                Text = "Add Choice",
                Width = 120
            };
            btnAddChoice.Click += (s, e) => AddMCQChoice();

            pnlDynamic.Controls.Add(btnAddChoice);
            pnlDynamic.Controls.Add(flpChoices);

            AddMCQChoice();
        }

        private void AddMCQChoice()
        {
            Panel p = new Panel() { Width = 400, Height = 30 };

            RadioButton rd = new RadioButton() { Left = 0, Top = 5 };
            TextBox txt = new TextBox() { Left = 25, Width = 300 };

            p.Controls.Add(rd);
            p.Controls.Add(txt);
            flpChoices.Controls.Add(p);
        }

        private void LoadUI_FillBlank()
        {
            Label lbl = new Label()
            {
                Text = "Correct Answer:",
                Top = 10
            };

            TextBox txt = new TextBox()
            {
                Top = 35,
                Width = 300,
                Name = "txtFillAnswer"
            };

            pnlDynamic.Controls.Add(lbl);
            pnlDynamic.Controls.Add(txt);
        }

        private DataGridView dgvMatching;

        private void LoadUI_Matching()
        {
            dgvMatching = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false
            };

            dgvMatching.Columns.Add("LeftItem", "Left");
            dgvMatching.Columns.Add("RightItem", "Right");

            pnlDynamic.Controls.Add(dgvMatching);
        }

        private void LoadUI_Essay()
        {
            TextBox txtEssay = new TextBox()
            {
                Multiline = true,
                Dock = DockStyle.Fill
            };

            pnlDynamic.Controls.Add(txtEssay);
        }

        private void LoadUI_Speaking()
        {
            Label lbl = new Label()
            {
                Text = "Prompt Text:",
                Top = 10
            };

            TextBox txt = new TextBox()
            {
                Top = 35,
                Width = 400,
                Height = 100,
                Multiline = true,
                Name = "txtSpeakingPrompt"
            };

            pnlDynamic.Controls.Add(lbl);
            pnlDynamic.Controls.Add(txt);
        }


    }
}