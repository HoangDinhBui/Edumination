using Sunny.UI;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.Results
{
    partial class AnswerResultForm
    {
        private System.ComponentModel.IContainer components = null;
        private UIPanel panelSidebar;
        private Panel panelMainContent;
        private PictureBox picAvatar;
        private Label lblUserName;
        private Label lblResultTitle;
        private Sunny.UI.UIRoundProcess progressCorrect;
        private Sunny.UI.UIRoundProcess progressBand;
        private Sunny.UI.UIRoundProcess progressTime;
        private FlowLayoutPanel flowPanelAnswers;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		private void InitializeComponent()
		{
			panelSidebar = new UIPanel();
			picAvatar = new PictureBox();
			lblUserName = new Label();
			panelMainContent = new Panel();
			lblResultTitle = new Label();
			progressCorrect = new UIRoundProcess();
			progressBand = new UIRoundProcess();
			progressTime = new UIRoundProcess();
			flowPanelAnswers = new FlowLayoutPanel();
			panelSidebar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
			panelMainContent.SuspendLayout();
			SuspendLayout();
			// 
			// panelSidebar
			// 
			panelSidebar.Controls.Add(picAvatar);
			panelSidebar.Controls.Add(lblUserName);
			panelSidebar.Dock = DockStyle.Left;
			panelSidebar.FillColor = Color.FromArgb(240, 248, 255);
			panelSidebar.Font = new Font("Microsoft Sans Serif", 12F);
			panelSidebar.Location = new Point(0, 0);
			panelSidebar.Margin = new Padding(4, 6, 4, 6);
			panelSidebar.MinimumSize = new Size(1, 1);
			panelSidebar.Name = "panelSidebar";
			panelSidebar.RectColor = Color.FromArgb(200, 200, 200);
			panelSidebar.Size = new Size(300, 1000);
			panelSidebar.Style = UIStyle.Custom;
			panelSidebar.TabIndex = 1;
			panelSidebar.Text = null;
			panelSidebar.TextAlignment = ContentAlignment.MiddleCenter;
			// 
			// picAvatar
			// 
			picAvatar.BackColor = Color.LightGray;
			picAvatar.Location = new Point(75, 40);
			picAvatar.Name = "picAvatar";
			picAvatar.Size = new Size(150, 150);
			picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
			picAvatar.TabIndex = 0;
			picAvatar.TabStop = false;
			// 
			// lblUserName
			// 
			lblUserName.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
			lblUserName.ForeColor = Color.FromArgb(0, 102, 204);
			lblUserName.Location = new Point(20, 210);
			lblUserName.Name = "lblUserName";
			lblUserName.Size = new Size(260, 30);
			lblUserName.TabIndex = 1;
			lblUserName.Text = "Student Name";
			lblUserName.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// panelMainContent
			// 
			panelMainContent.AutoScroll = true;
			panelMainContent.BackColor = Color.White;
			panelMainContent.Controls.Add(lblResultTitle);
			panelMainContent.Controls.Add(progressCorrect);
			panelMainContent.Controls.Add(progressBand);
			panelMainContent.Controls.Add(progressTime);
			panelMainContent.Controls.Add(flowPanelAnswers);
			panelMainContent.Dock = DockStyle.Fill;
			panelMainContent.Location = new Point(300, 0);
			panelMainContent.Margin = new Padding(3, 4, 3, 4);
			panelMainContent.Name = "panelMainContent";
			panelMainContent.Size = new Size(900, 1000);
			panelMainContent.TabIndex = 0;
			// 
			// lblResultTitle
			// 
			lblResultTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
			lblResultTitle.ForeColor = Color.FromArgb(0, 102, 204);
			lblResultTitle.Location = new Point(50, 38);
			lblResultTitle.Name = "lblResultTitle";
			lblResultTitle.Size = new Size(400, 50);
			lblResultTitle.TabIndex = 0;
			lblResultTitle.Text = "Your Test Results";
			// 
			// progressCorrect
			// 
			progressCorrect.Font = new Font("Microsoft Sans Serif", 12F);
			progressCorrect.ForeColor2 = Color.Black;
			progressCorrect.Location = new Point(489, 127);
			progressCorrect.Margin = new Padding(3, 4, 3, 4);
			progressCorrect.MinimumSize = new Size(1, 1);
			progressCorrect.Name = "progressCorrect";
			progressCorrect.Size = new Size(120, 150);
			progressCorrect.TabIndex = 1;
			progressCorrect.ValueChanged += progressCorrect_ValueChanged;
			// 
			// progressBand
			// 
			progressBand.Font = new Font("Microsoft Sans Serif", 12F);
			progressBand.ForeColor2 = Color.Black;
			progressBand.Location = new Point(262, 127);
			progressBand.Margin = new Padding(3, 4, 3, 4);
			progressBand.MinimumSize = new Size(1, 1);
			progressBand.Name = "progressBand";
			progressBand.Size = new Size(120, 150);
			progressBand.TabIndex = 2;
			// 
			// progressTime
			// 
			progressTime.Font = new Font("Microsoft Sans Serif", 12F);
			progressTime.ForeColor2 = Color.Black;
			progressTime.Location = new Point(34, 127);
			progressTime.Margin = new Padding(3, 4, 3, 4);
			progressTime.MinimumSize = new Size(1, 1);
			progressTime.Name = "progressTime";
			progressTime.ShowProcess = true;
			progressTime.Size = new Size(120, 150);
			progressTime.TabIndex = 3;
			progressTime.TextAlign = ContentAlignment.BottomCenter;
			// 
			// flowPanelAnswers
			// 
			flowPanelAnswers.AutoScroll = true;
			flowPanelAnswers.BackColor = Color.FromArgb(245, 247, 250);
			flowPanelAnswers.BorderStyle = BorderStyle.FixedSingle;
			flowPanelAnswers.Location = new Point(50, 438);
			flowPanelAnswers.Margin = new Padding(3, 4, 3, 4);
			flowPanelAnswers.Name = "flowPanelAnswers";
			flowPanelAnswers.Padding = new Padding(15, 19, 15, 19);
			flowPanelAnswers.Size = new Size(950, 562);
			flowPanelAnswers.TabIndex = 4;
			// 
			// AnswerResultForm
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1200, 1000);
			Controls.Add(panelMainContent);
			Controls.Add(panelSidebar);
			Margin = new Padding(3, 4, 3, 4);
			Name = "AnswerResultForm";
			Text = "IELTS Test Results";
			Load += AnswerResultForm_Load;
			panelSidebar.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
			panelMainContent.ResumeLayout(false);
			ResumeLayout(false);
		}

		private void SetupCircularProgress(UIRoundProcess bar, int x, int y, string text, int val)
        {
            bar.Location = new Point(x, y);
            bar.Size = new Size(150, 150);
            bar.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            bar.Text = text;
            bar.Value = val;
            bar.Style = UIStyle.Custom;
            bar.ForeColor = Color.FromArgb(80, 190, 240);
            bar.BackColor = Color.White;
        }

        private void AddStatLabel(string text, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Location = new Point(x, y);
            lbl.Size = new Size(150, 25);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lbl.ForeColor = Color.Gray;
            this.panelMainContent.Controls.Add(lbl);
        }

       
    }
}