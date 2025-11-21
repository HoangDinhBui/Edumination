using Sunny.UI;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.TestTaking.Controls
{
    partial class AudioRecorderPanel
    {
        private System.ComponentModel.IContainer components = null;

        private PictureBox picMic;
        private Label lblTime;
        private Button btnNext;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            picMic = new PictureBox();
            lblTime = new Label();
            btnNext = new Button();
            ((System.ComponentModel.ISupportInitialize)picMic).BeginInit();
            SuspendLayout();
            // 
            // picMic
            // 
            picMic.Location = new Point(47, 0);
            picMic.Name = "picMic";
            picMic.Size = new Size(80, 80);
            picMic.SizeMode = PictureBoxSizeMode.Zoom;
            picMic.TabIndex = 0;
            picMic.TabStop = false;
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTime.ForeColor = Color.FromArgb(39, 56, 146);
            lblTime.Location = new Point(57, 92);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(61, 25);
            lblTime.TabIndex = 1;
            lblTime.Text = "00:00";
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.FromArgb(39, 56, 146);
            btnNext.FlatAppearance.BorderSize = 0;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNext.ForeColor = Color.White;
            btnNext.Location = new Point(3, 132);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(160, 40);
            btnNext.TabIndex = 2;
            btnNext.Text = "Next question";
            btnNext.UseVisualStyleBackColor = false;
            btnNext.Click += btnNext_Click;
            // 
            // AudioRecorderPanel
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            Controls.Add(picMic);
            Controls.Add(lblTime);
            Controls.Add(btnNext);
            Name = "AudioRecorderPanel";
            Size = new Size(180, 190);
            ((System.ComponentModel.ISupportInitialize)picMic).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
