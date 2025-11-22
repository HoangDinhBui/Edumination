using Sunny.UI;

namespace IELTS.UI.User.TestTaking.Controls
{
    partial class WritingAnswerPanel
    {
        private System.ComponentModel.IContainer components = null;

        private Sunny.UI.UITextBox txtEssay;
        private Sunny.UI.UILabel lblWordCount;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtEssay = new Sunny.UI.UITextBox();
            lblWordCount = new Sunny.UI.UILabel();

            SuspendLayout();

            // txtEssay
            txtEssay.Multiline = true;
            txtEssay.Font = new Font("Segoe UI", 11F);
            txtEssay.ShowText = false;
            txtEssay.TextAlignment = ContentAlignment.TopLeft;
            txtEssay.Size = new Size(1000, 550);
            txtEssay.Location = new Point(25, 20);
            txtEssay.Radius = 10;

            // lblWordCount
            lblWordCount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblWordCount.ForeColor = Color.FromArgb(39, 56, 146);
            lblWordCount.Location = new Point(25, 580);
            lblWordCount.AutoSize = true;
            lblWordCount.Text = "Words: 0";

            // WritingAnswerPanel
            Controls.Add(txtEssay);
            Controls.Add(lblWordCount);
            Size = new Size(1100, 620);

            ResumeLayout(false);
        }
    }
}
