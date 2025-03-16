using System.Text.Json;
using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Services;
using Tests.Integration.Fixtures;

namespace Tests.Integration.Repositories;

public class CacheRepositoryTests(ConfigurationFixture configuration) : IClassFixture<ConfigurationFixture>
{
    private readonly ConfigurationService configurationService = new(configuration.Configuration);

    [Fact]
    public async Task GetAllAsyncEmptyWhenFileDoesNotExistTest()
    {
        var repository = new CacheRepository(configurationService);
        var result = await repository.GetAllAsync(CancellationToken.None);
        Assert.Empty(result);
    }

    [Fact]
    public async Task InsertAsyncTest()
    {
        var repository = new CacheRepository(configurationService);
        var item = GetTestItem();

        var result = await repository.InsertOrUpdateAsync(item, CancellationToken.None);
        var items = await repository.GetAllAsync(CancellationToken.None);

        Assert.True(result);
        Assert.Contains(item, items);
    }

    [Fact]
    public async Task InsertAsyncTryAddWrongItemTest()
    {
        var repository = new CacheRepository(configurationService);
        var item = new CachedCatBreedDto();

        var result = await repository.InsertOrUpdateAsync(item, CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsyncTest()
    {
        var repository = new CacheRepository(configurationService);
        var item = GetTestItem();

        var insert = await repository.InsertOrUpdateAsync(item, CancellationToken.None);
        var delete = await repository.DeleteAsync(item.Id!, CancellationToken.None);
        var items = await repository.GetAllAsync(CancellationToken.None);

        Assert.True(insert);
        Assert.True(delete);
        Assert.DoesNotContain(item, items);
    }

    [Fact]
    public async Task SaveChangesAsyncTest()
    {
        try
        {
            var repository = new CacheRepository(configurationService);
            var item = GetTestItem();

            var insert = await repository.InsertOrUpdateAsync(item, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);
            var fileContent = await File.ReadAllTextAsync(configurationService.TheCatFavorites);
            var savedItems = JsonSerializer.Deserialize<IReadOnlyList<CachedCatBreedDto>>(fileContent) ?? [];

            Assert.True(insert);
            Assert.True(savedItems.Count == 1);
            Assert.Equal(item.Id, savedItems[0].Id);
        }
        finally
        {
            DeleteTestFile(configurationService.TheCatFavorites);
        }
    }

    [Fact]
    public async Task InitializeAsyncLoadItemsFromFileTest()
    {
        try
        {
            var initialFavorites = new[] { GetTestItem() };
            await File.WriteAllTextAsync(configurationService.TheCatFavorites, JsonSerializer.Serialize(initialFavorites));

            var repository = new CacheRepository(configurationService);
            var items = (await repository.GetAllAsync(CancellationToken.None)).ToArray();

            Assert.True(items.Length == 1);
            Assert.Equal(initialFavorites[0].Id, items[0].Id);
        }
        finally
        {
            DeleteTestFile(configurationService.TheCatFavorites);
        }
    }

    private static CachedCatBreedDto GetTestItem()
    {
        return new CachedCatBreedDto()
        {
            Id = "test-item-id"
        };
    }

    private static void DeleteTestFile(string testFile)
    {
        if (File.Exists(testFile))
        {
            File.Delete(testFile);
        }
    }
}
