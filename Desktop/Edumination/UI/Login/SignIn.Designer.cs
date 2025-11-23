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

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // frmLogin properties
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(450, 550);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.Text = "Đăng nhập - IELTS Learning";
            this.BackColor = System.Drawing.Color.White;

            // lblTitle
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTitle.Text = "🎓 IELTS LEARNING SYSTEM";
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(80, 160, 255);
            this.lblTitle.Size = new System.Drawing.Size(350, 40);
            this.lblTitle.Location = new System.Drawing.Point(50, 70);
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Controls.Add(this.lblTitle);

            // lblEmail
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblEmail.Text = "Email:";
            this.lblEmail.Location = new System.Drawing.Point(50, 150);
            this.lblEmail.Size = new System.Drawing.Size(100, 30);
            this.Controls.Add(this.lblEmail);

            // txtEmail
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Location = new System.Drawing.Point(50, 180);
            this.txtEmail.Size = new System.Drawing.Size(350, 30);
            this.Controls.Add(this.txtEmail);

            // lblPassword
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblPassword.Text = "Mật khẩu:";
            this.lblPassword.Location = new System.Drawing.Point(50, 230);
            this.lblPassword.Size = new System.Drawing.Size(100, 30);
            this.Controls.Add(this.lblPassword);

            // txtPassword
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Location = new System.Drawing.Point(50, 260);
            this.txtPassword.Size = new System.Drawing.Size(350, 30);
            this.txtPassword.PasswordChar = '●';
            this.Controls.Add(this.txtPassword);

            // lnkForgotPassword
            this.lnkForgotPassword = new System.Windows.Forms.LinkLabel();
            this.lnkForgotPassword.Name = "lnkForgotPassword";
            this.lnkForgotPassword.Text = "Quên mật khẩu?";
            this.lnkForgotPassword.Location = new System.Drawing.Point(50, 295);
            this.lnkForgotPassword.Size = new System.Drawing.Size(150, 20);
            this.lnkForgotPassword.LinkColor = System.Drawing.Color.FromArgb(80, 160, 255);
            this.lnkForgotPassword.ActiveLinkColor = System.Drawing.Color.FromArgb(60, 140, 235);
            this.lnkForgotPassword.VisitedLinkColor = System.Drawing.Color.FromArgb(80, 160, 255);
            this.lnkForgotPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lnkForgotPassword.Click += new System.EventHandler(this.lnkForgotPassword_Click);
            this.Controls.Add(this.lnkForgotPassword);

            // btnLogin
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.btnLogin.Size = new System.Drawing.Size(350, 45);
            this.btnLogin.Location = new System.Drawing.Point(50, 330);
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(80, 160, 255);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            this.Controls.Add(this.btnLogin);

            // btnRegister
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Text = "Đăng ký tài khoản mới";
            this.btnRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnRegister.Size = new System.Drawing.Size(350, 40);
            this.btnRegister.Location = new System.Drawing.Point(50, 390);
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            this.Controls.Add(this.btnRegister);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}