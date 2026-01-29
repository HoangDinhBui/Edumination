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
            btnCreateSection = new Sunny.UI.UIButton();
            SuspendLayout();
            // 
            // flpSections
            // 
            flpSections.Location = new Point(0, 59);
            flpSections.Name = "flpSections";
            flpSections.Size = new Size(1430, 651);
            flpSections.TabIndex = 0;
            // 
            // btnCreateSection
            // 
            btnCreateSection.Font = new Font("Microsoft Sans Serif", 12F);
            btnCreateSection.Location = new Point(1198, 18);
            btnCreateSection.MinimumSize = new Size(1, 1);
            btnCreateSection.Name = "btnCreateSection";
            btnCreateSection.Size = new Size(229, 35);
            btnCreateSection.TabIndex = 11;
            btnCreateSection.Text = "Create New Section";
            btnCreateSection.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnCreateSection.Click += btnCreateSection_Click;
            // 
            // ShowSectionControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnCreateSection);
            Controls.Add(flpSections);
            Name = "ShowSectionControl";
            Size = new Size(1430, 710);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flpSections;
        private Sunny.UI.UIButton btnCreateSection;
    }
}
