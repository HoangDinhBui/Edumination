using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.Courses
{
    public partial class CourseCardPanel : UserControl
    {
        public CourseCardPanel()
        {
            InitializeComponent();
        }

        // ====== API để bind dữ liệu từ DB vào card ======
        public void BindCourse(
            string title,
            string levelText,
            string bandRange,
            string description,
            string bullet1,
            string bullet2,
            string bullet3,
            string bullet4,
            string bottomInfo,
            string successRate,
            string priceText,
            string badgeText,
            Color badgeColor,
            Color backgroundColor)
        {
            lblTitle.Text = title;
            lblLevel.Text = levelText;
            lblBandRange.Text = bandRange;
            lblShortDescription.Text = description;

            lblBullet1.Text = "• " + bullet1;
            lblBullet2.Text = "• " + bullet2;
            lblBullet3.Text = "• " + bullet3;
            lblBullet4.Text = "• " + bullet4;

            lblBottomInfo.Text = bottomInfo;
            lblSuccessRate.Text = successRate;
            lblPrice.Text = priceText;

            lblBadge.Text = badgeText;
            lblBadge.BackColor = badgeColor;

            panelRoot.BackColor = backgroundColor;
        }

        private void lblShortDescription_Click(object sender, EventArgs e)
        {

        }
    }
}
