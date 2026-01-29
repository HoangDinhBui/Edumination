namespace IELTS.UI.Admin.TestManager
{
    partial class NewShowQuestionCotrol
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
            flpQuestions.Location = new Point(27, 25);
            flpQuestions.Name = "flpQuestions";
            flpQuestions.Size = new Size(984, 433);
            flpQuestions.TabIndex = 0;
            // 
            // NewShowQuestionCotrol
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flpQuestions);
            Name = "NewShowQuestionCotrol";
            Size = new Size(1056, 505);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flpQuestions;
    }
}
