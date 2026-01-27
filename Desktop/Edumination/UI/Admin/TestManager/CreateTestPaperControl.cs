using Edumination.WinForms.UI.Admin.TestManager;
using IELTS.BLL;
using IELTS.DAL;
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

		public CreateTestPaperControl()
		{
			InitializeComponent();
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

			// Validation
			if (string.IsNullOrWhiteSpace(title))
			{
				MessageBox.Show("⚠️ Tiêu đề đề thi không được để trống!",
					"Thiếu Thông Tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtTitle.Focus();
				return;
			}

			if (string.IsNullOrWhiteSpace(selectedPdfPath) || !File.Exists(selectedPdfPath))
			{
				MessageBox.Show("⚠️ Vui lòng chọn file PDF hợp lệ!",
					"Thiếu File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// Xác nhận từ người dùng
			var confirm = MessageBox.Show(
				$"📋 Đề thi: {title}\n" +
				$"📄 File: {Path.GetFileName(selectedPdfPath)}\n" +
				$"🤖 AI sẽ tự động phân tích câu hỏi (có thể mất 10-30 giây)\n\n" +
				"Bạn có chắc chắn muốn tạo đề thi này?",
				"Xác Nhận Tạo Đề Thi",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);

			if (confirm != DialogResult.Yes) return;

			Form progressForm = null;

			try
			{
				// Khóa UI
				btnUpload.Enabled = false;
				btnChooseFile.Enabled = false;
				this.Cursor = Cursors.WaitCursor;

				// Hiển thị progress
				progressForm = new Form
				{
					Text = "Đang xử lý...",
					Size = new Size(400, 150),
					StartPosition = FormStartPosition.CenterParent,
					FormBorderStyle = FormBorderStyle.FixedDialog,
					ControlBox = false
				};
				var progressLabel = new Label
				{
					Text = "🔄 Đang tải file PDF lên server...",
					Dock = DockStyle.Fill,
					TextAlign = ContentAlignment.MiddleCenter,
					Font = new Font("Segoe UI", 10)
				};
				progressForm.Controls.Add(progressLabel);
				progressForm.Show();
				Application.DoEvents();

				// Xử lý file
				string assetsFolder = Path.Combine(Application.StartupPath, "..", "..", "UI", "assets");
				if (!Directory.Exists(assetsFolder))
					Directory.CreateDirectory(assetsFolder);

				string newFileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}.pdf";
				string destPath = Path.Combine(assetsFolder, newFileName);

				await Task.Run(() => File.Copy(selectedPdfPath, destPath, true));

				// Lưu TestPaper
				progressLabel.Text = "💾 Đang lưu thông tin đề thi...";
				Application.DoEvents();

				bool success = await Task.Run(() => testPaperBLL.CreateTestPaper(
					title: title,
					description: description,
					createdBy: currentUserId,
					pdfFullPath: destPath
				));

				if (!success)
				{
					progressForm?.Close();
					MessageBox.Show("❌ Tạo đề thi thất bại! Vui lòng kiểm tra Database.",
						"Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				int newPaperId = testPaperBLL.GetLatestTestPaperId();

				System.Diagnostics.Debug.WriteLine($"=== UPLOAD SUCCESS ===");
				System.Diagnostics.Debug.WriteLine($"📄 PaperId: {newPaperId}");
				System.Diagnostics.Debug.WriteLine($"📝 Title: {title}");
				System.Diagnostics.Debug.WriteLine($"📁 File: {newFileName}");
				System.Diagnostics.Debug.WriteLine($"📂 Path: {destPath}");
				System.Diagnostics.Debug.WriteLine($"======================");

				// Gọi AI phân tích
				progressLabel.Text = "🤖 AI đang phân tích file PDF...\n(Có thể mất 10-30 giây)";
				Application.DoEvents();

				var aiService = new AIQuestionAnalysisService();
				bool aiSuccess = await aiService.AnalyzePdfAndSaveQuestions(destPath, newPaperId);

				progressForm?.Close();
				progressForm = null;

				// ✅ Verify dữ liệu
				var verifyResult = VerifyDataSaved(newPaperId);

				System.Diagnostics.Debug.WriteLine($"=== VERIFY RESULT ===");
				System.Diagnostics.Debug.WriteLine($"Sections: {verifyResult.Sections}");
				System.Diagnostics.Debug.WriteLine($"Questions: {verifyResult.Questions}");
				System.Diagnostics.Debug.WriteLine($"Answers: {verifyResult.Answers}");
				System.Diagnostics.Debug.WriteLine($"=====================");

				if (aiSuccess && verifyResult.Questions > 0)
				{
					MessageBox.Show(
						$"✅ Tạo đề thi thành công!\n\n" +
						$"📊 Paper ID: {newPaperId}\n" +
						$"🤖 AI Analysis: Thành công\n" +
						$"💾 Database:\n" +
						$"   - Sections: {verifyResult.Sections}\n" +
						$"   - Questions: {verifyResult.Questions}\n" +
						$"   - Answers: {verifyResult.Answers}\n\n" +
						"📝 Bạn có thể xem và chỉnh sửa ở bước tiếp theo.",
						"Thành Công",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);
				}
				else if (aiSuccess && verifyResult.Questions == 0)
				{
					MessageBox.Show(
						$"⚠️ Cảnh báo!\n\n" +
						$"📊 Paper ID: {newPaperId}\n" +
						$"🤖 AI phân tích thành công nhưng KHÔNG lưu được câu hỏi vào DB!\n\n" +
						$"💾 Database:\n" +
						$"   - Sections: {verifyResult.Sections}\n" +
						$"   - Questions: {verifyResult.Questions} ❌\n" +
						$"   - Answers: {verifyResult.Answers} ❌\n\n" +
						"Kiểm tra Console Output để debug.",
						"Lỗi Lưu Database",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
				}
				else
				{
					MessageBox.Show(
						$"⚠️ AI không phân tích được file PDF!\n\n" +
						$"📊 Paper ID: {newPaperId}\n" +
						$"💾 Database: {verifyResult.Sections} sections, {verifyResult.Questions} questions\n\n" +
						"Nguyên nhân có thể:\n" +
						"• File không phải đề thi IELTS Reading\n" +
						"• PDF bị mã hóa hoặc scan chất lượng kém\n" +
						"• Định dạng không chuẩn\n\n" +
						"📝 Bạn cần thêm câu hỏi thủ công.",
						"AI Phân Tích Thất Bại",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
				}

				// Chuyển sang bước tiếp theo
				ResetForm();
				testManagerControl.GetAddSectionButtonControl().SetTestPaperId(newPaperId);
				testManagerControl.ShowPanel(testManagerControl.GetAddSectionButtonControl());
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"❌ Exception: {ex.Message}");
				System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");

				progressForm?.Close();

				MessageBox.Show(
					$"❌ Lỗi hệ thống:\n\n{ex.Message}\n\n" +
					"Chi tiết lỗi đã được ghi vào Console Output.",
					"Lỗi",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
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
	}
	public class VerifyResult
	{
		public int Sections { get; set; }
		public int Questions { get; set; }
		public int Answers { get; set; }
	}
}