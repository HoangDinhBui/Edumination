namespace Edumination.WinForms.UI.Forms.Home
{
    partial class IeltsTestCardPanel
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
        private Label lblTitle;
        private Label lblRating;

        private void InitializeComponent()
        {
            picThumb = new PictureBox();
            lblTitle = new Label();
            lblRating = new Label();
            ((System.ComponentModel.ISupportInitialize)picThumb).BeginInit();
            SuspendLayout();
            // 
            // picThumb
            // 
            picThumb.Location = new Point(0, 0);
            picThumb.Name = "picThumb";
            picThumb.Size = new Size(290, 273);
            picThumb.SizeMode = PictureBoxSizeMode.StretchImage;
            picThumb.TabIndex = 0;
            picThumb.TabStop = false;
            picThumb.Click += picThumb_Click;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Noto Serif SC", 12F, FontStyle.Bold, GraphicsUnit.Point, 163);
            lblTitle.Location = new Point(0, 296);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(290, 40);
            lblTitle.TabIndex = 1;
            lblTitle.TextAlign = ContentAlignment.TopCenter;
            lblTitle.Click += lblTitle_Click;
            // 
            // lblRating
            // 
            lblRating.Font = new Font("Noto Serif SC SemiBold", 10.1999989F, FontStyle.Bold, GraphicsUnit.Point, 163);
            lblRating.ForeColor = Color.Gray;
            lblRating.Location = new Point(22, 345);
            lblRating.Name = "lblRating";
            lblRating.Size = new Size(250, 25);
            lblRating.TabIndex = 2;
            lblRating.TextAlign = ContentAlignment.TopCenter;
            // 
            // IeltsTestCardPanel
            // 
            Controls.Add(picThumb);
            Controls.Add(lblTitle);
            Controls.Add(lblRating);
            Margin = new Padding(80, 10, 10, 10);
            Name = "IeltsTestCardPanel";
            Size = new Size(290, 389);
            Load += IeltsTestCardPanel_Load;
            ((System.ComponentModel.ISupportInitialize)picThumb).EndInit();
            ResumeLayout(false);
        }
    }
}
