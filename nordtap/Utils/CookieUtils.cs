using System;
using Nordtap.API;

namespace Nordtap.Utils;

public class CookieUtils
{
    public static string JoinCookies(Cookie[] requestCookies)
    {
        if (requestCookies.Length == 0)
        {
            return string.Empty;
        }
        string result = string.Empty;
        foreach (var cookie in requestCookies)
        {
            result +=  $"{cookie.Key}={cookie.Value}; ";
        }
        return result.TrimEnd(';');
    }
    
    public static void AssertCookieNotEmpty(Cookie cookie)
    {
        if (cookie.IsEmpty())
        {
            throw new Exception("Cookie is empty");
        }
    }
    public static void AssertCookieNotEmpty(string cookie)
    {
        if (string.IsNullOrEmpty(cookie) && cookie.Contains("NordtapCookie") == false)
        {
            throw new Exception("Cookie is empty");
        }
    }
}