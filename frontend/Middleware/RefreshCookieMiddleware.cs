namespace frontend.Middleware;

public class RefreshCookieMiddleware
{
    private readonly RequestDelegate _next;

    public RefreshCookieMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(10)
            };
            context.Response.Cookies.Append("NordtapCookie", context.Request.Cookies["NordtapCookie"], cookieOptions);
        }
        await _next(context);
    }
}