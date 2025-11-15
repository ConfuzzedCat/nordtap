using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nordtap.LocalStorage.Serialization;

namespace Nordtap.LocalStorage;

internal class RamLocalStorageServiceImpl : ILocalStorageService
{
    private readonly ConcurrentDictionary<string, string> _dict;
    private readonly IJsonSerializer _jsonSerializer;

    public RamLocalStorageServiceImpl(IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
        _dict = new ConcurrentDictionary<string, string>();
    }

    public ValueTask ClearAsync(CancellationToken cancellationToken = default)
    {
        _dict.Clear();
        return new ValueTask();
    }

    public ValueTask<T> GetItemAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (_dict.TryGetValue(key, out var value))
        {
            return ValueTask.FromResult(_jsonSerializer.Deserialize<T>(value));
        }
        throw new Exception("Key not found");
    }

    public ValueTask<string> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default)
    {
        if (_dict.TryGetValue(key, out var value))
        {
            return ValueTask.FromResult(value);
        }
        throw new Exception("Key not found");
    }

    public ValueTask<string> KeyAsync(int index, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(_dict.Keys.ToList()[index]);
    }

    public ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(_dict.Keys.AsEnumerable());
    }

    public ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(_dict.ContainsKey(key));
    }

    public ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(_dict.Count);
    }

    public ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
    {
        if (_dict.TryRemove(key, out _))
        {
            return ValueTask.CompletedTask;
        }
        
        return ValueTask.FromException(new Exception($"Couldn't find item with key: {key}"));
    }

    public ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        foreach (var key in keys)
        {
            RemoveItemAsync(key, cancellationToken);
        }
        return ValueTask.CompletedTask;
    }

    public ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default)
    {
        if (_dict.TryAdd(key, _jsonSerializer.Serialize(data)))
        {
            return ValueTask.CompletedTask;
        }
        
        return ValueTask.FromException(new Exception($"Failed to add item with key: {key}")); 
    }

    public ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default)
    {
        if (_dict.TryAdd(key, data))
        {
            return ValueTask.CompletedTask;
        }
        
        return ValueTask.FromException(new Exception($"Failed to add item with key: {key}"));
    }

    public event EventHandler<ChangingEventArgs> Changing;
    public event EventHandler<ChangedEventArgs> Changed;
}