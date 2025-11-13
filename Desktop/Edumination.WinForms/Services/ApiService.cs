using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Edumination.WinForms.Services
{
    class ApiService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public ApiService(string baseUrl = "http://localhost:8081")
        {
            _baseUrl = baseUrl;
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync($"{_baseUrl}{endpoint}", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<TResponse>(responseString);
            }

            return default;
        }

        public async Task<string> PostAsync<TRequest>(string endpoint, TRequest data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync($"{_baseUrl}{endpoint}", content);
            return await response.Content.ReadAsStringAsync();
        }

        public void Dispose() => _http.Dispose();
    }
}
