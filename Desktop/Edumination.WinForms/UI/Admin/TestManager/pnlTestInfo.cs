using Edumination.WinForms.Dto.Papers;
using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Admin.TestManager
{
    public partial class pnlTestInfo : UserControl
    {
        private static readonly HttpClient client = new HttpClient();
        private FlowLayoutPanel flowPanelSections;

        public pnlTestInfo()
        {
            InitializeComponent();
            InitializeCustomUI();
        }

        private void InitializeCustomUI()
        {
            flowPanelSections = new FlowLayoutPanel();
            flowPanelSections.Dock = DockStyle.Fill;
            flowPanelSections.AutoScroll = true;
            flowPanelSections.WrapContents = false;
            flowPanelSections.FlowDirection = FlowDirection.TopDown;
            this.Controls.Add(flowPanelSections);
        }

        // Load chi tiết Paper theo ID
        public async Task LoadPaperAsync(long testID)
        {
            try
            {
                string token = SessionManager.JwtToken;
                if (string.IsNullOrEmpty(token))
                {
                    MessageBox.Show("Vui lòng đăng nhập lại!", "Lỗi");
                    return;
                }

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Trim());

                string url = $"http://localhost:8081/api/v1/papers/{testID}";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi {response.StatusCode}: {error}", "Error");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var paper = System.Text.Json.JsonSerializer.Deserialize<DetailedPaperDto>(json);

                if (paper == null)
                {
                    MessageBox.Show("Không đọc được dữ liệu đề thi.", "Error");
                    return;
                }

                DisplayPaper(paper);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Error");
            }
        }

        // Hiển thị dữ liệu Paper lên FlowLayoutPanel
        private void DisplayPaper(DetailedPaperDto paper)
        {
            if (paper == null) return;

            // Cập nhật label đã có từ Designer
            lblTitle.Text = paper.Title;
            lblStatus.Text = $"Status: {paper.Status}";
            lblCreatedAt.Text = $"Created At: {paper.CreatedAt:yyyy-MM-dd HH:mm}";

            // Xóa các section cũ
            flowSections.Controls.Clear();

            if (paper.Sections != null && paper.Sections.Count > 0)
            {

                foreach (var sec in paper.Sections)
                {
                    MessageBox.Show($"Found Section ID: {sec.Id}, SectionNo: {sec.SectionNo}, Skill: {sec.Skill}");
                    // Section header
                    Sunny.UI.UILabel lblSection = new Sunny.UI.UILabel
                    {
                        Text = $"Section {sec.SectionNo} - Skill: {sec.Skill}",
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        AutoSize = true,
                        Padding = new Padding(5)
                    };
                    flowSections.Controls.Add(lblSection);

                    // Section info
                    Sunny.UI.UILabel lblInfo = new Sunny.UI.UILabel
                    {
                        Text = $"Time Limit: {sec.TimeLimitSec} sec, Published: {sec.IsPublished}",
                        Font = new Font("Segoe UI", 10, FontStyle.Regular),
                        AutoSize = true,
                        Padding = new Padding(15, 0, 0, 5)
                    };
                    flowSections.Controls.Add(lblInfo);

                    // Passages (nếu có)
                    if (sec.Passages != null && sec.Passages.Count > 0)
                    {
                        foreach (var passage in sec.Passages)
                        {
                            Sunny.UI.UILabel lblPassage = new Sunny.UI.UILabel
                            {
                                Text = passage.Title ?? "Passage", // Hoặc nội dung khác
                                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                                AutoSize = true,
                                Padding = new Padding(20, 0, 0, 5)
                            };
                            flowSections.Controls.Add(lblPassage);
                        }
                    }
                }
            }
            else
            {
                Sunny.UI.UILabel lblEmpty = new Sunny.UI.UILabel
                {
                    Text = "No sections found.",
                    Font = new Font("Segoe UI", 12, FontStyle.Italic),
                    AutoSize = true,
                    Padding = new Padding(5)
                };
                flowSections.Controls.Add(lblEmpty);
            }
        }

    }
}
