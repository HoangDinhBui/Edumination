using Edumination.WinForms.UI.Admin.TestManager;
using IELTS.BLL;
using IELTS.DTO;
using System;
using System.Drawing;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace IELTS.UI.Admin.TestManager
{
    public partial class ShowSectionControl : UserControl
    {
        private long _paperId;
        private readonly TestSectionBLL _bll = new();
        private TestManagerControl _testManagerControl;

        public TestManagerControl TestManagerControl
        {
            get => _testManagerControl;
            set => _testManagerControl = value;
        }


        // ===== PROPERTY =====
        public long PaperId
        {
            get => _paperId;
            set
            {
                if (_paperId != value)
                {
                    _paperId = value;

                    // chỉ load khi control đã sẵn sàng
                    if (!DesignMode && IsHandleCreated)
                        LoadSections();
                }
            }
        }

        // ===== CONSTRUCTOR =====
        public ShowSectionControl()
        {
            InitializeComponent();
        }
        public ShowSectionControl(TestManagerControl testManagerControl)
        {
            InitializeComponent();
            _testManagerControl = testManagerControl;
        }

        public ShowSectionControl(long paperId) : this()
        {
            PaperId = paperId; // auto load
        }

        // ===== LOAD DATA =====
        public void LoadSections()
        {
            flpSections.Controls.Clear();
            flpSections.BackColor = Color.FromArgb(248, 250, 252); // Nền xám rất nhạt kiểu chuyên nghiệp
            flpSections.Padding = new Padding(20);

            if (_paperId <= 0) return;

            var sections = _bll.GetSectionsByPaper(_paperId);

            if (sections.Count == 0)
            {
                Label lblEmpty = new Label
                {
                    Text = "No sections found for this test paper. Click 'Create New' to start.",
                    Font = new Font("Segoe UI", 11),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Margin = new Padding(20)
                };
                flpSections.Controls.Add(lblEmpty);
                return;
            }

            foreach (var section in sections)
            {
                flpSections.Controls.Add(CreateSectionItem(section));
            }
        }

        // ===== UI ITEM =====
        private Control CreateSectionItem(TestSectionDTO section)
        {
            // === Card Panel ===
            Panel pnl = new Panel
            {
                Width = 300,
                Height = 120, // Tăng chiều cao một chút cho thoáng
                Margin = new Padding(15),
                BackColor = Color.White,
                Tag = section.Id,
                Cursor = Cursors.Hand
            };

            // Xác định màu sắc theo kỹ năng
            Color skillColor = section.Skill.ToUpper() switch
            {
                "LISTENING" => Color.FromArgb(73, 182, 214),
                "READING" => Color.FromArgb(255, 107, 107),
                "WRITING" => Color.FromArgb(150, 123, 182),
                "SPEAKING" => Color.FromArgb(78, 205, 196),
                _ => Color.FromArgb(100, 116, 139)
            };

            // Vẽ viền và dải màu bên trái kiểu Card Web
            pnl.Paint += (s, e) =>
            {
                // Viền nhạt xung quanh
                ControlPaint.DrawBorder(e.Graphics, pnl.ClientRectangle, Color.FromArgb(230, 230, 230), ButtonBorderStyle.Solid);
                // Dải màu nhận diện kỹ năng ở lề trái (rộng 6px)
                using var brush = new SolidBrush(skillColor);
                e.Graphics.FillRectangle(brush, 0, 0, 6, pnl.Height);
            };

            // Label tiêu đề kỹ năng
            Label lblSkill = new Label
            {
                Text = section.Skill.ToUpper(),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = skillColor,
                Location = new Point(20, 20),
                AutoSize = true,
                Cursor = Cursors.Hand
            };

            // Label thông tin phụ (Thời gian)
            Label lblInfo = new Label
            {
                Text = $"⏱ Time: {(section.TimeLimitMinutes.HasValue ? section.TimeLimitMinutes + " mins" : "Unlimited")}",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(20, 55),
                AutoSize = true,
                Cursor = Cursors.Hand
            };

            // Hiệu ứng Hover
            pnl.MouseEnter += (s, e) => pnl.BackColor = Color.FromArgb(252, 253, 255);
            pnl.MouseLeave += (s, e) => pnl.BackColor = Color.White;

            // Gán sự kiện click cho tất cả
            pnl.Click += Section_Click;
            lblSkill.Click += Section_Click;
            lblInfo.Click += Section_Click;

            pnl.Controls.Add(lblSkill);
            pnl.Controls.Add(lblInfo);

            return pnl;
        }

        // ===== CLICK =====
        private void Section_Click(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            Panel pnl = c is Panel ? (Panel)c : (Panel)c.Parent;

            long sectionId = (long)pnl.Tag;

            //MessageBox.Show($"Open SectionId = {sectionId}");
            _testManagerControl.ShowPassageControl.SectionId = sectionId;
            _testManagerControl.ShowPanel(_testManagerControl.ShowPassageControl);
        }

        // ===== AUTO LOAD KHI HIỆN =====
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible && _paperId > 0)
                LoadSections();
        }

        private void btnCreateSection_Click(object sender, EventArgs e)
        {
            AddSectionForm addSectionForm = new AddSectionForm(this,_paperId);
            addSectionForm.ShowDialog();
        }
    }
}
