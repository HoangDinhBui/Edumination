using System.Drawing;
using System.Windows.Forms;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using IELTS.API;
using System.Diagnostics;

namespace IELTS.UI.User.Courses
{
    public partial class CourseCardPanel : UserControl
    {
        private long _courseId;
        private string _courseTitle;
        private int _priceVND;

        public CourseCardPanel()
        {
            InitializeComponent();
            btnExplore.Click += BtnExplore_Click;
        }

        // ====== API để bind dữ liệu từ DB vào card ======
        public void BindCourse(
            long courseId, // Thêm courseId
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
            int priceVND, // Thêm priceVND
            string badgeText,
            Color badgeColor,
            Color backgroundColor)
        {
            _courseId = courseId;
            _courseTitle = title;
            _priceVND = priceVND;

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
            
            // Cập nhật nút mua
            btnExplore.Text = "Mua ngay ➜";
        }

        private async void BtnExplore_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: Lấy userId từ session hiện tại
                long userId = 1; // Demo userId
                string userEmail = "student@test.com"; // Demo email

                var request = new CreatePaymentSessionRequestDTO
                {
                    CourseId = _courseId,
                    CourseTitle = _courseTitle,
                    PriceVND = _priceVND,
                    UserId = userId,
                    UserEmail = userEmail
                };

                using (var client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://localhost:5000/api/payment/create-session", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        dynamic result = JsonConvert.DeserializeObject(responseString);

                        if (result.success == true)
                        {
                            string sessionUrl = result.sessionUrl;
                            
                            // Mở trình duyệt để thanh toán
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = sessionUrl,
                                UseShellExecute = true
                            });

                            MessageBox.Show("Trình duyệt thanh toán đã được mở. Vui lòng hoàn tất thanh toán!", "Thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"Lỗi: {result.message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không thể kết nối đến server thanh toán.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblShortDescription_Click(object sender, EventArgs e)
        {

        }
    }
}
