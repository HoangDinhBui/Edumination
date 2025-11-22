namespace IELTS.UI.User.TestLibrary
{
    partial class MockTestItemPanel
    {
        private System.ComponentModel.IContainer components = null;

        private Panel card;
        private Label lblTitle;
        private Label lblTaken;
        private PictureBox icon;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.card = new Panel();
            this.lblTitle = new Label();
            this.lblTaken = new Label();
            this.icon = new PictureBox();

            this.SuspendLayout();

            // CARD PANEL
            this.card.BackColor = Color.White;
            this.card.Size = new Size(320, 110);
            this.card.Padding = new Padding(15);
            this.card.Location = new Point(0, 0);

            // Border + Radius + Shadow
            this.card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (var path = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 20))
                using (var pen = new Pen(Color.FromArgb(220, 225, 235)))
                {
                    // SHADOW
                    g.FillPath(new SolidBrush(Color.FromArgb(25, 0, 0, 0)),
                        RoundedRect(new Rectangle(3, 3, card.Width - 1, card.Height - 1), 20));

                    // CARD BACKGROUND
                    g.FillPath(new SolidBrush(Color.White), path);

                    // BORDER
                    g.DrawPath(pen, path);
                }
            };

            // TITLE
            lblTitle.Font = new Font("Noto Serif SC SemiBold", 12, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(40, 60, 90);
            lblTitle.AutoSize = false;
            lblTitle.Location = new Point(10, 10);
            lblTitle.Size = new Size(290, 45);

            // ICON
            icon.Size = new Size(18, 18);
            icon.Location = new Point(12, 70);
            icon.SizeMode = PictureBoxSizeMode.Zoom;

            // TAKEN TEXT
            lblTaken.Font = new Font("Segoe UI", 9.5F);
            lblTaken.ForeColor = Color.Gray;
            lblTaken.Location = new Point(35, 70);
            lblTaken.AutoSize = true;

            // HOVER EFFECT
            card.MouseEnter += (s, e) =>
            {
                card.BackColor = Color.FromArgb(250, 250, 255);
                card.Invalidate();
            };

            card.MouseLeave += (s, e) =>
            {
                card.BackColor = Color.White;
                card.Invalidate();
            };

            // ADD TO CONTROL
            this.Controls.Add(card);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblTaken);
            card.Controls.Add(icon);

            this.Size = new Size(320, 110);
            this.BackColor = Color.Transparent;

            this.ResumeLayout(false);
        }

        // Rounded Rectangle method
        private System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
