using Sunny.UI;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.Results
{
    partial class AnswerResultForm
    {
        private System.ComponentModel.IContainer components = null;

        private PictureBox picAvatar;
        private Label lblUserName;
        private Label lblTitleResult;

        private Label lblCorrectMain;
        private Label lblBandMain;
        private Label lblTimeMain;

        private Label lblCorrectLabel;
        private Label lblBandLabel;
        private Label lblTimeLabel;

        private Label lblAnswerKeysTitle;
        private Panel panelAnswerKeys;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            picAvatar = new PictureBox();
            lblUserName = new Label();
            lblTitleResult = new Label();
            lblCorrectMain = new Label();
            lblBandMain = new Label();
            lblTimeMain = new Label();
            lblCorrectLabel = new Label();
            lblBandLabel = new Label();
            lblTimeLabel = new Label();
            lblAnswerKeysTitle = new Label();
            panelAnswerKeys = new Panel();
            ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
            SuspendLayout();
            // 
            // picAvatar
            // 
            picAvatar.Location = new Point(829, 49);
            picAvatar.Margin = new Padding(3, 4, 3, 4);
            picAvatar.Name = "picAvatar";
            picAvatar.Size = new Size(126, 147);
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            picAvatar.TabIndex = 0;
            picAvatar.TabStop = false;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Segoe UI", 11F);
            lblUserName.Location = new Point(829, 200);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(131, 25);
            lblUserName.TabIndex = 1;
            lblUserName.Text = "Student Name";
            // 
            // lblTitleResult
            // 
            lblTitleResult.AutoSize = true;
            lblTitleResult.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblTitleResult.ForeColor = Color.FromArgb(39, 56, 146);
            lblTitleResult.Location = new Point(737, 247);
            lblTitleResult.Name = "lblTitleResult";
            lblTitleResult.Size = new Size(335, 60);
            lblTitleResult.TabIndex = 2;
            lblTitleResult.Text = "Reading Result";
            // 
            // lblCorrectMain
            // 
            lblCorrectMain.AutoSize = true;
            lblCorrectMain.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblCorrectMain.Location = new Point(632, 399);
            lblCorrectMain.Name = "lblCorrectMain";
            lblCorrectMain.Size = new Size(77, 37);
            lblCorrectMain.TabIndex = 4;
            lblCorrectMain.Text = "0/40";
            // 
            // lblBandMain
            // 
            lblBandMain.AutoSize = true;
            lblBandMain.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblBandMain.Location = new Point(878, 392);
            lblBandMain.Name = "lblBandMain";
            lblBandMain.Size = new Size(50, 60);
            lblBandMain.TabIndex = 6;
            lblBandMain.Text = "0";
            // 
            // lblTimeMain
            // 
            lblTimeMain.AutoSize = true;
            lblTimeMain.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTimeMain.Location = new Point(1064, 399);
            lblTimeMain.Name = "lblTimeMain";
            lblTimeMain.Size = new Size(88, 37);
            lblTimeMain.TabIndex = 8;
            lblTimeMain.Text = "00:00";
            // 
            // lblCorrectLabel
            // 
            lblCorrectLabel.AutoSize = true;
            lblCorrectLabel.Font = new Font("Segoe UI", 12F);
            lblCorrectLabel.Location = new Point(632, 359);
            lblCorrectLabel.Name = "lblCorrectLabel";
            lblCorrectLabel.Size = new Size(76, 28);
            lblCorrectLabel.TabIndex = 3;
            lblCorrectLabel.Text = "Correct";
            // 
            // lblBandLabel
            // 
            lblBandLabel.AutoSize = true;
            lblBandLabel.Font = new Font("Segoe UI", 12F);
            lblBandLabel.Location = new Point(861, 359);
            lblBandLabel.Name = "lblBandLabel";
            lblBandLabel.Size = new Size(123, 28);
            lblBandLabel.TabIndex = 5;
            lblBandLabel.Text = "Total Correct";
            // 
            // lblTimeLabel
            // 
            lblTimeLabel.AutoSize = true;
            lblTimeLabel.Font = new Font("Segoe UI", 12F);
            lblTimeLabel.Location = new Point(1078, 359);
            lblTimeLabel.Name = "lblTimeLabel";
            lblTimeLabel.Size = new Size(54, 28);
            lblTimeLabel.TabIndex = 7;
            lblTimeLabel.Text = "Time";
            // 
            // lblAnswerKeysTitle
            // 
            lblAnswerKeysTitle.AutoSize = true;
            lblAnswerKeysTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblAnswerKeysTitle.ForeColor = Color.FromArgb(39, 56, 146);
            lblAnswerKeysTitle.Location = new Point(137, 480);
            lblAnswerKeysTitle.Name = "lblAnswerKeysTitle";
            lblAnswerKeysTitle.Size = new Size(164, 35);
            lblAnswerKeysTitle.TabIndex = 9;
            lblAnswerKeysTitle.Text = "Answer Keys";
            // 
            // panelAnswerKeys
            // 
            panelAnswerKeys.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelAnswerKeys.AutoScroll = true;
            panelAnswerKeys.BackColor = Color.White;
            panelAnswerKeys.Location = new Point(164, 533);
            panelAnswerKeys.Margin = new Padding(3, 4, 3, 4);
            panelAnswerKeys.Name = "panelAnswerKeys";
            panelAnswerKeys.Size = new Size(1429, 453);
            panelAnswerKeys.TabIndex = 10;
            // 
            // AnswerResultForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1691, 1055);
            Controls.Add(picAvatar);
            Controls.Add(lblUserName);
            Controls.Add(lblTitleResult);
            Controls.Add(lblCorrectLabel);
            Controls.Add(lblCorrectMain);
            Controls.Add(lblBandLabel);
            Controls.Add(lblBandMain);
            Controls.Add(lblTimeLabel);
            Controls.Add(lblTimeMain);
            Controls.Add(lblAnswerKeysTitle);
            Controls.Add(panelAnswerKeys);
            Margin = new Padding(3, 4, 3, 4);
            Name = "AnswerResultForm";
            Text = "Test Result";
            Load += AnswerResultForm_Load;
            ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
