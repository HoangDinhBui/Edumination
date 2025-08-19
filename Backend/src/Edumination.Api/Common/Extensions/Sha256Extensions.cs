using System.Security.Cryptography;
using System.Text;

namespace Edumination.Api.Common.Extensions;

public static class Sha256Extensions
{
    public static string Sha256Hex(this string raw)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(bytes).ToLowerInvariant(); // 64 chars
    }
}
