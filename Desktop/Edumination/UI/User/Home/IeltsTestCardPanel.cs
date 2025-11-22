using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.User.Home
{
    public partial class IeltsTestCardPanel : UserControl
    {
        public IeltsTestCardPanel()
        {
            InitializeComponent();
        }

        public Image Thumbnail
        {
            get => picThumb.Image;
            set => picThumb.Image = value;
        }

        public string Title
        {
            get => lblTitle.Text;
            set => lblTitle.Text = value;
        }

        public string Rating
        {
            get => lblRating.Text;
            set => lblRating.Text = value;
        }

        private void IeltsTestCardPanel_Load(object sender, EventArgs e)
        {

        }

        private void picThumb_Click(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
