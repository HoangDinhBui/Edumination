namespace IELTS.UI.User.TestLibrary
{
    partial class TestLibrary
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelNavbar;
        private Label lblTitle;
        private FlowLayoutPanel panelSkills;
        private System.Windows.Forms.FlowLayoutPanel flowMain;


        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelNavbar = new Panel();
            lblTitle = new Label();
            panelSkills = new FlowLayoutPanel();
            txtSearch = new Sunny.UI.UITextBox();
            flowMain = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // panelNavbar
            // 
            panelNavbar.BackColor = Color.White;
            panelNavbar.Dock = DockStyle.Top;
            panelNavbar.Location = new Point(0, 0);
            panelNavbar.Name = "panelNavbar";
            panelNavbar.Size = new Size(1920, 70);
            panelNavbar.TabIndex = 4;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Noto Serif SC Black", 28.1999989F, FontStyle.Bold, GraphicsUnit.Point, 163);
            lblTitle.ForeColor = Color.FromArgb(41, 69, 99);
            lblTitle.Location = new Point(652, 101);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(658, 67);
            lblTitle.TabIndex = 3;
            lblTitle.Text = "IELTS Test Papers Library";
            // 
            // panelSkills
            // 
            panelSkills.Location = new Point(300, 188);
            panelSkills.Name = "panelSkills";
            panelSkills.Size = new Size(1300, 70);
            panelSkills.TabIndex = 2;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtSearch.Location = new Point(300, 294);
            txtSearch.Margin = new Padding(4, 5, 4, 5);
            txtSearch.MinimumSize = new Size(1, 16);
            txtSearch.Name = "txtSearch";
            txtSearch.Padding = new Padding(5);
            txtSearch.Radius = 20;
            txtSearch.ShowText = false;
            txtSearch.Size = new Size(1300, 58);
            txtSearch.TabIndex = 5;
            txtSearch.TextAlignment = ContentAlignment.MiddleLeft;
            txtSearch.Watermark = "";
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
            // 
            // flowMain
            // 
            flowMain.AutoScroll = true;
            flowMain.FlowDirection = FlowDirection.TopDown;
            flowMain.Location = new Point(148, 386);
            flowMain.Name = "flowMain";
            flowMain.Size = new Size(1600, 600);
            flowMain.TabIndex = 20;
            flowMain.WrapContents = false;
            // 
            // TestLibrary
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            ClientSize = new Size(1920, 1020);
            Controls.Add(txtSearch);
            Controls.Add(panelSkills);
            Controls.Add(lblTitle);
            Controls.Add(panelNavbar);
            Controls.Add(flowMain);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "TestLibrary";
            WindowState = FormWindowState.Maximized;
            Load += TestLibrary_Load;
            ResumeLayout(false);
            PerformLayout();

        }
        private Sunny.UI.UITextBox txtSearch;
    }
}
