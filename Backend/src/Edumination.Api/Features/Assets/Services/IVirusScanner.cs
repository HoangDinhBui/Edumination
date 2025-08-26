using System.Threading.Tasks;

namespace Edumination.Services.Interfaces;

public interface IVirusScanner
{
    Task<bool> ScanAsync(string url);
}