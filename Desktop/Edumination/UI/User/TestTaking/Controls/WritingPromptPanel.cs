using IELTS.UI.User.TestTaking.WritingTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.User.TestTaking.Controls
{
    public partial class WritingPromptPanel : UserControl
    {
        public WritingPromptPanel()
        {
            InitializeComponent();
        }

        public void DisplayTask(WritingTask task)
        {
            lblTitle.Text = task.Title;
            txtPrompt.Text = task.Prompt;
        }
    }
}
