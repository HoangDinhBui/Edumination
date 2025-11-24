using Sunny.UI;

namespace IELTS.UI.User.TestTaking.Controls
{
    partial class WritingPromptPanel
    {
        private System.ComponentModel.IContainer components = null;

        private UIPanel panelWrapper;
        private UILabel lblTitle;
        private UITextBox txtPrompt;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelWrapper = new UIPanel();
            lblTitle = new UILabel();
            txtPrompt = new UITextBox();

            panelWrapper.SuspendLayout();
            SuspendLayout();
            // 
            // panelWrapper
            // 
            panelWrapper.Dock = DockStyle.Fill;
            panelWrapper.FillColor = Color.White;
            panelWrapper.RectColor = Color.FromArgb(230, 230, 230);
            panelWrapper.RectSize = 2;
            panelWrapper.Radius = 20;
            panelWrapper.Padding = new Padding(25);
            panelWrapper.Controls.Add(lblTitle);
            panelWrapper.Controls.Add(txtPrompt);
            panelWrapper.Name = "panelWrapper";
            panelWrapper.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Noto Serif SC", 16F, FontStyle.Bold, GraphicsUnit.Point, 163);
            lblTitle.ForeColor = Color.FromArgb(41, 69, 99);
            lblTitle.Location = new Point(10, 10);
            lblTitle.Size = new Size(800, 40);
            lblTitle.Text = "Writing Task Title";
            // 
            // txtPrompt
            // 
            txtPrompt.Font = new Font("Segoe UI", 11F);
            txtPrompt.Location = new Point(10, 60);
            txtPrompt.MinimumSize = new Size(1, 1);
            txtPrompt.Size = new Size(820, 760);
            txtPrompt.Multiline = true;
            txtPrompt.ReadOnly = true;
            txtPrompt.ShowScrollBar = true;
            txtPrompt.FillColor = Color.White;
            txtPrompt.RectColor = Color.FromArgb(230, 230, 230);
            txtPrompt.Radius = 10;
            txtPrompt.Text = "";
            // 
            // WritingPromptPanel
            // 
            BackColor = Color.White;
            Controls.Add(panelWrapper);
            Name = "WritingPromptPanel";
            Size = new Size(920, 850);
            panelWrapper.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
