namespace IELTS.UI.Admin.TestManager
{
    partial class ShowQuestionControl
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
            flpQuestions = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // flpQuestions
            // 
            flpQuestions.AutoScroll = true;
            flpQuestions.Dock = DockStyle.Fill;
            flpQuestions.Location = new Point(0, 0);
            flpQuestions.Name = "flpQuestions";
            flpQuestions.Size = new Size(1430, 710);
            flpQuestions.TabIndex = 0;
            // 
            // ShowQuestionControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flpQuestions);
            Name = "ShowQuestionControl";
            Size = new Size(1430, 710);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flpQuestions;
    }
}
