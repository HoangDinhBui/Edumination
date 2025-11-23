using Sunny.UI;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace IELTS.UI.User.Results
{
    public partial class AnswerResultForm : Form
    {
        private readonly ExamResult _result;

        public AnswerResultForm(ExamResult result)
        {
            _result = result ?? throw new ArgumentNullException(nameof(result));
            InitializeComponent();

            WindowState = FormWindowState.Maximized;
        }

        private void AnswerResultForm_Load(object sender, EventArgs e)
        {
            // Avatar
            if (!string.IsNullOrWhiteSpace(_result.AvatarPath) &&
                File.Exists(_result.AvatarPath))
            {
                try
                {
                    picAvatar.Image = Image.FromFile(_result.AvatarPath);
                }
                catch
                {
                    // ignore lỗi đọc ảnh
                }
            }

            // User name & title
            lblUserName.Text = string.IsNullOrWhiteSpace(_result.UserName)
                ? "Student Name"
                : _result.UserName;

            lblTitleResult.Text = $"{_result.Skill} Result";

            // Correct
            lblCorrectMain.Text = $"{_result.CorrectCount}/{_result.TotalQuestions}";
            lblBandMain.Text = _result.CorrectCount.ToString();

            // Time
            var span = TimeSpan.FromSeconds(_result.TimeTakenSeconds);
            lblTimeMain.Text = $"{(int)span.TotalMinutes:D2}:{span.Seconds:D2}";

            BuildAnswerKeysUI();
        }

        private void BuildAnswerKeysUI()
        {
            panelAnswerKeys.Controls.Clear();

            if (_result.Parts == null || _result.Parts.Count == 0)
                return;

            int y = 0;

            foreach (var part in _result.Parts)
            {
                // Tiêu đề Part
                var lblPart = new UILabel
                {
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(41, 69, 99),
                    Location = new Point(0, y),
                    Size = new Size(600, 25),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Text = part.PartName
                };
                panelAnswerKeys.Controls.Add(lblPart);
                y += 32;

                // FLowLayout 2 cột
                var flow = new FlowLayoutPanel
                {
                    Location = new Point(0, y),
                    Width = panelAnswerKeys.Width - 40,
                    AutoSize = true,
                    WrapContents = false,
                    FlowDirection = FlowDirection.LeftToRight
                };

                var leftCol = new FlowLayoutPanel
                {
                    Width = 600,
                    AutoSize = true,
                    FlowDirection = FlowDirection.TopDown
                };

                var rightCol = new FlowLayoutPanel
                {
                    Width = 600,
                    AutoSize = true,
                    FlowDirection = FlowDirection.TopDown
                };

                // Cắt câu thành 2 phần
                var list = part.Questions.OrderBy(q => q.Number).ToList();
                int half = (int)Math.Ceiling(list.Count / 2.0);

                foreach (var q in list.Take(half))
                {
                    var row = new AnswerRowPanel();
                    row.Bind(q);
                    leftCol.Controls.Add(row);
                }

                foreach (var q in list.Skip(half))
                {
                    var row = new AnswerRowPanel();
                    row.Bind(q);
                    rightCol.Controls.Add(row);
                }

                flow.Controls.Add(leftCol);
                flow.Controls.Add(rightCol);

                panelAnswerKeys.Controls.Add(flow);

                y += flow.Height + 25;
            }
        }


        private Control BuildColumnPanel(System.Collections.Generic.List<QuestionReview> list)
        {
            var panel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Fill
            };

            int y = 0;
            foreach (var q in list)
            {
                var row = new AnswerRowPanel();
                row.Bind(q);
                row.Location = new Point(0, y);
                panel.Controls.Add(row);
                y += row.Height;
            }

            return panel;
        }
    }
}
