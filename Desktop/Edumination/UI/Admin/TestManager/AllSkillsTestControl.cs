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
            flowPanelMockTests.BackColor = Color.FromArgb(245, 247, 250); // Màu nền xám nhạt kiểu Web

            foreach (var mock in mocks)
            {
                // === CARD PANEL (MOCK TEST CONTAINER) ===
                var mockCard = new Panel
                {
                    Width = 700,
                    AutoSize = true,
                    BackColor = Color.White,
                    Padding = new Padding(20),
                    Margin = new Padding(0, 0, 0, 20),
                    Cursor = Cursors.Default
                };
                // Tạo viền mảnh màu xám thay vì BorderStyle mặc định
                mockCard.Paint += (s, e) => {
                    ControlPaint.DrawBorder(e.Graphics, mockCard.ClientRectangle, Color.FromArgb(230, 230, 230), ButtonBorderStyle.Solid);
                };

                // === HEADER: TITLE & YEAR ===
                var lblYear = new Label
                {
                    Text = $"COLLECTION {mock.Year}",
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    ForeColor = Color.FromArgb(0, 120, 215), // Màu xanh thương hiệu
                    AutoSize = true,
                    Location = new Point(20, 15)
                };

                var lblTitle = new Label
                {
                    Text = mock.Title.ToUpper(),
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(45, 55, 72),
                    AutoSize = true,
                    Location = new Point(18, 35)
                };

                // === CONTAINER CHỨA CÁC BÀI TEST (PAPERS) ===
                var flpPapers = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    AutoSize = true,
                    Width = 660,
                    Location = new Point(20, 75),
                    Margin = new Padding(0, 15, 0, 0),
                    BackColor = Color.Transparent
                };

                if (mock.Papers.Count == 0)
                {
                    flpPapers.Controls.Add(new Label
                    {
                        Text = "   No test papers available yet.",
                        ForeColor = Color.DarkGray,
                        Font = new Font("Segoe UI", 9, FontStyle.Italic),
                        AutoSize = true
                    });
                }
                else
                {
                    foreach (var p in mock.Papers)
                    {
                        // === ITEM BUTTON (KIỂU LIST ITEM TRÊN WEB) ===
                        var btnPaper = new Button
                        {
                            Width = 640,
                            Height = 50,
                            Text = $"   📝  {p.Title} — {p.TestMonth}",
                            TextAlign = ContentAlignment.MiddleLeft,
                            Font = new Font("Segoe UI", 10),
                            ForeColor = Color.FromArgb(74, 85, 104),
                            BackColor = Color.FromArgb(250, 251, 252),
                            FlatStyle = FlatStyle.Flat,
                            Margin = new Padding(0, 5, 0, 5),
                            Tag = p.Id,
                            Cursor = Cursors.Hand
                        };
                        btnPaper.FlatAppearance.BorderSize = 1;
                        btnPaper.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);

                        // Hiệu ứng Hover kiểu Web
                        btnPaper.MouseEnter += (s, e) => {
                            btnPaper.BackColor = Color.FromArgb(237, 242, 247);
                            btnPaper.ForeColor = Color.FromArgb(0, 120, 215);
                        };
                        btnPaper.MouseLeave += (s, e) => {
                            btnPaper.BackColor = Color.FromArgb(250, 251, 252);
                            btnPaper.ForeColor = Color.FromArgb(74, 85, 104);
                        };

                        btnPaper.Click += (s, e) =>
                        {
                            long testPaperId = (long)((Button)s).Tag;
                            _testManagerControl.ShowSectionControl.PaperId = testPaperId;
                            _testManagerControl.ShowPanel(_testManagerControl.ShowSectionControl);
                        };

                        flpPapers.Controls.Add(btnPaper);
                    }
                }

                mockCard.Controls.Add(lblYear);
                mockCard.Controls.Add(lblTitle);
                mockCard.Controls.Add(flpPapers);

                flowPanelMockTests.Controls.Add(mockCard);
            }
        }

    }

}
