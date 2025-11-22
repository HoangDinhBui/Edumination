namespace IELTS.BLL
{
    partial class AddTestSectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddTestSectionForm));
            label1 = new Label();
            textBox1 = new TextBox();
            btnOpenPdf = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            axAcroPDFViewer = new AxAcroPDFLib.AxAcroPDF();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)axAcroPDFViewer).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(143, 20);
            label1.Name = "label1";
            label1.Size = new Size(60, 20);
            label1.TabIndex = 0;
            label1.Text = "reading";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(324, 35);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(167, 27);
            textBox1.TabIndex = 1;
            // 
            // btnOpenPdf
            // 
            btnOpenPdf.Location = new Point(531, 33);
            btnOpenPdf.Name = "btnOpenPdf";
            btnOpenPdf.Size = new Size(86, 36);
            btnOpenPdf.TabIndex = 2;
            btnOpenPdf.Text = "chóoe file";
            btnOpenPdf.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(axAcroPDFViewer);
            flowLayoutPanel1.Location = new Point(52, 96);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1038, 757);
            flowLayoutPanel1.TabIndex = 3;
            // 
            // axAcroPDFViewer
            // 
            axAcroPDFViewer.Enabled = true;
            axAcroPDFViewer.Location = new Point(3, 3);
            axAcroPDFViewer.Name = "axAcroPDFViewer";
            axAcroPDFViewer.OcxState = (AxHost.State)resources.GetObject("axAcroPDFViewer.OcxState");
            axAcroPDFViewer.Size = new Size(541, 734);
            axAcroPDFViewer.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(680, 33);
            button1.Name = "button1";
            button1.Size = new Size(86, 36);
            button1.TabIndex = 4;
            button1.Text = "1";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(839, 33);
            button2.Name = "button2";
            button2.Size = new Size(86, 36);
            button2.TabIndex = 5;
            button2.Text = "2";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(980, 35);
            button3.Name = "button3";
            button3.Size = new Size(86, 36);
            button3.TabIndex = 6;
            button3.Text = "3";
            button3.UseVisualStyleBackColor = true;
            // 
            // AddTestSectionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1102, 877);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(btnOpenPdf);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Name = "AddTestSectionForm";
            Text = "AddTestSectionForm";
            flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)axAcroPDFViewer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Button btnOpenPdf;
        private FlowLayoutPanel flowLayoutPanel1;
        private AxAcroPDFLib.AxAcroPDF axAcroPDFViewer;
        private Button button1;
        private Button button2;
        private Button button3;
    }
}