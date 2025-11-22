using Sunny.UI;

namespace IELTS.UI.User.TestTaking.Controls
{
    partial class AudioPlayerPanel
    {
        private System.ComponentModel.IContainer components = null;

        private UIPanel panelWrapper;
        private UILabel lblTitle;
        private UILabel lblDescription;
        private UIButton btnPlay;
        private UIButton btnStop;

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
            lblDescription = new UILabel();
            btnPlay = new UIButton();
            btnStop = new UIButton();

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
            panelWrapper.Controls.Add(lblDescription);
            panelWrapper.Controls.Add(btnPlay);
            panelWrapper.Controls.Add(btnStop);
            panelWrapper.Name = "panelWrapper";
            panelWrapper.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Noto Serif SC", 16F, FontStyle.Bold, GraphicsUnit.Point, 163);
            lblTitle.ForeColor = Color.FromArgb(41, 69, 99);
            lblTitle.Location = new Point(10, 10);
            lblTitle.Size = new Size(800, 40);
            lblTitle.Text = "Listening Part Title";
            // 
            // lblDescription
            // 
            lblDescription.Font = new Font("Segoe UI", 10.5F);
            lblDescription.ForeColor = Color.DimGray;
            lblDescription.Location = new Point(12, 60);
            lblDescription.Size = new Size(820, 80);
            lblDescription.Text = "Short description of the recording / instructions.";
            // 
            // btnPlay
            // 
            btnPlay.Text = "Play";
            btnPlay.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnPlay.Size = new Size(120, 45);
            btnPlay.Location = new Point(15, 150);
            btnPlay.Radius = 22;
            btnPlay.FillColor = Color.FromArgb(46, 204, 113);
            btnPlay.RectColor = btnPlay.FillColor;
            btnPlay.ForeColor = Color.White;
            btnPlay.Click += btnPlay_Click;
            // 
            // btnStop
            // 
            btnStop.Text = "Stop";
            btnStop.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnStop.Size = new Size(120, 45);
            btnStop.Location = new Point(150, 150);
            btnStop.Radius = 22;
            btnStop.FillColor = Color.FromArgb(231, 76, 60);
            btnStop.RectColor = btnStop.FillColor;
            btnStop.ForeColor = Color.White;
            btnStop.Click += btnStop_Click;
            // 
            // AudioPlayerPanel
            // 
            BackColor = Color.White;
            Controls.Add(panelWrapper);
            Name = "AudioPlayerPanel";
            Size = new Size(920, 850);
            panelWrapper.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
