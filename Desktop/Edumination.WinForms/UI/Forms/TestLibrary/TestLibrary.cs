using System;
using System.Drawing;
using System.Windows.Forms;
using Edumination.WinForms.UI.Forms.Home;

namespace Edumination.WinForms.UI.Forms.TestLibrary
{
    public partial class TestLibrary : Form
    {
        private string activeSkill = "All Skills";

        public TestLibrary()
        {
            InitializeComponent();
        }

        private void TestLibrary_Load(object sender, EventArgs e)
        {
            // NAVBAR
            var nav = new UserNavbarPanel();
            nav.Dock = DockStyle.Fill;
            panelNavbar.Controls.Add(nav);

            BuildSkillButtons();
            LoadMockCards();
        }

        private void BuildSkillButtons()
        {
            //panelSkillFilters.Controls.Clear(); // clear trước

            //string[] skills = { "All Skills", "Listening", "Reading", "Writing", "Speaking" };

            //foreach (var skill in skills)
            //{
            //    var btn = new Button();
            //    btn.Text = skill;
            //    btn.Height = 40;
            //    btn.Width = 150;
            //    btn.Margin = new Padding(10, 0, 0, 0);
            //    btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            //    btn.FlatStyle = FlatStyle.Flat;
            //    btn.FlatAppearance.BorderSize = 0;

            //    // Highlight active button
            //    btn.BackColor = skill == activeSkill ? Color.RoyalBlue : Color.WhiteSmoke;
            //    btn.ForeColor = skill == activeSkill ? Color.White : Color.Black;

            //    btn.Click += (s, e) =>
            //    {
            //        activeSkill = skill;
            //        BuildSkillButtons();
            //    };

            //    panelSkillFilters.Controls.Add(btn);
            //}
        }

        private void LoadMockCards()
        {
            panelMain.Controls.Clear();

            var container = new MockTestContainerPanel();
            container.Dock = DockStyle.Top;

            container.AddItem("Quarter 1 Listening Practice Test 1", "951,605 tests taken");
            container.AddItem("Quarter 1 Listening Practice Test 2", "951,605 tests taken");
            container.AddItem("Quarter 2 Listening Practice Test 1", "951,605 tests taken");
            container.AddItem("Quarter 2 Listening Practice Test 2", "951,605 tests taken");

            panelMain.Controls.Add(container);
        }

        private void btnAll_Click(object sender, EventArgs e)
        {

        }
    }
}
