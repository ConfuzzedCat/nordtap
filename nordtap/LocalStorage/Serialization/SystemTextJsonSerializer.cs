using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Nordtap.LocalStorage.Serialization;

internal class SystemTextJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _options;

    public SystemTextJsonSerializer(IOptions<JsonSerializerOptions> options)
    {
        _options = options.Value;
    }

    public SystemTextJsonSerializer(JsonSerializerOptions localStorageOptions)
    {
        _options = localStorageOptions;
    }

    public T? Deserialize<T>(string data) 
        => JsonSerializer.Deserialize<T>(data, _options);

    public string Serialize<T>(T data)
        => JsonSerializer.Serialize(data, _options);
}