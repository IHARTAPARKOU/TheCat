using System.Collections.Concurrent;

namespace Common.Collections;

public class ConcurrentHashSet<T> where T : notnull
{
    private readonly ConcurrentDictionary<T, byte> dictionary = new();
    public IEnumerable<T> Items => dictionary.Keys;

    public bool TryAdd(T item)
    {
        return dictionary.TryAdd(item, 0);
    }

    public void AddRange(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            TryAdd(item);
        }
    }

    public bool ContainsKey(T item)
    {
        return dictionary.ContainsKey(item);
    }

    public bool TryRemove(T item)
    {
        return dictionary.TryRemove(item, out _);
    }
}