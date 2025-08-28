using System.Security.Cryptography;
using System.Text;

namespace Shared.Utils;

public class HashUtil
{
    public static string Sha256String(string value)
    {
        var sb = new StringBuilder();
        var enc = Encoding.UTF8;
        var result = SHA256.HashData(enc.GetBytes(value));

        foreach (var b in result)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}