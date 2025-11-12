namespace Edumination.WinForms.UI.Forms.LoginForm
{
    partial class SignInPanel
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblSubtitle;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnTogglePassword;
        private Button btnSignIn;
        private Label lblError;
        private Button btnGoogleLogin;
        private Label lblForgotPassword;
        private Label lblRegister;
        private PictureBox pictureBoxSlide;
        private Panel panelDots;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblSubtitle = new Label();
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            btnTogglePassword = new Button();
            btnSignIn = new Button();
            lblError = new Label();
            btnGoogleLogin = new Button();
            lblForgotPassword = new Label();
            lblRegister = new Label();
            pictureBoxSlide = new PictureBox();
            panelDots = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSlide).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft Sans Serif", 19.8F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 69, 99);
            lblTitle.Location = new Point(91, 60);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(254, 38);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Welcome back!";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Microsoft Sans Serif", 10.2F);
            lblSubtitle.ForeColor = Color.Gray;
            lblSubtitle.Location = new Point(130, 99);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(170, 20);
            lblSubtitle.TabIndex = 2;
            lblSubtitle.Text = "Login to your account";
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Microsoft Sans Serif", 13.8F);
            txtEmail.Location = new Point(43, 157);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Please enter Username/Email";
            txtEmail.Size = new Size(350, 34);
            txtEmail.TabIndex = 3;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Microsoft Sans Serif", 13.8F);
            txtPassword.Location = new Point(43, 219);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Please enter password";
            txtPassword.Size = new Size(350, 34);
            txtPassword.TabIndex = 4;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.FlatAppearance.BorderSize = 0;
            btnTogglePassword.FlatStyle = FlatStyle.Flat;
            btnTogglePassword.Location = new Point(360, 221);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(25, 30);
            btnTogglePassword.TabIndex = 5;
            btnTogglePassword.Text = "👁";
            // 
            // btnSignIn
            // 
            btnSignIn.BackColor = Color.FromArgb(116, 155, 194);
            btnSignIn.FlatStyle = FlatStyle.Flat;
            btnSignIn.ForeColor = Color.White;
            btnSignIn.Location = new Point(43, 289);
            btnSignIn.Name = "btnSignIn";
            btnSignIn.Size = new Size(350, 40);
            btnSignIn.TabIndex = 6;
            btnSignIn.Text = "Sign In";
            btnSignIn.UseVisualStyleBackColor = false;
            // 
            // lblError
            // 
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(43, 322);
            lblError.Name = "lblError";
            lblError.Size = new Size(350, 20);
            lblError.TabIndex = 7;
            // 
            // btnGoogleLogin
            // 
            btnGoogleLogin.BackColor = Color.White;
            btnGoogleLogin.FlatStyle = FlatStyle.Flat;
            btnGoogleLogin.ForeColor = Color.FromArgb(55, 55, 55);
            btnGoogleLogin.Location = new Point(43, 352);
            btnGoogleLogin.Name = "btnGoogleLogin";
            btnGoogleLogin.Size = new Size(350, 40);
            btnGoogleLogin.TabIndex = 8;
            btnGoogleLogin.Text = "Login with Google";
            btnGoogleLogin.UseVisualStyleBackColor = false;
            // 
            // lblForgotPassword
            // 
            lblForgotPassword.AutoSize = true;
            lblForgotPassword.ForeColor = Color.FromArgb(35, 176, 235);
            lblForgotPassword.Location = new Point(266, 256);
            lblForgotPassword.Name = "lblForgotPassword";
            lblForgotPassword.Size = new Size(127, 20);
            lblForgotPassword.TabIndex = 9;
            lblForgotPassword.Text = "Forgot password?";
            // 
            // lblRegister
            // 
            lblRegister.AutoSize = true;
            lblRegister.ForeColor = Color.FromArgb(116, 155, 194);
            lblRegister.Location = new Point(96, 446);
            lblRegister.Name = "lblRegister";
            lblRegister.Size = new Size(257, 20);
            lblRegister.TabIndex = 10;
            lblRegister.Text = "Don't have an account? Register now!";
            // 
            // pictureBoxSlide
            // 
            pictureBoxSlide.Dock = DockStyle.Fill;
            pictureBoxSlide.Location = new Point(0, 0);
            pictureBoxSlide.Name = "pictureBoxSlide";
            pictureBoxSlide.Size = new Size(918, 547);
            pictureBoxSlide.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxSlide.TabIndex = 11;
            pictureBoxSlide.TabStop = false;
            // 
            // panelDots
            // 
            panelDots.BackColor = Color.Transparent;
            panelDots.Dock = DockStyle.Bottom;
            panelDots.Location = new Point(0, 517);
            panelDots.Name = "panelDots";
            panelDots.Size = new Size(918, 30);
            panelDots.TabIndex = 0;
            // 
            // SignInPanel
            // 
            Controls.Add(panelDots);
            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(txtEmail);
            Controls.Add(txtPassword);
            Controls.Add(btnTogglePassword);
            Controls.Add(btnSignIn);
            Controls.Add(lblError);
            Controls.Add(btnGoogleLogin);
            Controls.Add(lblForgotPassword);
            Controls.Add(lblRegister);
            Controls.Add(pictureBoxSlide);
            Name = "SignInPanel";
            Size = new Size(918, 547);
            ((System.ComponentModel.ISupportInitialize)pictureBoxSlide).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
