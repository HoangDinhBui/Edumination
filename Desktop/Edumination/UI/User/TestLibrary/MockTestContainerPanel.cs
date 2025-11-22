using IELTS.UI.User.TestTaking.ReadingTest;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using IELTS.BLL;

namespace IELTS.UI.User.TestLibrary
{
    public partial class MockTestContainerPanel : UserControl
    {
        public List<string> Skills { get; set; } = new List<string>();
        public List<MockSectionItem> Items { get; set; } = new List<MockSectionItem>();

        public string TitleText => lblTitle.Text;
        public long SectionId { get; set; }

        public MockTestContainerPanel()
        {
            InitializeComponent();
        }

        public void SetTitle(string title)
        {
            lblTitle.Text = title;
        }
        private void Item_Click(object sender, EventArgs e)
        {
            var item = sender as MockTestItemPanel;
            if (item == null) return;

            // Lấy section từ DB
            var section = _sectionBLL.GetSectionById(item.SectionId);

            // Mở form test
            var form = new ReadingTest(section);
            form.Show();
        }

        public void AddItem(string skill, string title, string taken)
        {
            var item = new MockTestItemPanel();
            item.SectionId = sectionId;
            item.SetData(skill, title, taken);

            // Thêm sự kiện click
            item.Click += Item_Click;

            panelItems.Controls.Add(item);

            Items.Add(new MockSectionItem
            {
                Skill = skill,
                DisplayText = title,
                TakenText = taken
            });

            if (!Skills.Contains(skill))
                Skills.Add(skill);
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom,
            int width, int height);
    }

    public class MockSectionItem
    {
        public string Skill { get; set; }
        public string DisplayText { get; set; }
        public string TakenText { get; set; }
    }
}
