namespace IELTS.UI.User.TestTaking.Controls
{
    partial class AudioPlayerPanel
    {
        private System.ComponentModel.IContainer components = null;
        private Sunny.UI.UILabel lblTitle;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(AudioPlayerPanel));

            lblTitle = new Sunny.UI.UILabel();
            axWindowsMediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();

            ((System.ComponentModel.ISupportInitialize)(axWindowsMediaPlayer)).BeginInit();
            SuspendLayout();

            // lblTitle
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 69, 99);
            lblTitle.Location = new Point(10, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(400, 40);
            lblTitle.Text = "Listening Audio";

            // axWindowsMediaPlayer
            axWindowsMediaPlayer.Enabled = true;
            axWindowsMediaPlayer.OcxState =
                (AxHost.State)resources.GetObject("axWindowsMediaPlayer.OcxState");
            axWindowsMediaPlayer.Location = new Point(10, 60);
            axWindowsMediaPlayer.Size = new Size(860, 50);
            axWindowsMediaPlayer.Name = "axWindowsMediaPlayer";

            // Panel
            Controls.Add(lblTitle);
            Controls.Add(axWindowsMediaPlayer);
            Name = "AudioPlayerPanel";
            Size = new Size(900, 60);

            ((System.ComponentModel.ISupportInitialize)(axWindowsMediaPlayer)).EndInit();
            ResumeLayout(false);
        }
    }
}
