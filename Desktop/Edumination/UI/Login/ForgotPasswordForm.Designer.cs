namespace IELTS.UI.Login
{
    partial class ForgotPasswordForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Controls
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblInstruction;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnSendOtp;
        
        private System.Windows.Forms.Label lblOtp;
        private System.Windows.Forms.TextBox txtOtp;
        
        private System.Windows.Forms.Label lblNewPassword;
        private System.Windows.Forms.TextBox txtNewPassword;
        
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        
        private System.Windows.Forms.Button btnResetPassword;
        private System.Windows.Forms.Button btnCancel;

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

            // Form properties
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(500, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Text = "Quên mật khẩu - IELTS Learning";
            this.BackColor = System.Drawing.Color.White;

            // lblTitle
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTitle.Text = "🔐 Quên mật khẩu";
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(80, 160, 255);
            this.lblTitle.Size = new System.Drawing.Size(400, 40);
            this.lblTitle.Location = new System.Drawing.Point(50, 30);
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Controls.Add(this.lblTitle);

            // lblInstruction
            this.lblInstruction = new System.Windows.Forms.Label();
            this.lblInstruction.Text = "Nhập email của bạn để nhận mã OTP";
            this.lblInstruction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblInstruction.ForeColor = System.Drawing.Color.Gray;
            this.lblInstruction.Size = new System.Drawing.Size(400, 30);
            this.lblInstruction.Location = new System.Drawing.Point(50, 75);
            this.lblInstruction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Controls.Add(this.lblInstruction);

            // lblEmail
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblEmail.Text = "Email:";
            this.lblEmail.Location = new System.Drawing.Point(50, 120);
            this.lblEmail.Size = new System.Drawing.Size(100, 25);
            this.lblEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(this.lblEmail);

            // txtEmail
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Location = new System.Drawing.Point(50, 145);
            this.txtEmail.Size = new System.Drawing.Size(300, 30);
            this.txtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.Controls.Add(this.txtEmail);

            // btnSendOtp
            this.btnSendOtp = new System.Windows.Forms.Button();
            this.btnSendOtp.Name = "btnSendOtp";
            this.btnSendOtp.Text = "Gửi mã OTP";
            this.btnSendOtp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnSendOtp.Size = new System.Drawing.Size(90, 30);
            this.btnSendOtp.Location = new System.Drawing.Point(360, 145);
            this.btnSendOtp.BackColor = System.Drawing.Color.FromArgb(80, 160, 255);
            this.btnSendOtp.ForeColor = System.Drawing.Color.White;
            this.btnSendOtp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendOtp.FlatAppearance.BorderSize = 0;
            this.btnSendOtp.Click += new System.EventHandler(this.btnSendOtp_Click);
            this.Controls.Add(this.btnSendOtp);

            // lblOtp
            this.lblOtp = new System.Windows.Forms.Label();
            this.lblOtp.Text = "Mã OTP (6 số):";
            this.lblOtp.Location = new System.Drawing.Point(50, 200);
            this.lblOtp.Size = new System.Drawing.Size(150, 25);
            this.lblOtp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(this.lblOtp);

            // txtOtp
            this.txtOtp = new System.Windows.Forms.TextBox();
            this.txtOtp.Name = "txtOtp";
            this.txtOtp.Location = new System.Drawing.Point(50, 225);
            this.txtOtp.Size = new System.Drawing.Size(400, 30);
            this.txtOtp.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtOtp.MaxLength = 6;
            this.txtOtp.Enabled = false;
            this.Controls.Add(this.txtOtp);

            // lblNewPassword
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.lblNewPassword.Text = "Mật khẩu mới:";
            this.lblNewPassword.Location = new System.Drawing.Point(50, 280);
            this.lblNewPassword.Size = new System.Drawing.Size(150, 25);
            this.lblNewPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(this.lblNewPassword);

            // txtNewPassword
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Location = new System.Drawing.Point(50, 305);
            this.txtNewPassword.Size = new System.Drawing.Size(400, 30);
            this.txtNewPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtNewPassword.PasswordChar = '●';
            this.txtNewPassword.Enabled = false;
            this.Controls.Add(this.txtNewPassword);

            // lblConfirmPassword
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.lblConfirmPassword.Text = "Xác nhận mật khẩu:";
            this.lblConfirmPassword.Location = new System.Drawing.Point(50, 360);
            this.lblConfirmPassword.Size = new System.Drawing.Size(180, 25);
            this.lblConfirmPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Controls.Add(this.lblConfirmPassword);

            // txtConfirmPassword
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.Location = new System.Drawing.Point(50, 385);
            this.txtConfirmPassword.Size = new System.Drawing.Size(400, 30);
            this.txtConfirmPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.txtConfirmPassword.PasswordChar = '●';
            this.txtConfirmPassword.Enabled = false;
            this.Controls.Add(this.txtConfirmPassword);

            // btnResetPassword
            this.btnResetPassword = new System.Windows.Forms.Button();
            this.btnResetPassword.Name = "btnResetPassword";
            this.btnResetPassword.Text = "Đặt lại mật khẩu";
            this.btnResetPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.btnResetPassword.Size = new System.Drawing.Size(400, 45);
            this.btnResetPassword.Location = new System.Drawing.Point(50, 450);
            this.btnResetPassword.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnResetPassword.ForeColor = System.Drawing.Color.White;
            this.btnResetPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetPassword.FlatAppearance.BorderSize = 0;
            this.btnResetPassword.Enabled = false;
            this.btnResetPassword.Click += new System.EventHandler(this.btnResetPassword_Click);
            this.Controls.Add(this.btnResetPassword);

            // btnCancel
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Size = new System.Drawing.Size(400, 40);
            this.btnCancel.Location = new System.Drawing.Point(50, 510);
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.Controls.Add(this.btnCancel);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
