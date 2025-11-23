using IELTS.BLL;
using IELTS.DAL;
using IELTS.DTO;
using IELTS.UI.Admin.TestManager;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Admin.TestManager
{
    public partial class AllSkillsTestControl : UserControl
    {
        private FlowLayoutPanel flowPanelMockTests;
        private readonly TestPaperBLL _bll;

        public event Func<long, Task> OnMockTestSelected;

        public AllSkillsTestControl()
        {
            InitializeComponent();
            InitializeCustomUI();

            _bll = new TestPaperBLL();
        }

        private void InitializeCustomUI()
        {
            flowPanelMockTests = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(10)
            };

            this.Controls.Add(flowPanelMockTests);
        }

        // -------- LOAD from DB --------
        public void LoadData()
        {
            var list = _bll.GetAll();
            DisplayTestPapers(list);
        }

        // -------- HIỂN THỊ BUTTON --------
        private void DisplayTestPapers(List<TestPaperDTO> papers)
        {
            flowPanelMockTests.Controls.Clear();

            foreach (var p in papers)
            {
                var btn = new Button
                {
                    Width = 600,
                    Height = 80,
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Tag = p.Id,
                    Text =
                        $"• {p.Title}\n" +
                        $"Published: {(p.IsPublished ? "Yes" : "No")}   " +
                        $"By: {p.CreatorFullName}   " +
                        $"At: {p.CreatedAt:dd/MM/yyyy HH:mm}"
                };

                btn.Click += async (s, e) =>
                {
                    UpdateReadingTestSectionForm form = new UpdateReadingTestSectionForm(p.Id);
                    form.ShowDialog();
                };

                flowPanelMockTests.Controls.Add(btn);
            }
        }
    }
}
