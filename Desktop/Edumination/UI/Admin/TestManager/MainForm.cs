using Edumination.WinForms.UI.Admin.TestManager;
//using IELTS.UI.Admin.MockTestManager;
using IELTS.UI.Admin.TestManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.Admin
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadAllMockTestControl();
        }

        private void LoadAllMockTestControl()
        {

            //AllMockTestControl control = new AllMockTestControl();
            //CreateTestPaperControl control = new CreateTestPaperControl();
            //AddReadingSectionControl control = new AddReadingSectionControl();
            ShowQuestionControl control = new ShowQuestionControl(1,1);

            //ShowSectionControl control = new ShowSectionControl(1);
            //ShowPassageControl control = new ShowPassageControl(2);
            //AllSkillsTestControl control = new AllSkillsTestControl();

            control.Dock = DockStyle.Fill;

            this.Controls.Clear();
            this.Controls.Add(control);
        }
    }
}
