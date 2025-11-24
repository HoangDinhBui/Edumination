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

        /// <summary>
        /// Thêm 1 SECTION vào danh sách
        /// </summary>
        public void AddItem(long paperId,
                            long sectionId,
                            string skill,
                            string title,
                            string taken)
        {
            var item = new MockTestItemPanel();
            item.SetData(paperId, sectionId, skill, title, taken);

            panelItems.Controls.Add(item);

            // Lưu vào danh sách container để filter
            Items.Add(new MockSectionItem
            {
                PaperId = paperId,
                SectionId = sectionId,
                Skill = skill,
                DisplayText = title,
                TakenText = taken
            });

            if (!Skills.Contains(skill))
                Skills.Add(skill);
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int left, int top, int right, int bottom,
            int width, int height);
    }

    public class MockSectionItem
    {
        public long PaperId { get; set; }
        public long SectionId { get; set; }
        public string Skill { get; set; }
        public string DisplayText { get; set; }
        public string TakenText { get; set; }
    }
}
