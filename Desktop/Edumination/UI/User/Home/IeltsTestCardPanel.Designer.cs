namespace IELTS.UI.User.Home
{
    partial class IeltsTestCardPanel
    {
        private System.ComponentModel.IContainer components = null;

        private PictureBox picThumb;
        private Label lblTitle;
        private Label lblRating;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            picThumb = new PictureBox();
            lblTitle = new Label();
            lblRating = new Label();
            ((System.ComponentModel.ISupportInitialize)picThumb).BeginInit();
            SuspendLayout();

            picThumb.Location = new Point(0, 0);
            picThumb.Name = "picThumb";
            picThumb.Size = new Size(290, 273);
            picThumb.SizeMode = PictureBoxSizeMode.StretchImage;
            picThumb.TabStop = false;

            lblTitle.Font = new Font("Noto Serif SC", 12F, FontStyle.Bold);
            lblTitle.Location = new Point(0, 296);
            lblTitle.Size = new Size(290, 40);
            lblTitle.TextAlign = ContentAlignment.TopCenter;

            lblRating.Font = new Font("Noto Serif SC SemiBold", 10F, FontStyle.Bold);
            lblRating.ForeColor = Color.Gray;
            lblRating.Location = new Point(22, 345);
            lblRating.Size = new Size(250, 25);
            lblRating.TextAlign = ContentAlignment.TopCenter;

            Controls.Add(picThumb);
            Controls.Add(lblTitle);
            Controls.Add(lblRating);

            Margin = new Padding(80, 10, 10, 10);
            Name = "IeltsTestCardPanel";
            Size = new Size(290, 389);

            ((System.ComponentModel.ISupportInitialize)picThumb).EndInit();
            ResumeLayout(false);
        }
    }
}
