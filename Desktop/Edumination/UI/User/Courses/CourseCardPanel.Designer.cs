namespace IELTS.UI.User.Courses
{
    partial class CourseCardPanel
    {
        private System.ComponentModel.IContainer components = null;

        // Controls
        private Panel panelRoot;
        private Label lblBadge;
        private Label lblTitle;
        private Label lblLevel;
        private Label lblBandRange;
        private Label lblShortDescription;

        private Label lblBullet1;
        private Label lblBullet2;
        private Label lblBullet3;
        private Label lblBullet4;

        private Label lblBottomInfo;
        private Label lblSuccessRate;
        private Label lblPrice;
        private Button btnExplore;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelRoot = new Panel();
            lblBadge = new Label();
            lblTitle = new Label();
            lblLevel = new Label();
            lblBandRange = new Label();
            lblShortDescription = new Label();
            lblBullet1 = new Label();
            lblBullet2 = new Label();
            lblBullet3 = new Label();
            lblBullet4 = new Label();
            lblBottomInfo = new Label();
            lblSuccessRate = new Label();
            lblPrice = new Label();
            btnExplore = new Button();
            panelRoot.SuspendLayout();
            SuspendLayout();
            // 
            // panelRoot
            // 
            panelRoot.BackColor = Color.FromArgb(236, 250, 244);
            panelRoot.Controls.Add(lblBadge);
            panelRoot.Controls.Add(lblTitle);
            panelRoot.Controls.Add(lblLevel);
            panelRoot.Controls.Add(lblBandRange);
            panelRoot.Controls.Add(lblShortDescription);
            panelRoot.Controls.Add(lblBullet1);
            panelRoot.Controls.Add(lblBullet2);
            panelRoot.Controls.Add(lblBullet3);
            panelRoot.Controls.Add(lblBullet4);
            panelRoot.Controls.Add(lblBottomInfo);
            panelRoot.Controls.Add(lblSuccessRate);
            panelRoot.Controls.Add(lblPrice);
            panelRoot.Controls.Add(btnExplore);
            panelRoot.Dock = DockStyle.Fill;
            panelRoot.Location = new Point(0, 0);
            panelRoot.Name = "panelRoot";
            panelRoot.Padding = new Padding(25);
            panelRoot.Size = new Size(480, 440);
            panelRoot.TabIndex = 0;
            // 
            // lblBadge
            // 
            lblBadge.AutoSize = true;
            lblBadge.BackColor = Color.FromArgb(37, 201, 133);
            lblBadge.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblBadge.ForeColor = Color.White;
            lblBadge.Location = new Point(25, 10);
            lblBadge.Name = "lblBadge";
            lblBadge.Padding = new Padding(10, 4, 10, 4);
            lblBadge.Size = new Size(159, 28);
            lblBadge.TabIndex = 0;
            lblBadge.Text = "Best for Beginners";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Noto Serif SC", 22F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(25, 41, 88);
            lblTitle.Location = new Point(25, 43);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(252, 54);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Course Title";
            // 
            // lblLevel
            // 
            lblLevel.AutoSize = true;
            lblLevel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblLevel.ForeColor = Color.FromArgb(80, 80, 80);
            lblLevel.Location = new Point(28, 111);
            lblLevel.Name = "lblLevel";
            lblLevel.Size = new Size(82, 23);
            lblLevel.TabIndex = 2;
            lblLevel.Text = "Beginner";
            // 
            // lblBandRange
            // 
            lblBandRange.AutoSize = true;
            lblBandRange.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblBandRange.ForeColor = Color.FromArgb(25, 41, 88);
            lblBandRange.Location = new Point(25, 148);
            lblBandRange.Name = "lblBandRange";
            lblBandRange.Size = new Size(110, 32);
            lblBandRange.TabIndex = 3;
            lblBandRange.Text = "0.0 – 5.0";
            // 
            // lblShortDescription
            // 
            lblShortDescription.AutoSize = true;
            lblShortDescription.Font = new Font("Segoe UI", 9.5F);
            lblShortDescription.ForeColor = Color.FromArgb(70, 70, 70);
            lblShortDescription.Location = new Point(28, 202);
            lblShortDescription.MaximumSize = new Size(430, 0);
            lblShortDescription.Name = "lblShortDescription";
            lblShortDescription.Size = new Size(173, 21);
            lblShortDescription.TabIndex = 4;
            lblShortDescription.Text = "Short description here...";
            lblShortDescription.Click += lblShortDescription_Click;
            // 
            // lblBullet1
            // 
            lblBullet1.AutoSize = true;
            lblBullet1.Font = new Font("Segoe UI", 9.5F);
            lblBullet1.Location = new Point(28, 223);
            lblBullet1.Name = "lblBullet1";
            lblBullet1.Size = new Size(0, 21);
            lblBullet1.TabIndex = 5;
            // 
            // lblBullet2
            // 
            lblBullet2.AutoSize = true;
            lblBullet2.Font = new Font("Segoe UI", 9.5F);
            lblBullet2.Location = new Point(28, 250);
            lblBullet2.Name = "lblBullet2";
            lblBullet2.Size = new Size(0, 21);
            lblBullet2.TabIndex = 6;
            // 
            // lblBullet3
            // 
            lblBullet3.AutoSize = true;
            lblBullet3.Font = new Font("Segoe UI", 9.5F);
            lblBullet3.Location = new Point(28, 277);
            lblBullet3.Name = "lblBullet3";
            lblBullet3.Size = new Size(0, 21);
            lblBullet3.TabIndex = 7;
            // 
            // lblBullet4
            // 
            lblBullet4.AutoSize = true;
            lblBullet4.Font = new Font("Segoe UI", 9.5F);
            lblBullet4.Location = new Point(28, 304);
            lblBullet4.Name = "lblBullet4";
            lblBullet4.Size = new Size(0, 21);
            lblBullet4.TabIndex = 8;
            // 
            // lblBottomInfo
            // 
            lblBottomInfo.AutoSize = true;
            lblBottomInfo.Font = new Font("Segoe UI", 9F);
            lblBottomInfo.ForeColor = Color.FromArgb(70, 70, 70);
            lblBottomInfo.Location = new Point(28, 336);
            lblBottomInfo.Name = "lblBottomInfo";
            lblBottomInfo.Size = new Size(0, 20);
            lblBottomInfo.TabIndex = 9;
            // 
            // lblSuccessRate
            // 
            lblSuccessRate.AutoSize = true;
            lblSuccessRate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSuccessRate.ForeColor = Color.FromArgb(37, 201, 133);
            lblSuccessRate.Location = new Point(28, 360);
            lblSuccessRate.Name = "lblSuccessRate";
            lblSuccessRate.Size = new Size(0, 20);
            lblSuccessRate.TabIndex = 10;
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblPrice.ForeColor = Color.FromArgb(25, 41, 88);
            lblPrice.Location = new Point(25, 390);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(0, 37);
            lblPrice.TabIndex = 11;
            // 
            // btnExplore
            // 
            btnExplore.BackColor = Color.FromArgb(25, 41, 88);
            btnExplore.FlatAppearance.BorderSize = 0;
            btnExplore.FlatStyle = FlatStyle.Flat;
            btnExplore.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExplore.ForeColor = Color.White;
            btnExplore.Location = new Point(319, 374);
            btnExplore.Name = "btnExplore";
            btnExplore.Size = new Size(120, 38);
            btnExplore.TabIndex = 12;
            btnExplore.Text = "Explore ➜";
            btnExplore.UseVisualStyleBackColor = false;
            // 
            // CourseCardPanel
            // 
            BackColor = Color.Transparent;
            Controls.Add(panelRoot);
            Margin = new Padding(15);
            Name = "CourseCardPanel";
            Size = new Size(480, 440);
            panelRoot.ResumeLayout(false);
            panelRoot.PerformLayout();
            ResumeLayout(false);
        }
    }
}
