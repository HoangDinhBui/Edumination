namespace Edumination.WinForms.UI.Forms.TestLibrary
{
    partial class MockTestContainerPanel
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelContainer;
        private PictureBox picLaptop;
        private Label lblTitle;
        private FlowLayoutPanel panelItems;
        private Label lblViewMore;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelContainer = new Panel();
            this.picLaptop = new PictureBox();
            this.lblTitle = new Label();
            this.panelItems = new FlowLayoutPanel();
            this.lblViewMore = new Label();

            this.SuspendLayout();

            // panelContainer
            this.panelContainer.Dock = DockStyle.Fill;
            this.panelContainer.BackColor = Color.White;
            this.panelContainer.Padding = new Padding(30);
            this.panelContainer.Size = new Size(1450, 450);
            this.panelContainer.BorderStyle = BorderStyle.FixedSingle;

            // picLaptop
            this.picLaptop.Size = new Size(420, 300);
            this.picLaptop.Location = new Point(30, 80);
            this.picLaptop.SizeMode = PictureBoxSizeMode.Zoom;
            this.picLaptop.Image = Image.FromFile("assets/img/macbook.png");

            // lblTitle
            this.lblTitle.Text = "IELTS Mock Test 2025";
            this.lblTitle.Font = new Font("Georgia", 22, FontStyle.Bold);
            this.lblTitle.AutoSize = false;
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTitle.Size = new Size(650, 50);
            this.lblTitle.Location = new Point(500, 30);

            // panelItems
            this.panelItems.Location = new Point(500, 90);
            this.panelItems.Size = new Size(850, 260);
            this.panelItems.FlowDirection = FlowDirection.LeftToRight;
            this.panelItems.WrapContents = true;

            // lblViewMore
            this.lblViewMore.Text = "View more 2 tests ⌄";
            this.lblViewMore.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            this.lblViewMore.ForeColor = Color.Gray;
            this.lblViewMore.AutoSize = true;
            this.lblViewMore.Location = new Point(650, 370);

            // Add controls
            this.panelContainer.Controls.Add(picLaptop);
            this.panelContainer.Controls.Add(lblTitle);
            this.panelContainer.Controls.Add(panelItems);
            this.panelContainer.Controls.Add(lblViewMore);

            this.Controls.Add(panelContainer);

            this.Size = new Size(1450, 450);
            this.ResumeLayout(false);
        }
    }
}
