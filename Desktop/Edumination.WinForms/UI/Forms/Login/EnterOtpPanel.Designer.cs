
using System.Drawing;
using System.Windows.Forms;
using static ReaLTaiizor.Drawing.Poison.PoisonPaint;

namespace Edumination.WinForms.UI.Forms.Login
{
    partial class EnterOtpPanel
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblSubtitle;
        private TextBox[] txtOtp;
        private Button btnVerify;
        private Button btnBack;
        private Label lblError;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblSubtitle = new Label();
            txtOtp = new TextBox[6];
            btnVerify = new Button();
            btnBack = new Button();
            lblError = new Label();

            SuspendLayout();

            // 
            // btnBack
            // 
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Cursor = Cursors.Hand;
            btnBack.ForeColor = System.Drawing.Color.FromArgb(41, 69, 99);
            btnBack.Location = new System.Drawing.Point(20, 20);
            btnBack.Name = "btnBack";
            btnBack.Size = new System.Drawing.Size(120, 30);
            btnBack.TabIndex = 0;
            btnBack.Text = "< Back to Forgot";
            btnBack.UseVisualStyleBackColor = true;

            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(41, 69, 99);
            lblTitle.Location = new System.Drawing.Point(200, 100);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(150, 38);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Enter OTP";

            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            lblSubtitle.ForeColor = System.Drawing.Color.Gray;
            lblSubtitle.Location = new System.Drawing.Point(100, 150);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new System.Drawing.Size(380, 20);
            lblSubtitle.TabIndex = 2;
            lblSubtitle.Text = "Please enter the 6-digit code sent to your email";

            // 
            // OTP TextBoxes
            // 
            int startX = 150;
            int startY = 200;
            int width = 50;
            int spacing = 15;
            for (int i = 0; i < 6; i++)
            {
                txtOtp[i] = new TextBox();
                txtOtp[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
                txtOtp[i].Size = new System.Drawing.Size(width, 50);
                txtOtp[i].Location = new System.Drawing.Point(startX + i * (width + spacing), startY);
                txtOtp[i].TextAlign = HorizontalAlignment.Center;
                txtOtp[i].MaxLength = 1;
                txtOtp[i].TabIndex = 3 + i;
                Controls.Add(txtOtp[i]);
            }

            // 
            // btnVerify
            // 
            btnVerify.BackColor = System.Drawing.Color.FromArgb(116, 155, 194);
            btnVerify.Cursor = Cursors.Hand;
            btnVerify.FlatAppearance.BorderSize = 0;
            btnVerify.FlatStyle = FlatStyle.Flat;
            btnVerify.ForeColor = System.Drawing.Color.White;
            btnVerify.Location = new System.Drawing.Point(150, 270);
            btnVerify.Name = "btnVerify";
            btnVerify.Size = new System.Drawing.Size(350, 40);
            btnVerify.TabIndex = 9;
            btnVerify.Text = "Verify OTP";
            btnVerify.UseVisualStyleBackColor = false;

            // 
            // lblError
            // 
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Location = new System.Drawing.Point(150, 320);
            lblError.Name = "lblError";
            lblError.Size = new System.Drawing.Size(350, 20);
            lblError.TabIndex = 10;

            // 
            // EnterOtpPanel
            // 
            BackColor = System.Drawing.Color.White;
            Controls.Add(btnBack);
            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(btnVerify);
            Controls.Add(lblError);

            Name = "EnterOtpPanel";
            Padding = new Padding(40);
            Size = new System.Drawing.Size(760, 973);

            ResumeLayout(false);
            PerformLayout();
        }
    }
}

