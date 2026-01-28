namespace IELTS.UI.Admin.CoursesManager
{
	partial class frmLessonEditor
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			txtTitle = new Sunny.UI.UITextBox();
			txtVideoPath = new Sunny.UI.UITextBox();
			numPosition = new Sunny.UI.UIIntegerUpDown();
			btnSave = new Sunny.UI.UISymbolButton();
			tabQuestions = new Sunny.UI.UITabControl();
			uiLabel1 = new Sunny.UI.UILabel();
			uiLabel2 = new Sunny.UI.UILabel();
			uiLabel3 = new Sunny.UI.UILabel();
			btnSelectVideo = new Sunny.UI.UISymbolButton();
			SuspendLayout();
			// 
			// txtTitle
			// 
			txtTitle.Font = new Font("Microsoft Sans Serif", 12F);
			txtTitle.Location = new Point(30, 78);
			txtTitle.Margin = new Padding(4, 5, 4, 5);
			txtTitle.MinimumSize = new Size(1, 16);
			txtTitle.Name = "txtTitle";
			txtTitle.Padding = new Padding(5);
			txtTitle.ShowText = false;
			txtTitle.Size = new Size(550, 31);
			txtTitle.TabIndex = 7;
			txtTitle.TextAlignment = ContentAlignment.MiddleLeft;
			txtTitle.Watermark = "Nhập tiêu đề bài học...";
			// 
			// txtVideoPath
			// 
			txtVideoPath.Font = new Font("Microsoft Sans Serif", 12F);
			txtVideoPath.Location = new Point(30, 150);
			txtVideoPath.Margin = new Padding(4, 5, 4, 5);
			txtVideoPath.MinimumSize = new Size(1, 16);
			txtVideoPath.Name = "txtVideoPath";
			txtVideoPath.Padding = new Padding(5);
			txtVideoPath.ShowText = false;
			txtVideoPath.Size = new Size(440, 31);
			txtVideoPath.TabIndex = 3;
			txtVideoPath.TextAlignment = ContentAlignment.MiddleLeft;
			txtVideoPath.Watermark = "";
			// 
			// numPosition
			// 
			numPosition.Font = new Font("Microsoft Sans Serif", 12F);
			numPosition.Location = new Point(600, 80);
			numPosition.Margin = new Padding(4, 5, 4, 5);
			numPosition.MinimumSize = new Size(1, 16);
			numPosition.Name = "numPosition";
			numPosition.Padding = new Padding(5);
			numPosition.ShowText = false;
			numPosition.Size = new Size(150, 29);
			numPosition.TabIndex = 5;
			numPosition.Text = "0";
			numPosition.TextAlignment = ContentAlignment.MiddleCenter;
			// 
			// btnSave
			// 
			btnSave.Font = new Font("Microsoft Sans Serif", 12F);
			btnSave.Location = new Point(600, 600);
			btnSave.MinimumSize = new Size(1, 1);
			btnSave.Name = "btnSave";
			btnSave.Size = new Size(150, 45);
			btnSave.Symbol = 61639;
			btnSave.TabIndex = 1;
			btnSave.Text = "LƯU DỮ LIỆU";
			btnSave.TipsFont = new Font("Microsoft Sans Serif", 9F);
			// 
			// tabQuestions
			// 
			tabQuestions.DrawMode = TabDrawMode.OwnerDrawFixed;
			tabQuestions.Font = new Font("Microsoft Sans Serif", 12F);
			tabQuestions.ItemSize = new Size(70, 40);
			tabQuestions.Location = new Point(3, 200);
			tabQuestions.MainPage = "";
			tabQuestions.Name = "tabQuestions";
			tabQuestions.SelectedIndex = 0;
			tabQuestions.Size = new Size(758, 380);
			tabQuestions.SizeMode = TabSizeMode.Fixed;
			tabQuestions.TabIndex = 0;
			tabQuestions.TabUnSelectedForeColor = Color.FromArgb(240, 240, 240);
			tabQuestions.TipsFont = new Font("Microsoft Sans Serif", 9F);
			// 
			// uiLabel1
			// 
			uiLabel1.Font = new Font("Microsoft Sans Serif", 12F);
			uiLabel1.ForeColor = Color.FromArgb(48, 48, 48);
			uiLabel1.Location = new Point(30, 50);
			uiLabel1.Name = "uiLabel1";
			uiLabel1.Size = new Size(100, 23);
			uiLabel1.TabIndex = 8;
			uiLabel1.Text = "Tên bài học:";
			// 
			// uiLabel2
			// 
			uiLabel2.Font = new Font("Microsoft Sans Serif", 12F);
			uiLabel2.ForeColor = Color.FromArgb(48, 48, 48);
			uiLabel2.Location = new Point(30, 114);
			uiLabel2.Name = "uiLabel2";
			uiLabel2.Size = new Size(100, 31);
			uiLabel2.TabIndex = 4;
			uiLabel2.Text = "Đường dẫn Video:";
			// 
			// uiLabel3
			// 
			uiLabel3.Font = new Font("Microsoft Sans Serif", 12F);
			uiLabel3.ForeColor = Color.FromArgb(48, 48, 48);
			uiLabel3.Location = new Point(600, 50);
			uiLabel3.Name = "uiLabel3";
			uiLabel3.Size = new Size(100, 23);
			uiLabel3.TabIndex = 6;
			uiLabel3.Text = "Thứ tự:";
			// 
			// btnSelectVideo
			// 
			btnSelectVideo.Font = new Font("Microsoft Sans Serif", 12F);
			btnSelectVideo.Location = new Point(480, 150);
			btnSelectVideo.MinimumSize = new Size(1, 1);
			btnSelectVideo.Name = "btnSelectVideo";
			btnSelectVideo.Size = new Size(129, 29);
			btnSelectVideo.Symbol = 61717;
			btnSelectVideo.TabIndex = 2;
			btnSelectVideo.Text = "Chọn file";
			btnSelectVideo.TipsFont = new Font("Microsoft Sans Serif", 9F);
			// 
			// frmLessonEditor
			// 
			ClientSize = new Size(800, 680);
			Controls.Add(tabQuestions);
			Controls.Add(btnSave);
			Controls.Add(btnSelectVideo);
			Controls.Add(txtVideoPath);
			Controls.Add(uiLabel2);
			Controls.Add(numPosition);
			Controls.Add(uiLabel3);
			Controls.Add(txtTitle);
			Controls.Add(uiLabel1);
			Name = "frmLessonEditor";
			Text = "CẬP NHẬT NỘI DUNG BÀI HỌC";
			ZoomScaleRect = new Rectangle(19, 19, 800, 680);
			Load += frmLessonEditor_Load;
			ResumeLayout(false);
		}

		private Sunny.UI.UITextBox txtTitle;
		private Sunny.UI.UITextBox txtVideoPath;
		private Sunny.UI.UIIntegerUpDown numPosition;
		private Sunny.UI.UISymbolButton btnSave;
		private Sunny.UI.UITabControl tabQuestions;
		private Sunny.UI.UILabel uiLabel1;
		private Sunny.UI.UILabel uiLabel2;
		private Sunny.UI.UILabel uiLabel3;
		private Sunny.UI.UISymbolButton btnSelectVideo;
	}
}