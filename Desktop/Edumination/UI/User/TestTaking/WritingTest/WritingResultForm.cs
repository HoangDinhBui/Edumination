using System;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.TestTaking.WritingTest
{
    public partial class WritingResultForm : Form
    {
        public WritingResultForm(double bandScore, string feedback, string correction)
        {
            InitializeComponent();
            
            lblBandScore.Text = $"Band Score: {bandScore}";
            txtFeedback.Text = feedback;
            txtCorrection.Text = correction;
        }

        private void InitializeComponent()
        {
            this.lblBandScore = new System.Windows.Forms.Label();
            this.txtFeedback = new System.Windows.Forms.TextBox();
            this.txtCorrection = new System.Windows.Forms.TextBox();
            this.lblFeedbackTitle = new System.Windows.Forms.Label();
            this.lblCorrectionTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblBandScore
            // 
            this.lblBandScore.AutoSize = true;
            this.lblBandScore.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblBandScore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(41)))), ((int)(((byte)(88)))));
            this.lblBandScore.Location = new System.Drawing.Point(30, 20);
            this.lblBandScore.Name = "lblBandScore";
            this.lblBandScore.Size = new System.Drawing.Size(250, 45);
            this.lblBandScore.TabIndex = 0;
            this.lblBandScore.Text = "Band Score: 0.0";
            // 
            // lblFeedbackTitle
            // 
            this.lblFeedbackTitle.AutoSize = true;
            this.lblFeedbackTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblFeedbackTitle.Location = new System.Drawing.Point(35, 80);
            this.lblFeedbackTitle.Name = "lblFeedbackTitle";
            this.lblFeedbackTitle.Size = new System.Drawing.Size(82, 21);
            this.lblFeedbackTitle.TabIndex = 1;
            this.lblFeedbackTitle.Text = "Feedback";
            // 
            // txtFeedback
            // 
            this.txtFeedback.Location = new System.Drawing.Point(35, 110);
            this.txtFeedback.Multiline = true;
            this.txtFeedback.Name = "txtFeedback";
            this.txtFeedback.ReadOnly = true;
            this.txtFeedback.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFeedback.Size = new System.Drawing.Size(700, 150);
            this.txtFeedback.TabIndex = 2;
            this.txtFeedback.Font = new System.Drawing.Font("Segoe UI", 10F);
            // 
            // lblCorrectionTitle
            // 
            this.lblCorrectionTitle.AutoSize = true;
            this.lblCorrectionTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCorrectionTitle.Location = new System.Drawing.Point(35, 280);
            this.lblCorrectionTitle.Name = "lblCorrectionTitle";
            this.lblCorrectionTitle.Size = new System.Drawing.Size(90, 21);
            this.lblCorrectionTitle.TabIndex = 3;
            this.lblCorrectionTitle.Text = "Correction";
            // 
            // txtCorrection
            // 
            this.txtCorrection.Location = new System.Drawing.Point(35, 310);
            this.txtCorrection.Multiline = true;
            this.txtCorrection.Name = "txtCorrection";
            this.txtCorrection.ReadOnly = true;
            this.txtCorrection.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCorrection.Size = new System.Drawing.Size(700, 150);
            this.txtCorrection.TabIndex = 4;
            this.txtCorrection.Font = new System.Drawing.Font("Segoe UI", 10F);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(635, 480);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 40);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += (s, e) => this.Close();
            // 
            // WritingResultForm
            // 
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtCorrection);
            this.Controls.Add(this.lblCorrectionTitle);
            this.Controls.Add(this.txtFeedback);
            this.Controls.Add(this.lblFeedbackTitle);
            this.Controls.Add(this.lblBandScore);
            this.Name = "WritingResultForm";
            this.Text = "Writing Result (AI Graded)";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblBandScore;
        private System.Windows.Forms.TextBox txtFeedback;
        private System.Windows.Forms.TextBox txtCorrection;
        private System.Windows.Forms.Label lblFeedbackTitle;
        private System.Windows.Forms.Label lblCorrectionTitle;
        private System.Windows.Forms.Button btnClose;
    }
}
