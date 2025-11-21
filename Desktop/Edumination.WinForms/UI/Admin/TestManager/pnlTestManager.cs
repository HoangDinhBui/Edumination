using Edumination.WinForms.Dto.responses;
using Sunny.UI; // Giữ lại Sunny.UI cho các controls khác như UILabel, UITabControlMenu, v.v.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Admin.TestManager
{
    public partial class pnlTestManager : UserControl
    {
        public Panel MainPanel => pnlMain; // pnlMain làm container chuẩn
        private pnlTestInfo pnlTestInfo;
        private pnlAllSkillTest pnlAllSkillTest;

        public pnlTestInfo TestInfoPanel => pnlTestInfo;
        public pnlAllSkillTest AllSkillPanel => pnlAllSkillTest;

        private static readonly HttpClient client = new HttpClient();
        private string token = SessionManager.JwtToken; // Token lưu khi login

        public pnlTestManager()
        {
            InitializeComponent();

            // Khởi tạo các panel
            pnlTestInfo = new pnlTestInfo();
            pnlAllSkillTest = new pnlAllSkillTest();

            // Dock fill để chiếm toàn bộ pnlMain
            pnlTestInfo.Dock = DockStyle.Fill;
            pnlAllSkillTest.Dock = DockStyle.Fill;

            // Add vào pnlMain
            pnlMain.Controls.Add(pnlAllSkillTest);
            pnlMain.Controls.Add(pnlTestInfo);

            // Chỉ hiển thị pnlAllSkillTest lúc đầu
            pnlAllSkillTest.Visible = true;
            pnlTestInfo.Visible = false;

            // Đăng ký event từ pnlAllSkillTest
            pnlAllSkillTest.OnMockTestSelected += async (id) =>
            {
                await pnlTestInfo.LoadPaperAsync(id);
                ShowPanel(pnlTestInfo);
            };
        }

        // Hàm tiện ích để show 1 UserControl, ẩn tất cả panel khác
        private void ShowPanel(UserControl uc)
        {
            foreach (Control c in pnlMain.Controls)
                c.Visible = false;

            uc.Visible = true;
            uc.BringToFront();
        }

        // Nếu muốn quay lại pnlAllSkillTest
        public void ShowAllSkillPanel()
        {
            ShowPanel(pnlAllSkillTest);
        }

        // Hàm tạo giao diện cho 1 Card Mock Test Set
        private void InitMockTestCard(Panel panel, string title, int testsTaken)
        {
            panel.Size = new Size(400, 120);
            panel.Margin = new Padding(0, 10, 0, 10);
            panel.Padding = new Padding(20);
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Controls.Clear();

            // 1. Label Tiêu đề
            UILabel lblTitle = new UILabel
            {
                Text = title,
                Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 30,
                ForeColor = Color.Black
            };
            panel.Controls.Add(lblTitle);

            // 2. Label Số bài test đã làm
            UILabel lblTestsTaken = new UILabel
            {
                Text = $"{testsTaken} tests taken",
                Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular),
                Dock = DockStyle.Top,
                Height = 25,
                ForeColor = Color.Gray
            };
            panel.Controls.Add(lblTestsTaken);

            // 3. Icon Lightning
            UIAvatar iconLightning = new UIAvatar
            {
                Symbol = 61460,
                SymbolSize = 20,
                Size = new Size(30, 30),
                FillColor = Color.Transparent,
                Style = UIStyle.Custom,
                ForeColor = Color.Orange,
                Location = new Point(20, 75)
            };
            panel.Controls.Add(iconLightning);
        }

        // Hàm tải hình ảnh laptop
        private void LoadLaptopImage()
        {
            PictureBox pictureBox = new PictureBox
            {
                Image = null, // Thay bằng ảnh thật
                BackColor = Color.LightGray,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(150, 270),
                Size = new Size(400, 300)
            };
            pictureBox.BringToFront();
            this.Controls.Add(pictureBox);
        }

        private async void btnAllSkills_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlAllSkillTest != null)
                {
                    // Tải dữ liệu mới từ API
                    await pnlAllSkillTest.LoadMockTestsAsync();

                    // Hiển thị pnlAllSkillTest, ẩn panel khác
                    ShowPanel(pnlAllSkillTest);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load All Skill Test: " + ex.Message, "Error");
            }
        }

    }
}
