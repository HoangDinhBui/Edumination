namespace IELTS.UI.User.TestTaking.Controls
{
    partial class AnswerPanel
    {
        private System.ComponentModel.IContainer components = null;

        private FlowLayoutPanel flowAnswers;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            flowAnswers = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // flowAnswers
            // 
            flowAnswers.Dock = DockStyle.Fill;
            flowAnswers.FlowDirection = FlowDirection.TopDown;
            flowAnswers.WrapContents = false;
            flowAnswers.AutoScroll = true;
            flowAnswers.Padding = new Padding(25);
            flowAnswers.BackColor = Color.White;
            flowAnswers.Name = "flowAnswers";
            flowAnswers.Size = new Size(960, 850);
            // 
            // AnswerPanel
            // 
            BackColor = Color.White;
            Controls.Add(flowAnswers);
            Name = "AnswerPanel";
            Size = new Size(960, 850);
            ResumeLayout(false);
        }
    }
}
