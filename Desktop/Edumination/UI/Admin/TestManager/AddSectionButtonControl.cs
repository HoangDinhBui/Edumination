using IELTS.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.Admin.TestManager
{
    public partial class AddSectionButtonControl : UserControl
    {
        private int TestPaperId;
        public int GetTestPaperId() { return TestPaperId; }
        public void SetTestPaperId( int id) { TestPaperId = id; }

        public AddSectionButtonControl()
        {
            InitializeComponent();
        }

        private void btnAddReadingSection_Click(object sender, EventArgs e)
        {
            AddTestSectionForm addTestSectionForm = new AddTestSectionForm(TestPaperId);
            addTestSectionForm.ShowDialog();
        }
    }
}
