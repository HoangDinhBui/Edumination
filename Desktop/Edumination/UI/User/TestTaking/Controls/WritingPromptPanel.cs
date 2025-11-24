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

        // Display the writing task (title + prompt)
        public void DisplayTask(WritingTask task)
        {
            // lblTitle and txtPrompt are defined in the Designer file
            lblTitle.Text = task.Title;
            var formatted = task.Prompt?.Replace("\\n", Environment.NewLine) ?? string.Empty;
            txtPrompt.Text = formatted; // show the prompt in the read‑only UITextBox
        }
    }
}
