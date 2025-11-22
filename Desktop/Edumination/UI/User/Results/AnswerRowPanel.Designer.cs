using Sunny.UI;

namespace IELTS.UI.User.Results
{
    partial class AnswerRowPanel
    {
        private System.ComponentModel.IContainer components = null;

        private UILabel lblNumber;
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
            components = new System.ComponentModel.Container();
            lblNumber = new UILabel();
            lblUserAnswer = new UILabel();
            lblCorrectAnswer = new UILabel();
            lblIcon = new Label();

            SuspendLayout();
            // 
            // lblNumber (vòng tròn xanh chứa số câu)
            // 
            lblNumber.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNumber.ForeColor = Color.White;
            lblNumber.BackColor = Color.FromArgb(0, 122, 204);
            lblNumber.Size = new Size(28, 28);
            lblNumber.Location = new Point(5, 8);
            lblNumber.TextAlign = ContentAlignment.MiddleCenter;
            lblNumber.Radius = 14;
            lblNumber.Padding = new Padding(0);
            lblNumber.Text = "1";
            // 
            // lblUserAnswer
            // 
            lblUserAnswer.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblUserAnswer.ForeColor = Color.FromArgb(51, 51, 51);
            lblUserAnswer.Location = new Point(45, 6);
            lblUserAnswer.Size = new Size(260, 30);
            lblUserAnswer.TextAlign = ContentAlignment.MiddleLeft;
            lblUserAnswer.Text = "User: A";
            // 
            // lblCorrectAnswer
            // 
            lblCorrectAnswer.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblCorrectAnswer.ForeColor = Color.FromArgb(120, 120, 120);
            lblCorrectAnswer.Location = new Point(310, 6);
            lblCorrectAnswer.Size = new Size(260, 30);
            lblCorrectAnswer.TextAlign = ContentAlignment.MiddleLeft;
            lblCorrectAnswer.Text = "Correct: C";
            // 
            // lblIcon
            // 
            lblIcon.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblIcon.Location = new Point(580, 7);
            lblIcon.AutoSize = false;
            lblIcon.Size = new Size(30, 30);
            lblIcon.TextAlign = ContentAlignment.MiddleCenter;
            lblIcon.Text = "✓";
            lblIcon.ForeColor = Color.FromArgb(0, 160, 80);

            // 
            // AnswerRowPanel
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            BorderStyle = BorderStyle.None;
            Controls.Add(lblNumber);
            Controls.Add(lblUserAnswer);
            Controls.Add(lblCorrectAnswer);
            Controls.Add(lblIcon);
            Name = "AnswerRowPanel";
            Size = new Size(630, 45);
            ResumeLayout(false);
        }
    }
}
