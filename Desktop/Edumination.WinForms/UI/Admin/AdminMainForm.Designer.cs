namespace Edumination.WinForms.UI.Admin
{
    partial class AdminMainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlNavBar = new Panel();
            pnlContent = new Panel();
            SuspendLayout();
            // 
            // pnlNavBar
            // 
            pnlNavBar.Location = new Point(0, 1);
            pnlNavBar.Name = "pnlNavBar";
            pnlNavBar.Size = new Size(250, 1020);
            pnlNavBar.TabIndex = 0;
            // 
            // pnlContent
            // 
            pnlContent.Location = new Point(250, 1);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1730, 1020);
            pnlContent.TabIndex = 1;
            // 
            // AdminMainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1902, 973);
            Controls.Add(pnlContent);
            Controls.Add(pnlNavBar);
            Name = "AdminMainForm";
            Text = "AdminMainForm";
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlNavBar;
        private Panel pnlContent;
    }
}