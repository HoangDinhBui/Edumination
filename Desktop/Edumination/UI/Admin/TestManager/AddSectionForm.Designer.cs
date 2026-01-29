namespace IELTS.UI.Admin.TestManager
{
    partial class AddSectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddSectionForm));
            cboTimeLimit = new ComboBox();
            label2 = new Label();
            label5 = new Label();
            btnCreate = new Sunny.UI.UIButton();
            btnChooseAudio = new Sunny.UI.UIButton();
            txtFileName = new Sunny.UI.UITextBox();
            btnChooseFile = new Sunny.UI.UIButton();
            axAcroPDFViewer = new AxAcroPDFLib.AxAcroPDF();
            cboSkill = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)axAcroPDFViewer).BeginInit();
            SuspendLayout();
            // 
            // cboTimeLimit
            // 
            cboTimeLimit.FormattingEnabled = true;
            cboTimeLimit.Location = new Point(710, 218);
            cboTimeLimit.Name = "cboTimeLimit";
            cboTimeLimit.Size = new Size(379, 28);
            cboTimeLimit.TabIndex = 130;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(606, 226);
            label2.Name = "label2";
            label2.Size = new Size(82, 20);
            label2.TabIndex = 131;
            label2.Text = "Time Limit:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(606, 169);
            label5.Name = "label5";
            label5.Size = new Size(39, 20);
            label5.TabIndex = 129;
            label5.Text = "Skill:";
            // 
            // btnCreate
            // 
            btnCreate.Font = new Font("Microsoft Sans Serif", 12F);
            btnCreate.Location = new Point(1120, 502);
            btnCreate.MinimumSize = new Size(1, 1);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(214, 62);
            btnCreate.TabIndex = 127;
            btnCreate.Text = "Create";
            btnCreate.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnCreate.Click += btnCreate_Click;
            // 
            // btnChooseAudio
            // 
            btnChooseAudio.Font = new Font("Microsoft Sans Serif", 12F);
            btnChooseAudio.Location = new Point(604, 319);
            btnChooseAudio.MinimumSize = new Size(1, 1);
            btnChooseAudio.Name = "btnChooseAudio";
            btnChooseAudio.Size = new Size(125, 44);
            btnChooseAudio.TabIndex = 126;
            btnChooseAudio.Text = "Audio";
            btnChooseAudio.TipsFont = new Font("Microsoft Sans Serif", 9F);
            // 
            // txtFileName
            // 
            txtFileName.Font = new Font("Microsoft Sans Serif", 12F);
            txtFileName.Location = new Point(604, 75);
            txtFileName.Margin = new Padding(4, 5, 4, 5);
            txtFileName.MinimumSize = new Size(1, 16);
            txtFileName.Name = "txtFileName";
            txtFileName.Padding = new Padding(5);
            txtFileName.ReadOnly = true;
            txtFileName.ShowText = false;
            txtFileName.Size = new Size(485, 57);
            txtFileName.TabIndex = 121;
            txtFileName.TextAlignment = ContentAlignment.MiddleLeft;
            txtFileName.Watermark = "file name";
            // 
            // btnChooseFile
            // 
            btnChooseFile.Font = new Font("Microsoft Sans Serif", 12F);
            btnChooseFile.Location = new Point(1120, 70);
            btnChooseFile.MinimumSize = new Size(1, 1);
            btnChooseFile.Name = "btnChooseFile";
            btnChooseFile.Size = new Size(214, 62);
            btnChooseFile.TabIndex = 120;
            btnChooseFile.Text = "Choose PDF";
            btnChooseFile.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnChooseFile.Click += btnChooseFile_Click;
            // 
            // axAcroPDFViewer
            // 
            axAcroPDFViewer.Enabled = true;
            axAcroPDFViewer.Location = new Point(12, 70);
            axAcroPDFViewer.Name = "axAcroPDFViewer";
            axAcroPDFViewer.OcxState = (AxHost.State)resources.GetObject("axAcroPDFViewer.OcxState");
            axAcroPDFViewer.Size = new Size(569, 519);
            axAcroPDFViewer.TabIndex = 119;
            // 
            // cboSkill
            // 
            cboSkill.FormattingEnabled = true;
            cboSkill.Location = new Point(710, 166);
            cboSkill.Name = "cboSkill";
            cboSkill.Size = new Size(379, 28);
            cboSkill.TabIndex = 132;
            // 
            // AddSectionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1373, 610);
            Controls.Add(cboSkill);
            Controls.Add(cboTimeLimit);
            Controls.Add(label2);
            Controls.Add(label5);
            Controls.Add(btnCreate);
            Controls.Add(btnChooseAudio);
            Controls.Add(txtFileName);
            Controls.Add(btnChooseFile);
            Controls.Add(axAcroPDFViewer);
            Name = "AddSectionForm";
            Text = "AddSectionForm";
            ((System.ComponentModel.ISupportInitialize)axAcroPDFViewer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cboTimeLimit;
        private Label label2;
        private Label label5;
        private Sunny.UI.UIButton btnCreate;
        private Sunny.UI.UIButton btnChooseAudio;
        private Sunny.UI.UITextBox txtFileName;
        private Sunny.UI.UIButton btnChooseFile;
        private AxAcroPDFLib.AxAcroPDF axAcroPDFViewer;
        private ComboBox cboSkill;
    }
}