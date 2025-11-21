namespace Edumination.WinForms.UI.Forms.Login
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
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
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft Sans Serif", 19.8F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 69, 99);
            lblTitle.Location = new Point(229, 174);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(254, 38);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Welcome back!";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Microsoft Sans Serif", 10.2F);
            lblSubtitle.ForeColor = Color.Gray;
            lblSubtitle.Location = new Point(268, 213);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(170, 20);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Login to your account";
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Microsoft Sans Serif", 13.8F);
            txtEmail.Location = new Point(181, 271);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Please enter Username/Email";
            txtEmail.Size = new Size(350, 34);
            txtEmail.TabIndex = 2;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Microsoft Sans Serif", 13.8F);
            txtPassword.Location = new Point(181, 333);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Please enter password";
            txtPassword.Size = new Size(350, 34);
            txtPassword.TabIndex = 3;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.FlatAppearance.BorderSize = 0;
            btnTogglePassword.FlatStyle = FlatStyle.Flat;
            btnTogglePassword.Location = new Point(498, 335);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(25, 30);
            btnTogglePassword.TabIndex = 4;
            btnTogglePassword.Text = "👁";
            // 
            // btnSignIn
            // 
            btnSignIn.BackColor = Color.FromArgb(116, 155, 194);
            btnSignIn.Cursor = Cursors.Hand;
            btnSignIn.FlatAppearance.BorderSize = 0;
            btnSignIn.FlatStyle = FlatStyle.Flat;
            btnSignIn.ForeColor = Color.White;
            btnSignIn.Location = new Point(181, 403);
            btnSignIn.Name = "btnSignIn";
            btnSignIn.Size = new Size(350, 40);
            btnSignIn.TabIndex = 5;
            btnSignIn.Text = "Sign In";
            btnSignIn.UseVisualStyleBackColor = false;
            // 
            // lblError
            // 
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(181, 446);
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
            btnGoogleLogin.Location = new Point(181, 476);
            btnGoogleLogin.Name = "btnGoogleLogin";
            btnGoogleLogin.Size = new Size(350, 40);
            btnGoogleLogin.TabIndex = 7;
            btnGoogleLogin.Text = "Login with Google";
            btnGoogleLogin.UseVisualStyleBackColor = false;
            // 
            // lblForgotPassword
            // 
            lblForgotPassword.AutoSize = true;
            lblForgotPassword.Cursor = Cursors.Hand;
            lblForgotPassword.ForeColor = Color.FromArgb(35, 176, 235);
            lblForgotPassword.Location = new Point(404, 519);
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
            lblRegister.Location = new Point(234, 560);
            lblRegister.Name = "lblRegister";
            lblRegister.Size = new Size(257, 20);
            lblRegister.TabIndex = 9;
            lblRegister.Text = "Don't have an account? Register now!";
            // 
            // SignInPanel
            // 
            BackColor = Color.White;
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
            Name = "SignInPanel";
            Padding = new Padding(40);
            Size = new Size(760, 973);
            Load += SignInPanel_Load;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
