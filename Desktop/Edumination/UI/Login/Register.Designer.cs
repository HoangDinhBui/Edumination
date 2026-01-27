using Sunny.UI;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.Login
{
    partial class Register
    {
        private System.ComponentModel.IContainer components = null;

        private UIPanel panelRegister;

        private UILabel lblTitle;
        private UILabel lblSub;

        private PictureBox picFullName;
        private UITextBox txtFullName;

        private PictureBox picEmail;
        private UITextBox txtEmail;

        private PictureBox picPassword;
        private UITextBox txtPassword;

        private PictureBox picConfirmPassword;
        private UITextBox txtConfirmPassword;

        private UIButton btnRegister;
        private UILinkLabel lnkBackToLogin;

        // ===== GIỮ SLIDE (ẨN – KHÔNG XOÁ LOGIC) =====
        private PictureBox pictureBoxSlide;
        private Panel panelDots;
        private System.Windows.Forms.Timer timerSlide;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelRegister = new UIPanel();
            lblTitle = new UILabel();
            lblSub = new UILabel();
            picFullName = new PictureBox();
            txtFullName = new UITextBox();
            picEmail = new PictureBox();
            txtEmail = new UITextBox();
            picPassword = new PictureBox();
            txtPassword = new UITextBox();
            picConfirmPassword = new PictureBox();
            txtConfirmPassword = new UITextBox();
            btnRegister = new UIButton();
            lnkBackToLogin = new UILinkLabel();
            pictureBoxSlide = new PictureBox();
            panelDots = new Panel();
            timerSlide = new System.Windows.Forms.Timer(components);
            panelRegister.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picFullName).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picEmail).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPassword).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picConfirmPassword).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSlide).BeginInit();
            SuspendLayout();
            // 
            // panelRegister
            // 
            panelRegister.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            panelRegister.Controls.Add(lblTitle);
            panelRegister.Controls.Add(lblSub);
            panelRegister.Controls.Add(picFullName);
            panelRegister.Controls.Add(txtFullName);
            panelRegister.Controls.Add(picEmail);
            panelRegister.Controls.Add(txtEmail);
            panelRegister.Controls.Add(picPassword);
            panelRegister.Controls.Add(txtPassword);
            panelRegister.Controls.Add(picConfirmPassword);
            panelRegister.Controls.Add(txtConfirmPassword);
            panelRegister.Controls.Add(btnRegister);
            panelRegister.Controls.Add(lnkBackToLogin);
            panelRegister.FillColor = Color.White;
            panelRegister.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            panelRegister.Location = new Point(1080, 48);
            panelRegister.Margin = new Padding(4, 5, 4, 5);
            panelRegister.MinimumSize = new Size(1, 1);
            panelRegister.Name = "panelRegister";
            panelRegister.Padding = new Padding(30);
            panelRegister.Radius = 0;
            panelRegister.RectSides = ToolStripStatusLabelBorderSides.None;
            panelRegister.Size = new Size(685, 899);
            panelRegister.TabIndex = 0;
            panelRegister.Text = null;
            panelRegister.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.FromArgb(0, 0, 0, 0);
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Paytone One", 28F);
            lblTitle.ForeColor = Color.DodgerBlue;
            lblTitle.Location = new Point(30, 70);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(625, 75);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "EDUMINATION";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSub
            // 
            lblSub.BackColor = Color.FromArgb(0, 0, 0, 0);
            lblSub.Dock = DockStyle.Top;
            lblSub.Font = new Font("Segoe UI", 13F);
            lblSub.ForeColor = Color.Gray;
            lblSub.Location = new Point(30, 30);
            lblSub.Name = "lblSub";
            lblSub.Size = new Size(625, 40);
            lblSub.TabIndex = 1;
            lblSub.Text = "Create your account 🚀";
            lblSub.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picFullName
            // 
            picFullName.BackColor = Color.FromArgb(0, 0, 0, 0);
            picFullName.BackgroundImage = Properties.Resources.userIcon;
            picFullName.Location = new Point(60, 215);
            picFullName.Name = "picFullName";
            picFullName.Size = new Size(45, 45);
            picFullName.SizeMode = PictureBoxSizeMode.Zoom;
            picFullName.TabIndex = 2;
            picFullName.TabStop = false;
            // 
            // txtFullName
            // 
            txtFullName.Font = new Font("Segoe UI", 11F);
            txtFullName.Location = new Point(134, 208);
            txtFullName.Margin = new Padding(4, 5, 4, 5);
            txtFullName.MinimumSize = new Size(1, 16);
            txtFullName.Name = "txtFullName";
            txtFullName.Padding = new Padding(6);
            txtFullName.Radius = 8;
            txtFullName.RectColor = Color.FromArgb(220, 224, 229);
            txtFullName.RectSides = ToolStripStatusLabelBorderSides.Bottom;
            txtFullName.ShowText = false;
            txtFullName.Size = new Size(479, 59);
            txtFullName.TabIndex = 3;
            txtFullName.TextAlignment = ContentAlignment.MiddleLeft;
            txtFullName.Watermark = "Họ và tên";
            // 
            // picEmail
            // 
            picEmail.BackColor = Color.FromArgb(0, 0, 0, 0);
            picEmail.BackgroundImage = Properties.Resources.emailIcon;
            picEmail.Location = new Point(60, 307);
            picEmail.Name = "picEmail";
            picEmail.Size = new Size(45, 45);
            picEmail.SizeMode = PictureBoxSizeMode.Zoom;
            picEmail.TabIndex = 4;
            picEmail.TabStop = false;
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 11F);
            txtEmail.Location = new Point(134, 299);
            txtEmail.Margin = new Padding(4, 5, 4, 5);
            txtEmail.MinimumSize = new Size(1, 16);
            txtEmail.Name = "txtEmail";
            txtEmail.Padding = new Padding(6);
            txtEmail.Radius = 8;
            txtEmail.RectColor = Color.FromArgb(220, 224, 229);
            txtEmail.RectSides = ToolStripStatusLabelBorderSides.Bottom;
            txtEmail.ShowText = false;
            txtEmail.Size = new Size(479, 59);
            txtEmail.TabIndex = 5;
            txtEmail.TextAlignment = ContentAlignment.MiddleLeft;
            txtEmail.Watermark = "Email";
            // 
            // picPassword
            // 
            picPassword.BackColor = Color.FromArgb(0, 0, 0, 0);
            picPassword.BackgroundImage = Properties.Resources.passwordIcon;
            picPassword.Location = new Point(60, 397);
            picPassword.Name = "picPassword";
            picPassword.Size = new Size(45, 45);
            picPassword.SizeMode = PictureBoxSizeMode.Zoom;
            picPassword.TabIndex = 6;
            picPassword.TabStop = false;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.Location = new Point(134, 388);
            txtPassword.Margin = new Padding(4, 5, 4, 5);
            txtPassword.MinimumSize = new Size(1, 16);
            txtPassword.Name = "txtPassword";
            txtPassword.Padding = new Padding(6);
            txtPassword.PasswordChar = '●';
            txtPassword.Radius = 8;
            txtPassword.RectColor = Color.FromArgb(220, 224, 229);
            txtPassword.RectSides = ToolStripStatusLabelBorderSides.Bottom;
            txtPassword.ShowText = false;
            txtPassword.Size = new Size(479, 59);
            txtPassword.TabIndex = 7;
            txtPassword.TextAlignment = ContentAlignment.MiddleLeft;
            txtPassword.Watermark = "Mật khẩu";
            // 
            // picConfirmPassword
            // 
            picConfirmPassword.BackColor = Color.FromArgb(0, 0, 0, 0);
            picConfirmPassword.BackgroundImage = Properties.Resources.passwordIcon;
            picConfirmPassword.Location = new Point(60, 485);
            picConfirmPassword.Name = "picConfirmPassword";
            picConfirmPassword.Size = new Size(45, 45);
            picConfirmPassword.SizeMode = PictureBoxSizeMode.Zoom;
            picConfirmPassword.TabIndex = 8;
            picConfirmPassword.TabStop = false;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Font = new Font("Segoe UI", 11F);
            txtConfirmPassword.Location = new Point(134, 477);
            txtConfirmPassword.Margin = new Padding(4, 5, 4, 5);
            txtConfirmPassword.MinimumSize = new Size(1, 16);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Padding = new Padding(6);
            txtConfirmPassword.PasswordChar = '●';
            txtConfirmPassword.Radius = 8;
            txtConfirmPassword.RectColor = Color.FromArgb(220, 224, 229);
            txtConfirmPassword.RectSides = ToolStripStatusLabelBorderSides.Bottom;
            txtConfirmPassword.ShowText = false;
            txtConfirmPassword.Size = new Size(479, 59);
            txtConfirmPassword.TabIndex = 9;
            txtConfirmPassword.TextAlignment = ContentAlignment.MiddleLeft;
            txtConfirmPassword.Watermark = "Xác nhận mật khẩu";
            // 
            // btnRegister
            // 
            btnRegister.FillColor = Color.FromArgb(13, 110, 253);
            btnRegister.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnRegister.Location = new Point(60, 580);
            btnRegister.MinimumSize = new Size(1, 1);
            btnRegister.Name = "btnRegister";
            btnRegister.Radius = 10;
            btnRegister.Size = new Size(553, 63);
            btnRegister.TabIndex = 10;
            btnRegister.Text = "Đăng ký";
            btnRegister.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnRegister.Click += btnRegister_Click;
            // 
            // lnkBackToLogin
            // 
            lnkBackToLogin.ActiveLinkColor = Color.FromArgb(80, 160, 255);
            lnkBackToLogin.BackColor = Color.FromArgb(0, 0, 0, 0);
            lnkBackToLogin.Font = new Font("Segoe UI", 10.5F);
            lnkBackToLogin.ForeColor = Color.FromArgb(48, 48, 48);
            lnkBackToLogin.LinkBehavior = LinkBehavior.AlwaysUnderline;
            lnkBackToLogin.LinkColor = Color.FromArgb(13, 110, 253);
            lnkBackToLogin.Location = new Point(60, 677);
            lnkBackToLogin.Name = "lnkBackToLogin";
            lnkBackToLogin.Size = new Size(100, 23);
            lnkBackToLogin.TabIndex = 11;
            lnkBackToLogin.TabStop = true;
            lnkBackToLogin.Text = "← Quay về đăng nhập";
            lnkBackToLogin.VisitedLinkColor = Color.FromArgb(230, 80, 80);
            lnkBackToLogin.Click += lnkBackToLogin_Click;
            // 
            // pictureBoxSlide
            // 
            pictureBoxSlide.Location = new Point(0, 0);
            pictureBoxSlide.Name = "pictureBoxSlide";
            pictureBoxSlide.Size = new Size(100, 50);
            pictureBoxSlide.TabIndex = 0;
            pictureBoxSlide.TabStop = false;
            pictureBoxSlide.Visible = false;
            // 
            // panelDots
            // 
            panelDots.Location = new Point(0, 0);
            panelDots.Name = "panelDots";
            panelDots.Size = new Size(200, 100);
            panelDots.TabIndex = 0;
            panelDots.Visible = false;
            // 
            // Register
            // 
            BackgroundImage = Properties.Resources.bg;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1900, 950);
            Controls.Add(panelRegister);
            MaximizeBox = false;
            Name = "Register";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng ký tài khoản - IELTS Learning";
            Load += Register_Load;
            panelRegister.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picFullName).EndInit();
            ((System.ComponentModel.ISupportInitialize)picEmail).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPassword).EndInit();
            ((System.ComponentModel.ISupportInitialize)picConfirmPassword).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSlide).EndInit();
            ResumeLayout(false);
        }
    }
}
