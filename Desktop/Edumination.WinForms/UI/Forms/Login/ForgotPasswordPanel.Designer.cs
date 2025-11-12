using System.Drawing;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.Login
{
    partial class ForgotPasswordPanel
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblSubtitle;
        private TextBox txtEmail;
        private Button btnSendLink;
        private Label lblError;
        private Button btnBack;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblSubtitle = new Label();
            txtEmail = new TextBox();
            btnSendLink = new Button();
            lblError = new Label();
            btnBack = new Button();
            label1 = new Label();
            lblMessage = new Label();
            SuspendLayout();
            // 
            // lblSubtitle
            // 
            lblSubtitle.Location = new Point(144, 295);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(400, 26);
            lblSubtitle.TabIndex = 2;
            lblSubtitle.Text = "Enter your registered email to reset your password";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmail.Location = new Point(159, 358);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Please enter your email";
            txtEmail.Size = new Size(360, 34);
            txtEmail.TabIndex = 3;
            // 
            // btnSendLink
            // 
            btnSendLink.BackColor = Color.FromArgb(116, 155, 194);
            btnSendLink.FlatStyle = FlatStyle.Flat;
            btnSendLink.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSendLink.ForeColor = Color.White;
            btnSendLink.Location = new Point(159, 418);
            btnSendLink.Name = "btnSendLink";
            btnSendLink.Size = new Size(360, 42);
            btnSendLink.TabIndex = 4;
            btnSendLink.Text = "Send reset link";
            btnSendLink.UseVisualStyleBackColor = false;
            // 
            // lblError
            // 
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(179, 484);
            lblError.Name = "lblError";
            lblError.Size = new Size(320, 25);
            lblError.TabIndex = 5;
            lblError.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnBack
            // 
            btnBack.Cursor = Cursors.Hand;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Location = new Point(129, 191);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(120, 29);
            btnBack.TabIndex = 0;
            btnBack.Text = "< Back to login";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 19.8F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(41, 69, 99);
            label1.Location = new Point(190, 252);
            label1.Name = "label1";
            label1.Size = new Size(299, 38);
            label1.TabIndex = 7;
            label1.Text = "Forgot Password?";
            // 
            // lblMessage
            // 
            lblMessage.ForeColor = Color.Green;
            lblMessage.Location = new Point(179, 532);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(320, 25);
            lblMessage.TabIndex = 6;
            lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ForgotPasswordPanel
            // 
            BackColor = Color.White;
            Controls.Add(label1);
            Controls.Add(btnBack);
            Controls.Add(lblSubtitle);
            Controls.Add(txtEmail);
            Controls.Add(btnSendLink);
            Controls.Add(lblError);
            Controls.Add(lblMessage);
            Name = "ForgotPasswordPanel";
            Padding = new Padding(40);
            Size = new Size(760, 973);
            ResumeLayout(false);
            PerformLayout();
        }
        private Label label1;
        private Label lblMessage;
    }
}
