using System.Text.Json;

namespace Nordtap;

public class Contants
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
    };
}