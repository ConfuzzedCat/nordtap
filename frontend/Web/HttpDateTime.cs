namespace frontend.Web;

public class HttpDateTime
{
    public bool IsSession { get; set; }
    public DateTime Date { get; set; }
    
    public static HttpDateTime Session { get; } = new HttpDateTime { IsSession = true, Date = DateTime.Now };

    public static HttpDateTime Parse(string expires)
    {
        return expires.Equals("session", StringComparison.InvariantCultureIgnoreCase) ? Session : new HttpDateTime { IsSession = false, Date = DateTime.Parse(expires) };
    }
}