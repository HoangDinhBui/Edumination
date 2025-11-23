using Sunny.UI;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.Results
{
    partial class AnswerRowPanel
    {
        private System.ComponentModel.IContainer components = null;

        private UIButton lblNumber;
        private UILabel lblUserAnswer;
        private UILabel lblCorrectAnswer;
        private Label lblIcon;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblNumber = new UIButton();
            lblUserAnswer = new UILabel();
            lblCorrectAnswer = new UILabel();
            lblIcon = new Label();

            SuspendLayout();

            // ===================== NUMBER BADGE =====================
            lblNumber.FillColor = Color.FromArgb(225, 240, 255);
            lblNumber.FillHoverColor = lblNumber.FillColor;
            lblNumber.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNumber.ForeColor = Color.FromArgb(39, 56, 146);
            lblNumber.Location = new Point(0, 3);
            lblNumber.MinimumSize = new Size(1, 1);
            lblNumber.Radius = 16;
            lblNumber.RectColor = Color.FromArgb(210, 230, 250);
            lblNumber.RectHoverColor = lblNumber.RectColor;
            lblNumber.Size = new Size(32, 32);
            lblNumber.Style = UIStyle.Custom;
            lblNumber.TextAlign = ContentAlignment.MiddleCenter;
            lblNumber.Text = "1";

            // ===================== USER ANSWER =====================
            lblUserAnswer.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            lblUserAnswer.ForeColor = Color.FromArgb(39, 56, 146);
            lblUserAnswer.Location = new Point(48, 3);
            lblUserAnswer.Size = new Size(215, 32);
            lblUserAnswer.TextAlign = ContentAlignment.MiddleLeft;
            lblUserAnswer.Text = "1. Keiko";

            // ===================== CORRECT ANSWER =====================
            lblCorrectAnswer.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            lblCorrectAnswer.ForeColor = Color.FromArgb(140, 140, 140);
            lblCorrectAnswer.Location = new Point(265, 3);
            lblCorrectAnswer.Size = new Size(215, 32);
            lblCorrectAnswer.TextAlign = ContentAlignment.MiddleLeft;
            lblCorrectAnswer.Text = "Correct: B";

            // ===================== ICON ✓ / ✕ =====================
            lblIcon.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblIcon.Location = new Point(500, 3);
            lblIcon.Size = new Size(32, 32);
            lblIcon.Text = "✕";
            lblIcon.ForeColor = Color.FromArgb(235, 85, 85);
            lblIcon.TextAlign = ContentAlignment.MiddleCenter;

            // ===================== PANEL WRAPPER =====================
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;

            // FIX SIZE PRECISELY
            Size = new Size(540, 38);
            MinimumSize = new Size(540, 38);
            MaximumSize = new Size(540, 38);
            Margin = new Padding(0, 4, 0, 4);

            Controls.Add(lblNumber);
            Controls.Add(lblUserAnswer);
            Controls.Add(lblCorrectAnswer);
            Controls.Add(lblIcon);

            Name = "AnswerRowPanel";

            ResumeLayout(false);
        }
    }
}
