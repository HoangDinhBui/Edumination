namespace IELTS.UI.Admin.TestManager
{
    partial class ShowSectionControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            flpSections = new FlowLayoutPanel();
            btnCreateTestPaper = new Sunny.UI.UIButton();
            SuspendLayout();
            // 
            // flpSections
            // 
            flpSections.Location = new Point(0, 59);
            flpSections.Name = "flpSections";
            flpSections.Size = new Size(1430, 651);
            flpSections.TabIndex = 0;
            // 
            // btnCreateTestPaper
            // 
            btnCreateTestPaper.Font = new Font("Microsoft Sans Serif", 12F);
            btnCreateTestPaper.Location = new Point(1198, 18);
            btnCreateTestPaper.MinimumSize = new Size(1, 1);
            btnCreateTestPaper.Name = "btnCreateTestPaper";
            btnCreateTestPaper.Size = new Size(229, 35);
            btnCreateTestPaper.TabIndex = 11;
            btnCreateTestPaper.Text = "Create New Section";
            btnCreateTestPaper.TipsFont = new Font("Microsoft Sans Serif", 9F);
            // 
            // ShowSectionControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnCreateTestPaper);
            Controls.Add(flpSections);
            Name = "ShowSectionControl";
            Size = new Size(1430, 710);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flpSections;
        private Sunny.UI.UIButton btnCreateTestPaper;
    }
}
