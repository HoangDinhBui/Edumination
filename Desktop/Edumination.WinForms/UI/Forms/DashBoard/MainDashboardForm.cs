//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//using System;
//using System.Drawing; // Thêm để dùng Color
//using System.Windows.Forms;
//using ReaLTaiizor.Controls; // Quan trọng: Import namespace của RealTaizor Controls
//namespace Edumination.WinForms.UI.Forms.DashBoard

//{
//    public partial class MainDashboardForm : Form
//    {
//        public MainDashboardForm()
//        {
//            InitializeComponent();
//            // Điều chỉnh một số thuộc tính của RealTaizor controls sau khi InitializeComponent nếu cần
//            ApplyRealTaizorStyling();
//        }

//        private void MainDashboardForm_Load(object sender, EventArgs e)
//        {
//            // Load dữ liệu ban đầu
//            LoadTestData();
//            LoadReportData(); // Gọi hàm load data cho Report tab
//            SetupMockTestPanels(); // Setup các ô Quarter với dữ liệu giả định
//        }

//        private void ApplyRealTaizorStyling()
//        {
//            // Ví dụ: Điều chỉnh màu sắc cho các button kỹ năng
//            realButtonAllSkills.NormalColor = Color.FromArgb(85, 153, 255);
//            realButtonAllSkills.HoverColor = Color.FromArgb(100, 168, 255);
//            realButtonAllSkills.TextColor = Color.White;
//            realButtonAllSkills.RoundCorners = true;

//            // Các nút skill còn lại
//            RealButton[] skillButtons = { realButtonListening, realButtonReading, realButtonWriting, realButtonSpeaking };
//            foreach (var btn in skillButtons)
//            {
//                btn.NormalColor = Color.White;
//                btn.HoverColor = Color.FromArgb(230, 230, 230); // Hover hơi xám
//                btn.TextColor = Color.Gray;
//                btn.RoundCorners = true;
//                btn.BorderColor = Color.LightGray; // Có thể thêm border mỏng
//                btn.BorderSize = 1;
//            }

//            // Styling cho các nút Quarter
//            RealButton[] quarterButtons = { realButtonQuarter1, realButtonQuarter2, realButtonQuarter3, realButtonQuarter4 };
//            foreach (var btn in quarterButtons)
//            {
//                btn.NormalColor = Color.White; // Nền trắng
//                btn.HoverColor = Color.FromArgb(240, 240, 240); // Hover hơi xám nhạt
//                btn.TextColor = Color.Black;
//                btn.RoundCorners = true;
//                btn.BorderColor = Color.LightGray;
//                btn.BorderSize = 1;
//                btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
//                btn.TextHorizontalAlignment = StringAlignment.Near;
//                btn.TextVerticalAlignment = StringAlignment.Near;
//                btn.Padding = new Padding(10); // Khoảng cách từ text đến viền
//            }
//        }

//        #region Quản lý Bài Test (tabPageTestManagement)

//        private void LoadTestData()
//        {
//            // Giả lập dữ liệu cho realDataGridViewTests (nếu bạn sử dụng nó)
//            // DataTable dt = new DataTable();
//            // dt.Columns.Add("TestName");
//            // dt.Columns.Add("Skill");
//            // dt.Columns.Add("DateCreated");
//            // dt.Rows.Add("IELTS Mock Test - Q1", "All Skills", "2025-01-01");
//            // realDataGridViewTests.DataSource = dt;
//        }

//        private void realButtonSkill_Click(object sender, EventArgs e)
//        {
//            RealButton clickedButton = sender as RealButton;
//            if (clickedButton != null)
//            {
//                // Reset màu sắc của tất cả các nút kỹ năng
//                RealButton[] allSkillButtons = { realButtonAllSkills, realButtonListening, realButtonReading, realButtonWriting, realButtonSpeaking };
//                foreach (var btn in allSkillButtons)
//                {
//                    btn.NormalColor = Color.White;
//                    btn.TextColor = Color.Gray;
//                }

//                // Đặt màu cho nút được click
//                clickedButton.NormalColor = Color.FromArgb(85, 153, 255);
//                clickedButton.TextColor = Color.White;

//                // Thực hiện logic lọc bài test theo kỹ năng
//                MessageBox.Show($"Lọc theo kỹ năng: {clickedButton.Text}");
//            }
//        }

//        private void SetupMockTestPanels()
//        {
//            // Cập nhật số liệu vào các nút Quarter
//            realButtonQuarter1.Text = "Quarter 1\n⚡ 951,605 tests taken";
//            realButtonQuarter2.Text = "Quarter 2\n⚡ 951,605 tests taken";
//            realButtonQuarter3.Text = "Quarter 3\n⚡ 951,605 tests taken";
//            realButtonQuarter4.Text = "Quarter 4\n⚡ 951,605 tests taken";

//            // Bố trí các nút Quarter thủ công (hoặc dùng FlowLayoutPanel trong Designer)
//            int startX = 20;
//            int startY = 70;
//            int buttonWidth = 220;
//            int buttonHeight = 100;
//            int paddingX = 20;
//            int paddingY = 20;

//            realButtonQuarter1.Location = new Point(startX, startY);
//            realButtonQuarter2.Location = new Point(startX + buttonWidth + paddingX, startY);
//            realButtonQuarter3.Location = new Point(startX, startY + buttonHeight + paddingY);
//            realButtonQuarter4.Location = new Point(startX + buttonWidth + paddingX, startY + buttonHeight + paddingY);

//            // Có thể thêm icon máy tính ở góc trái cho phần Mock Test 2025
//            // realPanelMockTests.Controls.Add(new PictureBox() { Image = Resources.laptop_icon, Location = new Point(700, 50) });
//        }


//        #endregion

//        #region Thống kê / Report (tabPageReport)

//        private void LoadReportData()
//        {
//            // Logic để lấy dữ liệu thống kê thực tế từ database
//            // realLabelTotalTestsTaken.Text = "Tổng số bài đã làm: " + DatabaseHelper.GetTotalTestsTaken();
//            // realLabelAvgScore.Text = "Điểm trung bình: " + DatabaseHelper.GetAverageScore();
//            // Bạn có thể thêm các biểu đồ từ thư viện RealTaizor nếu có control tương ứng (ví dụ RealChart)
//        }

//        #endregion

//        // ... Các region tương tự cho Quản lý Câu Hỏi, Bài Nộp Học Viên, Quản lý User ...


//        // --- Xử lý sự kiện Logout ---

//        private void realButtonLogout_Click(object sender, EventArgs e)
//        {
//            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
//            {
//                this.Hide();
//                LoginForm loginForm = new LoginForm();
//                loginForm.Show();
//                // this.Close(); // Có thể đóng form này nếu không muốn nó chạy ẩn
//            }
//        }
//    }

//    // Ví dụ về Form đăng nhập (cần tạo riêng)
//    public partial class LoginForm : Form
//    {
//        public LoginForm()
//        {
//            InitializeComponent();
//            this.Text = "Đăng nhập";
//            this.ClientSize = new Size(400, 300);
//            this.StartPosition = FormStartPosition.CenterScreen;
//            // Add RealTextBox for username, password and RealButton for login
//        }
//    }
//}
