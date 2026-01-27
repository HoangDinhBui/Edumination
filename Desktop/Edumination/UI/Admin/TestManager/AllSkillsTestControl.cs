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
        private readonly MockTestBLL _mockTestBLL = new MockTestBLL();
        private readonly TestManagerControl _testManagerControl;
        public event Func<long, Task> OnMockTestSelected;

        public AllSkillsTestControl(TestManagerControl testManagerControl)
        {
            _testManagerControl = testManagerControl;
            InitializeComponent();
            InitializeCustomUI();

            _bll = new TestPaperBLL();
            LoadData();
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
            var mocks = _mockTestBLL.GetAllMockTestsWithPapers();
            DisplayMockTests(mocks);
        }

        // -------- HIỂN THỊ BUTTON --------
        //private void DisplayTestPapers(List<TestPaperDTO> papers)
        //{
        //    flowPanelMockTests.Controls.Clear();

        //    foreach (var p in papers)
        //    {
        //        var btn = new Button
        //        {
        //            Width = 600,
        //            Height = 80,
        //            BackColor = Color.White,
        //            FlatStyle = FlatStyle.Flat,
        //            Tag = p.Id,
        //            Text =
        //                $"• {p.Title}\n" +
        //                $"Published: {(p.IsPublished ? "Yes" : "No")}   " +
        //                $"By: {p.CreatorFullName}   " +
        //                $"At: {p.CreatedAt:dd/MM/yyyy HH:mm}"
        //        };

        //        btn.Click += async (s, e) =>
        //        {
        //            UpdateReadingTestSectionForm form = new UpdateReadingTestSectionForm(p.Id);
        //            form.ShowDialog();
        //            //if (form)
        //            //{
        //            //    form.ShowDialog();
        //            //}

        //        };

        //        flowPanelMockTests.Controls.Add(btn);
        //    }
        //}

        private void DisplayMockTests(List<MockTestDTO> mocks)
        {
            flowPanelMockTests.Controls.Clear();

            foreach (var mock in mocks)
            {
                // === Panel của MOCK TEST ===
                var mockPanel = new Panel
                {
                    Width = 650,
                    AutoSize = true,
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(10),
                    Margin = new Padding(0, 0, 0, 15)
                };

                var lblTitle = new Label
                {
                    Text = $"Mock {mock.Year}: {mock.Title}",
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    AutoSize = true
                };

                mockPanel.Controls.Add(lblTitle);

                // === Flow chứa TEST PAPERS ===
                var flpPapers = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    AutoSize = true,
                    Margin = new Padding(0, 10, 0, 0)
                };

                if (mock.Papers.Count == 0)
                {
                    flpPapers.Controls.Add(new Label
                    {
                        Text = "⚠ Chưa có bài test",
                        ForeColor = Color.Gray,
                        AutoSize = true
                    });
                }
                else
                {
                    foreach (var p in mock.Papers)
                    {
                        var btn = new Button
                        {
                            Width = 600,
                            Height = 60,
                            Text = $"• {p.Title} ({p.TestMonth})",
                            Tag = p.Id
                        };

                        btn.Click += (s, e) =>
                        {
                            long testPaperId = (long)((Button)s).Tag;
                            MessageBox.Show($"Clicked on Test Paper ID: {testPaperId}");
                            _testManagerControl.ShowSectionControl.PaperId = testPaperId;
                            _testManagerControl.ShowPanel(_testManagerControl.ShowSectionControl);
                            //new UpdateReadingTestSectionForm(testPaperId).ShowDialog();
                        };

                        flpPapers.Controls.Add(btn);
                    }
                }

                mockPanel.Controls.Add(flpPapers);
                flowPanelMockTests.Controls.Add(mockPanel);
            }
        }

    }

}
