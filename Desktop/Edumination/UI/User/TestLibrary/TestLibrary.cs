//using System;
//using System.Drawing;
//using System.Runtime.InteropServices;
//using System.Windows.Forms;
//using IELTS.UI.User.Home;
//using IELTS.UI.User.TestLibrary;

//namespace IELTS.UI.User.TestLibrary
//{
//    public partial class TestLibrary : Form
//    {
//        private List<MockTestContainerPanel> allMockTests = new List<MockTestContainerPanel>();

//        private string activeSkill = "All Skills";

//        public TestLibrary()
//        {
//            InitializeComponent();
//        }

//        private void TestLibrary_Load(object sender, EventArgs e)
//        {
//            // Navbar load
//            var nav = new UserNavbarPanel();
//            nav.Dock = DockStyle.Fill;
//            panelNavbar.Controls.Add(nav);

//            BuildSkillButtons();
//            LoadMockData();
//        }

//        private void CenterSkillButtons()
//        {
//            int totalWidth = 0;

//            foreach (Control c in panelSkills.Controls)
//                totalWidth += c.Width + c.Margin.Left + c.Margin.Right;

//            panelSkills.Padding = new Padding((panelSkills.Width - totalWidth) / 2, 0, 0, 0);
//        }


//        private void BuildSkillButtons()
//        {
//            panelSkills.Controls.Clear();

//            string[] skills = { "All Skills", "Listening", "Reading", "Writing", "Speaking" };

//            foreach (var skill in skills)
//            {
//                var btn = new Button();
//                btn.Text = skill;
//                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
//                btn.Size = new Size(180, 65);
//                btn.Margin = new Padding(10, 0, 10, 0);

//                btn.FlatStyle = FlatStyle.Flat;
//                btn.FlatAppearance.BorderSize = 0;

//                btn.BackColor = skill == activeSkill ? Color.MidnightBlue : Color.White;
//                btn.ForeColor = skill == activeSkill ? Color.White : Color.MidnightBlue;

//                btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 25, 25));

//                btn.Click += (s, e) =>
//                {
//                    activeSkill = skill;
//                    BuildSkillButtons();
//                };

//                panelSkills.Controls.Add(btn);
//            }

//            // ⭐⭐ RẤT QUAN TRỌNG: căn giữa sau khi add nút
//            CenterSkillButtons();
//        }


//        private void LoadMockData()
//        {
//            flowMain.Controls.Clear();
//            allMockTests.Clear();

//            var c1 = new MockTestContainerPanel();
//            c1.SetTitle("IELTS Mock Test 2025");
//            c1.AddItem("Q1 Listening Test 1", "951,605 tests taken");
//            c1.AddItem("Q1 Listening Test 2", "951,605 tests taken");
//            flowMain.Controls.Add(c1);
//            allMockTests.Add(c1);

//            var c2 = new MockTestContainerPanel();
//            c2.SetTitle("IELTS Mock Test 2024 – Quarter Collection");
//            c2.AddItem("Q1 Reading Test", "501,200 tests taken");
//            c2.AddItem("Q3 Speaking Test", "393,001 tests taken");
//            flowMain.Controls.Add(c2);
//            allMockTests.Add(c2);
//        }

//        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
//        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom,
//            int width, int height);

//        private void txtSearch_TextChanged(object sender, EventArgs e)
//        {

//        }

//        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
//        {
//            if (e.KeyCode == Keys.Enter)
//            {
//                e.SuppressKeyPress = true;

//                string keyword = txtSearch.Text.Trim().ToLower();
//                flowMain.Controls.Clear();

//                if (string.IsNullOrWhiteSpace(keyword))
//                {
//                    foreach (var m in allMockTests)
//                        flowMain.Controls.Add(m);
//                    return;
//                }

//                // Tách tất cả từ khóa
//                string[] parts = keyword.Split(' ', StringSplitOptions.RemoveEmptyEntries);

//                // Search không theo thứ tự
//                var filtered = allMockTests
//                    .Where(m =>
//                    {
//                        string title = m.TitleText.ToLower();
//                        return parts.All(p => title.Contains(p));
//                    })
//                    .ToList();

//                if (filtered.Count == 0)
//                {
//                    flowMain.Controls.Add(new Label()
//                    {
//                        Text = "No mock tests found.",
//                        AutoSize = true,
//                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
//                        ForeColor = Color.Gray,
//                        Margin = new Padding(20)
//                    });
//                    return;
//                }

//                foreach (var f in filtered)
//                    flowMain.Controls.Add(f);
//            }
//        }


//        //private void panelContent_Paint(object sender, PaintEventArgs e)
//        //{

//        //}
//    }
//}
