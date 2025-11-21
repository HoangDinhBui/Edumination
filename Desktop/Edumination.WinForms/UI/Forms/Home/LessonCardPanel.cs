using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.Home
{
    public partial class LessonCardPanel : UserControl
    {
        public LessonCardPanel()
        {
            InitializeComponent();
        }

        public Image Thumbnail { get => picThumb.Image; set => picThumb.Image = value; }
        public string Category { get => lblCategory.Text; set => lblCategory.Text = value; }
        public string TitleText { get => lblTitle.Text; set => lblTitle.Text = value; }
        public string TimeText { get => lblTime.Text; set => lblTime.Text = value; }
        public string Attending { get => lblAttending.Text; set => lblAttending.Text = value; }

        private void LessonCardPanel_Load(object sender, EventArgs e)
        {

        }

        private void lblAttending_Click(object sender, EventArgs e)
        {

        }
    }
}
