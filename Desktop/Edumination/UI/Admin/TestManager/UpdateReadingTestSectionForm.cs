using AxAcroPDFLib;
using Edumination.WinForms.UI.Admin.TestManager;
using IELTS.BLL;
using IELTS.DAL;
using IELTS.DTO;
using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.Admin.TestManager
{
    public partial class UpdateReadingTestSectionForm : Form
    {
        #region Private Fields

        private long TestPaperId;
        private TestSectionBLL _testSectionBLL;
        private string _connectionString;

        private TestSectionDTO _currentSection;
        private List<QuestionDTO> _questions;

        // UI Storage
        private string[] answerTypes = new string[41];
        private string[] answers = new string[41];
        private int[] questionGroupStart = new int[41]; // Lưu câu bắt đầu của nhóm
        private int[] questionGroupEnd = new int[41];   // Lưu câu kết thúc của nhóm
        private Dictionary<int, int> questionRanges = new Dictionary<int, int>();

        private Button selectedButton = null;
        private UIPanel formCard;
        private string currentPdfPath = "";

        #endregion

        #region Constructor & Load

        public UpdateReadingTestSectionForm(long testPaperId)
        {
            InitializeComponent();

            TestPaperId = testPaperId;
            _testSectionBLL = new TestSectionBLL();

            InitializeUI();
            LoadExistingData();
        }

        private void InitializeUI()
        {
            LoadQuestionTypes();

            // ⭐ Đảm bảo PDF viewer ở phía trước
            if (axAcroPDFViewer != null)
            {
                axAcroPDFViewer.BringToFront();
                axAcroPDFViewer.Visible = true;
            }

            // Setup event handlers
            foreach (Control c in this.Controls)
            {
                if (c is Button && c.Name.StartsWith("btnQ"))
                {
                    c.Click += QuestionButton_Click;
                }
            }
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

        #endregion

        #region Load Existing Data

        private void LoadExistingData()
        {
            try
            {
                var sections = _testSectionBLL.GetTestSectionsByPaperId(TestPaperId);
                var readingSection = sections.FirstOrDefault(s => s.Skill == "READING");

                if (readingSection == null)
                {
                    MessageBox.Show("No Reading section found for this test paper!",
                        "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                _currentSection = _testSectionBLL.GetTestSectionWithQuestions(readingSection.Id);

                if (_currentSection == null)
                {
                    MessageBox.Show("Failed to load section data!",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ⭐ Load PDF
                if (!string.IsNullOrEmpty(_currentSection.PdfFileName))
                {
                    if (!string.IsNullOrEmpty(_currentSection.PdfFilePath) &&
                        File.Exists(_currentSection.PdfFilePath))
                    {
                        currentPdfPath = _currentSection.PdfFilePath;
                    }
                    else
                    {
                        currentPdfPath = Path.Combine(Application.StartupPath, "UI", "assets", _currentSection.PdfFileName);

                        if (!File.Exists(currentPdfPath))
                        {
                            MessageBox.Show($"PDF file not found!\n\nExpected path:\n{currentPdfPath}\n\nPlease check the file location.",
                                "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    LoadPdf(currentPdfPath);
                    txtFileName.Text = _currentSection.PdfFileName;
                    lblPdfName.Text = _currentSection.PdfFileName ?? "PDF Loaded";
                }

                // Load Questions
                _questions = _currentSection.Questions ?? new List<QuestionDTO>();
                LoadQuestionsToUI();

                lblStatus.Text = $"✓ Loaded {_questions.Count} questions";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}\n\nStack: {ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadQuestionsToUI()
        {
            // Clear current data
            Array.Clear(answerTypes, 0, answerTypes.Length);
            Array.Clear(answers, 0, answers.Length);
            Array.Clear(questionGroupStart, 0, questionGroupStart.Length);
            Array.Clear(questionGroupEnd, 0, questionGroupEnd.Length);
            questionRanges.Clear();

            // Track processed positions for group questions
            HashSet<int> processedPositions = new HashSet<int>();

            foreach (var question in _questions.OrderBy(q => q.Position))
            {
                int pos = question.Position;

                if (processedPositions.Contains(pos))
                    continue;

                // Store question type and answer
                answerTypes[pos] = question.QuestionType;
                answers[pos] = ParseAnswerFromJson(question.QuestionType, question.AnswerData);

                // Mark button as saved
                Button btn = this.Controls.Find($"btnQ{pos}", true).FirstOrDefault() as Button;
                if (btn != null)
                {
                    btn.BackColor = Color.LightGreen;
                }

                // Handle group questions (MATCHING, ORDERING, MULTI_SELECT)
                if (IsGroupQuestion(question.QuestionType))
                {
                    // Find end position
                    var groupQuestions = _questions
                        .Where(q => q.QuestionType == question.QuestionType
                                 && q.QuestionText == question.QuestionText
                                 && q.Position >= pos)
                        .OrderBy(q => q.Position)
                        .ToList();

                    if (groupQuestions.Count > 1)
                    {
                        int endPos = groupQuestions.Last().Position;
                        questionRanges[pos] = endPos;

                        // Build group answer
                        string groupAnswer = BuildGroupAnswer(question.QuestionType, groupQuestions);
                        answers[pos] = groupAnswer;

                        // ⭐ Lưu thông tin nhóm
                        for (int i = pos; i <= endPos; i++)
                        {
                            answerTypes[i] = question.QuestionType;
                            answers[i] = groupAnswer;
                            questionGroupStart[i] = pos;
                            questionGroupEnd[i] = endPos;
                            processedPositions.Add(i);

                            // Mark all buttons in range
                            Button rangeBtn = this.Controls.Find($"btnQ{i}", true).FirstOrDefault() as Button;
                            if (rangeBtn != null)
                            {
                                rangeBtn.BackColor = Color.LightGreen;
                            }
                        }
                    }
                    else
                    {
                        processedPositions.Add(pos);
                    }
                }
                else
                {
                    processedPositions.Add(pos);
                }
            }
        }

        private bool IsGroupQuestion(string questionType)
        {
            return questionType == "MATCHING"
                || questionType == "ORDERING"
                || questionType == "MULTI_SELECT";
        }

        private string BuildGroupAnswer(string questionType, List<QuestionDTO> groupQuestions)
        {
            if (questionType == "MATCHING" || questionType == "ORDERING")
            {
                // ⭐ Parse từ JSON format: {"2":"E","3":"D","4":"C","5":"B","6":"A"}
                // Build thành: "2:E;3:D;4:C;5:B;6:A"

                if (groupQuestions.Count > 0 && !string.IsNullOrEmpty(groupQuestions[0].AnswerData))
                {
                    try
                    {
                        var answerJson = groupQuestions[0].AnswerData;
                        var answerDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(answerJson);

                        if (answerDict != null && answerDict.Count > 0)
                        {
                            var pairs = answerDict
                                .OrderBy(kv => int.Parse(kv.Key))
                                .Select(kv => $"{kv.Key}:{kv.Value}");
                            return string.Join(";", pairs);
                        }
                    }
                    catch
                    {
                        // Fallback: lấy từng câu
                        var pairs = groupQuestions.Select(q =>
                            $"{q.Position}:{ParseAnswerFromJson(q.QuestionType, q.AnswerData)}");
                        return string.Join(";", pairs);
                    }
                }
            }
            else if (questionType == "MULTI_SELECT")
            {
                // ⭐ Parse từ JSON format: {"answers": ["A", "C", "E"]}
                // Build thành: "A,C,E" cho mỗi câu, ngăn cách bởi "|"

                var items = groupQuestions.Select(q =>
                {
                    if (!string.IsNullOrEmpty(q.AnswerData))
                    {
                        try
                        {
                            dynamic obj = JsonConvert.DeserializeObject(q.AnswerData);
                            if (obj.answers != null)
                            {
                                var answers = new List<string>();
                                foreach (var ans in obj.answers)
                                {
                                    answers.Add(ans.ToString());
                                }
                                return string.Join(",", answers);
                            }
                        }
                        catch { }
                    }
                    return "";
                });

                return string.Join("|", items.Where(s => !string.IsNullOrEmpty(s)));
            }

            return "";
        }

        private string ParseAnswerFromJson(string questionType, string answerJson)
        {
            if (string.IsNullOrEmpty(answerJson))
                return "";

            try
            {
                switch (questionType)
                {
                    case "MCQ":
                    case "FILL_BLANK":
                    case "SHORT_ANSWER":
                    case "ESSAY":
                        // ⭐ Format: {"answer": "B"} hoặc {"answer":"lamm"}
                        dynamic obj = JsonConvert.DeserializeObject(answerJson);
                        return obj.answer?.ToString() ?? "";

                    case "MULTI_SELECT":
                        // ⭐ Format: {"answers": ["A", "C", "E"]}
                        dynamic multiObj = JsonConvert.DeserializeObject(answerJson);
                        if (multiObj.answers != null)
                        {
                            var answers = new List<string>();
                            foreach (var ans in multiObj.answers)
                            {
                                answers.Add(ans.ToString());
                            }
                            return string.Join(",", answers);
                        }
                        return "";

                    case "MATCHING":
                    case "ORDERING":
                        // ⭐ Format: {"2":"E","3":"D","4":"C","5":"B","6":"A"}
                        // Trả về cả dictionary dạng string để hiển thị
                        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(answerJson);
                        if (dict != null && dict.Count > 0)
                        {
                            var pairs = dict
                                .OrderBy(kv => int.Parse(kv.Key))
                                .Select(kv => $"{kv.Key}:{kv.Value}");
                            return string.Join(";", pairs);
                        }
                        return "";

                    case "SPEAK_PROMPT":
                        dynamic speakObj = JsonConvert.DeserializeObject(answerJson);
                        return speakObj.audioPath?.ToString() ?? "";

                    default:
                        return "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing answer JSON: {ex.Message}\nJSON: {answerJson}");
                return "";
            }
        }

        #endregion

        #region Question Selection

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

        private void QuestionButton_Click(object sender, EventArgs e)
        {
            Button clicked = sender as Button;

            // Clear previous selection
            if (selectedButton != null)
            {
                selectedButton.FlatStyle = FlatStyle.Standard;
                selectedButton.FlatAppearance.BorderSize = 1;
                selectedButton.FlatAppearance.BorderColor = SystemColors.ControlDark;
            }

            // Set new selection
            selectedButton = clicked;
            clicked.FlatStyle = FlatStyle.Flat;
            clicked.FlatAppearance.BorderSize = 2;
            clicked.FlatAppearance.BorderColor = Color.Blue;

            txtSelectedButton.Text = clicked.Text;

            // Load existing data if available
            if (int.TryParse(clicked.Text, out int i))
            {
                if (i >= 1 && i <= 40 && !string.IsNullOrEmpty(answerTypes[i]))
                {
                    cboQuestionType.Text = answerTypes[i];
                    lblCorrect.Text = "Correct Answer: " + answers[i];

                    // ⭐ Nếu là câu hỏi nhóm, hiển thị range
                    if (IsGroupQuestion(answerTypes[i]))
                    {
                        int groupStart = questionGroupStart[i];
                        int groupEnd = questionGroupEnd[i];

                        if (groupStart > 0 && groupEnd > 0)
                        {
                            txtSelectedButton.Text = groupStart.ToString();
                            nmEnd.Value = groupEnd;
                        }
                    }
                    else
                    {
                        nmEnd.Value = i;
                    }

                    // Trigger UI load
                    CboQuestionType_SelectedIndexChanged(cboQuestionType, EventArgs.Empty);
                }
                else
                {
                    cboQuestionType.Text = "";
                    pnlDynamic.Controls.Clear();
                    lblCorrect.Text = "Correct";
                    nmEnd.Value = i;
                }
            }
        }

        #endregion

        #region Question Type Change

        private void CboQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlDynamic.Controls.Clear();

            if (!int.TryParse(txtSelectedButton.Text, out int questionNum))
                return;

            string selectedType = cboQuestionType.SelectedItem?.ToString();
            string questionText;

            // ⭐ Tạo tiêu đề phù hợp cho câu hỏi nhóm hoặc đơn
            if (IsGroupQuestion(selectedType))
            {
                int groupEnd = (int)nmEnd.Value;
                questionText = $"Question {questionNum} - {groupEnd}";
            }
            else
            {
                questionText = "Question " + questionNum;
            }

            string savedAnswer = (questionNum >= 1 && questionNum <= 40)
                ? answers[questionNum]
                : null;

            // Show/Hide end position control
            bool isGroupQuestion = IsGroupQuestion(selectedType);

            lblEnd.Visible = isGroupQuestion;
            nmEnd.Visible = isGroupQuestion;

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

        #endregion

        #region Load UI Methods

        private void LoadUI_MCQ(string questionText, string savedAnswer = null)
        {
            formCard = QuestionCardGenerator.CreateQuestionCard(
                1, "MCQ", questionText, null, savedAnswer);
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private void LoadUI_MultiSelect(string questionText, string savedAnswer = null)
        {
            int.TryParse(txtSelectedButton.Text, out int start);
            int end = (int)nmEnd.Value;


            // Tạo danh sách options A, B, C, D...
            int count =10;
            //List<string> options = new List<string>();
            //for (int i = 0; i < count; i++)
            //{
            //    options.Add(((char)('A' + i)).ToString());
            //}

            formCard = QuestionCardGenerator.CreateQuestionCard(
                1, "MULTI_SELECT", questionText, count, savedAnswer);
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private void LoadUI_FillBlank(string questionText, string savedAnswer = null)
        {
            formCard = QuestionCardGenerator.CreateQuestionCard(
                1, "FILL_BLANK", questionText, null, savedAnswer);
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private void LoadUI_Matching(string questionText, string savedAnswer = null)
        {
            int.TryParse(txtSelectedButton.Text, out int start);
            int end = (int)nmEnd.Value;

            Dictionary<string, List<string>> matchData = GenerateMatchData(start, end);
            formCard = QuestionCardGenerator.CreateQuestionCard(
                1, "MATCHING", questionText, matchData, savedAnswer);
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private void LoadUI_Ordering(string questionText, string savedAnswer = null)
        {
            int.TryParse(txtSelectedButton.Text, out int start);
            int end = (int)nmEnd.Value;

            int count = end - start + 1;
            List<string> options = new List<string>();
            for (int i = 0; i < count; i++)
            {
                options.Add(((char)('A' + i)).ToString());
            }

            formCard = QuestionCardGenerator.CreateQuestionCard(
                1, "ORDERING", questionText, options, savedAnswer);
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private void LoadUI_ShortAnswer(string questionText, string savedAnswer = null)
        {
            formCard = QuestionCardGenerator.CreateQuestionCard(
                1, "SHORT_ANSWER", questionText, null, savedAnswer);
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private void LoadUI_Essay(string questionText, string savedAnswer = null)
        {
            formCard = QuestionCardGenerator.CreateQuestionCard(
                1, "ESSAY", questionText, null, savedAnswer);
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private void LoadUI_Speaking(string questionText, string savedAnswer = null)
        {
            formCard = QuestionCardGenerator.CreateQuestionCard(
                1, "SPEAK_PROMPT", questionText, null, savedAnswer);
            formCard.Location = new Point(10, 0);
            pnlDynamic.Controls.Add(formCard);
        }

        private Dictionary<string, List<string>> GenerateMatchData(int x, int y)
        {
            int count = y - x + 1;
            List<string> options = new List<string>();
            for (int i = 0; i < count; i++)
            {
                options.Add(((char)('A' + i)).ToString());
            }

            Dictionary<string, List<string>> matchData = new Dictionary<string, List<string>>();
            for (int i = x; i <= y; i++)
            {
                matchData.Add(i.ToString(), new List<string>(options));
            }

            return matchData;
        }

        #endregion

        #region PDF Management

        private void btnChangePdf_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF Files|*.pdf";
                ofd.Title = "Select new PDF file";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    currentPdfPath = ofd.FileName;
                    LoadPdf(currentPdfPath);
                    lblPdfName.Text = Path.GetFileName(currentPdfPath);

                    MessageBox.Show("PDF file changed successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void LoadPdf(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("PDF path is empty!");
                return;
            }

            if (!File.Exists(path))
            {
                MessageBox.Show($"PDF file not found!\nPath: {path}");
                return;
            }

            try
            {
                // Đóng file cũ trước
                try
                {
                    axAcroPDFViewer.src = "";
                }
                catch { }

                // Load file mới
                bool ok = axAcroPDFViewer.LoadFile(path);

                if (!ok)
                {
                    axAcroPDFViewer.src = path;
                }

                axAcroPDFViewer.Visible = true;
                axAcroPDFViewer.BringToFront();
                axAcroPDFViewer.setShowToolbar(true);
                axAcroPDFViewer.setShowScrollbars(true);
                axAcroPDFViewer.setView("FitWidth");
                axAcroPDFViewer.setLayoutMode("SinglePage");
                axAcroPDFViewer.Refresh();
                this.Refresh();

                MessageBox.Show($"PDF Loaded: {ok}\nPath: {path}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading PDF: {ex.Message}\n\nPath: {path}\n\nStack: {ex.StackTrace}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Save & Update

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSelectedButton.Text, out int i) || i < 1 || i > 40)
            {
                MessageBox.Show("Please select a question number!",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedValue = cboQuestionType.Text?.Trim();

            if (string.IsNullOrEmpty(selectedValue))
            {
                MessageBox.Show("Please select a question type!",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (formCard == null)
            {
                MessageBox.Show("No question form found!",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string answer = QuestionCardGenerator.GetStudentAnswer(formCard, selectedValue);

                // Handle group questions
                bool isGroupQuestion = IsGroupQuestion(selectedValue);

                if (isGroupQuestion)
                {
                    int start = i;
                    int endPos = (int)nmEnd.Value;

                    if (endPos < start)
                    {
                        MessageBox.Show("Câu kết thúc phải lớn hơn hoặc bằng câu bắt đầu!");
                        return;
                    }

                    questionRanges[start] = endPos;

                    // Lưu cho tất cả các câu trong nhóm
                    for (int pos = start; pos <= endPos; pos++)
                    {
                        answerTypes[pos] = selectedValue;
                        answers[pos] = answer;
                        questionGroupStart[pos] = start;
                        questionGroupEnd[pos] = endPos;

                        Button btn = this.Controls.Find($"btnQ{pos}", true).FirstOrDefault() as Button;
                        if (btn != null)
                        {
                            btn.BackColor = Color.LightBlue;
                        }
                    }

                    MessageBox.Show($"✅ Questions {start} - {endPos} updated!\n\nType: {selectedValue}\nAnswer: {answer}",
                        "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    answerTypes[i] = selectedValue;
                    answers[i] = answer;
                    questionGroupStart[i] = 0;
                    questionGroupEnd[i] = 0;

                    if (selectedButton != null)
                    {
                        selectedButton.BackColor = Color.LightBlue;
                    }

                    MessageBox.Show($"✅ Question {i} updated!\n\nType: {selectedValue}\nAnswer: {answer}",
                        "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                lblCorrect.Text = "Correct Answer: " + answer;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentSection == null)
                {
                    MessageBox.Show("No section loaded!",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool hasChanges = false;
                for (int i = 1; i <= 40; i++)
                {
                    Button btn = this.Controls.Find($"btnQ{i}", true).FirstOrDefault() as Button;
                    if (btn != null && btn.BackColor == Color.LightBlue)
                    {
                        hasChanges = true;
                        break;
                    }
                }

                if (!hasChanges && currentPdfPath == _currentSection.PdfFilePath)
                {
                    MessageBox.Show("No changes detected!",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Are you sure you want to update this Reading section?",
                    "Confirm Update",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                // Delete existing questions
                foreach (var question in _questions)
                {
                    _testSectionBLL.DeleteQuestion(question.Id);
                }

                // Update section info
                _currentSection.PdfFileName = !string.IsNullOrEmpty(currentPdfPath)
                    ? Path.GetFileName(currentPdfPath)
                    : _currentSection.PdfFileName;
                _currentSection.PdfFilePath = currentPdfPath;

                _testSectionBLL.UpdateTestSection(_currentSection);

                // Insert new questions
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

                bool success = _testSectionBLL.SaveQuestionsToSection(
                    _currentSection.Id,
                    questionTypesDict,
                    answersDict,
                    questionRanges
                );

                if (success)
                {
                    MessageBox.Show("✅ Section updated successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadExistingData();
                }
                else
                {
                    MessageBox.Show("❌ Failed to update section!",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSelectedButton.Text, out int i) || i < 1 || i > 40)
            {
                MessageBox.Show("Please select a question to delete!",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Are you sure you want to delete Question {i}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            // Check if it's a group question
            if (IsGroupQuestion(answerTypes[i]))
            {
                int start = questionGroupStart[i];
                int end = questionGroupEnd[i];

                // Clear entire group
                for (int pos = start; pos <= end; pos++)
                {
                    answerTypes[pos] = null;
                    answers[pos] = null;
                    questionGroupStart[pos] = 0;
                    questionGroupEnd[pos] = 0;

                    Button btn = this.Controls.Find($"btnQ{pos}", true).FirstOrDefault() as Button;
                    if (btn != null)
                    {
                        btn.BackColor = SystemColors.Control;
                    }
                }

                if (questionRanges.ContainsKey(start))
                {
                    questionRanges.Remove(start);
                }
            }
            else
            {
                // Clear single question
                answerTypes[i] = null;
                answers[i] = null;

                if (selectedButton != null)
                {
                    selectedButton.BackColor = SystemColors.Control;
                }
            }

            pnlDynamic.Controls.Clear();
            cboQuestionType.Text = "";
            lblCorrect.Text = "Correct";

            MessageBox.Show($"Question {i} deleted!",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }
}