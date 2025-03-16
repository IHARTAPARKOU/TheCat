using System.Text.Json;
using Common.Collections;
using DataAccess.Repositories.Abstractions;

namespace DataAccess.Repositories;

public class FavoritesRepository : IFavoritesRepository
{
    private readonly string filePath;
    private readonly ConcurrentHashSet<string> cache = new();
    private readonly Task initializationTask;

    public FavoritesRepository(string filePath)
    {
        this.filePath = filePath;
        initializationTask = InitializeAsync(filePath);
    }

    private async Task InitializeAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        if (File.Exists(filePath) == false)
        {
            return;
        }

        var json = await File.ReadAllTextAsync(filePath);
        var items = JsonSerializer.Deserialize<IEnumerable<string>>(json) ?? [];

        cache.AddRange(items);
    }

    public async Task<IEnumerable<string>> GetAllAsync(CancellationToken token)
    {
        await initializationTask.WaitAsync(token);
        return cache.Items;
    }

    public async Task<bool> InsertAsync(string id, CancellationToken token)
    {
        await initializationTask.WaitAsync(token);
        return cache.TryAdd(id);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken token)
    {
        await initializationTask.WaitAsync(token);
        return cache.TryRemove(id);
    }

    public async Task SaveChangesAsync(CancellationToken token)
    {
        await initializationTask.WaitAsync(token);
        var json = JsonSerializer.Serialize(cache.Items);
        await File.WriteAllTextAsync(filePath, json, token);
    }
}
