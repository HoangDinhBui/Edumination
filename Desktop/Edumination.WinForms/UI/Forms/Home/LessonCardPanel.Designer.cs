namespace Edumination.WinForms.UI.Forms.Home
{
    partial class LessonCardPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private PictureBox picThumb;
        private Label lblCategory;
        private Label lblTitle;
        private Label lblTime;
        private Label lblAttending;
        private Button btnJoin;

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
            // 
            // picThumb
            // 
            picThumb.Location = new Point(0, 0);
            picThumb.Name = "picThumb";
            picThumb.Size = new Size(350, 224);
            picThumb.SizeMode = PictureBoxSizeMode.StretchImage;
            picThumb.TabIndex = 0;
            picThumb.TabStop = false;
            // 
            // lblCategory
            // 
            lblCategory.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCategory.Location = new Point(10, 170);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(330, 22);
            lblCategory.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Noto Serif SC", 12F, FontStyle.Bold, GraphicsUnit.Point, 163);
            lblTitle.Location = new Point(10, 248);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(330, 71);
            lblTitle.TabIndex = 2;
            // 
            // lblTime
            // 
            lblTime.Location = new Point(10, 331);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(330, 25);
            lblTime.TabIndex = 3;
            // 
            // lblAttending
            // 
            lblAttending.Location = new Point(10, 369);
            lblAttending.Name = "lblAttending";
            lblAttending.Size = new Size(330, 25);
            lblAttending.TabIndex = 4;
            lblAttending.Click += lblAttending_Click;
            // 
            // btnJoin
            // 
            btnJoin.BackColor = Color.FromArgb(70, 137, 255);
            btnJoin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnJoin.ForeColor = Color.White;
            btnJoin.Location = new Point(10, 409);
            btnJoin.Name = "btnJoin";
            btnJoin.Size = new Size(330, 40);
            btnJoin.TabIndex = 5;
            btnJoin.Text = "Join";
            btnJoin.UseVisualStyleBackColor = false;
            // 
            // LessonCardPanel
            // 
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(picThumb);
            Controls.Add(lblCategory);
            Controls.Add(lblTitle);
            Controls.Add(lblTime);
            Controls.Add(lblAttending);
            Controls.Add(btnJoin);
            ForeColor = SystemColors.ActiveCaptionText;
            Margin = new Padding(30, 10, 10, 10);
            Name = "LessonCardPanel";
            Size = new Size(348, 462);
            Load += LessonCardPanel_Load;
            ((System.ComponentModel.ISupportInitialize)picThumb).EndInit();
            ResumeLayout(false);
        }
    }
}
