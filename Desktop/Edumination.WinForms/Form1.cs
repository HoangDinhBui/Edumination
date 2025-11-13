using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

// Namespace cho WinForms
namespace Edumination.WinForms
{
    // Class TestPaper đơn giản để deserialize JSON từ API
    public class TestPaper
    {
        public long Id { get; set; }
        public string Title { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }

    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ListBox _listBoxPapers = new ListBox { Dock = DockStyle.Top, Height = 300 };
        private readonly TextBox _output = new TextBox { Multiline = true, Dock = DockStyle.Fill, ScrollBars = ScrollBars.Both };

        public Form1()
        {
            Text = "Edumination Desktop";
            Width = 800;
            Height = 600;

            Controls.Add(_output);
            Controls.Add(_listBoxPapers);

            Load += Form1_Load;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string token = "JWT_TOKEN_CỦA_BẠN"; // Thay bằng token thực
                var papers = await GetPapersAsync(token);

                if (papers.Count == 0)
                {
                    _output.Text = "Không lấy được dữ liệu Paper. Kiểm tra token/API.";
                    return;
                }

                _listBoxPapers.DataSource = papers;
                _listBoxPapers.DisplayMember = "Title";
                _listBoxPapers.ValueMember = "Id";

                _output.Text = $"Đã load {papers.Count} Paper từ API.";
            }
            catch (Exception ex)
            {
                _output.Text = $"Lỗi khi load Paper:\r\n{ex}";
            }
        }

        private async Task<List<TestPaper>> GetPapersAsync(string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("https://localhost:8081/api/v1/papers?status=PUBLISHED");

                if (!response.IsSuccessStatusCode)
                {
                    _output.Text = $"API trả lỗi: {response.StatusCode}";
                    return new List<TestPaper>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var papers = JsonSerializer.Deserialize<List<TestPaper>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return papers ?? new List<TestPaper>();
            }
            catch (Exception ex)
            {
                _output.Text = $"Exception khi gọi API:\r\n{ex}";
                return new List<TestPaper>();
            }
        }
    }
}
