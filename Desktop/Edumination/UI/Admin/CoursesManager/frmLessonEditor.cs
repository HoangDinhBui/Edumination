using IELTS.BLL;
using IELTS.DTO;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace IELTS.UI.Admin.CoursesManager
{
	public partial class frmLessonEditor : Sunny.UI.UIForm
	{
		private long _targetId; // Là CourseId nếu thêm mới, là LessonId nếu sửa
		private bool _isEdit;
		private readonly LessonBLL _lessonBll;

		public frmLessonEditor(long id, bool isEdit = false)
		{
			InitializeComponent();
			_lessonBll = new LessonBLL(); // Khởi tạo BLL
			this._targetId = id;
			this._isEdit = isEdit;

			CreateQuestionTabs();
			// Đăng ký sự kiện nút bấm
			this.btnSelectVideo.Click += btnSelectVideo_Click;
			this.btnSave.Click += btnSave_Click;

			if (_isEdit)
			{
				this.Text = "CHỈNH SỬA BÀI HỌC";
				LoadOldData();
			}
			else
			{
				this.Text = "THÊM BÀI HỌC MỚI";
				// Nếu thêm mới, mặc định tạo 1 bài test trống trong DAL
			}
		}

		private void LoadOldData()
		{
			try
			{
				// Lấy thông tin bài học từ BLL
				var lesson = _lessonBll.GetById(_targetId);
				if (lesson != null)
				{
					txtTitle.Text = lesson.Title;
					txtVideoPath.Text = lesson.VideoFilePath;
					numPosition.Value = lesson.Position;

					// Load 10 câu hỏi trắc nghiệm nếu có
					var questions = _lessonBll.GetQuestionsByLessonId(_targetId);
					// Ở đây bạn sẽ viết code để đổ questions vào các Tab nhập liệu
				}
			}
			catch (Exception ex)
			{
				UIMessageBox.ShowError("Lỗi tải dữ liệu: " + ex.Message);
			}
		}

		private void btnSelectVideo_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog())
			{
				ofd.Filter = "Video Files (*.mp4;*.avi;*.mkv)|*.mp4;*.avi;*.mkv";
				ofd.Title = "Chọn video bài học";

				if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					txtVideoPath.Text = ofd.FileName;
				}
			}
		}

		private async void btnSave_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtTitle.Text))
			{
				UIMessageTip.ShowWarning("Vui lòng nhập tên bài học!");
				return;
			}

			try
			{
				this.Cursor = Cursors.WaitCursor;
				string finalVideoName = txtVideoPath.Text.Trim();

				// NẾU LÀ FILE MỚI CHỌN (Đường dẫn có chứa dấu gạch chéo của ổ cứng)
				if (File.Exists(finalVideoName) && Path.IsPathRooted(finalVideoName))
				{
					UIMessageTip.ShowOk("Đang xử lý tải video lên...");
					finalVideoName = await SaveVideoToAssetsAsync(finalVideoName);
				}

				var lesson = new LessonDTO
				{
					Id = _isEdit ? _targetId : 0,
					CourseId = _isEdit ? 0 : _targetId,
					Title = txtTitle.Text.Trim(),
					VideoFilePath = finalVideoName, // Lưu tên file an toàn vào DB
					Position = (int)numPosition.Value,
					IsPublished = true
				};

				// Thu thập 10 câu hỏi
				List<LessonTestQuestionDTO> questions = CollectQuestionsFromTabs();

				string error = _isEdit ? _lessonBll.UpdateLesson(lesson) : _lessonBll.AddLesson(lesson);

				if (string.IsNullOrEmpty(error))
				{
					long lessonId = _isEdit ? _targetId : lesson.Id;
					_lessonBll.SaveLessonQuiz(lessonId, questions);

					UIMessageTip.ShowOk("Lưu bài học và Video thành công!");
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
				else { UIMessageBox.ShowError(error); }
			}
			catch (Exception ex)
			{
				UIMessageBox.ShowError("Lỗi hệ thống: " + ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		private List<LessonTestQuestionDTO> CollectQuestionsFromTabs()
		{
			List<LessonTestQuestionDTO> list = new List<LessonTestQuestionDTO>();

			for (int i = 1; i <= 10; i++)
			{
				// Tìm các control dựa trên Name chúng ta đã đặt ở hàm CreateQuestionTabs
				var txtQ = tabQuestions.TabPages[i - 1].Controls.Find($"txtQ{i}", true).FirstOrDefault() as UITextBox;
				var txtA = tabQuestions.TabPages[i - 1].Controls.Find($"txtA{i}", true).FirstOrDefault() as UITextBox;
				var txtB = tabQuestions.TabPages[i - 1].Controls.Find($"txtB{i}", true).FirstOrDefault() as UITextBox;
				var txtC = tabQuestions.TabPages[i - 1].Controls.Find($"txtC{i}", true).FirstOrDefault() as UITextBox;
				var txtD = tabQuestions.TabPages[i - 1].Controls.Find($"txtD{i}", true).FirstOrDefault() as UITextBox;
				var cbCorrect = tabQuestions.TabPages[i - 1].Controls.Find($"cbCorrect{i}", true).FirstOrDefault() as UIComboBox;

				// Chỉ lưu nếu câu hỏi không trống
				if (txtQ != null && !string.IsNullOrWhiteSpace(txtQ.Text))
				{
					list.Add(new LessonTestQuestionDTO
					{
						QuestionText = txtQ.Text.Trim(),
						ChoiceA = txtA?.Text.Trim() ?? "",
						ChoiceB = txtB?.Text.Trim() ?? "",
						ChoiceC = txtC?.Text.Trim() ?? "",
						ChoiceD = txtD?.Text.Trim() ?? "",
						CorrectAnswer = cbCorrect?.SelectedItem?.ToString() ?? "A",
						Position = i
					});
				}
			}
			return list;
		}

		private void CreateQuestionTabs()
		{
			tabQuestions.TabPages.Clear();
			for (int i = 1; i <= 10; i++)
			{
				TabPage tp = new TabPage($"Câu {i}");
				tp.Padding = new Padding(15);
				tp.BackColor = Color.White;

				// --- 1. Nội dung câu hỏi (Dòng 1) ---
				UILabel lblQ = new UILabel { Text = "Nội dung câu hỏi:", Location = new Point(15, 15), AutoSize = true };
				UITextBox txtQ = new UITextBox
				{
					Name = $"txtQ{i}",
					Location = new Point(15, 45),
					Size = new Size(670, 70), // Tăng chiều cao để nhập được nhiều chữ
					Multiline = true
				};

				// --- 2. Đáp án A & B (Dòng 2) ---
				UILabel lblA = new UILabel { Text = "A", Location = new Point(15, 135), AutoSize = true };
				UITextBox txtA = new UITextBox { Name = $"txtA{i}", Location = new Point(45, 130), Size = new Size(310, 30) };

				UILabel lblB = new UILabel { Text = "B", Location = new Point(375, 135), AutoSize = true };
				UITextBox txtB = new UITextBox { Name = $"txtB{i}", Location = new Point(405, 130), Size = new Size(310, 30) };

				// --- 3. Đáp án C & D (Dòng 3) ---
				UILabel lblC = new UILabel { Text = "C", Location = new Point(15, 175), AutoSize = true };
				UITextBox txtC = new UITextBox { Name = $"txtC{i}", Location = new Point(45, 170), Size = new Size(310, 30) };

				UILabel lblD = new UILabel { Text = "D", Location = new Point(375, 175), AutoSize = true };
				UITextBox txtD = new UITextBox { Name = $"txtD{i}", Location = new Point(405, 170), Size = new Size(310, 30) };

				// --- 4. Đáp án đúng (Dòng 4) ---
				UILabel lblCorrect = new UILabel { Text = "Đáp án đúng:", Location = new Point(15, 215), AutoSize = true };
				UIComboBox cbCorrect = new UIComboBox
				{
					Name = $"cbCorrect{i}",
					Location = new Point(135, 210), // Tăng X lên để không đè lên Label
					Size = new Size(120, 30),
					Items = { "A", "B", "C", "D" },
					DropDownStyle = UIDropDownStyle.DropDownList // Chỉ cho chọn, không cho gõ
				};
				cbCorrect.SelectedIndex = 0;

				// Thêm tất cả vào TabPage
				tp.Controls.AddRange(new Control[] { lblQ, txtQ, lblA, txtA, lblB, txtB, lblC, txtC, lblD, txtD, lblCorrect, cbCorrect });
				tabQuestions.TabPages.Add(tp);
			}
		}

		private void frmLessonEditor_Load(object sender, EventArgs e)
		{

		}

		private async Task<string> SaveVideoToAssetsAsync(string sourcePath)
		{
			try
			{
				// 1. Xác định thư mục assets theo đúng logic của CreateTestPaperControl
				// Thoát ra khỏi bin/Debug để vào UI/assets
				string assetsFolder = Path.Combine(Application.StartupPath, "..", "..", "UI", "assets", "videos");

				if (!Directory.Exists(assetsFolder))
					Directory.CreateDirectory(assetsFolder);

				// 2. Tạo tên file an toàn (Không chứa đường dẫn ổ cứng)
				string extension = Path.GetExtension(sourcePath);
				string safeFileName = $"vid_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid().ToString().Substring(0, 8)}{extension}";

				string destPath = Path.Combine(assetsFolder, safeFileName);

				// 3. Thực hiện copy file vật lý
				await Task.Run(() => File.Copy(sourcePath, destPath, true));

				// Trả về duy nhất TÊN FILE để lưu vào Database
				return safeFileName;
			}
			catch (Exception ex)
			{
				throw new Exception("Lỗi khi đồng bộ file vào assets: " + ex.Message);
			}
		}
	}
}