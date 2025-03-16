using System.Text.Json;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using DataAccess.Services.Abstractions;

namespace DataAccess.Repositories;

public class CacheRepository : BaseRepository<CachedCatBreedDto>, ICacheRepository
{
    private readonly string filePath;
    private readonly Task initializationTask;

    protected override bool IsNeedToCache => true;

    public CacheRepository(IConfigurationService configurationService)
    {
        filePath = configurationService.TheCatFavorites;
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
        var items = JsonSerializer.Deserialize<IEnumerable<CachedCatBreedDto>>(json) ?? [];

        AddToCache(items);
    }

    public bool TryGetLocalValue(string id, out CachedCatBreedDto? item) => Cache.TryGetValue(id, out item);

    public async Task<IEnumerable<CachedCatBreedDto>> GetAllAsync(CancellationToken token)
    {
        await initializationTask.WaitAsync(token);
        return Cache.Values;
    }

    public async Task<bool> InsertOrUpdateAsync(CachedCatBreedDto item, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(item.Id))
        {
            return false;
        }

        await initializationTask.WaitAsync(token);

        Cache[item.Id] = item;
        return true;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken token)
    {
        await initializationTask.WaitAsync(token);
        return Cache.TryRemove(id, out _);
    }

    public async Task SaveChangesAsync(CancellationToken token)
    {
        await initializationTask.WaitAsync(token);

        if (Cache.IsEmpty)
        {
            return;
        }

        var json = JsonSerializer.Serialize(Cache.Values);
        await File.WriteAllTextAsync(filePath, json, token);
    }
}
