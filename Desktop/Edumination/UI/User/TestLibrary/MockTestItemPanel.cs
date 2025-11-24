using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace IELTS.UI.User.TestLibrary
{
    public partial class MockTestItemPanel : UserControl
    {
        public string Skill { get; set; }   // ⭐ Lưu skill để filter

        public MockTestItemPanel()
        {
            InitializeComponent();
        }

        public void SetData(string skill, string title, string taken)
        {
            Skill = skill;       // ⭐ Lưu skill vào item
            lblTitle.Text = title;
            lblTaken.Text = taken;

            // Gán sự kiện Click cho panel và các label con
            this.Click += OnItemClicked;
            lblTitle.Click += OnItemClicked;
            lblTaken.Click += OnItemClicked;
        }

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
                    testForm = new IELTS.UI.User.TestTaking.WritingTest.WritingTest();
                    break;
                case "READING":
                    testForm = new IELTS.UI.User.TestTaking.ReadingTest.ReadingTest();
                    // MessageBox.Show("Reading test is coming soon!", "Info");
                    //parentForm.Show();
                    //return;
                    break;
                case "LISTENING":
                    // testForm = new IELTS.UI.User.TestTaking.ListeningTest.ListeningTest();
                    MessageBox.Show("Listening test is coming soon!", "Info");
                    parentForm.Show();
                    return;
                case "SPEAKING":
                    // testForm = new IELTS.UI.User.TestTaking.SpeakingTest.SpeakingTest();
                    MessageBox.Show("Speaking test is coming soon!", "Info");
                    parentForm.Show();
                    return;
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
        private static extern IntPtr CreateRoundRectRgn(int left, int top, int right, int bottom,
            int width, int height);
    }
}
