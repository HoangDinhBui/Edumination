using Edumination.Services.Interfaces;
using System.Threading.Tasks;

namespace Edumination.Services;

public class VirusScanner : IVirusScanner
{
    public async Task<bool> ScanAsync(string url)
    {
        // Triển khai thực tế với ClamAV hoặc dịch vụ bên thứ ba
        await Task.Delay(100); // Simulate scan
        return true;
    }
}