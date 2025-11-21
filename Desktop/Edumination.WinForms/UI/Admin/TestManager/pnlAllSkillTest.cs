using Edumination.WinForms.Dto.Papers;
using ReaLTaiizor.Forms; // Nếu bạn dùng SunnyUI, import namespace này
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Admin.TestManager
{
    public partial class pnlAllSkillTest : UserControl
    {
        private static readonly HttpClient client = new HttpClient();
        private string token = SessionManager.JwtToken;

        // FlowLayoutPanel để chứa button và title dynamically
        private FlowLayoutPanel flowPanelMockTests;

        // Event để thông báo cho pnlTestManager khi user click button
        public event Func<long, Task> OnMockTestSelected;

        public pnlAllSkillTest()
        {
            InitializeComponent();
            InitializeCustomUI();
        }

        private void InitializeCustomUI()
        {
            flowPanelMockTests = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown
            };

            this.Controls.Add(flowPanelMockTests);

            // Load dữ liệu async khi panel đã tạo
            this.Load += async (s, e) => await LoadMockTestsAsync();
        }

        // Load danh sách mock tests từ API
        public async Task LoadMockTestsAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    MessageBox.Show("Vui lòng đăng nhập lại!", "Lỗi");
                    return;
                }

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Trim());

                string url = "http://localhost:8081/api/v1/papers/mocktests";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Lỗi {response.StatusCode}: {error}", "Error");
                    return;
                }

                var mockTests = await response.Content.ReadFromJsonAsync<List<PaperLibraryResponseDto>>();

                if (mockTests == null || mockTests.Count == 0)
                {
                    MessageBox.Show("Không có mock test nào.", "Thông tin");
                    return;
                }

                DisplayMockTests(mockTests);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Error");
            }
        }

        // Hiển thị danh sách mock tests
        private void DisplayMockTests(List<PaperLibraryResponseDto> mockTests)
        {
            flowPanelMockTests.Controls.Clear();

            foreach (var group in mockTests)
            {
                // ► Title của nhóm
                Label titleLabel = new Label
                {
                    Text = group.Title,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding(10, 15, 10, 5)
                };
                flowPanelMockTests.Controls.Add(titleLabel);

                // ► Button cho từng item
                foreach (var item in group.Items)
                {
                    Button btn = new Button
                    {
                        Text = item.Name + (item.Taken == 1 ? " (Taken)" : ""),
                        Tag = item.Id,
                        Width = 350,
                        Height = 45,
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.White,
                        ForeColor = Color.FromArgb(33, 33, 33),
                        Font = new Font("Segoe UI", 10, FontStyle.Regular),
                        TextAlign = ContentAlignment.MiddleLeft,
                        Margin = new Padding(10, 5, 10, 5)
                    };
                    btn.FlatAppearance.BorderSize = 0;

                    // Hover effect
                    btn.MouseEnter += (s, e) => { btn.BackColor = Color.LightGray; };
                    btn.MouseLeave += (s, e) => { btn.BackColor = Color.White; };

                    // Click: gọi event tới pnlTestManager
                    btn.Click += async (s, e) =>
                    {
                        long id = (long)btn.Tag;
                        MessageBox.Show("id= " + id);
                        if (OnMockTestSelected != null)
                        {
                            await OnMockTestSelected.Invoke(id);
                        }
                    };

                    flowPanelMockTests.Controls.Add(btn);
                }
            }
        }
    }
}
