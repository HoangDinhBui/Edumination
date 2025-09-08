using System;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Forms;

namespace Edumination.WinForms;

public partial class Form1 : Form
{
    private readonly Button _btnPingApi = new() { Text = "Ping API", AutoSize = true, Dock = DockStyle.Top };
    private readonly TextBox _output = new() { Multiline = true, Dock = DockStyle.Fill, ScrollBars = ScrollBars.Both };

    public Form1()
    {
        Text = "Edumination Desktop";
        Width = 800;
        Height = 600;

        Controls.Add(_output);
        Controls.Add(_btnPingApi);

        _btnPingApi.Click += async (_, __) =>
        {
            try
            {
                using var http = new HttpClient { BaseAddress = new Uri("http://localhost:8082/") };
                var json = await http.GetStringAsync("api/v1/courses?page=1&pageSize=5");
                var doc = JsonDocument.Parse(json);
                _output.Text = JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                _output.Text = ex.ToString();
            }
        };
    }
}
