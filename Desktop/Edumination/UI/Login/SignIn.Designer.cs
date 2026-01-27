using Sunny.UI;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.Login
{
    partial class SignIn
    {
        private System.ComponentModel.IContainer components = null;

        private UIPanel panelLogin;
        private UILabel lblTitle;
        private UILabel lblSub;

        private UITextBox txtEmail;
        private UITextBox txtPassword;

        private UIButton btnLogin;
        private UIButton btnRegister;

        private UILinkLabel lnkForgotPassword;

        private PictureBox pictureBoxSlide;

        private PictureBox picEmail;
        private PictureBox picPassword;

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
            panelLogin = new UIPanel();
            lblTitle = new UILabel();
            lblSub = new UILabel();
            picEmail = new PictureBox();
            txtEmail = new UITextBox();
            picPassword = new PictureBox();
            txtPassword = new UITextBox();
            lnkForgotPassword = new UILinkLabel();
            btnLogin = new UIButton();
            btnRegister = new UIButton();
            pictureBoxSlide = new PictureBox();
            timerSlide = new System.Windows.Forms.Timer(components);
            panelLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picEmail).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPassword).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSlide).BeginInit();
            SuspendLayout();
            // 
            // panelLogin
            // 
            panelLogin.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            panelLogin.Controls.Add(lblTitle);
            panelLogin.Controls.Add(lblSub);
            panelLogin.Controls.Add(picEmail);
            panelLogin.Controls.Add(txtEmail);
            panelLogin.Controls.Add(picPassword);
            panelLogin.Controls.Add(txtPassword);
            panelLogin.Controls.Add(lnkForgotPassword);
            panelLogin.Controls.Add(btnLogin);
            panelLogin.Controls.Add(btnRegister);
            panelLogin.FillColor = Color.White;
            panelLogin.Font = new Font("Segoe UI", 12F);
            panelLogin.Location = new Point(1080, 48);
            panelLogin.Margin = new Padding(4, 5, 4, 5);
            panelLogin.MinimumSize = new Size(1, 1);
            panelLogin.Name = "panelLogin";
            panelLogin.Padding = new Padding(30);
            panelLogin.Radius = 0;
            panelLogin.RectColor = Color.FromArgb(230, 235, 245);
            panelLogin.RectSides = ToolStripStatusLabelBorderSides.None;
            panelLogin.Size = new Size(685, 899);
            panelLogin.TabIndex = 1;
            panelLogin.Text = null;
            panelLogin.TextAlignment = ContentAlignment.MiddleCenter;
            panelLogin.Click += panelLogin_Click;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.FromArgb(0, 0, 0, 0);
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Paytone One", 28.2F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblTitle.ForeColor = Color.DodgerBlue;
            lblTitle.Location = new Point(30, 60);
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
            lblSub.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblSub.ForeColor = Color.Gray;
            lblSub.Location = new Point(30, 30);
            lblSub.Name = "lblSub";
            lblSub.Size = new Size(625, 30);
            lblSub.TabIndex = 1;
            lblSub.Text = "Sign in to continue learning 🎓";
            lblSub.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picEmail
            // 
            picEmail.BackColor = Color.FromArgb(0, 0, 0, 0);
            picEmail.BackgroundImage = Properties.Resources.emailIcon;
            picEmail.Location = new Point(60, 204);
            picEmail.Name = "picEmail";
            picEmail.Size = new Size(45, 45);
            picEmail.SizeMode = PictureBoxSizeMode.Zoom;
            picEmail.TabIndex = 2;
            picEmail.TabStop = false;
            picEmail.Click += picEmail_Click;
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 11F);
            txtEmail.Location = new Point(134, 198);
            txtEmail.Margin = new Padding(4, 5, 4, 5);
            txtEmail.MinimumSize = new Size(1, 16);
            txtEmail.Name = "txtEmail";
            txtEmail.Padding = new Padding(6);
            txtEmail.Radius = 8;
            txtEmail.RectColor = Color.FromArgb(220, 224, 229);
            txtEmail.RectSides = ToolStripStatusLabelBorderSides.Bottom;
            txtEmail.ShowText = false;
            txtEmail.Size = new Size(479, 59);
            txtEmail.TabIndex = 3;
            txtEmail.TextAlignment = ContentAlignment.MiddleLeft;
            txtEmail.Watermark = "Email";
            // 
            // picPassword
            // 
            picPassword.BackColor = Color.FromArgb(0, 0, 0, 0);
            picPassword.BackgroundImage = Properties.Resources.passwordIcon;
            picPassword.Location = new Point(60, 293);
            picPassword.Name = "picPassword";
            picPassword.Size = new Size(45, 45);
            picPassword.SizeMode = PictureBoxSizeMode.Zoom;
            picPassword.TabIndex = 4;
            picPassword.TabStop = false;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.Location = new Point(134, 289);
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
            txtPassword.TabIndex = 5;
            txtPassword.TextAlignment = ContentAlignment.MiddleLeft;
            txtPassword.Watermark = "Mật khẩu";
            // 
            // lnkForgotPassword
            // 
            lnkForgotPassword.ActiveLinkColor = Color.FromArgb(80, 160, 255);
            lnkForgotPassword.BackColor = Color.FromArgb(0, 0, 0, 0);
            lnkForgotPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lnkForgotPassword.ForeColor = Color.FromArgb(48, 48, 48);
            lnkForgotPassword.LinkBehavior = LinkBehavior.AlwaysUnderline;
            lnkForgotPassword.LinkColor = Color.FromArgb(13, 110, 253);
            lnkForgotPassword.Location = new Point(465, 380);
            lnkForgotPassword.Name = "lnkForgotPassword";
            lnkForgotPassword.Size = new Size(167, 33);
            lnkForgotPassword.TabIndex = 6;
            lnkForgotPassword.TabStop = true;
            lnkForgotPassword.Text = "Quên mật khẩu?";
            lnkForgotPassword.VisitedLinkColor = Color.FromArgb(230, 80, 80);
            lnkForgotPassword.Click += lnkForgotPassword_Click;
            // 
            // btnLogin
            // 
            btnLogin.FillColor = Color.FromArgb(13, 110, 253);
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.Location = new Point(60, 454);
            btnLogin.MinimumSize = new Size(1, 1);
            btnLogin.Name = "btnLogin";
            btnLogin.Radius = 10;
            btnLogin.Size = new Size(553, 63);
            btnLogin.TabIndex = 7;
            btnLogin.Text = "Đăng nhập";
            btnLogin.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnLogin.Click += btnLogin_Click;
            // 
            // btnRegister
            // 
            btnRegister.FillColor = Color.White;
            btnRegister.Font = new Font("Segoe UI", 10.5F);
            btnRegister.ForeColor = Color.FromArgb(13, 110, 253);
            btnRegister.Location = new Point(60, 545);
            btnRegister.MinimumSize = new Size(1, 1);
            btnRegister.Name = "btnRegister";
            btnRegister.Radius = 10;
            btnRegister.RectColor = Color.FromArgb(13, 110, 253);
            btnRegister.Size = new Size(553, 60);
            btnRegister.TabIndex = 8;
            btnRegister.Text = "Đăng ký tài khoản mới";
            btnRegister.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnRegister.Click += btnRegister_Click;
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
            // SignIn
            // 
            BackgroundImage = Properties.Resources.bg;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1900, 950);
            Controls.Add(panelLogin);
            MaximizeBox = false;
            Name = "SignIn";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập - IELTS Learning";
            Load += SignIn_Load;
            panelLogin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picEmail).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPassword).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSlide).EndInit();
            ResumeLayout(false);
        }
    }
}
