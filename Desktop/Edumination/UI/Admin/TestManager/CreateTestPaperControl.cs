using Edumination.WinForms.UI.Admin.TestManager;
using IELTS.BLL;
using IELTS.DAL;
using IELTS.DTO;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;

namespace IELTS.UI.Admin.TestManager
{
    // ✅ Class helper - đặt NGOÀI class CreateTestPaperControl


    public partial class CreateTestPaperControl : UserControl
    {
        private TestManagerControl testManagerControl;
        private TestPaperBLL testPaperBLL = new TestPaperBLL();
        private string selectedPdfPath = "";
        private string selectedAudioPath = "";
        private readonly MockTestBLL _mockTestBLL = new MockTestBLL();

        public TestManagerControl TestManagerControl { get; set; }

        public CreateTestPaperControl()
        {
            InitializeComponent();
            LoadTestMonths();
            LoadMockTests();
        }

        public TestManagerControl GetTestManagerControl() { return testManagerControl; }

        public void SetManagerControl(TestManagerControl testManagerControl)
        {
            this.testManagerControl = testManagerControl;
        }

        internal void SetTestPaperControl(TestManagerControl testManagerControl)
        {
            this.testManagerControl = testManagerControl;
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF Files|*.pdf";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedPdfPath = ofd.FileName;
                    txtFileName.Text = Path.GetFileName(selectedPdfPath);
                    LoadPdf(selectedPdfPath);
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

        private void ClearPdf()
        {
            axAcroPDFViewer.src = "about:blank";
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string description = txtDescription.Text.Trim();
            long currentUserId = SessionManager.CurrentUserId;

            // 1. Validation cơ bản
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("⚠️ Tiêu đề đề thi không được để trống!", "Thiếu Thông Tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(selectedPdfPath) || !File.Exists(selectedPdfPath))
            {
                MessageBox.Show("⚠️ Vui lòng chọn file PDF hợp lệ!", "Thiếu File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"📋 Đề thi: {title}\n📄 File: {Path.GetFileName(selectedPdfPath)}\n🤖 AI sẽ tự động phân tích câu hỏi.\n\nBạn có chắc chắn muốn tạo đề thi này?",
                "Xác Nhận Tạo Đề Thi", MessageBoxButtons.YesNo, MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes) return;

            Form progressForm = null;

            try
            {
                btnUpload.Enabled = false;
                btnChooseFile.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                progressForm = new Form { Text = "Đang xử lý...", Size = new Size(400, 150), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, ControlBox = false };
                var progressLabel = new Label { Text = "🔄 Đang tải file PDF lên server...", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10) };
                progressForm.Controls.Add(progressLabel);
                progressForm.Show();
                Application.DoEvents();

                // 2. Xử lý lưu file PDF
                string assetsFolder = Path.Combine(Application.StartupPath, "..", "..", "UI", "assets");
                if (!Directory.Exists(assetsFolder)) Directory.CreateDirectory(assetsFolder);

                string newFileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.pdf";
                string destPath = Path.Combine(assetsFolder, newFileName);
                await Task.Run(() => File.Copy(selectedPdfPath, destPath, true));

                // 3. Lưu TestPaper vào Database
                progressLabel.Text = "💾 Đang lưu thông tin đề thi...";
                Application.DoEvents();

                bool success = await Task.Run(() => testPaperBLL.CreateTestPaper(title, description, currentUserId, destPath));

                if (!success)
                {
                    progressForm?.Close();
                    MessageBox.Show("❌ Tạo đề thi thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int newPaperId = testPaperBLL.GetLatestTestPaperId();

                // 4. Gọi AI phân tích file PDF
                progressLabel.Text = "🤖 AI đang phân tích file PDF...\n(Có thể mất 10-30 giây)";
                Application.DoEvents();

                var aiService = new AIQuestionAnalysisService();
                // Gọi AI phân tích lần đầu để xác định kỹ năng
                bool aiSuccess = await aiService.AnalyzePdfAndSaveQuestions(destPath, newPaperId);

                // --- PHẦN SỬA MỚI: KIỂM TRA VÀ YÊU CẦU FILE ÂM THANH ---
                if (IsListeningWithoutAudio(newPaperId))
                {
                    progressForm.Hide(); // Tạm ẩn form chờ để hiện hộp thoại chọn file
                    MessageBox.Show("🤖 AI xác định đây là đề LISTENING. Vui lòng chọn file âm thanh cho bài thi này!",
                                    "Yêu cầu bổ sung", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Audio Files|*.mp3;*.wav" })
                    {
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            string audioFolder = Path.Combine(assetsFolder, "audios");
                            if (!Directory.Exists(audioFolder)) Directory.CreateDirectory(audioFolder);

                            // FIX: Tạo tên file an toàn (Không dấu, không khoảng trắng)
                            string extension = Path.GetExtension(ofd.FileName);
                            string safeFileName = $"{DateTime.Now:yyyyMMdd_HHmm}_{Guid.NewGuid().ToString().Substring(0, 8)}{extension}";

                            string destAudio = Path.Combine(audioFolder, safeFileName);
                            File.Copy(ofd.FileName, destAudio, true);

                            // Cập nhật đường dẫn (chỉ lưu tên file an toàn hoặc đường dẫn tương đối) vào Database
                            UpdateAudioPathInSection(newPaperId, safeFileName);
                        }
                    }
                    progressForm.Show();
                }
                // --- KẾT THÚC PHẦN SỬA MỚI ---

                progressForm?.Close();
                progressForm = null;

                // 5. Verify và hiển thị kết quả
                var verifyResult = VerifyDataSaved(newPaperId);

                if (aiSuccess && verifyResult.Questions > 0)
                {
                    MessageBox.Show($"✅ Tạo đề thi thành công!\n📊 Paper ID: {newPaperId}\n🤖 AI Analysis: Thành công", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("⚠️ AI không phân tích được hoặc dữ liệu trống. Bạn cần kiểm tra lại nội dung.", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Reset và chuyển trang
                ResetForm();
                testManagerControl.GetAddSectionButtonControl().SetTestPaperId(newPaperId);
                testManagerControl.ShowPanel(testManagerControl.GetAddSectionButtonControl());
            }
            catch (Exception ex)
            {
                progressForm?.Close();
                MessageBox.Show($"❌ Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressForm?.Close();
                btnUpload.Enabled = true;
                btnChooseFile.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }
        // ✅ CHỈ GIỮ 1 METHOD DUY NHẤT
        private VerifyResult VerifyDataSaved(int paperId)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    int sections = 0, questions = 0, answers = 0;

                    // Count sections
                    string sqlSections = "SELECT COUNT(*) FROM TestSections WHERE PaperId = @PaperId";
                    using (var cmdSections = new SqlCommand(sqlSections, conn))
                    {
                        cmdSections.Parameters.AddWithValue("@PaperId", paperId);
                        sections = (int)cmdSections.ExecuteScalar();
                    }

                    // Count questions
                    string sqlQuestions = @"
                        SELECT COUNT(*) 
                        FROM Questions q 
                        INNER JOIN TestSections ts ON q.SectionId = ts.Id 
                        WHERE ts.PaperId = @PaperId";
                    using (var cmdQuestions = new SqlCommand(sqlQuestions, conn))
                    {
                        cmdQuestions.Parameters.AddWithValue("@PaperId", paperId);
                        questions = (int)cmdQuestions.ExecuteScalar();
                    }

                    // Count answers
                    string sqlAnswers = @"
                        SELECT COUNT(*) 
                        FROM QuestionAnswerKeys qak
                        INNER JOIN Questions q ON qak.QuestionId = q.Id
                        INNER JOIN TestSections ts ON q.SectionId = ts.Id
                        WHERE ts.PaperId = @PaperId";
                    using (var cmdAnswers = new SqlCommand(sqlAnswers, conn))
                    {
                        cmdAnswers.Parameters.AddWithValue("@PaperId", paperId);
                        answers = (int)cmdAnswers.ExecuteScalar();
                    }

                    return new VerifyResult
                    {
                        Sections = sections,
                        Questions = questions,
                        Answers = answers
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Verify error: {ex.Message}");
                return new VerifyResult { Sections = 0, Questions = 0, Answers = 0 };
            }
        }

        public void ResetForm()
        {
            txtFileName.Text = "";
            txtTitle.Text = "";
            txtDescription.Text = "";
            ClearPdf();
        }

        private async void btnListModels_Click(object sender, EventArgs e)
        {
            try
            {
                string apiKey = ConfigurationManager.AppSettings["GeminiApiKey"];
                string url = $"https://generativelanguage.googleapis.com/v1beta/models?key={apiKey}";

                var client = new HttpClient();
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                dynamic result = JsonConvert.DeserializeObject(json);

                var sb = new StringBuilder("📋 Models Khả Dụng:\n\n");

                if (result?.models != null)
                {
                    foreach (var model in result.models)
                    {
                        string name = model.name.ToString().Replace("models/", "");
                        string displayName = model.displayName?.ToString() ?? "";

                        var methods = model.supportedGenerationMethods?.ToString() ?? "";
                        if (methods.Contains("generateContent"))
                        {
                            sb.AppendLine($"✅ {name}");
                            sb.AppendLine($"   Display: {displayName}");
                            sb.AppendLine();
                        }
                    }
                }

                MessageBox.Show(sb.ToString(), "Danh Sách Models");
                Clipboard.SetText(sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi");
            }
        }

        private void btnChooseAudio_Click(object sender, EventArgs e)
        {
            // Sử dụng OpenFileDialog để mở cửa sổ chọn file trên máy tính
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                // Thiết lập bộ lọc chỉ hiển thị các định dạng âm thanh phổ biến
                ofd.Filter = "Audio Files|*.mp3;*.wav;*.m4a";
                ofd.Title = "Chọn file âm thanh cho bài Listening";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Lưu đường dẫn file vào biến toàn cục để dùng khi bấm nút Upload
                    selectedAudioPath = ofd.FileName;

                    // Thông báo cho người dùng biết đã chọn thành công
                    MessageBox.Show("✅ Đã chọn file âm thanh: " + Path.GetFileName(selectedAudioPath),
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Nếu bạn có một TextBox để hiển thị tên file, hãy gán giá trị tại đây
                    // txtAudioFileName.Text = Path.GetFileName(selectedAudioPath);
                }
            }
        }

        // Hàm này dùng để cập nhật đường dẫn file âm thanh vào Database sau khi Admin chọn file
        private void UpdateAudioPathInSection(int paperId, string audioPath)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection()) // Sử dụng kết nối từ DAL
                {
                    conn.Open();
                    // Câu lệnh SQL tác động trực tiếp vào bảng TestSections dựa trên PaperId và Skill Listening
                    string sql = "UPDATE TestSections SET AudioFilePath = @path WHERE PaperId = @id AND Skill = 'LISTENING'";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@path", audioPath); // Đường dẫn file đã copy vào assets
                        cmd.Parameters.AddWithValue("@id", paperId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật đường dẫn âm thanh: {ex.Message}", "Lỗi DB");
            }
        }

        // Hàm bổ trợ để kiểm tra xem Section có phải Listening và đang thiếu Audio không
        private bool IsListeningWithoutAudio(int paperId)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT COUNT(*) FROM TestSections WHERE PaperId = @id AND Skill = 'LISTENING' AND (AudioFilePath IS NULL OR AudioFilePath = '')";
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", paperId);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch { return false; }
        }

        private void btnCreate_Click(object sender, EventArgs e)

        {
            MessageBox.Show($"UserId = {testManagerControl.UserId}");

            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter title");
                return;
            }

            if (cboMockTest.SelectedIndex == -1 || cmbTestMonth.SelectedIndex == -1)
            {
                MessageBox.Show("Please select mock test and month");
                return;
            }

            TestPaperDTO paper = new TestPaperDTO
            {
                Title = txtTitle.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                PdfFileName = txtFileName.Text,
                PdfFilePath = selectedPdfPath,
                MockTestId = (long)cboMockTest.SelectedValue,
                TestMonth = (int)cmbTestMonth.SelectedItem,
                CreatedAt = DateTime.Now,
                CreatedBy = testManagerControl.UserId
            };

            long newId = testPaperBLL.Insert(paper);

            MessageBox.Show("Create Test Paper successfully!");

            // Load lại AllSkillsTestManager
            testManagerControl.AllSkillsTestControl.LoadData();
            testManagerControl.ShowPanel(testManagerControl.AllSkillsTestControl);
        }


        private void LoadTestMonths()
        {
            cmbTestMonth.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                cmbTestMonth.Items.Add(i);
            }
            cmbTestMonth.SelectedIndex = -1;
        }

        private void LoadMockTests()
        {
            var data = _mockTestBLL.GetAll();

            cboMockTest.DisplayMember = "Year";
            cboMockTest.ValueMember = "Id";
            cboMockTest.DataSource = data;

            cboMockTest.SelectedIndex = -1;
        }


    }
    public class VerifyResult
	{
		public int Sections { get; set; }
		public int Questions { get; set; }
		public int Answers { get; set; }
	}
}