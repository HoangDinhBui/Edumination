using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.TestTaking.Controls
{
    public partial class TestNavBarPanel : UserControl
    {
        public event Action OnExitRequested;
        public event Action OnSubmitRequested;
        public TestNavBarPanel()
        {
            InitializeComponent();
        }
        public void SetTimeText(string text)
        {
            lblTimer.Text = text;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            OnExitRequested?.Invoke();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            OnSubmitRequested?.Invoke();
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            OnExitRequested?.Invoke();
        }

        private void btnSubmit_Click_1(object sender, EventArgs e)
        {
            OnSubmitRequested?.Invoke();
        }

        private void TestNavBarPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
