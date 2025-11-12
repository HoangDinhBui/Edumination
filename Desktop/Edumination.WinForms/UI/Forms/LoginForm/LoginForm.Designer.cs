namespace Edumination.WinForms

{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnTogglePassword;
        private System.Windows.Forms.Button btnSignIn;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnGoogleLogin;
        private System.Windows.Forms.Label lblForgotPassword;
        private System.Windows.Forms.Label lblRegister;
        private System.Windows.Forms.PictureBox pictureBoxSlide;
        private System.Windows.Forms.Timer timerSlide;
        private System.Windows.Forms.Panel panelDots; // dot indicators

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelLeft = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            txtEmail = new TextBox();
            btnTogglePassword = new Button();
            btnSignIn = new Button();
            lblError = new Label();
            btnGoogleLogin = new Button();
            lblForgotPassword = new Label();
            lblRegister = new Label();
            txtPassword = new TextBox();
            panelRight = new Panel();
            pictureBoxSlide = new PictureBox();
            panelDots = new Panel();
            timerSlide = new System.Windows.Forms.Timer(components);
            panelLeft.SuspendLayout();
            panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSlide).BeginInit();
            SuspendLayout();
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.White;
            panelLeft.Controls.Add(lblTitle);
            panelLeft.Controls.Add(lblSubtitle);
            panelLeft.Controls.Add(txtEmail);
            panelLeft.Controls.Add(btnTogglePassword);
            panelLeft.Controls.Add(btnSignIn);
            panelLeft.Controls.Add(lblError);
            panelLeft.Controls.Add(btnGoogleLogin);
            panelLeft.Controls.Add(lblForgotPassword);
            panelLeft.Controls.Add(lblRegister);
            panelLeft.Controls.Add(txtPassword);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Name = "panelLeft";
            panelLeft.Padding = new Padding(40);
            panelLeft.Size = new Size(450, 500);
            panelLeft.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft Sans Serif", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(41, 69, 99);
            lblTitle.Location = new Point(88, 54);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(262, 39);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Welcome back!";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitle.ForeColor = Color.Gray;
            lblSubtitle.Location = new Point(127, 93);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(170, 20);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Login to your account";
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Microsoft Sans Serif", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmail.Location = new Point(40, 151);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Please enter Username/Email";
            txtEmail.Size = new Size(350, 34);
            txtEmail.TabIndex = 2;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.FlatAppearance.BorderSize = 0;
            btnTogglePassword.FlatStyle = FlatStyle.Flat;
            btnTogglePassword.Location = new Point(357, 215);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(25, 30);
            btnTogglePassword.TabIndex = 4;
            btnTogglePassword.Text = "👁";
            btnTogglePassword.Click += BtnTogglePassword_Click;
            // 
            // btnSignIn
            // 
            btnSignIn.BackColor = Color.FromArgb(116, 155, 194);
            btnSignIn.Cursor = Cursors.Hand;
            btnSignIn.FlatAppearance.BorderSize = 0;
            btnSignIn.FlatStyle = FlatStyle.Flat;
            btnSignIn.ForeColor = Color.White;
            btnSignIn.Location = new Point(40, 283);
            btnSignIn.Name = "btnSignIn";
            btnSignIn.Size = new Size(350, 40);
            btnSignIn.TabIndex = 5;
            btnSignIn.Text = "Sign In";
            btnSignIn.UseVisualStyleBackColor = false;
            btnSignIn.Click += BtnSignIn_Click;
            // 
            // lblError
            // 
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(40, 316);
            lblError.Name = "lblError";
            lblError.Size = new Size(350, 20);
            lblError.TabIndex = 6;
            // 
            // btnGoogleLogin
            // 
            btnGoogleLogin.BackColor = Color.White;
            btnGoogleLogin.Cursor = Cursors.Hand;
            btnGoogleLogin.FlatStyle = FlatStyle.Flat;
            btnGoogleLogin.ForeColor = Color.FromArgb(55, 55, 55);
            btnGoogleLogin.Location = new Point(40, 346);
            btnGoogleLogin.Name = "btnGoogleLogin";
            btnGoogleLogin.Size = new Size(350, 40);
            btnGoogleLogin.TabIndex = 7;
            btnGoogleLogin.Text = "Login with Google";
            btnGoogleLogin.UseVisualStyleBackColor = false;
            btnGoogleLogin.Click += BtnGoogleLogin_Click;
            // 
            // lblForgotPassword
            // 
            lblForgotPassword.AutoSize = true;
            lblForgotPassword.Cursor = Cursors.Hand;
            lblForgotPassword.ForeColor = Color.FromArgb(35, 176, 235);
            lblForgotPassword.Location = new Point(240, 396);
            lblForgotPassword.Name = "lblForgotPassword";
            lblForgotPassword.Size = new Size(127, 20);
            lblForgotPassword.TabIndex = 8;
            lblForgotPassword.Text = "Forgot password?";
            // 
            // lblRegister
            // 
            lblRegister.AutoSize = true;
            lblRegister.Cursor = Cursors.Hand;
            lblRegister.ForeColor = Color.FromArgb(116, 155, 194);
            lblRegister.Location = new Point(93, 440);
            lblRegister.Name = "lblRegister";
            lblRegister.Size = new Size(257, 20);
            lblRegister.TabIndex = 9;
            lblRegister.Text = "Don't have an account? Register now!";
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Microsoft Sans Serif", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassword.Location = new Point(40, 213);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Please enter password";
            txtPassword.Size = new Size(350, 34);
            txtPassword.TabIndex = 3;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.FromArgb(36, 86, 128);
            panelRight.Controls.Add(pictureBoxSlide);
            panelRight.Controls.Add(panelDots);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(450, 0);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(450, 500);
            panelRight.TabIndex = 0;
            // 
            // pictureBoxSlide
            // 
            pictureBoxSlide.Dock = DockStyle.Fill;
            pictureBoxSlide.Location = new Point(0, 0);
            pictureBoxSlide.Name = "pictureBoxSlide";
            pictureBoxSlide.Size = new Size(450, 470);
            pictureBoxSlide.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxSlide.TabIndex = 0;
            pictureBoxSlide.TabStop = false;
            // 
            // panelDots
            // 
            panelDots.BackColor = Color.Transparent;
            panelDots.Dock = DockStyle.Bottom;
            panelDots.Location = new Point(0, 470);
            panelDots.Name = "panelDots";
            panelDots.Size = new Size(450, 30);
            panelDots.TabIndex = 1;
            // 
            // timerSlide
            // 
            timerSlide.Interval = 4000;
            timerSlide.Tick += TimerSlide_Tick;
            // 
            // LoginForm
            // 
            ClientSize = new Size(900, 500);
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            Name = "LoginForm";
            Text = "Sign In";
            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxSlide).EndInit();
            ResumeLayout(false);
        }
    }
}

