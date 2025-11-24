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

        public void SetData(long paperId,
                            long sectionId,
                            string skill,
                            string title,
                            string taken)
        {
            PaperId = paperId;
            SectionId = sectionId;
            Skill = skill ?? "";

            lblTitle.Text = title;
            lblTaken.Text = taken;

            if (lblSkill != null)
                lblSkill.Text = skill;
        }

        private void OpenTest(object sender, EventArgs e)
        {
            Form parent = this.FindForm();

            Form target = null;

            switch (Skill.ToUpper())
            {
                case "READING":
                    target = new IELTS.UI.User.TestTaking.ReadingTest.ReadingTest(PaperId, SectionId);
                    break;

                case "LISTENING":
                  //  MessageBox.Show("Listening test chưa được triển khai.");
                  //  return;

                case "WRITING":
                    //MessageBox.Show("Writing test chưa được triển khai.");
                   // return;

                case "SPEAKING":
                   // MessageBox.Show("Speaking test chưa được triển khai.");
                   // return;

                default:
                    MessageBox.Show("Skill không hỗ trợ: " + Skill);
                    return;
            }

            parent?.Hide();
            target.Show();
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int left, int top, int right, int bottom,
            int width, int height);
    }
}
