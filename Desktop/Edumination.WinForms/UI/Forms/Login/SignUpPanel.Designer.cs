namespace Edumination.WinForms.UI.Forms.Login
{
    partial class SignUpPanel
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblFirstName;
        private Label lblLastName;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnTogglePassword;
        private Label lblConfirmPassword;
        private TextBox txtConfirmPassword;
        private Button btnToggleConfirmPassword;
        private Label lblError;
        private Button btnSignUp;
        private Button btnGoogle;
        private Label lblSignInLink;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblFirstName = new Label();
            lblLastName = new Label();
            txtFirstName = new TextBox();
            txtLastName = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnTogglePassword = new Button();
            lblConfirmPassword = new Label();
            txtConfirmPassword = new TextBox();
            btnToggleConfirmPassword = new Button();
            lblError = new Label();
            btnSignUp = new Button();
            btnGoogle = new Button();
            lblSignInLink = new Label();
            label1 = new Label();
            SuspendLayout();
            // 
            // lblFirstName
            // 
            lblFirstName.AutoSize = true;
            lblFirstName.Location = new Point(137, 220);
            lblFirstName.Name = "lblFirstName";
            lblFirstName.Size = new Size(80, 20);
            lblFirstName.TabIndex = 1;
            lblFirstName.Text = "First Name";
            // 
            // lblLastName
            // 
            lblLastName.AutoSize = true;
            lblLastName.Location = new Point(357, 220);
            lblLastName.Name = "lblLastName";
            lblLastName.Size = new Size(79, 20);
            lblLastName.TabIndex = 2;
            lblLastName.Text = "Last Name";
            // 
            // txtFirstName
            // 
            txtFirstName.Location = new Point(137, 240);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(200, 27);
            txtFirstName.TabIndex = 3;
            // 
            // txtLastName
            // 
            txtLastName.Location = new Point(357, 240);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(200, 27);
            txtLastName.TabIndex = 4;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(137, 285);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(46, 20);
            lblEmail.TabIndex = 5;
            lblEmail.Text = "Email";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(137, 305);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(420, 27);
            txtEmail.TabIndex = 6;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(137, 350);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(70, 20);
            lblPassword.TabIndex = 7;
            lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(137, 370);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(380, 27);
            txtPassword.TabIndex = 8;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.Location = new Point(527, 370);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(30, 27);
            btnTogglePassword.TabIndex = 9;
            btnTogglePassword.Text = "👁";
            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Location = new Point(137, 415);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(127, 20);
            lblConfirmPassword.TabIndex = 10;
            lblConfirmPassword.Text = "Confirm Password";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Location = new Point(137, 435);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(380, 27);
            txtConfirmPassword.TabIndex = 11;
            txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // btnToggleConfirmPassword
            // 
            btnToggleConfirmPassword.Location = new Point(527, 435);
            btnToggleConfirmPassword.Name = "btnToggleConfirmPassword";
            btnToggleConfirmPassword.Size = new Size(30, 27);
            btnToggleConfirmPassword.TabIndex = 12;
            btnToggleConfirmPassword.Text = "👁";
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(137, 475);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 20);
            lblError.TabIndex = 13;
            // 
            // btnSignUp
            // 
            btnSignUp.BackColor = Color.SlateGray;
            btnSignUp.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSignUp.ForeColor = Color.White;
            btnSignUp.Location = new Point(137, 510);
            btnSignUp.Name = "btnSignUp";
            btnSignUp.Size = new Size(420, 40);
            btnSignUp.TabIndex = 14;
            btnSignUp.Text = "Sign Up";
            btnSignUp.UseVisualStyleBackColor = false;
            // 
            // btnGoogle
            // 
            btnGoogle.Location = new Point(137, 565);
            btnGoogle.Name = "btnGoogle";
            btnGoogle.Size = new Size(420, 40);
            btnGoogle.TabIndex = 15;
            btnGoogle.Text = "Login with Google";
            // 
            // lblSignInLink
            // 
            lblSignInLink.AutoSize = true;
            lblSignInLink.Cursor = Cursors.Hand;
            lblSignInLink.ForeColor = Color.Blue;
            lblSignInLink.Location = new Point(222, 626);
            lblSignInLink.Name = "lblSignInLink";
            lblSignInLink.Size = new Size(227, 20);
            lblSignInLink.TabIndex = 16;
            lblSignInLink.Text = "Already have an account? Sign in";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 19.8F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(41, 69, 99);
            label1.Location = new Point(186, 155);
            label1.Name = "label1";
            label1.Size = new Size(331, 38);
            label1.TabIndex = 17;
            label1.Text = "Create your account";
            // 
            // SignUpPanel
            // 
            BackColor = Color.White;
            Controls.Add(label1);
            Controls.Add(lblFirstName);
            Controls.Add(lblLastName);
            Controls.Add(txtFirstName);
            Controls.Add(txtLastName);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnTogglePassword);
            Controls.Add(lblConfirmPassword);
            Controls.Add(txtConfirmPassword);
            Controls.Add(btnToggleConfirmPassword);
            Controls.Add(lblError);
            Controls.Add(btnSignUp);
            Controls.Add(btnGoogle);
            Controls.Add(lblSignInLink);
            Name = "SignUpPanel";
            Size = new Size(760, 973);
            ResumeLayout(false);
            PerformLayout();
        }
        private Label label1;
    }
}
