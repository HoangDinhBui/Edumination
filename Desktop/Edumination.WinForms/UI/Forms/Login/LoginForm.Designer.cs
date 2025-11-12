namespace Edumination.WinForms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.PictureBox pictureBoxSlide;
        private System.Windows.Forms.Timer timerSlide;
        private System.Windows.Forms.Panel panelDots;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelLeft = new Panel();
            panelRight = new Panel();
            pictureBoxSlide = new PictureBox();
            panelDots = new Panel();
            timerSlide = new System.Windows.Forms.Timer(components);
            panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSlide).BeginInit();
            SuspendLayout();
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.White;
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Name = "panelLeft";
            panelLeft.Padding = new Padding(40);
            panelLeft.Size = new Size(760, 973);
            panelLeft.TabIndex = 1;
            // 
            // panelRight
            // 
            panelRight.BackColor = Color.FromArgb(36, 86, 128);
            panelRight.Controls.Add(pictureBoxSlide);
            panelRight.Controls.Add(panelDots);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(760, 0);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(1142, 973);
            panelRight.TabIndex = 0;
            // 
            // pictureBoxSlide
            // 
            pictureBoxSlide.Dock = DockStyle.Fill;
            pictureBoxSlide.Location = new Point(0, 0);
            pictureBoxSlide.Name = "pictureBoxSlide";
            pictureBoxSlide.Size = new Size(1142, 933);
            pictureBoxSlide.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxSlide.TabIndex = 0;
            pictureBoxSlide.TabStop = false;
            // 
            // panelDots
            // 
            panelDots.BackColor = Color.Transparent;
            panelDots.Dock = DockStyle.Bottom;
            panelDots.Location = new Point(0, 933);
            panelDots.Name = "panelDots";
            panelDots.Size = new Size(1142, 40);
            panelDots.TabIndex = 1;
            // 
            // timerSlide
            // 
            timerSlide.Interval = 4000;
            timerSlide.Tick += TimerSlide_Tick;
            // 
            // LoginForm
            // 
            ClientSize = new Size(1902, 973);
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.Manual;
            Load += LoginForm_Load;
            panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxSlide).EndInit();
            ResumeLayout(false);
        }
    }
}
