using System;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.Home
{
    public partial class IeltsTestCardPanel : UserControl
    {
        public IeltsTestCardPanel()
        {
            InitializeComponent();

            // Gắn click cho toàn bộ card
            this.Click += OpenTestLibrary;
            foreach (Control c in this.Controls)
                c.Click += OpenTestLibrary;
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

        // Mở TestLibrary
        private void OpenTestLibrary(object sender, EventArgs e)
        {
            var form = new IELTS.UI.User.TestLibrary.TestLibrary();
            form.Show();

            Form parent = this.FindForm();
            parent?.Hide();
        }
    }
}
