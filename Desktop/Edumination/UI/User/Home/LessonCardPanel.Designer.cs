namespace IELTS.UI.User.Home
{
    partial class LessonCardPanel
    {
        private System.ComponentModel.IContainer components = null;

        private PictureBox picThumb;
        private Label lblCategory;
        private Label lblTitle;
        private Label lblTime;
        private Label lblAttending;
        private Button btnJoin;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            picThumb = new PictureBox();
            lblCategory = new Label();
            lblTitle = new Label();
            lblTime = new Label();
            lblAttending = new Label();
            btnJoin = new Button();

            ((System.ComponentModel.ISupportInitialize)picThumb).BeginInit();
            SuspendLayout();

            picThumb.Location = new Point(0, 0);
            picThumb.Size = new Size(350, 224);
            picThumb.SizeMode = PictureBoxSizeMode.StretchImage;
            picThumb.TabStop = false;

            lblCategory.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCategory.Location = new Point(10, 170);
            lblCategory.Size = new Size(330, 22);

            lblTitle.Font = new Font("Noto Serif SC", 12F, FontStyle.Bold);
            lblTitle.Location = new Point(10, 248);
            lblTitle.Size = new Size(330, 71);

            lblTime.Location = new Point(10, 331);
            lblTime.Size = new Size(330, 25);

            lblAttending.Location = new Point(10, 369);
            lblAttending.Size = new Size(330, 25);

            btnJoin.BackColor = Color.FromArgb(70, 137, 255);
            btnJoin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnJoin.ForeColor = Color.White;
            btnJoin.Location = new Point(10, 409);
            btnJoin.Size = new Size(330, 40);
            btnJoin.Text = "Join";

            Controls.Add(picThumb);
            Controls.Add(lblCategory);
            Controls.Add(lblTitle);
            Controls.Add(lblTime);
            Controls.Add(lblAttending);
            Controls.Add(btnJoin);

            BorderStyle = BorderStyle.FixedSingle;
            Margin = new Padding(30, 10, 10, 10);
            Name = "LessonCardPanel";
            Size = new Size(348, 462);

            ((System.ComponentModel.ISupportInitialize)picThumb).EndInit();
            ResumeLayout(false);
        }
    }
}
