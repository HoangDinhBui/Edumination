using System;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.TestTaking.Controls
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

        private void TestNavBarPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
