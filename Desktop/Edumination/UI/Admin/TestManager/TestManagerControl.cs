
using IELTS.UI.Admin.TestManager;
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
        public AdminMainForm _adminMainForm;
        public AdminMainForm AdminMainForm
        {
            get => _adminMainForm;
            set => _adminMainForm = value;
        }
        private long userId;
        public long UserId
        {
            get => userId;
            set => userId = value;
        }

        private AllSkillsTestControl allSkillsTestControl { get ; set; }
        public AllSkillsTestControl AllSkillsTestControl
        {
            get => allSkillsTestControl;
            set => allSkillsTestControl = value;
        }
        
        private CreateTestPaperControl createTestPaperControl;
        private ShowSectionControl showSectionControl { get ; set; }
        public ShowSectionControl ShowSectionControl
        {
            get => showSectionControl;
            set => showSectionControl = value;
        }

        private ShowPassageControl showPassageControl { get; set; }
        public ShowPassageControl ShowPassageControl
        {
            get => showPassageControl;
            set => showPassageControl = value;
        }

        private ShowQuestionControl showQuestionControl { get; set; }
        public ShowQuestionControl ShowQuestionControl
        {
            get => showQuestionControl;
            set => showQuestionControl = value;
        }

        private NewShowQuestionCotrol newShowQuestionCotrol { get; set; }
        public NewShowQuestionCotrol NewShowQuestionCotrol
        {
            get => newShowQuestionCotrol;
            set => newShowQuestionCotrol = value;
        }
        public CreateTestPaperControl GetCreateTestPaperControl()
        {
            return createTestPaperControl;
        }
        public void SetCreatTestPaperControl(CreateTestPaperControl createTestPaperControl)
        {
            this.createTestPaperControl = createTestPaperControl;
        }
        private AddSectionButtonControl addSectionButtonControl;

        public AddSectionButtonControl GetAddSectionButtonControl()
        {
            return addSectionButtonControl;
        }

        public void SetAddSectionButtonControl(AddSectionButtonControl addSectionButtonControl)
        {
            this.addSectionButtonControl = addSectionButtonControl;
        }
        //public pnlTestInfo TestInfoPanel => pnlTestInfo;
        public AllSkillsTestControl AllSkillsControl => allSkillsTestControl;


        public TestManagerControl()
        {
            InitializeComponent();
            //pnlTestInfo = new pnlTestInfo();
            showSectionControl = new ShowSectionControl(this);
            showPassageControl = new ShowPassageControl(this);
            showQuestionControl = new ShowQuestionControl(this);
            newShowQuestionCotrol = new NewShowQuestionCotrol();
            allSkillsTestControl = new AllSkillsTestControl(this);
            createTestPaperControl = new CreateTestPaperControl();
            createTestPaperControl.TestManagerControl = this;            
            addSectionButtonControl = new AddSectionButtonControl();

            // Dock fill để chiếm toàn bộ pnlMain
            //pnlTestInfo.Dock = DockStyle.Fill;
            allSkillsTestControl.Dock = DockStyle.Fill;
            createTestPaperControl.Dock = DockStyle.Fill;
            addSectionButtonControl.Dock = DockStyle.Fill;
            showSectionControl.Dock = DockStyle.Fill;
            showPassageControl.Dock = DockStyle.Fill;
            showQuestionControl.Dock = DockStyle.Fill;
            newShowQuestionCotrol.Dock = DockStyle.Fill;

            // Add vào pnlMain
            pnlMain.Controls.Add(allSkillsTestControl);
            pnlMain.Controls.Add(createTestPaperControl);
            createTestPaperControl.SetTestPaperControl(this);
            pnlMain.Controls.Add(addSectionButtonControl);
            pnlMain.Controls.Add(showSectionControl);
            pnlMain.Controls.Add(showPassageControl);
            pnlMain.Controls.Add(showQuestionControl);
            pnlMain.Controls.Add(newShowQuestionCotrol);
            //pnlMain.Controls.Add(pnlTestInfo);

            // Chỉ hiển thị pnlAllSkillTest lúc đầu
            allSkillsTestControl.Visible = true;
            //pnlTestInfo.Visible = false;

        }

        // Hàm tiện ích để show 1 UserControl, ẩn tất cả panel khác
        public void ShowPanel(UserControl uc)
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
                //MessageBox.Show("ok");
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

        private void btnCreateTestPaper_Click(object sender, EventArgs e)
        {
            createTestPaperControl.ResetForm();
            ShowPanel(createTestPaperControl);
        }

        public void LoadNewShowQuestionControl(long sectionId, long passageId)
        {
            pnlMain.Controls.Clear();

            var uc = new NewShowQuestionCotrol
            {
                SectionId = sectionId,
                PassageId = passageId,
                Dock = DockStyle.Fill
            };

            pnlMain.Controls.Add(uc);
        }

    }
}
