namespace Edumination.WinForms.UI.Forms.TestLibrary
{
    partial class MockTestContainerPanel
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private PictureBox picLaptop;
        private FlowLayoutPanel panelItems;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            picLaptop = new PictureBox();
            panelItems = new FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)picLaptop).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Noto Serif SC SemiBold", 24F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 69, 99);
            lblTitle.Location = new Point(450, 40);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(454, 57);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "IELTS Mock Test 2025";
            // 
            // picLaptop
            // 
            picLaptop.Image = Properties.Resources.laptop;
            picLaptop.Location = new Point(60, 120);
            picLaptop.Name = "picLaptop";
            picLaptop.Size = new Size(350, 300);
            picLaptop.SizeMode = PictureBoxSizeMode.Zoom;
            picLaptop.TabIndex = 1;
            picLaptop.TabStop = false;
            // 
            // panelItems
            // 
            panelItems.Location = new Point(450, 120);
            panelItems.Name = "panelItems";
            panelItems.Size = new Size(1100, 300);
            panelItems.FlowDirection = FlowDirection.LeftToRight;
            panelItems.WrapContents = true;
            panelItems.AutoScroll = true;
            panelItems.Padding = new Padding(10);
            panelItems.Margin = new Padding(10);
            panelItems.BackColor = Color.White;

            // 
            // MockTestContainerPanel
            // 
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(lblTitle);
            Controls.Add(picLaptop);
            Controls.Add(panelItems);
            Name = "MockTestContainerPanel";
            Padding = new Padding(20);
            Size = new Size(1600, 480);
            ((System.ComponentModel.ISupportInitialize)picLaptop).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
