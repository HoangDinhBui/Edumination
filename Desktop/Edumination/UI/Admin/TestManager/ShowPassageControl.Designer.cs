namespace IELTS.UI.Admin.TestManager
{
    partial class ShowPassageControl
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
            flpPassages = new FlowLayoutPanel();
            btnCreatePassage = new Sunny.UI.UIButton();
            SuspendLayout();
            // 
            // flpPassages
            // 
            flpPassages.Location = new Point(0, 68);
            flpPassages.Name = "flpPassages";
            flpPassages.Size = new Size(1430, 642);
            flpPassages.TabIndex = 0;
            // 
            // btnCreatePassage
            // 
            btnCreatePassage.Font = new Font("Microsoft Sans Serif", 12F);
            btnCreatePassage.Location = new Point(1198, 14);
            btnCreatePassage.MinimumSize = new Size(1, 1);
            btnCreatePassage.Name = "btnCreatePassage";
            btnCreatePassage.Size = new Size(229, 35);
            btnCreatePassage.TabIndex = 12;
            btnCreatePassage.Text = "Create New Passage";
            btnCreatePassage.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnCreatePassage.Click += btnCreatePassage_Click;
            // 
            // ShowPassageControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnCreatePassage);
            Controls.Add(flpPassages);
            Name = "ShowPassageControl";
            Size = new Size(1430, 710);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flpPassages;
        private Sunny.UI.UIButton btnCreatePassage;
    }
}
