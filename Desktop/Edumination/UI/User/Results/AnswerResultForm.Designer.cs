using Sunny.UI;

namespace IELTS.UI.User.Results
{
    partial class AnswerResultForm
    {
        private System.ComponentModel.IContainer components = null;

        private PictureBox picAvatar;
        private UILabel lblUserName;
        private UILabel lblTitleResult;

        private UIPanel panelCorrectCircle;
        private UIPanel panelBandCircle;
        private UIPanel panelTimeCircle;

        private UILabel lblCorrectMain;
        private UILabel lblCorrectSub;

        private UILabel lblBandMain;
        private UILabel lblBandSub;

        private UILabel lblTimeMain;
        private UILabel lblTimeSub;

        private UILabel lblAnswerKeysTitle;
        private Panel panelAnswerKeys;   // scroll area

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            picAvatar = new PictureBox();
            lblUserName = new UILabel();
            lblTitleResult = new UILabel();

            panelCorrectCircle = new UIPanel();
            panelBandCircle = new UIPanel();
            panelTimeCircle = new UIPanel();

            lblCorrectMain = new UILabel();
            lblCorrectSub = new UILabel();

            lblBandMain = new UILabel();
            lblBandSub = new UILabel();

            lblTimeMain = new UILabel();
            lblTimeSub = new UILabel();

            lblAnswerKeysTitle = new UILabel();
            panelAnswerKeys = new Panel();

            ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
            SuspendLayout();

            // Form
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            ClientSize = new Size(1920, 1020);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "AnswerResultForm";
            Text = "Test Result";

            // Avatar
            picAvatar.Location = new Point(910, 40);
            picAvatar.Size = new Size(100, 100);
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            picAvatar.BorderStyle = BorderStyle.None;

            // User Name
            lblUserName.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            lblUserName.ForeColor = Color.FromArgb(60, 60, 60);
            lblUserName.Location = new Point(0, 145);
            lblUserName.Size = new Size(1920, 25);
            lblUserName.TextAlign = ContentAlignment.MiddleCenter;
            lblUserName.Text = "Student Name";

            // Title "Result"
            lblTitleResult.Font = new Font("Noto Serif SC", 24F, FontStyle.Bold);
            lblTitleResult.ForeColor = Color.FromArgb(41, 69, 99);
            lblTitleResult.Location = new Point(0, 170);
            lblTitleResult.Size = new Size(1920, 60);
            lblTitleResult.TextAlign = ContentAlignment.MiddleCenter;
            lblTitleResult.Text = "Result";

            // ======= CIRCLE PANELS =======
            int circleY = 240;
            int circleW = 180;
            int circleH = 180;
            int centerX = 1920 / 2;

            // Correct circle
            panelCorrectCircle.Size = new Size(circleW, circleH);
            panelCorrectCircle.Location = new Point(centerX - circleW - 140, circleY);
            panelCorrectCircle.Radius = circleW / 2;
            panelCorrectCircle.FillColor = Color.White;
            panelCorrectCircle.RectColor = Color.FromArgb(0, 185, 241);
            panelCorrectCircle.RectSize = 4;

            // Band circle (ở đây dùng "total correct" dạng số, giống hình demo)
            panelBandCircle.Size = new Size(circleW, circleH);
            panelBandCircle.Location = new Point(centerX - circleW / 2, circleY);
            panelBandCircle.Radius = circleW / 2;
            panelBandCircle.FillColor = Color.White;
            panelBandCircle.RectColor = Color.FromArgb(0, 185, 241);
            panelBandCircle.RectSize = 4;

            // Time circle
            panelTimeCircle.Size = new Size(circleW, circleH);
            panelTimeCircle.Location = new Point(centerX + circleW + 60, circleY);
            panelTimeCircle.Radius = circleW / 2;
            panelTimeCircle.FillColor = Color.White;
            panelTimeCircle.RectColor = Color.FromArgb(0, 185, 241);
            panelTimeCircle.RectSize = 4;

            // Correct labels
            lblCorrectMain.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblCorrectMain.Location = new Point(0, 45);
            lblCorrectMain.Size = new Size(circleW, 40);
            lblCorrectMain.TextAlign = ContentAlignment.MiddleCenter;
            lblCorrectMain.Text = "0/40";

            lblCorrectSub.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblCorrectSub.Location = new Point(10, 90);
            lblCorrectSub.Size = new Size(circleW - 20, 40);
            lblCorrectSub.TextAlign = ContentAlignment.TopCenter;
            lblCorrectSub.ForeColor = Color.Gray;
            lblCorrectSub.Text = "The correct answer";

            panelCorrectCircle.Controls.Add(lblCorrectMain);
            panelCorrectCircle.Controls.Add(lblCorrectSub);

            // Band labels (ở đây dùng "score" – số câu đúng hoặc bandscore nếu có)
            lblBandMain.Font = new Font("Segoe UI", 32F, FontStyle.Bold);
            lblBandMain.Location = new Point(0, 55);
            lblBandMain.Size = new Size(circleW, 60);
            lblBandMain.TextAlign = ContentAlignment.MiddleCenter;
            lblBandMain.Text = "0";

            lblBandSub.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblBandSub.Location = new Point(10, 115);
            lblBandSub.Size = new Size(circleW - 20, 40);
            lblBandSub.TextAlign = ContentAlignment.TopCenter;
            lblBandSub.ForeColor = Color.Gray;
            lblBandSub.Text = "Score";

            panelBandCircle.Controls.Add(lblBandMain);
            panelBandCircle.Controls.Add(lblBandSub);

            // Time labels
            lblTimeMain.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTimeMain.Location = new Point(0, 50);
            lblTimeMain.Size = new Size(circleW, 50);
            lblTimeMain.TextAlign = ContentAlignment.MiddleCenter;
            lblTimeMain.Text = "00:00";

            lblTimeSub.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblTimeSub.Location = new Point(10, 100);
            lblTimeSub.Size = new Size(circleW - 20, 40);
            lblTimeSub.TextAlign = ContentAlignment.TopCenter;
            lblTimeSub.ForeColor = Color.Gray;
            lblTimeSub.Text = "Time to complete\nthe exam";

            panelTimeCircle.Controls.Add(lblTimeMain);
            panelTimeCircle.Controls.Add(lblTimeSub);

            // ====== Answer Keys Title ======
            lblAnswerKeysTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblAnswerKeysTitle.Location = new Point(260, 450);
            lblAnswerKeysTitle.Size = new Size(300, 30);
            lblAnswerKeysTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblAnswerKeysTitle.Text = "Answer Keys";

            // Panel AnswerKeys
            panelAnswerKeys.Location = new Point(260, 485);
            panelAnswerKeys.Size = new Size(1400, 480);
            panelAnswerKeys.BackColor = Color.White;
            panelAnswerKeys.AutoScroll = true;
            panelAnswerKeys.BorderStyle = BorderStyle.None;

            // Add controls to form
            Controls.Add(picAvatar);
            Controls.Add(lblUserName);
            Controls.Add(lblTitleResult);

            Controls.Add(panelCorrectCircle);
            Controls.Add(panelBandCircle);
            Controls.Add(panelTimeCircle);

            Controls.Add(lblAnswerKeysTitle);
            Controls.Add(panelAnswerKeys);

            ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
            ResumeLayout(false);
        }
    }
}