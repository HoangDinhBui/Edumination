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
    public partial class WritingAnswerPanel : UserControl
    {
        public WritingAnswerPanel()
        {
            InitializeComponent();
            txtEssay.TextChanged += TxtEssay_TextChanged;
        }

        private void TxtEssay_TextChanged(object sender, EventArgs e)
        {
            UpdateWordCount();
        }

        private void UpdateWordCount()
        {
            int count = txtEssay.Text
                .Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .Length;

            if (string.IsNullOrWhiteSpace(txtEssay.Text))
                count = 0;

            lblWordCount.Text = $"Words: {count}";
        }

        public void SetEssay(string text)
        {
            txtEssay.Text = text;
            UpdateWordCount();
        }

        public string GetEssay()
        {
            return txtEssay.Text;
        }
    }
}
