using Sunny.UI;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.Results
{
    partial class AnswerRowPanel
    {
        private System.ComponentModel.IContainer components = null;
        private Sunny.UI.UIButton btnNo;
        private Sunny.UI.UILabel lblUserAns;
        private Sunny.UI.UILabel lblCorrectHint;
        private System.Windows.Forms.Label lblIcon;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnNo = new Sunny.UI.UIButton();
            lblUserAns = new Sunny.UI.UILabel();
            lblCorrectHint = new Sunny.UI.UILabel();
            lblIcon = new System.Windows.Forms.Label();

            SuspendLayout();

            // --- 1. Số thứ tự (Vòng tròn xanh) ---
            btnNo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNo.Location = new Point(10, 8);
            btnNo.Size = new Size(35, 35);
            btnNo.Cursor = Cursors.Default;
            btnNo.FillColor = Color.FromArgb(0, 159, 227);
            btnNo.RectColor = Color.FromArgb(0, 159, 227);
            btnNo.ForeColor = Color.White;
            btnNo.Radius = 17;
            btnNo.Style = UIStyle.Custom;
            btnNo.Text = "1";
            btnNo.TextAlign = ContentAlignment.MiddleCenter;

            // --- 2. Câu trả lời của học viên ---
            lblUserAns.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblUserAns.ForeColor = Color.FromArgb(0, 159, 227);
            lblUserAns.Location = new Point(55, 8);
            lblUserAns.Size = new Size(120, 35); // ✅ TĂNG WIDTH
            lblUserAns.Style = UIStyle.Custom;
            lblUserAns.Text = "Answer:";
            lblUserAns.TextAlign = ContentAlignment.MiddleLeft;

            // --- 3. Icon đúng/sai ---
            lblIcon.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblIcon.Location = new Point(185, 8);
            lblIcon.Size = new Size(30, 35);
            lblIcon.Text = "✕";
            lblIcon.ForeColor = Color.FromArgb(235, 85, 85);
            lblIcon.TextAlign = ContentAlignment.MiddleCenter;

            // --- 4. Gợi ý đáp án đúng ---
            lblCorrectHint.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblCorrectHint.ForeColor = Color.FromArgb(100, 100, 100);
            lblCorrectHint.Location = new Point(225, 8);
            lblCorrectHint.Size = new Size(200, 35); // ✅ TĂNG WIDTH
            lblCorrectHint.Style = UIStyle.Custom;
            lblCorrectHint.Text = "Correct: Example";
            lblCorrectHint.TextAlign = ContentAlignment.MiddleLeft;
            lblCorrectHint.AutoSize = false; // ✅ QUAN TRỌNG: Tắt AutoSize
            lblCorrectHint.Visible = true; // Hiển thị để test

            // --- 5. Panel chính ---
            this.BackColor = Color.White;
            this.Size = new Size(450, 50); // ✅ TĂNG WIDTH TỔNG THỂ
            this.Margin = new Padding(8, 5, 8, 5);
            this.BorderStyle = BorderStyle.FixedSingle; // ✅ Thêm viền để dễ nhìn

            this.Controls.Add(btnNo);
            this.Controls.Add(lblUserAns);
            this.Controls.Add(lblIcon);
            this.Controls.Add(lblCorrectHint);

            this.Name = "AnswerRowPanel";
            this.ResumeLayout(false);
        }
    }
}