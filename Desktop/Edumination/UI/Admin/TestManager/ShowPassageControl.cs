using Edumination.WinForms.UI.Admin.TestManager;
using IELTS.BLL;
using IELTS.DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.Admin.TestManager
{
    public partial class ShowPassageControl : UserControl
    {
        private long _sectionId;
        private readonly PassageBLL _bll = new();
        private TestManagerControl _testManagerCotrol;

        // ===== PROPERTY =====
        public long SectionId
        {
            get => _sectionId;
            set
            {
                _sectionId = value;
                if (this.Visible) LoadPassages(); // Chỉ load khi đang hiển thị
            }
        }

        // Ghi đè OnVisibleChanged để tự động load khi chuyển tab vào
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible && _sectionId > 0)
            {
                LoadPassages();
            }
        }

        // ===== CONSTRUCTOR =====
        public ShowPassageControl()
        {
            InitializeComponent();
        }

        public ShowPassageControl(TestManagerControl testManagerCotrol)
        {
            _testManagerCotrol = testManagerCotrol;
            InitializeComponent();

        }
        public ShowPassageControl(long sectionId) : this()
        {
            SectionId = sectionId;
        }

        // ===== LOAD DATA =====
        private void LoadPassages()
        {
            if (flpPassages == null) return;

            flpPassages.Controls.Clear();
            flpPassages.Padding = new Padding(30, 20, 30, 20);
            flpPassages.BackColor = Color.FromArgb(245, 247, 250);

            var passages = _bll.GetPassagesBySection(_sectionId);

            if (passages == null || passages.Count == 0)
            {
                Label lblNone = new Label
                {
                    Text = "No passages found. Please add a passage to continue.",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Margin = new Padding(10)
                };
                flpPassages.Controls.Add(lblNone);
                return;
            }

            foreach (var p in passages)
            {
                flpPassages.Controls.Add(CreatePassageItem(p));
            }
        }

        // ===== UI ITEM =====
        private Control CreatePassageItem(PassageDTO p)
        {
            // === Card Panel ===
            Panel pnl = new Panel
            {
                Width = flpPassages.Width - 60, // Trừa khoảng trống cho scrollbar
                Height = 80,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = Color.White,
                Tag = p.Id,
                Cursor = Cursors.Hand
            };

            // Vẽ viền và hiệu ứng Hover
            pnl.Paint += (s, e) => {
                ControlPaint.DrawBorder(e.Graphics, pnl.ClientRectangle, Color.FromArgb(230, 234, 238), ButtonBorderStyle.Solid);
            };

            // --- Số thứ tự (Badge) ---
            Label lblBadge = new Label
            {
                Text = p.Position.ToString(),
                Size = new Size(40, 40),
                Location = new Point(20, 20),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = Color.FromArgb(30, 41, 59),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            // Làm cho badge hình tròn (giả lập)
            lblBadge.Paint += (s, e) => {
                // Bạn có thể dùng Region để bo tròn thật, hoặc cứ để hình vuông bo nhẹ cũng rất đẹp
            };

            // --- Tiêu đề Passage ---
            Label lblTitle = new Label
            {
                Text = string.IsNullOrWhiteSpace(p.Title) ? $"Passage {p.Position} (No Title)" : p.Title,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 65, 85),
                Location = new Point(75, 18),
                Width = pnl.Width - 150,
                AutoSize = false,
                Height = 25,
                Cursor = Cursors.Hand
            };

            // --- Mô tả nhỏ bên dưới ---
            Label lblSub = new Label
            {
                Text = "Click to manage questions and content for this passage",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(148, 163, 184),
                Location = new Point(75, 45),
                AutoSize = true,
                Cursor = Cursors.Hand
            };

            // Hiệu ứng Hover
            pnl.MouseEnter += (s, e) => pnl.BackColor = Color.FromArgb(248, 250, 252);
            pnl.MouseLeave += (s, e) => pnl.BackColor = Color.White;

            // Sự kiện click
            pnl.Click += Passage_Click;
            lblBadge.Click += Passage_Click;
            lblTitle.Click += Passage_Click;
            lblSub.Click += Passage_Click;

            pnl.Controls.AddRange(new Control[] { lblBadge, lblTitle, lblSub });
            return pnl;
        }

        // ===== CLICK =====
        private void Passage_Click(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            Panel pnl = c is Panel ? (Panel)c : (Panel)c.Parent;

            long passageId = (long)pnl.Tag;

            //MessageBox.Show($"Open PassageId = {passageId}");
            _testManagerCotrol.ShowQuestionControl.PassageId = passageId;
            _testManagerCotrol.ShowPanel(_testManagerCotrol.ShowQuestionControl);
        }
    }
}
