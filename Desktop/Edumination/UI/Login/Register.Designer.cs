namespace IELTS.UI.Login
{
    partial class Register
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Timer timerSlide;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitle = new Label();
            lblFullName = new Label();
            txtFullName = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            lblConfirmPassword = new Label();
            txtConfirmPassword = new TextBox();
            btnRegister = new Button();
            panel1 = new Panel();
            pictureBoxSlide = new PictureBox();
            panelDots = new Panel();
            panel1.SuspendLayout();
            timerSlide = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)pictureBoxSlide).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(80, 160, 255);
            lblTitle.Location = new Point(160, 235);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(440, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📝 ĐĂNG KÝ TÀI KHOẢN";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFullName
            // 
            lblFullName.Location = new Point(182, 307);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(400, 25);
            lblFullName.TabIndex = 1;
            lblFullName.Text = "Họ và tên:";
            // 
            // txtFullName
            // 
            txtFullName.Location = new Point(182, 337);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(400, 27);
            txtFullName.TabIndex = 2;
            // 
            // lblEmail
            // 
            lblEmail.Location = new Point(182, 397);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(400, 25);
            lblEmail.TabIndex = 3;
            lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(182, 427);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(400, 27);
            txtEmail.TabIndex = 4;
            // 
            // lblPassword
            // 
            lblPassword.Location = new Point(182, 487);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(400, 25);
            lblPassword.TabIndex = 5;
            lblPassword.Text = "Mật khẩu:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(182, 517);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.Size = new Size(400, 27);
            txtPassword.TabIndex = 6;
            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.Location = new Point(182, 577);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(400, 25);
            lblConfirmPassword.TabIndex = 7;
            lblConfirmPassword.Text = "Xác nhận mật khẩu:";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Location = new Point(182, 607);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PasswordChar = '●';
            txtConfirmPassword.Size = new Size(400, 27);
            txtConfirmPassword.TabIndex = 8;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.FromArgb(80, 160, 255);
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            btnRegister.ForeColor = Color.White;
            btnRegister.Location = new Point(182, 677);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(400, 50);
            btnRegister.TabIndex = 9;
            btnRegister.Text = "Đăng ký";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += btnRegister_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBoxSlide);
            panel1.Controls.Add(panelDots);
            panel1.Location = new Point(800, 8);
            panel1.Name = "panel1";
            panel1.Size = new Size(1090, 953);
            panel1.TabIndex = 10;
            // 
            // pictureBoxSlide
            // 
            pictureBoxSlide.Dock = DockStyle.Fill;
            pictureBoxSlide.Location = new Point(0, 0);
            pictureBoxSlide.Name = "pictureBoxSlide";
            pictureBoxSlide.Size = new Size(1090, 913);
            pictureBoxSlide.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxSlide.TabIndex = 4;
            pictureBoxSlide.TabStop = false;
            // 
            // panelDots
            // 
            panelDots.BackColor = Color.Transparent;
            panelDots.Dock = DockStyle.Bottom;
            panelDots.Location = new Point(0, 913);
            panelDots.Name = "panelDots";
            panelDots.Size = new Size(1090, 40);
            panelDots.TabIndex = 5;
            // 
            // timerSlide
            // 
            timerSlide.Interval = 4000;
            timerSlide.Tick += TimerSlide_Tick;
            // 
            // Register
            // 
            BackColor = Color.White;
            ClientSize = new Size(1902, 973);
            Controls.Add(panel1);
            Controls.Add(lblTitle);
            Controls.Add(lblFullName);
            Controls.Add(txtFullName);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(lblConfirmPassword);
            Controls.Add(txtConfirmPassword);
            Controls.Add(btnRegister);
            MaximizeBox = false;
            Name = "Register";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng ký tài khoản - IELTS Learning";
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
