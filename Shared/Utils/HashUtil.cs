using System.Security.Cryptography;
using System.Text;

namespace Shared.Utils;

public static class HashUtil
{
    public static string Sha256String(string value)
    {
        var byteArray = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(byteArray);
    }
}