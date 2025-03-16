using System.Collections.Concurrent;
using DataAccess.Entities.Abstractions;

namespace DataAccess.Repositories.Abstractions;

public abstract class BaseRepository<T>()
    where T : IBaseEntity, new()
{
    protected readonly ConcurrentDictionary<string, T> Cache = [];

    protected virtual bool IsNeedToCache => false;

    protected virtual void AddToCache(IEnumerable<T> items)
    {
        if (IsNeedToCache == false)
        {
            return;
        }

        foreach (var item in items)
        {
            AddToCache(item);
        }
    }

    protected virtual bool AddToCache(T? item)
    {
        return item?.Id is not null && Cache.TryAdd(item.Id, item);
    }
}
