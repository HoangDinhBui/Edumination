using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace IELTS.UI.User.TestLibrary
{
    public partial class MockTestContainerPanel : UserControl
    {
        public List<string> Skills { get; set; } = new List<string>();
        public List<MockSectionItem> Items { get; set; } = new List<MockSectionItem>();

        public string TitleText => lblTitle.Text;

        public MockTestContainerPanel()
        {
            InitializeComponent();
        }

        public void SetTitle(string title)
        {
            lblTitle.Text = title;
        }

        // 2. MockTestContainerPanel.cs
        public void AddItem(string skill, string title, string taken, long sectionId = 0)
        {
            var item = new MockTestItemPanel();
            item.SetData(skill, title, taken, sectionId);
            panelItems.Controls.Add(item);
            Items.Add(new MockSectionItem
            {
                Skill = skill,
                DisplayText = title,
                TakenText = taken,
                SectionId = sectionId
            });
            if (!Skills.Contains(skill)) Skills.Add(skill);
        }
        public class MockSectionItem
        {
            public string Skill { get; set; }
            public string DisplayText { get; set; }
            public string TakenText { get; set; }
            public long SectionId { get; set; }
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
        public long SectionId { get; set; }   // mới
    }
}
