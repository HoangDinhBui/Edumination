namespace IELTS.UI.Login
{
    partial class SignIn
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Controls
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.LinkLabel lnkForgotPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Timer timerSlide;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitle = new Label();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            lnkForgotPassword = new LinkLabel();
            btnLogin = new Button();
            btnRegister = new Button();
            panel1 = new Panel();
            pictureBoxSlide = new PictureBox();
            panelDots = new Panel();
            timerSlide = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSlide).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(80, 160, 255);
            lblTitle.Location = new Point(99, 225);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(350, 40);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🎓 IELTS LEARNING SYSTEM";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEmail
            // 
            lblEmail.Location = new Point(99, 305);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(100, 30);
            lblEmail.TabIndex = 1;
            lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(99, 335);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(350, 27);
            txtEmail.TabIndex = 2;
            // 
            // lblPassword
            // 
            lblPassword.Location = new Point(99, 385);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(100, 30);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Mật khẩu:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(99, 415);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.Size = new Size(350, 27);
            txtPassword.TabIndex = 4;
            // 
            // lnkForgotPassword
            // 
            lnkForgotPassword.ActiveLinkColor = Color.FromArgb(60, 140, 235);
            lnkForgotPassword.Font = new Font("Microsoft Sans Serif", 9F);
            lnkForgotPassword.LinkColor = Color.FromArgb(80, 160, 255);
            lnkForgotPassword.Location = new Point(99, 450);
            lnkForgotPassword.Name = "lnkForgotPassword";
            lnkForgotPassword.Size = new Size(150, 20);
            lnkForgotPassword.TabIndex = 5;
            lnkForgotPassword.TabStop = true;
            lnkForgotPassword.Text = "Quên mật khẩu?";
            lnkForgotPassword.VisitedLinkColor = Color.FromArgb(80, 160, 255);
            lnkForgotPassword.Click += lnkForgotPassword_Click;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(80, 160, 255);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(99, 485);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(350, 45);
            btnLogin.TabIndex = 6;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.FromArgb(100, 100, 100);
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Microsoft Sans Serif", 10F);
            btnRegister.ForeColor = Color.White;
            btnRegister.Location = new Point(99, 545);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(350, 40);
            btnRegister.TabIndex = 7;
            btnRegister.Text = "Đăng ký tài khoản mới";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += btnRegister_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBoxSlide);
            panel1.Controls.Add(panelDots);
            panel1.Location = new Point(572, -1);
            panel1.Name = "panel1";
            panel1.Size = new Size(1334, 962);
            panel1.TabIndex = 8;
            // 
            // pictureBoxSlide
            // 
            pictureBoxSlide.Dock = DockStyle.Fill;
            pictureBoxSlide.Location = new Point(0, 0);
            pictureBoxSlide.Name = "pictureBoxSlide";
            pictureBoxSlide.Size = new Size(1334, 922);
            pictureBoxSlide.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxSlide.TabIndex = 2;
            pictureBoxSlide.TabStop = false;
            // 
            // panelDots
            // 
            panelDots.BackColor = Color.Transparent;
            panelDots.Dock = DockStyle.Bottom;
            panelDots.Location = new Point(0, 922);
            panelDots.Name = "panelDots";
            panelDots.Size = new Size(1334, 40);
            panelDots.TabIndex = 3;
            // 
            // timerSlide
            // 
            timerSlide.Interval = 4000;
            timerSlide.Tick += TimerSlide_Tick;
            // 
            // SignIn
            // 
            BackColor = Color.White;
            ClientSize = new Size(1902, 973);
            Controls.Add(panel1);
            Controls.Add(lblTitle);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(lnkForgotPassword);
            Controls.Add(btnLogin);
            Controls.Add(btnRegister);
            MaximizeBox = false;
            Name = "SignIn";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập - IELTS Learning";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxSlide).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private PictureBox pictureBoxSlide;
        private Panel panelDots;
    }
}