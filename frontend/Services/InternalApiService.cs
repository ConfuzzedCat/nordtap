using System.Net.Http.Headers;
using System.Text.Json;
using backend.Data.Entities;
using backend.Extensions.Models;
using frontend.Utils;
using frontend.Web;

namespace frontend.Services;

public class InternalApiService : IInternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<InternalApiService> _logger;
    private readonly string _baseUrl;

    public InternalApiService(HttpClient httpClient, ILogger<InternalApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _baseUrl = "http://localhost:5296";
    }
    
    public async Task<string> GetUsername(string cookie)
    {
        CookieUtils.AssertCookieNotEmpty(cookie);
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_baseUrl}/manage/info"),
            Headers =
            {
                { "Accept", "application/json" },
                { "Connection", "keep-alive" },
                { "Cookie", cookie }
            }
        };
        using (var response = await _httpClient.SendAsync(request))
        {
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("response: {body}",body);
                var returnValue = JsonSerializer.Deserialize<InfoResponse>(body, Contants.JsonSerializerOptions);
                return returnValue!.Username;
            }
            _logger.LogWarning("response status: {status}", response.StatusCode);
        }
        return string.Empty;
    }

    public async Task<string[]> GetRoles(string cookie)
    {
        CookieUtils.AssertCookieNotEmpty(cookie);
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_baseUrl}/manage/info"),
            Headers =
            {
                { "Accept", "application/json" },
                { "Connection", "keep-alive" },
                { "Cookie", cookie }
            }
        };
        using (var response = await _httpClient.SendAsync(request))
        {
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("response: {body}",body);
                return JsonSerializer.Deserialize<InfoResponse>(body, Contants.JsonSerializerOptions)!.Roles;
            }
            _logger.LogWarning("response status: {status}", response.StatusCode);
        }
        return [];
    }

    public async Task<InfoResponse?> GetUserInfo(string cookie)
    {
        CookieUtils.AssertCookieNotEmpty(cookie);
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_baseUrl}/manage/info"),
            Headers =
            {
                { "Accept", "application/json" },
                { "Connection", "keep-alive" },
                { "Cookie", cookie }
            }
        };
        using (var response = await _httpClient.SendAsync(request))
        {
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("response: {body}",body);

                return JsonSerializer.Deserialize<InfoResponse>(body, Contants.JsonSerializerOptions);
            }
            _logger.LogWarning("response status: {status}", response.StatusCode);
        }
        return null;
    }

    public async Task<InfoResponse?> PostUserInfo(string cookie, InfoRequest request)
    {
        CookieUtils.AssertCookieNotEmpty(cookie);
        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{_baseUrl}/manage/info"),
            Headers =
            {
                { "Accept", "application/json" },
                { "Connection", "keep-alive" },
            },
            Content = new StringContent(JsonSerializer.Serialize(request))
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            }
        };
        using (var response = await _httpClient.SendAsync(httpRequest))
        {
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("response: {body}",body);
                return JsonSerializer.Deserialize<InfoResponse>(body, Contants.JsonSerializerOptions);
            }
            _logger.LogWarning("response status: {status}", response.StatusCode);
        }
        return null;
    }

    public async Task<Cookie> PostLogin(LoginRequest request)
    {
        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{_baseUrl}/login?useCookies=true&useSessionCookies=true"),
            Headers =
            {
                { "Accept", "application/json" },
                { "Connection", "keep-alive" },
            },
            Content = new StringContent(JsonSerializer.Serialize(request))
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            }
        };
        using (var response = await _httpClient.SendAsync(httpRequest))
        {
            if (response.IsSuccessStatusCode)
            {
                if (response.Headers.TryGetValues("Set-Cookie", out var values))
                {
                    string cookieStr = values.First();
                    return Cookie.Parse(cookieStr);
                }
            }
            _logger.LogWarning("response status: {status}", response.StatusCode);
        }
        return Cookie.Empty;
    }

    public async Task<bool> PostRegister(RegisterRequest request)
    {
        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{_baseUrl}/register"),
            Headers =
            {
                { "Accept", "application/json" },
                { "Connection", "keep-alive" },
            },
            Content = new StringContent(JsonSerializer.Serialize(request))
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            }
        };
        using (var response = await _httpClient.SendAsync(httpRequest))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            _logger.LogWarning("response status: {status}", response.StatusCode);
        }
        return false;
    }

    public async Task<InviteCode?> GetInviteCode(string cookie)
    {
        CookieUtils.AssertCookieNotEmpty(cookie);
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_baseUrl}/InviteCodeGenerator"),
            Headers =
            {
                { "Accept", "application/json" },
                { "Connection", "keep-alive" },
                { "Cookie", cookie }
            }
        };
        using (var response = await _httpClient.SendAsync(request))
        {
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("response: {body}",body);
                return JsonSerializer.Deserialize<InviteCode>(body, Contants.JsonSerializerOptions);
            }
            _logger.LogWarning("response status: {status}", response.StatusCode);
        }
        return null;
    }
}