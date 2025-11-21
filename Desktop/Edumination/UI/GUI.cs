using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FontAwesome.Sharp;
namespace IELTS.UI
{
    // =========================================================
    // UI Layer - Presentation Layer với SunnyUI
    // Cài đặt NuGet: Install-Package SunnyUI
    // =========================================================

    using global::IELTS.BLL;
    //using IELTS.BLL;
    using Sunny.UI;
    using System;
    using System.Data;
    using System.Windows.Forms;

    namespace IELTS.UI
    {
        // =========================================================
        // 1. LOGIN FORM
        // =========================================================
        public partial class frmLogin : UIForm
        {
            private UserBLL userBLL = new UserBLL();

            public frmLogin()
            {
                InitializeComponent();
                this.Text = "Đăng nhập - IELTS Learning";
                this.ShowTitle = true;
                this.TitleColor = Color.FromArgb(80, 160, 255);
            }

            private void InitializeComponent()
            {
                this.Size = new Size(450, 550);
                this.StartPosition = FormStartPosition.CenterScreen;
                this.MaximizeBox = false;

                // Logo/Title Label
                UILabel lblTitle = new UILabel
                {
                    Text = "🎓 IELTS LEARNING SYSTEM",
                    Font = new Font("Microsoft Sans Serif", 16, FontStyle.Bold),
                    Location = new Point(50, 70),
                    Size = new Size(350, 40),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.FromArgb(80, 160, 255)
                };
                this.Controls.Add(lblTitle);

                // Email Label
                UILabel lblEmail = new UILabel
                {
                    Text = "Email:",
                    Location = new Point(50, 150),
                    Size = new Size(100, 30)
                };
                this.Controls.Add(lblEmail);

                // Email TextBox
                UITextBox txtEmail = new UITextBox
                {
                    Name = "txtEmail",
                    Location = new Point(50, 180),
                    Size = new Size(350, 35),
                    Watermark = "Nhập email của bạn..."
                };
                this.Controls.Add(txtEmail);

                // Password Label
                UILabel lblPassword = new UILabel
                {
                    Text = "Mật khẩu:",
                    Location = new Point(50, 230),
                    Size = new Size(100, 30)
                };
                this.Controls.Add(lblPassword);

                // Password TextBox
                UITextBox txtPassword = new UITextBox
                {
                    Name = "txtPassword",
                    Location = new Point(50, 260),
                    Size = new Size(350, 35),
                    PasswordChar = '●',
                    Watermark = "Nhập mật khẩu..."
                };
                this.Controls.Add(txtPassword);

                // Login Button
                UIButton btnLogin = new UIButton
                {
                    Name = "btnLogin",
                    Text = "Đăng nhập",
                    Location = new Point(50, 330),
                    Size = new Size(350, 45),
                    Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold),
                    TipsFont = new Font("Microsoft Sans Serif", 9)
                };
                btnLogin.Click += BtnLogin_Click;
                this.Controls.Add(btnLogin);

                // Register Button
                UIButton btnRegister = new UIButton
                {
                    Name = "btnRegister",
                    Text = "Đăng ký tài khoản mới",
                    Location = new Point(50, 390),
                    Size = new Size(350, 40),
                    FillColor = Color.FromArgb(100, 100, 100),
                    FillHoverColor = Color.FromArgb(120, 120, 120),
                    Font = new Font("Microsoft Sans Serif", 10)
                };
                btnRegister.Click += BtnRegister_Click;
                this.Controls.Add(btnRegister);

                // Store controls for later access
                this.Tag = new { txtEmail, txtPassword };
            }

            private void BtnLogin_Click(object sender, EventArgs e)
            {
                try
                {
                    dynamic controls = this.Tag;
                    UITextBox txtEmail = controls.txtEmail;
                    UITextBox txtPassword = controls.txtPassword;

                    string email = txtEmail.Text.Trim();
                    string password = txtPassword.Text;

                    DataTable result = userBLL.Login(email, password);

                    if (result.Rows.Count > 0)
                    {
                        SessionManager.Login(result.Rows[0]);
                        UIMessageBox.ShowSuccess($"Chào mừng {SessionManager.CurrentUserName}!");

                        this.Hide();
                        frmMain mainForm = new frmMain();
                        mainForm.ShowDialog();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    UIMessageBox.ShowError($"Đăng nhập thất bại!\n{ex.Message}");
                }
            }

            private void BtnRegister_Click(object sender, EventArgs e)
            {
                frmRegister registerForm = new frmRegister();
                registerForm.ShowDialog();
            }
        }

        // =========================================================
        // 2. REGISTER FORM
        // =========================================================
        public partial class frmRegister : UIForm
        {
            private UserBLL userBLL = new UserBLL();

            public frmRegister()
            {
                InitializeComponent();
                this.Text = "Đăng ký tài khoản - IELTS Learning";
            }

            private void InitializeComponent()
            {
                this.Size = new Size(450, 650);
                this.StartPosition = FormStartPosition.CenterScreen;
                this.MaximizeBox = false;

                int yPos = 80;

                // Title
                UILabel lblTitle = new UILabel
                {
                    Text = "📝 ĐĂNG KÝ TÀI KHOẢN",
                    Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold),
                    Location = new Point(50, yPos),
                    Size = new Size(350, 35),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                this.Controls.Add(lblTitle);
                yPos += 60;

                // Full Name
                this.Controls.Add(new UILabel { Text = "Họ và tên:", Location = new Point(50, yPos), Size = new Size(100, 25) });
                UITextBox txtFullName = new UITextBox { Name = "txtFullName", Location = new Point(50, yPos + 25), Size = new Size(350, 35) };
                this.Controls.Add(txtFullName);
                yPos += 70;

                // Email
                this.Controls.Add(new UILabel { Text = "Email:", Location = new Point(50, yPos), Size = new Size(100, 25) });
                UITextBox txtEmail = new UITextBox { Name = "txtEmail", Location = new Point(50, yPos + 25), Size = new Size(350, 35) };
                this.Controls.Add(txtEmail);
                yPos += 70;

                // Password
                this.Controls.Add(new UILabel { Text = "Mật khẩu:", Location = new Point(50, yPos), Size = new Size(100, 25) });
                UITextBox txtPassword = new UITextBox { Name = "txtPassword", Location = new Point(50, yPos + 25), Size = new Size(350, 35), PasswordChar = '●' };
                this.Controls.Add(txtPassword);
                yPos += 70;

                // Confirm Password
                this.Controls.Add(new UILabel { Text = "Xác nhận mật khẩu:", Location = new Point(50, yPos), Size = new Size(150, 25) });
                UITextBox txtConfirmPassword = new UITextBox { Name = "txtConfirmPassword", Location = new Point(50, yPos + 25), Size = new Size(350, 35), PasswordChar = '●' };
                this.Controls.Add(txtConfirmPassword);
                yPos += 80;

                // Register Button
                UIButton btnRegister = new UIButton
                {
                    Text = "Đăng ký",
                    Location = new Point(50, yPos),
                    Size = new Size(350, 45),
                    Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold)
                };
                btnRegister.Click += BtnRegister_Click;
                this.Controls.Add(btnRegister);

                // Store controls
                this.Tag = new { txtFullName, txtEmail, txtPassword, txtConfirmPassword };
            }

            private void BtnRegister_Click(object sender, EventArgs e)
            {
                try
                {
                    dynamic controls = this.Tag;

                    bool success = userBLL.Register(
                        controls.txtEmail.Text.Trim(),
                        controls.txtPassword.Text,
                        controls.txtConfirmPassword.Text,
                        controls.txtFullName.Text.Trim()
                    );

                    if (success)
                    {
                        UIMessageBox.ShowSuccess("Đăng ký thành công!\nVui lòng đăng nhập.");
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    UIMessageBox.ShowError($"Đăng ký thất bại!\n{ex.Message}");
                }
            }
        }

        // =========================================================
        // 3. MAIN FORM (Dashboard)
        // =========================================================
        public partial class frmMain : UIHeaderMainFrame
        {
            public frmMain()
            {
                InitializeComponent();
                this.Text = "IELTS Learning System";
                this.Header.Text = $"Xin chào, {SessionManager.CurrentUserName}!";
            }

            private void InitializeComponent()
            {
                this.Size = new Size(1200, 700);
                this.StartPosition = FormStartPosition.CenterScreen;

                // Header settings
                this.Header.Height = 80;
                this.Header.BackColor = Color.FromArgb(80, 160, 255);

                // Add menu items
                AddPage(new frmDashboard(), 1001, "Dashboard", "📊 Tổng quan", IconChar.Home);
                AddPage(new frmTestList(), 1002, "Tests", "📝 Làm bài thi", IconChar.PencilAlt);
                AddPage(new frmMyCourses(), 1003, "Courses", "📚 Khóa học", IconChar.Book);
                AddPage(new frmProfile(), 1004, "Profile", "👤 Hồ sơ", IconChar.User);

                // Admin menu
                if (SessionManager.IsAdmin || SessionManager.IsTeacher)
                {
                    AddPage(new frmManageTests(), 1005, "ManageTests", "⚙️ Quản lý đề thi", IconChar.Cog);
                }

                // Logout button
                UIButton btnLogout = new UIButton
                {
                    Text = "Đăng xuất",
                    Size = new Size(100, 35),
                    Location = new Point(this.Width - 150, 25),
                    FillColor = Color.FromArgb(220, 53, 69),
                    FillHoverColor = Color.FromArgb(200, 35, 51)
                };
                btnLogout.Click += BtnLogout_Click;
                this.Header.Controls.Add(btnLogout);
            }

            public void AddPage(Form page, int id, string title, string description, IconChar icon)
            {
                // Tạo IconButton từ FontAwesome.Sharp
                IconButton btn = new IconButton
                {
                    Name = "btnPage" + id,
                    Text = title,
                    TextAlign = ContentAlignment.MiddleLeft,
                    TextImageRelation = TextImageRelation.ImageBeforeText,
                    ImageAlign = ContentAlignment.MiddleLeft,
                    IconChar = icon,
                    IconColor = Color.White,
                    IconSize = 24,
                    Size = new Size(200, 50),
                    BackColor = Color.FromArgb(80, 160, 255),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btn.FlatAppearance.BorderSize = 0;

                // Tính vị trí nút
                int y = this.Header.Controls.OfType<IconButton>().Count() * (btn.Height + 5) + 10;
                btn.Location = new Point(10, y);

                // Click event để mở Form tương ứng
                btn.Click += (s, e) =>
                {
                    // Đóng các Form đang mở trong panel MainContent (giả sử có Panel MainContent)
                    Panel mainPanel = this.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "MainContent");

                    if (mainPanel != null)
                    {
                        // Đóng tất cả Form con trong panel
                        foreach (Control c in mainPanel.Controls)
                        {
                            c.Dispose();
                        }

                        page.TopLevel = false;
                        page.FormBorderStyle = FormBorderStyle.None;
                        page.Dock = DockStyle.Fill;

                        mainPanel.Controls.Add(page);
                        page.Show();
                    }

                    page.TopLevel = false;
                    page.FormBorderStyle = FormBorderStyle.None;
                    page.Dock = DockStyle.Fill;

                    //Panel mainPanel = this.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "MainContent");
                    //if (mainPanel != null)
                    //{
                    //    mainPanel.Controls.Add(page);
                    //    page.Show();
                    //}
                };

                // Thêm nút vào Header
                this.Header.Controls.Add(btn);
            }
            private void BtnLogout_Click(object sender, EventArgs e)
            {
                if (UIMessageBox.ShowAsk("Bạn có chắc muốn đăng xuất?"))
                {
                    SessionManager.Logout();
                    this.Close();

                    frmLogin loginForm = new frmLogin();
                    loginForm.Show();
                }
            }
        }

        // =========================================================
        // 4. DASHBOARD PAGE
        // =========================================================
        public partial class frmDashboard : UIPage
        {
            private StatisticsBLL statisticsBLL = new StatisticsBLL();

            public frmDashboard()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                this.Text = "Tổng quan";

                // Welcome Panel
                UIPanel pnlWelcome = new UIPanel
                {
                    Location = new Point(20, 20),
                    Size = new Size(760, 120),
                    FillColor = Color.FromArgb(80, 160, 255),
                    RectColor = Color.FromArgb(80, 160, 255),
                    Radius = 10
                };

                UILabel lblWelcome = new UILabel
                {
                    Text = $"Chào mừng trở lại, {SessionManager.CurrentUserName}! 👋",
                    Font = new Font("Microsoft Sans Serif", 18, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(30, 25),
                    Size = new Size(700, 35)
                };
                pnlWelcome.Controls.Add(lblWelcome);

                UILabel lblSubtitle = new UILabel
                {
                    Text = "Sẵn sàng chinh phục IELTS hôm nay chưa?",
                    Font = new Font("Microsoft Sans Serif", 12),
                    ForeColor = Color.White,
                    Location = new Point(30, 65),
                    Size = new Size(700, 25)
                };
                pnlWelcome.Controls.Add(lblSubtitle);

                this.Controls.Add(pnlWelcome);

                // Statistics Cards
                CreateStatCard("📊 Tổng số bài thi", "0", 20, 160);
                CreateStatCard("⭐ Điểm cao nhất", "0.0", 270, 160);
                CreateStatCard("📈 Điểm trung bình", "0.0", 520, 160);

                // Load statistics
                LoadStatistics();

                // Recent Tests DataGridView
                UILabel lblRecent = new UILabel
                {
                    Text = "📝 Bài thi gần đây",
                    Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold),
                    Location = new Point(20, 310),
                    Size = new Size(200, 30)
                };
                this.Controls.Add(lblRecent);

                UIDataGridView dgvTests = new UIDataGridView
                {
                    Name = "dgvTests",
                    Location = new Point(20, 350),
                    Size = new Size(760, 250),
                    ReadOnly = true,
                    AllowUserToAddRows = false
                };
                this.Controls.Add(dgvTests);

                LoadRecentTests(dgvTests);
            }

            private void CreateStatCard(string title, string value, int x, int y)
            {
                UIPanel panel = new UIPanel
                {
                    Location = new Point(x, y),
                    Size = new Size(230, 120),
                    Radius = 10,
                    FillColor = Color.White,
                    RectColor = Color.FromArgb(200, 200, 200)
                };

                UILabel lblTitle = new UILabel
                {
                    Text = title,
                    Font = new Font("Microsoft Sans Serif", 10),
                    ForeColor = Color.Gray,
                    Location = new Point(15, 15),
                    Size = new Size(200, 25)
                };
                panel.Controls.Add(lblTitle);

                UILabel lblValue = new UILabel
                {
                    Name = "lblValue_" + title,
                    Text = value,
                    Font = new Font("Microsoft Sans Serif", 24, FontStyle.Bold),
                    ForeColor = Color.FromArgb(80, 160, 255),
                    Location = new Point(15, 50),
                    Size = new Size(200, 40)
                };
                panel.Controls.Add(lblValue);

                this.Controls.Add(panel);
            }

            private void LoadStatistics()
            {
                try
                {
                    DataTable stats = statisticsBLL.GetUserStatistics(SessionManager.CurrentUserId);

                    if (stats.Rows.Count > 0)
                    {
                        DataRow row = stats.Rows[0];

                        // Update stat cards (cần tìm controls theo Name)
                        foreach (Control panel in this.Controls)
                        {
                            if (panel is UIPanel)
                            {
                                foreach (Control lbl in panel.Controls)
                                {
                                    if (lbl.Name.StartsWith("lblValue_"))
                                    {
                                        if (lbl.Name.Contains("Tổng số"))
                                            ((UILabel)lbl).Text = row["TotalTests"].ToString();
                                        else if (lbl.Name.Contains("cao nhất"))
                                            ((UILabel)lbl).Text = row["BestBand"] != DBNull.Value ? row["BestBand"].ToString() : "N/A";
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    UIMessageBox.ShowError("Lỗi tải thống kê: " + ex.Message);
                }
            }

            private void LoadRecentTests(UIDataGridView dgv)
            {
                try
                {
                    TestAttemptBLL attemptBLL = new TestAttemptBLL();
                    DataTable tests = attemptBLL.GetAttemptsByUserId(SessionManager.CurrentUserId);
                    dgv.DataSource = tests;

                    // Rename columns
                    if (dgv.Columns.Contains("PaperTitle"))
                        dgv.Columns["PaperTitle"].HeaderText = "Đề thi";
                    if (dgv.Columns.Contains("StartedAt"))
                        dgv.Columns["StartedAt"].HeaderText = "Ngày thi";
                    if (dgv.Columns.Contains("OverallBand"))
                        dgv.Columns["OverallBand"].HeaderText = "Điểm";
                }
                catch (Exception ex)
                {
                    UIMessageBox.ShowError("Lỗi tải danh sách: " + ex.Message);
                }
            }
        }

        // =========================================================
        // 5. TEST LIST PAGE (Danh sách đề thi)
        // =========================================================
        public partial class frmTestList : UIPage
        {
            private TestPaperBLL paperBLL = new TestPaperBLL();

            public frmTestList()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                this.Text = "Danh sách đề thi";

                UILabel lblTitle = new UILabel
                {
                    Text = "📝 DANH SÁCH ĐỀ THI IELTS",
                    Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold),
                    Location = new Point(20, 20),
                    Size = new Size(400, 35)
                };
                this.Controls.Add(lblTitle);

                UIDataGridView dgvPapers = new UIDataGridView
                {
                    Name = "dgvPapers",
                    Location = new Point(20, 70),
                    Size = new Size(760, 450),
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect
                };
                this.Controls.Add(dgvPapers);

                UIButton btnStart = new UIButton
                {
                    Text = "Bắt đầu làm bài",
                    Location = new Point(20, 540),
                    Size = new Size(150, 40),
                    Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
                };
                btnStart.Click += BtnStart_Click;
                this.Controls.Add(btnStart);

                UIButton btnRefresh = new UIButton
                {
                    Text = "Làm mới",
                    Location = new Point(190, 540),
                    Size = new Size(100, 40),
                    FillColor = Color.Gray
                };
                btnRefresh.Click += (s, e) => LoadPapers(dgvPapers);
                this.Controls.Add(btnRefresh);

                LoadPapers(dgvPapers);
            }

            private void LoadPapers(UIDataGridView dgv)
            {
                try
                {
                    DataTable papers = paperBLL.GetAllPublishedPapers();
                    dgv.DataSource = papers;

                    if (dgv.Columns.Contains("Title"))
                        dgv.Columns["Title"].HeaderText = "Tiêu đề";
                    if (dgv.Columns.Contains("Code"))
                        dgv.Columns["Code"].HeaderText = "Mã đề";
                    if (dgv.Columns.Contains("CreatedByName"))
                        dgv.Columns["CreatedByName"].HeaderText = "Người tạo";
                }
                catch (Exception ex)
                {
                    UIMessageBox.ShowError("Lỗi tải danh sách: " + ex.Message);
                }
            }

            private void BtnStart_Click(object sender, EventArgs e)
            {
                UIDataGridView dgv = (UIDataGridView)this.Controls.Find("dgvPapers", true)[0];

                if (dgv.SelectedRows.Count == 0)
                {
                    UIMessageBox.ShowWarning("Vui lòng chọn đề thi!");
                    return;
                }

                long paperId = Convert.ToInt64(dgv.SelectedRows[0].Cells["Id"].Value);
                string paperTitle = dgv.SelectedRows[0].Cells["Title"].Value.ToString();

                if (UIMessageBox.ShowAsk($"Bắt đầu làm bài: {paperTitle}?"))
                {
                    try
                    {
                        TestAttemptBLL attemptBLL = new TestAttemptBLL();
                        long attemptId = attemptBLL.CreateAttempt(SessionManager.CurrentUserId, paperId);

                        UIMessageBox.ShowSuccess("Đã tạo lượt thi mới!\nChúc bạn làm bài tốt!");

                        // Mở form làm bài (cần implement)
                        // frmTakeTest testForm = new frmTakeTest(attemptId, paperId);
                        // testForm.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        UIMessageBox.ShowError("Lỗi tạo lượt thi: " + ex.Message);
                    }
                }
            }
        }

        // =========================================================
        // 6. PROFILE PAGE
        // =========================================================
        public partial class frmProfile : UIPage
        {
            private UserBLL userBLL = new UserBLL();

            public frmProfile()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                this.Text = "Hồ sơ cá nhân";

                UILabel lblTitle = new UILabel
                {
                    Text = "👤 HỒ SƠ CÁ NHÂN",
                    Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold),
                    Location = new Point(20, 20),
                    Size = new Size(300, 35)
                };
                this.Controls.Add(lblTitle);

                int yPos = 80;

                // Full Name
                this.Controls.Add(new UILabel { Text = "Họ và tên:", Location = new Point(20, yPos), Size = new Size(100, 25) });
                UITextBox txtFullName = new UITextBox { Name = "txtFullName", Location = new Point(20, yPos + 25), Size = new Size(400, 35) };
                this.Controls.Add(txtFullName);
                yPos += 70;

                // Email (readonly)
                this.Controls.Add(new UILabel { Text = "Email:", Location = new Point(20, yPos), Size = new Size(100, 25) });
                UITextBox txtEmail = new UITextBox { Name = "txtEmail", Location = new Point(20, yPos + 25), Size = new Size(400, 35), ReadOnly = true };
                this.Controls.Add(txtEmail);
                yPos += 70;

                // Phone
                this.Controls.Add(new UILabel { Text = "Số điện thoại:", Location = new Point(20, yPos), Size = new Size(100, 25) });
                UITextBox txtPhone = new UITextBox { Name = "txtPhone", Location = new Point(20, yPos + 25), Size = new Size(400, 35) };
                this.Controls.Add(txtPhone);
                yPos += 70;

                // Date of Birth
                this.Controls.Add(new UILabel { Text = "Ngày sinh:", Location = new Point(20, yPos), Size = new Size(100, 25) });
                UIDatePicker dtpDOB = new UIDatePicker { Name = "dtpDOB", Location = new Point(20, yPos + 25), Size = new Size(400, 35) };
                this.Controls.Add(dtpDOB);
                yPos += 80;

                // Update Button
                UIButton btnUpdate = new UIButton
                {
                    Text = "Cập nhật thông tin",
                    Location = new Point(20, yPos),
                    Size = new Size(200, 40),
                    Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold)
                };
                btnUpdate.Click += BtnUpdate_Click;
                this.Controls.Add(btnUpdate);

                LoadProfile();
            }

            private void LoadProfile()
            {
                try
                {
                    DataTable user = userBLL.GetUserById(SessionManager.CurrentUserId);

                    if (user.Rows.Count > 0)
                    {
                        DataRow row = user.Rows[0];

                        ((UITextBox)this.Controls.Find("txtFullName", true)[0]).Text = row["FullName"].ToString();
                        ((UITextBox)this.Controls.Find("txtEmail", true)[0]).Text = row["Email"].ToString();

                        if (row["Phone"] != DBNull.Value)
                            ((UITextBox)this.Controls.Find("txtPhone", true)[0]).Text = row["Phone"].ToString();

                        if (row["DateOfBirth"] != DBNull.Value)
                            ((UIDatePicker)this.Controls.Find("dtpDOB", true)[0]).Value = Convert.ToDateTime(row["DateOfBirth"]);
                    }
                }
                catch (Exception ex)
                {
                    UIMessageBox.ShowError("Lỗi tải thông tin: " + ex.Message);
                }
            }

            private void BtnUpdate_Click(object sender, EventArgs e)
            {
                try
                {
                    string fullName = ((UITextBox)this.Controls.Find("txtFullName", true)[0]).Text;
                    string phone = ((UITextBox)this.Controls.Find("txtPhone", true)[0]).Text;
                    DateTime? dob = ((UIDatePicker)this.Controls.Find("dtpDOB", true)[0]).Value;

                    bool success = userBLL.UpdateProfile(SessionManager.CurrentUserId, fullName, phone, dob);

                    if (success)
                    {
                        SessionManager.CurrentUserName = fullName;
                        UIMessageBox.ShowSuccess("Cập nhật thông tin thành công!");
                    }
                }
                catch (Exception ex)
                {
                    UIMessageBox.ShowError("Lỗi cập nhật: " + ex.Message);
                }
            }
        }

        public partial class frmMyCourses : UIPage
        {
            private UserBLL userBLL = new UserBLL();

            public frmMyCourses()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
            }
        }
        public partial class frmManageTests : UIPage
        {
            private UserBLL userBLL = new UserBLL();

            public frmManageTests()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
            }
        }
        
    }
}
