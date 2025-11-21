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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();

            // Form properties
            this.ClientSize = new System.Drawing.Size(1920, 1020);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng ký tài khoản - IELTS Learning";
            this.BackColor = System.Drawing.Color.White;
            this.MaximizeBox = false;

            int controlWidth = 400;
            int controlHeight = 40;
            int labelHeight = 25;
            int xCenter = 760; // 1920/2 - 400/2, căn giữa

            // Title
            lblTitle = new System.Windows.Forms.Label();
            lblTitle.Text = "📝 ĐĂNG KÝ TÀI KHOẢN";
            lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(80, 160, 255);
            lblTitle.Size = new System.Drawing.Size(controlWidth, 50);
            lblTitle.Location = new System.Drawing.Point(xCenter, 80);
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitle);

            // Full Name
            lblFullName = new System.Windows.Forms.Label();
            lblFullName.Text = "Họ và tên:";
            lblFullName.Size = new System.Drawing.Size(controlWidth, labelHeight);
            lblFullName.Location = new System.Drawing.Point(xCenter, 150);
            this.Controls.Add(lblFullName);

            txtFullName = new System.Windows.Forms.TextBox();
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new System.Drawing.Size(controlWidth, controlHeight);
            txtFullName.Location = new System.Drawing.Point(xCenter, 180);
            this.Controls.Add(txtFullName);

            // Email
            lblEmail = new System.Windows.Forms.Label();
            lblEmail.Text = "Email:";
            lblEmail.Size = new System.Drawing.Size(controlWidth, labelHeight);
            lblEmail.Location = new System.Drawing.Point(xCenter, 240);
            this.Controls.Add(lblEmail);

            txtEmail = new System.Windows.Forms.TextBox();
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new System.Drawing.Size(controlWidth, controlHeight);
            txtEmail.Location = new System.Drawing.Point(xCenter, 270);
            this.Controls.Add(txtEmail);

            // Password
            lblPassword = new System.Windows.Forms.Label();
            lblPassword.Text = "Mật khẩu:";
            lblPassword.Size = new System.Drawing.Size(controlWidth, labelHeight);
            lblPassword.Location = new System.Drawing.Point(xCenter, 330);
            this.Controls.Add(lblPassword);

            txtPassword = new System.Windows.Forms.TextBox();
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new System.Drawing.Size(controlWidth, controlHeight);
            txtPassword.Location = new System.Drawing.Point(xCenter, 360);
            txtPassword.PasswordChar = '●';
            this.Controls.Add(txtPassword);

            // Confirm Password
            lblConfirmPassword = new System.Windows.Forms.Label();
            lblConfirmPassword.Text = "Xác nhận mật khẩu:";
            lblConfirmPassword.Size = new System.Drawing.Size(controlWidth, labelHeight);
            lblConfirmPassword.Location = new System.Drawing.Point(xCenter, 420);
            this.Controls.Add(lblConfirmPassword);

            txtConfirmPassword = new System.Windows.Forms.TextBox();
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new System.Drawing.Size(controlWidth, controlHeight);
            txtConfirmPassword.Location = new System.Drawing.Point(xCenter, 450);
            txtConfirmPassword.PasswordChar = '●';
            this.Controls.Add(txtConfirmPassword);

            // Register Button
            btnRegister = new System.Windows.Forms.Button();
            btnRegister.Text = "Đăng ký";
            btnRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            btnRegister.Size = new System.Drawing.Size(controlWidth, 50);
            btnRegister.Location = new System.Drawing.Point(xCenter, 520);
            btnRegister.BackColor = System.Drawing.Color.FromArgb(80, 160, 255);
            btnRegister.ForeColor = System.Drawing.Color.White;
            btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            this.Controls.Add(btnRegister);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
