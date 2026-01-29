namespace IELTS.UI.Admin.TestManager
{
    partial class AddPassageForm
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
            label1 = new Label();
            txtTitle = new Sunny.UI.UITextBox();
            cboPassagePosition = new ComboBox();
            label2 = new Label();
            btnCreate = new Sunny.UI.UIButton();
            richTextBox1 = new RichTextBox();
            label3 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(93, 56);
            label1.Name = "label1";
            label1.Size = new Size(165, 20);
            label1.TabIndex = 14;
            label1.Text = "Enter the passage's title";
            // 
            // txtTitle
            // 
            txtTitle.Font = new Font("Microsoft Sans Serif", 12F);
            txtTitle.Location = new Point(93, 81);
            txtTitle.Margin = new Padding(4, 5, 4, 5);
            txtTitle.MinimumSize = new Size(1, 16);
            txtTitle.Name = "txtTitle";
            txtTitle.Padding = new Padding(5);
            txtTitle.ShowText = false;
            txtTitle.Size = new Size(485, 56);
            txtTitle.TabIndex = 13;
            txtTitle.TextAlignment = ContentAlignment.MiddleLeft;
            txtTitle.Watermark = "title";
            // 
            // cboPassagePosition
            // 
            cboPassagePosition.FormattingEnabled = true;
            cboPassagePosition.Location = new Point(237, 160);
            cboPassagePosition.Name = "cboPassagePosition";
            cboPassagePosition.Size = new Size(341, 28);
            cboPassagePosition.TabIndex = 133;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(95, 168);
            label2.Name = "label2";
            label2.Size = new Size(118, 20);
            label2.TabIndex = 134;
            label2.Text = "Pasage's postion";
            // 
            // btnCreate
            // 
            btnCreate.Font = new Font("Microsoft Sans Serif", 12F);
            btnCreate.Location = new Point(175, 474);
            btnCreate.MinimumSize = new Size(1, 1);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(214, 62);
            btnCreate.TabIndex = 132;
            btnCreate.Text = "Create";
            btnCreate.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnCreate.Click += btnCreate_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(93, 235);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(482, 178);
            richTextBox1.TabIndex = 135;
            richTextBox1.Text = "";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(93, 212);
            label3.Name = "label3";
            label3.Size = new Size(95, 20);
            label3.TabIndex = 136;
            label3.Text = "Passage Text:";
            // 
            // AddPassageForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(665, 561);
            Controls.Add(label3);
            Controls.Add(richTextBox1);
            Controls.Add(cboPassagePosition);
            Controls.Add(label2);
            Controls.Add(btnCreate);
            Controls.Add(label1);
            Controls.Add(txtTitle);
            Name = "AddPassageForm";
            Text = "AddPassageForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Sunny.UI.UITextBox txtTitle;
        private ComboBox cboPassagePosition;
        private Label label2;
        private Sunny.UI.UIButton btnCreate;
        private RichTextBox richTextBox1;
        private Label label3;
    }
}