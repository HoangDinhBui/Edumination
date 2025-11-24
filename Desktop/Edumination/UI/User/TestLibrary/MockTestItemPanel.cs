using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace IELTS.UI.User.TestLibrary
{
    public partial class MockTestItemPanel : UserControl
    {
        public long PaperId { get; set; }
        public long SectionId { get; set; }
        public string Skill { get; set; }

        public MockTestItemPanel()
        {
            InitializeComponent();

            this.Cursor = Cursors.Hand;

            this.Click += OpenTest;
            foreach (Control c in this.Controls)
                c.Click += OpenTest;
        }

        public void SetData(string skill, string title, string taken, long sectionId = 0)
        {
            Skill = skill;
            lblTitle.Text = title;
            lblTaken.Text = taken;
            SectionId = sectionId;   // lưu ID để truyền

            // gán sự kiện click
            this.Click += OnItemClicked;
            lblTitle.Click += OnItemClicked;
            lblTaken.Click += OnItemClicked;
        }
        public long SectionId { get; private set; }

        private void OnItemClicked(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm == null) return;

            // Ẩn form hiện tại (TestLibrary)
            parentForm.Hide();

            // Mở form bài thi tương ứng
            Form testForm = null;

            switch (Skill?.ToUpper())
            {
                case "WRITING":
                    testForm = new IELTS.UI.User.TestTaking.WritingTest.WritingTest(SectionId);
                    break;
                case "READING":
                    testForm = new IELTS.UI.User.TestTaking.ReadingTest.ReadingTest();
                    break;
                case "LISTENING":
                    testForm = new IELTS.UI.User.TestTaking.ListeningTest.ListeningTest();
                    break;
                case "SPEAKING":
                    testForm = new IELTS.UI.User.TestTaking.SpeakingTest.SpeakingTest();
                    break;
                default:
                    MessageBox.Show($"Test for skill '{Skill}' is not implemented yet.", "Info");
                    parentForm.Show();
                    return;
            }

            if (testForm != null)
            {
                testForm.Show();
                // Khi form bài thi đóng, hiện lại TestLibrary (nếu cần logic này, nhưng thường bài thi sẽ tự handle việc quay lại)
                // testForm.FormClosed += (s, args) => parentForm.Show(); 
            }
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int left, int top, int right, int bottom,
            int width, int height);
    }
}
