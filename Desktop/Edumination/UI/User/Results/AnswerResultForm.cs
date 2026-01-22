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
            lblUserName.Text = string.IsNullOrWhiteSpace(_result.UserName) ? "Student" : _result.UserName;
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

            foreach (var part in _result.Parts)
            {
                // 1. Tạo Label tiêu đề cho từng Part (Ví dụ: Part 1: Question 1 - 10)
                UILabel lblPartHeader = new UILabel
                {
                    Text = part.PartName,
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(41, 69, 99),
                    AutoSize = true,
                    Margin = new Padding(0, 25, 0, 10) // Tạo khoảng cách giữa các Part
                };
                panelAnswerKeys.Controls.Add(lblPartHeader);

                // 2. Tạo một TableLayoutPanel để chia 2 cột cho các câu hỏi TRONG Part này
                TableLayoutPanel partGrid = new TableLayoutPanel
                {
                    ColumnCount = 2,
                    RowCount = 1,
                    Width = panelAnswerKeys.Width - 50, // Trừ hao khoảng cách scrollbar
                    AutoSize = true,
                    Margin = new Padding(0, 0, 0, 20)
                };
                // Chia 2 cột bằng nhau (50% - 50%)
                partGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                partGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

                // 3. Logic chia câu hỏi của Part thành 2 danh sách (Trái - Phải)
                var questions = part.Questions.OrderBy(q => q.Number).ToList();
                int mid = (int)Math.Ceiling(questions.Count / 2.0);

                FlowLayoutPanel leftCol = CreatePartColumn();
                FlowLayoutPanel rightCol = CreatePartColumn();

                for (int i = 0; i < questions.Count; i++)
                {
                    AnswerRowPanel row = new AnswerRowPanel();
                    row.Bind(questions[i]);

                    if (i < mid)
                        leftCol.Controls.Add(row);
                    else
                        rightCol.Controls.Add(row);
                }

                // 4. Thêm 2 cột vào grid của Part
                partGrid.Controls.Add(leftCol, 0, 0);
                partGrid.Controls.Add(rightCol, 1, 0);

                // 5. Thêm grid này vào panel chính
                panelAnswerKeys.Controls.Add(partGrid);
            }
        }

        private FlowLayoutPanel CreatePartColumn()
        {
            return new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };
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
