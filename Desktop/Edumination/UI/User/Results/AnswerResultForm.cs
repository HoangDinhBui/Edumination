using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IELTS.UI.User.Results
{
	public partial class AnswerResultForm : Form
	{
		private readonly ExamResult _result;

		public AnswerResultForm(ExamResult result)
		{
			_result = result ?? throw new ArgumentNullException(nameof(result));
			InitializeComponent();
			this.Shown += AnswerResultForm_Load;

			// ✅ GỌI NGAY SAU InitializeComponent() để đảm bảo controls đã tồn tại

		}

		private void AnswerResultForm_Load(object sender, EventArgs e)
		{
			// ✅ Gọi lại 1 lần nữa khi Form đã hiển thị hoàn toàn
			LoadResultData();
		}

		// ✅ HÀM TỔNG HỢP - Gọi tất cả logic load data
		private void LoadResultData()
		{
			// Kiểm tra ngay lập tức tên trong Session
			System.Diagnostics.Debug.WriteLine($"[CRITICAL CHECK] Tên trong Session: {IELTS.UI.User.SessionManager.FullName}");
			try
			{
				UpdateUserProfile();
				UpdateExamStatistics();
				BuildAnswerKeysUI();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error loading result data: {ex.Message}",
					"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}

		private void UpdateUserProfile()
		{
			// Kiểm tra trực tiếp dữ liệu từ SessionManager
			string sessionName = SessionManager.FullName;
			Console.WriteLine($"[DEBUG RESULT FORM] Tên từ Session: {sessionName}");

			lblUserName.Text = sessionName; // "Student" là giá trị mặc định nếu rỗng

			lblUserName.Text = _result.UserName ?? "Student";

			if (picAvatar.Width > 0 && picAvatar.Height > 0)
			{
				GraphicsPath gp = new GraphicsPath();
				gp.AddEllipse(0, 0, picAvatar.Width, picAvatar.Height);
				picAvatar.Region = new Region(gp);
			}

			if (!string.IsNullOrEmpty(_result.AvatarPath) &&
				System.IO.File.Exists(_result.AvatarPath))
			{
				try
				{
					picAvatar.Image = Image.FromFile(_result.AvatarPath);
				}
				catch { }
			}
		}

		private void BuildAnswerKeysUI()
		{
			flowPanelAnswers.Controls.Clear();

			if (_result.Parts == null || _result.Parts.Count == 0)
			{
				Label lblEmpty = new Label
				{
					Text = "No answer data available.",
					AutoSize = true,
					Font = new Font("Segoe UI", 11F, FontStyle.Italic),
					ForeColor = Color.Gray,
					Margin = new Padding(20)
				};
				flowPanelAnswers.Controls.Add(lblEmpty);
				return;
			}

			// ✅ Duyệt qua TẤT CẢ Parts và Questions
			foreach (var part in _result.Parts)
			{
				if (part.Questions == null) continue;

				foreach (var q in part.Questions)
				{
					AnswerRowPanel row = new AnswerRowPanel();
					row.Bind(q);
					flowPanelAnswers.Controls.Add(row);
				}
			}

			// ✅ DEBUG: In số lượng câu đã thêm
			System.Diagnostics.Debug.WriteLine(
				$"[BuildAnswerKeysUI] Added {flowPanelAnswers.Controls.Count} answer rows"
			);
		}
		private void UpdateExamStatistics()
		{
			// 1. Vòng tròn Trái: Số câu đúng
			StyleCircle(progressCorrect, Color.FromArgb(0, 159, 227), "The correct answer", $"{_result.CorrectCount}/{_result.TotalQuestions}");
			progressCorrect.Value = _result.TotalQuestions > 0 ? (int)((double)_result.CorrectCount / _result.TotalQuestions * 100) : 0;

			// 2. Vòng tròn Giữa: Band Score (Hiện số 2 lớn)
			StyleCircle(progressBand, Color.FromArgb(80, 227, 255), "Band Score", _result.Band.ToString("0.#"));
			progressBand.Value = (int)((_result.Band / 9.0) * 100);

			// 3. Vòng tròn Phải: Thời gian
			var span = TimeSpan.FromSeconds(_result.TimeTakenSeconds);
			string timeStr = $"{(int)span.TotalMinutes:D2}:{span.Seconds:D2}";
			StyleCircle(progressTime, Color.FromArgb(0, 159, 227), "Time taken", timeStr);
			progressTime.Value = 100;
		}
		private void StyleCircle(Sunny.UI.UIRoundProcess bar, Color color, string subTitle, string mainText)
		{
			bar.ShowProcess = true; // Hiện thanh xanh
			bar.Inner = 46;         // Làm vòng tròn mỏng
			bar.Outer = 50;
			bar.Text = mainText;    // Hiển thị con số ở giữa
			bar.ForeColor = color;
			
			bar.Font = new Font("Segoe UI", 16F, FontStyle.Bold);

			// Xóa các nhãn cũ nếu có để tránh ghi đè khi gọi lại
			foreach (Control c in panelMainContent.Controls)
			{
				if (c is Label && c.Tag?.ToString() == bar.Name)
				{
					panelMainContent.Controls.Remove(c); break;
				}
			}

			// Thêm chữ trang trí bên dưới vòng tròn
			Label lbl = new Label
			{
				Text = subTitle,
				Font = new Font("Segoe UI", 9F),
				ForeColor = Color.Gray,
				TextAlign = ContentAlignment.MiddleCenter,
				Location = new Point(bar.Location.X, bar.Location.Y + bar.Height + 5),
				Size = new Size(bar.Width, 20),
				Tag = bar.Name
			};
			panelMainContent.Controls.Add(lbl);
		}
		private void progressCorrect_ValueChanged(object sender, int value)
		{

		}
	}
}