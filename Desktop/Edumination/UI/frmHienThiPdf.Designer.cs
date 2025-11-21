namespace IELTS.UI
{
    partial class frmHienThiPdf
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHienThiPdf));
            axAcroPDF = new AxAcroPDFLib.AxAcroPDF();
            btnChooseFile = new Sunny.UI.UIButton();
            txtFileName = new Sunny.UI.UITextBox();
            btnUpload = new Sunny.UI.UIButton();
            txtTitle = new Sunny.UI.UITextBox();
            txtDescription = new Sunny.UI.UITextBox();
            ((System.ComponentModel.ISupportInitialize)axAcroPDF).BeginInit();
            SuspendLayout();
            // 
            // axAcroPDF
            // 
            axAcroPDF.Enabled = true;
            axAcroPDF.Location = new Point(46, 39);
            axAcroPDF.Name = "axAcroPDF";
            axAcroPDF.OcxState = (AxHost.State)resources.GetObject("axAcroPDF.OcxState");
            axAcroPDF.Size = new Size(569, 641);
            axAcroPDF.TabIndex = 0;
            // 
            // btnChooseFile
            // 
            btnChooseFile.Font = new Font("Microsoft Sans Serif", 12F);
            btnChooseFile.Location = new Point(711, 104);
            btnChooseFile.MinimumSize = new Size(1, 1);
            btnChooseFile.Name = "btnChooseFile";
            btnChooseFile.Size = new Size(214, 62);
            btnChooseFile.TabIndex = 1;
            btnChooseFile.Text = "choose";
            btnChooseFile.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnChooseFile.Click += btnChooseFile_Click;
            // 
            // txtFileName
            // 
            txtFileName.Font = new Font("Microsoft Sans Serif", 12F);
            txtFileName.Location = new Point(711, 209);
            txtFileName.Margin = new Padding(4, 5, 4, 5);
            txtFileName.MinimumSize = new Size(1, 16);
            txtFileName.Name = "txtFileName";
            txtFileName.Padding = new Padding(5);
            txtFileName.ShowText = false;
            txtFileName.Size = new Size(214, 57);
            txtFileName.TabIndex = 2;
            txtFileName.TextAlignment = ContentAlignment.MiddleLeft;
            txtFileName.Watermark = "file name";
            // 
            // btnUpload
            // 
            btnUpload.Font = new Font("Microsoft Sans Serif", 12F);
            btnUpload.Location = new Point(711, 413);
            btnUpload.MinimumSize = new Size(1, 1);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(214, 62);
            btnUpload.TabIndex = 3;
            btnUpload.Text = "up";
            btnUpload.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnUpload.Click += btnUpload_Click;
            // 
            // txtTitle
            // 
            txtTitle.Font = new Font("Microsoft Sans Serif", 12F);
            txtTitle.Location = new Point(622, 303);
            txtTitle.Margin = new Padding(4, 5, 4, 5);
            txtTitle.MinimumSize = new Size(1, 16);
            txtTitle.Name = "txtTitle";
            txtTitle.Padding = new Padding(5);
            txtTitle.ShowText = false;
            txtTitle.Size = new Size(146, 57);
            txtTitle.TabIndex = 4;
            txtTitle.TextAlignment = ContentAlignment.MiddleLeft;
            txtTitle.Watermark = "title";
            // 
            // txtDescription
            // 
            txtDescription.Font = new Font("Microsoft Sans Serif", 12F);
            txtDescription.Location = new Point(854, 303);
            txtDescription.Margin = new Padding(4, 5, 4, 5);
            txtDescription.MinimumSize = new Size(1, 16);
            txtDescription.Name = "txtDescription";
            txtDescription.Padding = new Padding(5);
            txtDescription.ShowText = false;
            txtDescription.Size = new Size(149, 57);
            txtDescription.TabIndex = 3;
            txtDescription.TextAlignment = ContentAlignment.MiddleLeft;
            txtDescription.Watermark = "des";
            // 
            // frmHienThiPdf
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1016, 815);
            Controls.Add(txtDescription);
            Controls.Add(txtTitle);
            Controls.Add(btnUpload);
            Controls.Add(txtFileName);
            Controls.Add(btnChooseFile);
            Controls.Add(axAcroPDF);
            Name = "frmHienThiPdf";
            Text = "frmHienThiPdf";
            Load += frmHienThiPdf_Load_1;
            ((System.ComponentModel.ISupportInitialize)axAcroPDF).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private AxAcroPDFLib.AxAcroPDF axAcroPDF;
        private Sunny.UI.UIButton btnChooseFile;
        private Sunny.UI.UITextBox txtFileName;
        private Sunny.UI.UIButton btnUpload;
        private Sunny.UI.UITextBox txtTitle;
        private Sunny.UI.UITextBox txtDescription;
    }
}