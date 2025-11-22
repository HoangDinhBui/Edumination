
using Sunny.UI; // Giữ lại Sunny.UI cho các controls khác như UILabel, UITabControlMenu, v.v.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Admin.TestManager
{
    public partial class TestManagerControl : UserControl
    {
        public Panel MainPanel => pnlMain; // pnlMain làm container chuẩn
        //private pnlTestInfo pnlTestInfo;
        private AllSkillsTestControl allSkillsTestControl;

        //public pnlTestInfo TestInfoPanel => pnlTestInfo;
        public AllSkillsTestControl AllSkillsControl => allSkillsTestControl;


        public TestManagerControl()
        {
            InitializeComponent();
            //pnlTestInfo = new pnlTestInfo();
            allSkillsTestControl = new AllSkillsTestControl();

            // Dock fill để chiếm toàn bộ pnlMain
            //pnlTestInfo.Dock = DockStyle.Fill;
            allSkillsTestControl.Dock = DockStyle.Fill;

            // Add vào pnlMain
            pnlMain.Controls.Add(allSkillsTestControl);
            //pnlMain.Controls.Add(pnlTestInfo);

            // Chỉ hiển thị pnlAllSkillTest lúc đầu
            allSkillsTestControl.Visible = true;
            //pnlTestInfo.Visible = false;

        }

        // Hàm tiện ích để show 1 UserControl, ẩn tất cả panel khác
        private void ShowPanel(UserControl uc)
        {
            foreach (Control c in pnlMain.Controls)
                c.Visible = false;

            uc.Visible = true;
            uc.BringToFront();
        }

        // Nếu muốn quay lại pnlAllSkillTest
        public void ShowAllSkillPanel()
        {
            ShowPanel(allSkillsTestControl);
        }


        private void btnAllSkills_Click(object sender, EventArgs e)
        {
            if (btnAllSkills.BackColor == SystemColors.Control)

            {
                MessageBox.Show("ok");
                btnAllSkills.BackColor = Color.FromArgb(80, 160, 255);
            }
            
            try
            {
                if (allSkillsTestControl != null)
                {
                    // Tải dữ liệu mới từ API
                    allSkillsTestControl.LoadData();

                    // Hiển thị pnlAllSkillTest, ẩn panel khác
                    ShowPanel(allSkillsTestControl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load All Skill Test: " + ex.Message, "Error");
            }
        }

    }
}
