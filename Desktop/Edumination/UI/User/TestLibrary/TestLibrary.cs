using IELTS.BLL;
using IELTS.UI.User.Home;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace IELTS.UI.User.TestLibrary
{
    public partial class TestLibrary : Form
    {
        private readonly TestPaperBLL _paperBLL = new TestPaperBLL();
        private readonly TestSectionBLL _sectionBLL = new TestSectionBLL();

        private List<MockTestContainerPanel> allMockTests = new List<MockTestContainerPanel>();

        private string activeSkill = "All Skills";

        public TestLibrary()
        {
            InitializeComponent();
        }

        private void TestLibrary_Load(object sender, EventArgs e)
        {
            var nav = new UserNavbarPanel();
            nav.Dock = DockStyle.Fill;
            panelNavbar.Controls.Add(nav);

            BuildSkillButtons();
            LoadPapersFromDatabase();
        }

        private void CenterSkillButtons()
        {
            int totalWidth = 0;
            foreach (Control c in panelSkills.Controls)
                totalWidth += c.Width + c.Margin.Left + c.Margin.Right;

            panelSkills.Padding = new Padding((panelSkills.Width - totalWidth) / 2, 0, 0, 0);
        }

        private void BuildSkillButtons()
        {
            panelSkills.Controls.Clear();

            string[] skills = { "All Skills", "Listening", "Reading", "Writing", "Speaking" };

            foreach (var skill in skills)
            {
                var btn = new Button();
                btn.Text = skill;
                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btn.Size = new Size(180, 65);
                btn.Margin = new Padding(10, 0, 10, 0);

                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;

                btn.BackColor = skill == activeSkill ? Color.MidnightBlue : Color.White;
                btn.ForeColor = skill == activeSkill ? Color.White : Color.MidnightBlue;

                btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, 25, 25));

                btn.Click += (s, e) =>
                {
                    activeSkill = skill;
                    BuildSkillButtons();
                    FilterMockTests();
                };

                panelSkills.Controls.Add(btn);
            }

            CenterSkillButtons();
        }

        private void LoadPapersFromDatabase()
        {
            flowMain.Controls.Clear();
            allMockTests.Clear();

            var dt = _paperBLL.GetAllPublishedPapers();

            foreach (DataRow row in dt.Rows)
            {
                long paperId = Convert.ToInt64(row["Id"]);
                string paperTitle = row["Title"].ToString();

                var dtSection = _sectionBLL.GetSectionsByPaperId(paperId);

                if (dtSection.Rows.Count == 0)
                    continue;

                var container = new MockTestContainerPanel();
                container.SetTitle(paperTitle);

<<<<<<< HEAD
                // Loop qua từng section
                foreach (DataRow s in dtSection.Rows)
                {
                    string skill = s["Skill"].ToString().Trim().ToUpper();  // LISTENING, READING...
                    long sectionId = Convert.ToInt64(s["Id"]);
=======
                foreach (DataRow s in dtSection.Rows)
                {
                    long sectionId = Convert.ToInt64(s["Id"]);
                    string skill = s["Skill"].ToString().Trim().ToUpper();
>>>>>>> feat/tests

                    int? time = s["TimeLimitMinutes"] != DBNull.Value
                        ? Convert.ToInt32(s["TimeLimitMinutes"])
                        : (int?)null;

                    string testName = $"{skill} Test";
                    if (time.HasValue)
                        testName += $" – {time.Value} minutes";

<<<<<<< HEAD
                    // Truyền đúng thứ tự (skill, title, taken, sectionId) cho Writing
                    if (skill == "WRITING")
                        container.AddItem(skill, testName, "Available", sectionId);
                    else
                        container.AddItem(skill, testName, "Available");
=======
                    // ⭐ TRUYỀN ĐÚNG 5 THAM SỐ MỚI
                    container.AddItem(
                        paperId,
                        sectionId,
                        skill,
                        testName,
                        "Available"
                    );
>>>>>>> feat/tests
                }

                allMockTests.Add(container);
                flowMain.Controls.Add(container);
            }
        }

        private void FilterMockTests()
        {
            flowMain.Controls.Clear();

            if (activeSkill == "All Skills")
            {
                foreach (var m in allMockTests)
                    flowMain.Controls.Add(m);
                return;
            }

            string filterSkill = activeSkill.ToUpper();

            foreach (var container in allMockTests)
            {
                var matchedItems = container.Items
                    .Where(i => i.Skill.ToUpper() == filterSkill)
                    .ToList();

                if (matchedItems.Count == 0)
                    continue;

                var filteredContainer = new MockTestContainerPanel();
                filteredContainer.SetTitle(container.TitleText);

                foreach (var it in matchedItems)
                {
<<<<<<< HEAD
                    // Preserve SectionId khi sao chép
                    filteredContainer.AddItem(it.Skill, it.DisplayText, it.TakenText, it.SectionId);
=======
                    filteredContainer.AddItem(
                        it.PaperId,
                        it.SectionId,
                        it.Skill,
                        it.DisplayText,
                        it.TakenText
                    );
>>>>>>> feat/tests
                }

                flowMain.Controls.Add(filteredContainer);
            }

            if (flowMain.Controls.Count == 0)
            {
                flowMain.Controls.Add(new Label()
                {
                    Text = "No tests found for this skill.",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.Gray,
                    Margin = new Padding(20)
                });
            }
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom,
            int width, int height);

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string keyword = txtSearch.Text.Trim().ToLower();
                flowMain.Controls.Clear();

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    foreach (var m in allMockTests)
                        flowMain.Controls.Add(m);
                    return;
                }

                string[] parts = keyword.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var filtered = allMockTests
                    .Where(m =>
                    {
                        string title = m.TitleText.ToLower();
                        return parts.All(p => title.Contains(p));
                    })
                    .ToList();

                if (filtered.Count == 0)
                {
                    flowMain.Controls.Add(new Label()
                    {
                        Text = "No mock tests found.",
                        AutoSize = true,
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        ForeColor = Color.Gray,
                        Margin = new Padding(20)
                    });
                    return;
                }

                foreach (var f in filtered)
                    flowMain.Controls.Add(f);
            }
        }
    }
}
