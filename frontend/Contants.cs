using System.Text.Json;

namespace frontend;

public class Contants
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
    };
}