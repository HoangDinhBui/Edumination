namespace Edumination.WinForms.UI.Forms.TestLibrary
{
    partial class TestLibrary
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelNavbar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelNavbar = new Panel();
            lblLibrary = new Sunny.UI.UILabel();
            btnAll = new Sunny.UI.UIButton();
            btnListen = new Sunny.UI.UIButton();
            uiButton1 = new Sunny.UI.UIButton();
            uiButton2 = new Sunny.UI.UIButton();
            uiButton3 = new Sunny.UI.UIButton();
            txtSearch = new Sunny.UI.UITextBox();
            SuspendLayout();
            // 
            // panelNavbar
            // 
            panelNavbar.Dock = DockStyle.Top;
            panelNavbar.Location = new Point(0, 0);
            panelNavbar.Name = "panelNavbar";
            panelNavbar.Size = new Size(1920, 72);
            panelNavbar.TabIndex = 4;
            // 
            // lblLibrary
            // 
            lblLibrary.Font = new Font("Noto Serif SC", 19.7999973F, FontStyle.Bold, GraphicsUnit.Point, 163);
            lblLibrary.ForeColor = Color.FromArgb(41, 69, 99);
            lblLibrary.Location = new Point(711, 102);
            lblLibrary.Name = "lblLibrary";
            lblLibrary.Size = new Size(454, 45);
            lblLibrary.TabIndex = 5;
            lblLibrary.Text = "IELTS Test Papers Library";
            // 
            // btnAll
            // 
            btnAll.FillColor = Color.White;
            btnAll.FillColor2 = Color.White;
            btnAll.FillHoverColor = Color.FromArgb(39, 56, 146);
            btnAll.FillPressColor = Color.FromArgb(39, 56, 146);
            btnAll.FillSelectedColor = Color.FromArgb(39, 56, 146);
            btnAll.Font = new Font("Noto Serif SC SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            btnAll.ForeColor = Color.FromArgb(39, 56, 146);
            btnAll.Location = new Point(322, 177);
            btnAll.MinimumSize = new Size(1, 1);
            btnAll.Name = "btnAll";
            btnAll.Radius = 30;
            btnAll.RectColor = Color.FromArgb(39, 56, 146);
            btnAll.RectPressColor = Color.FromArgb(39, 56, 146);
            btnAll.RectSelectedColor = Color.FromArgb(39, 56, 146);
            btnAll.Size = new Size(220, 59);
            btnAll.TabIndex = 7;
            btnAll.Text = "All Skills";
            btnAll.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnAll.Click += btnAll_Click;
            // 
            // btnListen
            // 
            btnListen.FillColor = Color.White;
            btnListen.FillColor2 = Color.White;
            btnListen.FillHoverColor = Color.FromArgb(39, 56, 146);
            btnListen.FillPressColor = Color.FromArgb(39, 56, 146);
            btnListen.FillSelectedColor = Color.FromArgb(39, 56, 146);
            btnListen.Font = new Font("Noto Serif SC SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            btnListen.ForeColor = Color.FromArgb(39, 56, 146);
            btnListen.Location = new Point(571, 177);
            btnListen.MinimumSize = new Size(1, 1);
            btnListen.Name = "btnListen";
            btnListen.Radius = 30;
            btnListen.RectColor = Color.FromArgb(39, 56, 146);
            btnListen.RectPressColor = Color.FromArgb(39, 56, 146);
            btnListen.RectSelectedColor = Color.FromArgb(39, 56, 146);
            btnListen.Size = new Size(220, 59);
            btnListen.TabIndex = 8;
            btnListen.Text = "Listening";
            btnListen.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // uiButton1
            // 
            uiButton1.FillColor = Color.White;
            uiButton1.FillColor2 = Color.White;
            uiButton1.FillHoverColor = Color.FromArgb(39, 56, 146);
            uiButton1.FillPressColor = Color.FromArgb(39, 56, 146);
            uiButton1.FillSelectedColor = Color.FromArgb(39, 56, 146);
            uiButton1.Font = new Font("Noto Serif SC SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            uiButton1.ForeColor = Color.FromArgb(39, 56, 146);
            uiButton1.Location = new Point(822, 177);
            uiButton1.MinimumSize = new Size(1, 1);
            uiButton1.Name = "uiButton1";
            uiButton1.Radius = 30;
            uiButton1.RectColor = Color.FromArgb(39, 56, 146);
            uiButton1.RectPressColor = Color.FromArgb(39, 56, 146);
            uiButton1.RectSelectedColor = Color.FromArgb(39, 56, 146);
            uiButton1.Size = new Size(220, 59);
            uiButton1.TabIndex = 9;
            uiButton1.Text = "Reading";
            uiButton1.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // uiButton2
            // 
            uiButton2.FillColor = Color.White;
            uiButton2.FillColor2 = Color.White;
            uiButton2.FillHoverColor = Color.FromArgb(39, 56, 146);
            uiButton2.FillPressColor = Color.FromArgb(39, 56, 146);
            uiButton2.FillSelectedColor = Color.FromArgb(39, 56, 146);
            uiButton2.Font = new Font("Noto Serif SC SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            uiButton2.ForeColor = Color.FromArgb(39, 56, 146);
            uiButton2.Location = new Point(1077, 177);
            uiButton2.MinimumSize = new Size(1, 1);
            uiButton2.Name = "uiButton2";
            uiButton2.Radius = 30;
            uiButton2.RectColor = Color.FromArgb(39, 56, 146);
            uiButton2.RectPressColor = Color.FromArgb(39, 56, 146);
            uiButton2.RectSelectedColor = Color.FromArgb(39, 56, 146);
            uiButton2.Size = new Size(220, 59);
            uiButton2.TabIndex = 10;
            uiButton2.Text = "Writing";
            uiButton2.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // uiButton3
            // 
            uiButton3.FillColor = Color.White;
            uiButton3.FillColor2 = Color.White;
            uiButton3.FillHoverColor = Color.FromArgb(39, 56, 146);
            uiButton3.FillPressColor = Color.FromArgb(39, 56, 146);
            uiButton3.FillSelectedColor = Color.FromArgb(39, 56, 146);
            uiButton3.Font = new Font("Noto Serif SC SemiBold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            uiButton3.ForeColor = Color.FromArgb(39, 56, 146);
            uiButton3.Location = new Point(1329, 177);
            uiButton3.MinimumSize = new Size(1, 1);
            uiButton3.Name = "uiButton3";
            uiButton3.Radius = 30;
            uiButton3.RectColor = Color.FromArgb(39, 56, 146);
            uiButton3.RectPressColor = Color.FromArgb(39, 56, 146);
            uiButton3.RectSelectedColor = Color.FromArgb(39, 56, 146);
            uiButton3.Size = new Size(220, 59);
            uiButton3.TabIndex = 11;
            uiButton3.Text = "Speaking";
            uiButton3.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtSearch.Location = new Point(322, 265);
            txtSearch.Margin = new Padding(4, 5, 4, 5);
            txtSearch.MinimumSize = new Size(1, 16);
            txtSearch.Name = "txtSearch";
            txtSearch.Padding = new Padding(5);
            txtSearch.Radius = 10;
            txtSearch.ShowText = false;
            txtSearch.Size = new Size(1227, 57);
            txtSearch.TabIndex = 13;
            txtSearch.TextAlignment = ContentAlignment.MiddleLeft;
            txtSearch.Watermark = "Search";
            // 
            // TestLibrary
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            ClientSize = new Size(1920, 1020);
            Controls.Add(txtSearch);
            Controls.Add(uiButton3);
            Controls.Add(uiButton2);
            Controls.Add(uiButton1);
            Controls.Add(btnListen);
            Controls.Add(btnAll);
            Controls.Add(lblLibrary);
            Controls.Add(panelNavbar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "TestLibrary";
            Load += TestLibrary_Load;
            ResumeLayout(false);
        }
        private Sunny.UI.UILabel lblLibrary;
        private Sunny.UI.UIButton btnAll;
        private Sunny.UI.UIButton btnListen;
        private Sunny.UI.UIButton uiButton1;
        private Sunny.UI.UIButton uiButton2;
        private Sunny.UI.UIButton uiButton3;
        private Sunny.UI.UITextBox txtSearch;
    }
}
