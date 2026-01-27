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

            var passages = _bll.GetPassagesBySection(_sectionId);

            foreach (var p in passages)
            {
                flpPassages.Controls.Add(CreatePassageItem(p));
            }
        }

        // ===== UI ITEM =====
        private Control CreatePassageItem(PassageDTO p)
        {
            Panel pnl = new Panel
            {
                Width = flpPassages.Width - 30,
                Height = 90,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                BackColor = Color.White,
                Tag = p.Id
            };

            Label lbl = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Text =
                    $"Passage {p.Position}\n" +
                    $"{(string.IsNullOrWhiteSpace(p.Title) ? "No title" : p.Title)}"
            };

            pnl.Click += Passage_Click;
            lbl.Click += Passage_Click;

            pnl.Controls.Add(lbl);
            return pnl;
        }

        // ===== CLICK =====
        private void Passage_Click(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            Panel pnl = c is Panel ? (Panel)c : (Panel)c.Parent;

            long passageId = (long)pnl.Tag;

            MessageBox.Show($"Open PassageId = {passageId}");
            _testManagerCotrol.ShowQuestionControl.PassageId = passageId;
            _testManagerCotrol.ShowPanel(_testManagerCotrol.ShowQuestionControl);
        }
    }
}
