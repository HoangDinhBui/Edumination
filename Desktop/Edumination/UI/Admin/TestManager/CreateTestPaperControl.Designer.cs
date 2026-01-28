namespace IELTS.UI.Admin.TestManager
{
    partial class CreateTestPaperControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateTestPaperControl));
            txtDescription = new Sunny.UI.UITextBox();
            txtTitle = new Sunny.UI.UITextBox();
            btnUpload = new Sunny.UI.UIButton();
            txtFileName = new Sunny.UI.UITextBox();
            btnChooseFile = new Sunny.UI.UIButton();
            axAcroPDFViewer = new AxAcroPDFLib.AxAcroPDF();
            uiLabelTitle = new Sunny.UI.UILabel();
            label1 = new Label();
            btnChooseAudio = new Sunny.UI.UIButton();
            btnCreate = new Sunny.UI.UIButton();
            label2 = new Label();
            cmbTestMonth = new ComboBox();
            label5 = new Label();
            cboMockTest = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)axAcroPDFViewer).BeginInit();
            SuspendLayout();
            // 
            // txtDescription
            // 
            txtDescription.Font = new Font("Microsoft Sans Serif", 12F);
            txtDescription.Location = new Point(681, 366);
            txtDescription.Margin = new Padding(4, 5, 4, 5);
            txtDescription.MinimumSize = new Size(1, 16);
            txtDescription.Name = "txtDescription";
            txtDescription.Padding = new Padding(5);
            txtDescription.ShowText = false;
            txtDescription.Size = new Size(485, 57);
            txtDescription.TabIndex = 8;
            txtDescription.TextAlignment = ContentAlignment.MiddleLeft;
            txtDescription.Watermark = "des";
            // 
            // txtTitle
            // 
            txtTitle.Font = new Font("Microsoft Sans Serif", 12F);
            txtTitle.Location = new Point(681, 263);
            txtTitle.Margin = new Padding(4, 5, 4, 5);
            txtTitle.MinimumSize = new Size(1, 16);
            txtTitle.Name = "txtTitle";
            txtTitle.Padding = new Padding(5);
            txtTitle.ShowText = false;
            txtTitle.Size = new Size(485, 56);
            txtTitle.TabIndex = 10;
            txtTitle.TextAlignment = ContentAlignment.MiddleLeft;
            txtTitle.Watermark = "title";
            // 
            // btnUpload
            // 
            btnUpload.Font = new Font("Microsoft Sans Serif", 12F);
            btnUpload.Location = new Point(1197, 583);
            btnUpload.MinimumSize = new Size(1, 1);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(214, 62);
            btnUpload.TabIndex = 9;
            btnUpload.Text = "Next Step";
            btnUpload.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnUpload.Click += btnUpload_Click;
            // 
            // txtFileName
            // 
            txtFileName.Font = new Font("Microsoft Sans Serif", 12F);
            txtFileName.Location = new Point(681, 158);
            txtFileName.Margin = new Padding(4, 5, 4, 5);
            txtFileName.MinimumSize = new Size(1, 16);
            txtFileName.Name = "txtFileName";
            txtFileName.Padding = new Padding(5);
            txtFileName.ReadOnly = true;
            txtFileName.ShowText = false;
            txtFileName.Size = new Size(485, 57);
            txtFileName.TabIndex = 7;
            txtFileName.TextAlignment = ContentAlignment.MiddleLeft;
            txtFileName.Watermark = "file name";
            // 
            // btnChooseFile
            // 
            btnChooseFile.Font = new Font("Microsoft Sans Serif", 12F);
            btnChooseFile.Location = new Point(1197, 153);
            btnChooseFile.MinimumSize = new Size(1, 1);
            btnChooseFile.Name = "btnChooseFile";
            btnChooseFile.Size = new Size(214, 62);
            btnChooseFile.TabIndex = 6;
            btnChooseFile.Text = "Choose PDF";
            btnChooseFile.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnChooseFile.Click += btnChooseFile_Click;
            // 
            // axAcroPDFViewer
            // 
            axAcroPDFViewer.Enabled = true;
            axAcroPDFViewer.Location = new Point(39, 158);
            axAcroPDFViewer.Name = "axAcroPDFViewer";
            axAcroPDFViewer.OcxState = (AxHost.State)resources.GetObject("axAcroPDFViewer.OcxState");
            axAcroPDFViewer.Size = new Size(569, 519);
            axAcroPDFViewer.TabIndex = 5;
            // 
            // uiLabelTitle
            // 
            uiLabelTitle.BackColor = SystemColors.GradientInactiveCaption;
            uiLabelTitle.Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            uiLabelTitle.ForeColor = SystemColors.Highlight;
            uiLabelTitle.Location = new Point(0, 0);
            uiLabelTitle.Name = "uiLabelTitle";
            uiLabelTitle.Size = new Size(1430, 70);
            uiLabelTitle.Style = Sunny.UI.UIStyle.Custom;
            uiLabelTitle.TabIndex = 11;
            uiLabelTitle.Text = "Create New Test Paper";
            uiLabelTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(681, 238);
            label1.Name = "label1";
            label1.Size = new Size(135, 20);
            label1.TabIndex = 12;
            label1.Text = "Enter the test's title";
            // 
            // btnChooseAudio
            // 
            btnChooseAudio.Font = new Font("Microsoft Sans Serif", 12F);
            btnChooseAudio.Location = new Point(681, 533);
            btnChooseAudio.MinimumSize = new Size(1, 1);
            btnChooseAudio.Name = "btnChooseAudio";
            btnChooseAudio.Size = new Size(125, 44);
            btnChooseAudio.TabIndex = 13;
            btnChooseAudio.Text = "Audio";
            btnChooseAudio.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnChooseAudio.Click += btnChooseAudio_Click;
            // 
            // btnCreate
            // 
            btnCreate.Font = new Font("Microsoft Sans Serif", 12F);
            btnCreate.Location = new Point(681, 583);
            btnCreate.MinimumSize = new Size(1, 1);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(214, 62);
            btnCreate.TabIndex = 14;
            btnCreate.Text = "Create";
            btnCreate.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnCreate.Click += btnCreate_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(986, 450);
            label2.Name = "label2";
            label2.Size = new Size(85, 20);
            label2.TabIndex = 118;
            label2.Text = "Test Month:";
            // 
            // cmbTestMonth
            // 
            cmbTestMonth.FormattingEnabled = true;
            cmbTestMonth.Location = new Point(1077, 442);
            cmbTestMonth.Name = "cmbTestMonth";
            cmbTestMonth.Size = new Size(89, 28);
            cmbTestMonth.TabIndex = 117;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(683, 450);
            label5.Name = "label5";
            label5.Size = new Size(78, 20);
            label5.TabIndex = 116;
            label5.Text = "Mock Test:";
            // 
            // cboMockTest
            // 
            cboMockTest.FormattingEnabled = true;
            cboMockTest.Location = new Point(767, 442);
            cboMockTest.Name = "cboMockTest";
            cboMockTest.Size = new Size(78, 28);
            cboMockTest.TabIndex = 115;
            // 
            // CreateTestPaperControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(cmbTestMonth);
            Controls.Add(label2);
            Controls.Add(label5);
            Controls.Add(cboMockTest);
            Controls.Add(btnCreate);
            Controls.Add(btnChooseAudio);
            Controls.Add(label1);
            Controls.Add(uiLabelTitle);
            Controls.Add(txtDescription);
            Controls.Add(txtTitle);
            Controls.Add(btnUpload);
            Controls.Add(txtFileName);
            Controls.Add(btnChooseFile);
            Controls.Add(axAcroPDFViewer);
            Name = "CreateTestPaperControl";
            Size = new Size(1430, 710);
            ((System.ComponentModel.ISupportInitialize)axAcroPDFViewer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Sunny.UI.UITextBox txtDescription;
        private Sunny.UI.UITextBox txtTitle;
        private Sunny.UI.UIButton btnUpload;
        private Sunny.UI.UITextBox txtFileName;
        private Sunny.UI.UIButton btnChooseFile;
        private AxAcroPDFLib.AxAcroPDF axAcroPDFViewer;
        private Sunny.UI.UILabel uiLabelTitle;
        private Label label1;
		private Sunny.UI.UIButton btnChooseAudio;
        private Sunny.UI.UIButton btnCreate;
        private Label label2;
        private ComboBox cmbTestMonth;
        private Label label5;
        private ComboBox cboMockTest;
    }
}
