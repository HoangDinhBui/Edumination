using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                catch { /* ignore */ }
            }

            // User name
            lblUserName.Text = string.IsNullOrWhiteSpace(_result.UserName)
                ? "Student"
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

            int y = 10;

            foreach (var part in _result.Parts)
            {
                // Part title
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
                y += 30;

                // Table 2 cột
                var table = new TableLayoutPanel
                {
                    Location = new Point(0, y),
                    Size = new Size(1300, 10),
                    ColumnCount = 2,
                    RowCount = 1,
                    AutoSize = true
                };
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

                // Cắt list câu thành 2 nửa
                var list = part.Questions.OrderBy(q => q.Number).ToList();
                int half = (int)Math.Ceiling(list.Count / 2.0);

                var leftPanel = BuildColumnPanel(list.Take(half).ToList());
                var rightPanel = BuildColumnPanel(list.Skip(half).ToList());

                table.Controls.Add(leftPanel, 0, 0);
                table.Controls.Add(rightPanel, 1, 0);

                panelAnswerKeys.Controls.Add(table);

                y += table.Height + 20;
            }
        }

        private Control BuildColumnPanel(System.Collections.Generic.List<QuestionReview> list)
        {
            var panel = new Panel
            {
                AutoSize = true,
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
