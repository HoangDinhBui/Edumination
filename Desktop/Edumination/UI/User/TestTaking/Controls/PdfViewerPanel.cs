using IELTS.UI.User.TestTaking.ReadingTest;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.TestTaking.Controls
{
    public partial class PdfViewerPanel : UserControl
    {
        public PdfViewerPanel()
        {
            InitializeComponent();
        }

        public void DisplayPart(ReadingPart part)
        {
            lblTitle.Text = part.PassageTitle;
            lblPassage.Text = part.PassageText;
        }
    }
}
