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
        private void LoadSections()
        {
            flpSections.Controls.Clear();

            if (_paperId <= 0) return;

            var sections = _bll.GetSectionsByPaper(_paperId);

            foreach (var section in sections)
            {
                flpSections.Controls.Add(CreateSectionItem(section));
            }
        }

        // ===== UI ITEM =====
        private Control CreateSectionItem(TestSectionDTO section)
        {
            Panel pnl = new Panel
            {
                Width = 320,
                Height = 80,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                BackColor = Color.White,
                Tag = section.Id
            };

            Label lbl = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text =
                    $"{section.Skill}\n" +
                    $"Time: {(section.TimeLimitMinutes.HasValue ? section.TimeLimitMinutes + " mins" : "N/A")}"
            };

            pnl.Click += Section_Click;
            lbl.Click += Section_Click;

            pnl.Controls.Add(lbl);
            return pnl;
        }

        // ===== CLICK =====
        private void Section_Click(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            Panel pnl = c is Panel ? (Panel)c : (Panel)c.Parent;

            long sectionId = (long)pnl.Tag;

            MessageBox.Show($"Open SectionId = {sectionId}");
            _testManagerControl.ShowPassageControl.SectionId= sectionId;
            _testManagerControl.ShowPanel(_testManagerControl.ShowPassageControl);
        }

        // ===== AUTO LOAD KHI HIỆN =====
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible && _paperId > 0)
                LoadSections();
        }
    }
}
