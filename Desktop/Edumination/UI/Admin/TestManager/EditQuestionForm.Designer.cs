namespace IELTS.UI.Admin.TestManager
{
    partial class EditQuestionForm
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
            label2 = new Label();
            nmNumberOfChoices = new NumericUpDown();
            lblCorrect = new Label();
            btnSave = new Button();
            pnlDynamic = new Panel();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            txtSelectedButton = new TextBox();
            cboQuestionType = new ComboBox();
            nmEnd = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)nmNumberOfChoices).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nmEnd).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(420, 150);
            label2.Name = "label2";
            label2.Size = new Size(158, 23);
            label2.TabIndex = 172;
            label2.Text = "Number of choices:";
            // 
            // nmNumberOfChoices
            // 
            nmNumberOfChoices.Location = new Point(580, 147);
            nmNumberOfChoices.Name = "nmNumberOfChoices";
            nmNumberOfChoices.Size = new Size(80, 30);
            nmNumberOfChoices.TabIndex = 171;
            nmNumberOfChoices.TextAlign = HorizontalAlignment.Center;
            nmNumberOfChoices.ValueChanged += nmNumberOfChoices_ValueChanged;
            // 
            // lblCorrect
            // 
            lblCorrect.AutoSize = true;
            lblCorrect.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblCorrect.ForeColor = Color.DimGray;
            lblCorrect.Location = new Point(240, 565);
            lblCorrect.Name = "lblCorrect";
            lblCorrect.Size = new Size(106, 20);
            lblCorrect.TabIndex = 167;
            lblCorrect.Text = "Correct Answer";
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(0, 120, 215);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(980, 560);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(110, 38);
            btnSave.TabIndex = 165;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // pnlDynamic
            // 
            pnlDynamic.BackColor = Color.White;
            pnlDynamic.BorderStyle = BorderStyle.FixedSingle;
            pnlDynamic.Location = new Point(240, 210);
            pnlDynamic.Name = "pnlDynamic";
            pnlDynamic.Size = new Size(880, 340);
            pnlDynamic.TabIndex = 166;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(240, 95);
            label5.Name = "label5";
            label5.Size = new Size(123, 23);
            label5.TabIndex = 164;
            label5.Text = "Question Type:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(240, 150);
            label4.Name = "label4";
            label4.Size = new Size(43, 23);
            label4.TabIndex = 163;
            label4.Text = "End:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(240, 45);
            label3.Name = "label3";
            label3.Size = new Size(135, 23);
            label3.TabIndex = 162;
            label3.Text = "Selected Button:";
            // 
            // txtSelectedButton
            // 
            txtSelectedButton.Location = new Point(380, 42);
            txtSelectedButton.Name = "txtSelectedButton";
            txtSelectedButton.ReadOnly = true;
            txtSelectedButton.Size = new Size(80, 30);
            txtSelectedButton.TabIndex = 161;
            txtSelectedButton.TextAlign = HorizontalAlignment.Center;
            // 
            // cboQuestionType
            // 
            cboQuestionType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboQuestionType.FlatStyle = FlatStyle.Popup;
            cboQuestionType.FormattingEnabled = true;
            cboQuestionType.Location = new Point(380, 92);
            cboQuestionType.Name = "cboQuestionType";
            cboQuestionType.Size = new Size(300, 31);
            cboQuestionType.TabIndex = 160;
            cboQuestionType.SelectedIndexChanged += cboQuestionType_SelectedIndexChanged;
            // 
            // nmEnd
            // 
            nmEnd.Location = new Point(300, 147);
            nmEnd.Name = "nmEnd";
            nmEnd.Size = new Size(80, 30);
            nmEnd.TabIndex = 159;
            nmEnd.TextAlign = HorizontalAlignment.Center;
            nmEnd.ValueChanged += nmEnd_ValueChanged;
            // 
            // EditQuestionForm
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1300, 720);
            Controls.Add(label2);
            Controls.Add(nmNumberOfChoices);
            Controls.Add(lblCorrect);
            Controls.Add(btnSave);
            Controls.Add(pnlDynamic);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(txtSelectedButton);
            Controls.Add(cboQuestionType);
            Controls.Add(nmEnd);
            Font = new Font("Segoe UI", 10F);
            Name = "EditQuestionForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Edit Question";
            ((System.ComponentModel.ISupportInitialize)nmNumberOfChoices).EndInit();
            ((System.ComponentModel.ISupportInitialize)nmEnd).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private NumericUpDown nmNumberOfChoices;
        private Label lblCorrect;
        private Button btnSave;
        private Panel pnlDynamic;
        private Label label5;
        private Label label4;
        private Label label3;
        private TextBox txtSelectedButton;
        private ComboBox cboQuestionType;
        private NumericUpDown nmEnd;
    }
}
