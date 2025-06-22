using System.Reflection;

namespace frontend.Web;

public struct Cookie
{
    public static Cookie Empty => new Cookie();
    public string Key { get; set; }
    public string Value { get; set; }
    public CookieOptions Options { get; set; }
    private string _strValue { get; set; }
    
    public Cookie()
    {
    }

    public static Cookie Parse(string _CookieString)
    {
        string _cstr = _CookieString;
        string[] _CookieStringSplit = _CookieString.Split(';');
        string _Key = _CookieStringSplit[0].Split('=')[0].Trim();
        string _Value = _CookieStringSplit[0].Split('=')[1].Trim();
        string _Path = "/", _Expires = "session";
        bool _Secure = false, _HttpOnly = false;
        SameSiteMode _sameSite = SameSiteMode.Unspecified;
        for (int i = 1; i < _CookieStringSplit.Length; i++)
        {
            var part = _CookieStringSplit[i].Trim();
            if(part.StartsWith("expires=", StringComparison.InvariantCultureIgnoreCase))
            {
                _Expires = part.Substring("expires=".Length);
            }

            if (part.StartsWith("path=", StringComparison.InvariantCultureIgnoreCase))
            {
                _Path = part.Substring("path=".Length);
            }

            if (part.StartsWith("samesite=", StringComparison.InvariantCultureIgnoreCase))
            {
                var _sameSiteStr = part.Substring("samesite=".Length);
                switch (_sameSiteStr.ToLower())
                {
                    case "strict":
                        _sameSite = SameSiteMode.Strict;
                        break;
                    case "lax":
                        _sameSite = SameSiteMode.Lax;
                        break;
                }
            }

            if (part.StartsWith("secure", StringComparison.InvariantCultureIgnoreCase))
            {
                _Secure = true;
            }

            if (part.StartsWith("httpOnly", StringComparison.InvariantCultureIgnoreCase))
            {
                _HttpOnly = true;
            }
        }
        var b_dtoExpires = DateTimeOffset.TryParse(_Expires, out var dtoExpires);
        return new Cookie
        {
            Key = _Key,
            Value = _Value,
            Options = new CookieOptions
            {
                Secure = _Secure,
                HttpOnly = _HttpOnly,
                SameSite = _sameSite,
                Path = _Path,
                Expires = b_dtoExpires ? dtoExpires.UtcDateTime : null
            },
            _strValue = _cstr
        };
    }

    public override string ToString()
    {
        return _strValue;
    }

    public bool IsEmpty()
    {
        return string.IsNullOrWhiteSpace(_strValue) && string.IsNullOrWhiteSpace(Key);
    }
}   