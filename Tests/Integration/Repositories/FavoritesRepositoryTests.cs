using System.Text.Json;
using DataAccess.Repositories;

namespace Tests.Integration.Repositories;

public class FavoritesRepositoryTests
{
    private readonly string testFilePath = Path.Combine(Directory.GetCurrentDirectory(), "testFavorites.json");

    [Fact]
    public async Task InitializeAsyncThrowWhenFilePathIsNullTest()
    {
        var repository = new FavoritesRepository(string.Empty);
        await Assert.ThrowsAsync<ArgumentNullException>(() => repository.GetAllAsync(CancellationToken.None));
    }

    [Fact]
    public async Task GetAllAsyncEmptyWhenFileDoesNotExistTest()
    {
        var repository = new FavoritesRepository(testFilePath);
        var result = await repository.GetAllAsync(CancellationToken.None);
        Assert.Empty(result);
    }

    [Fact]
    public async Task InsertAsyncTest()
    {
        var repository = new FavoritesRepository(testFilePath);
        var id = "test-item";

        var result = await repository.InsertAsync(id, CancellationToken.None);
        var items = await repository.GetAllAsync(CancellationToken.None);

        Assert.True(result);
        Assert.Contains(id, items);
    }

    [Fact]
    public async Task InsertAsyncTryAddDuplicateItemTest()
    {
        var repository = new FavoritesRepository(testFilePath);
        var id = "test-item";

        var firstInsert = await repository.InsertAsync(id, CancellationToken.None);
        var secondInsert = await repository.InsertAsync(id, CancellationToken.None);

        Assert.True(firstInsert);
        Assert.False(secondInsert);
    }

    [Fact]
    public async Task DeleteAsyncTest()
    {
        var repository = new FavoritesRepository(testFilePath);
        var id = "test-item";

        var insert = await repository.InsertAsync(id, CancellationToken.None);
        var delete = await repository.DeleteAsync(id, CancellationToken.None);
        var items = await repository.GetAllAsync(CancellationToken.None);

        Assert.True(insert);
        Assert.True(delete);
        Assert.DoesNotContain(id, items);
    }

    [Fact]
    public async Task SaveChangesAsyncTest()
    {
        try
        {
            var repository = new FavoritesRepository(testFilePath);
            var id = "test-item";

            var insert = await repository.InsertAsync(id, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);
            var fileContent = await File.ReadAllTextAsync(testFilePath);
            var savedItems = JsonSerializer.Deserialize<IEnumerable<string>>(fileContent) ?? [];

            Assert.True(insert);
            Assert.Contains(id, savedItems);
        }
        finally
        {
            DeleteTestFile(testFilePath);
        }
    }

    [Fact]
    public async Task InitializeAsyncLoadItemsFromFileTest()
    {
        try
        {
            var initialFavorites = new[] { "item1", "item2" };
            await File.WriteAllTextAsync(testFilePath, JsonSerializer.Serialize(initialFavorites));

            var repository = new FavoritesRepository(testFilePath);
            var items = await repository.GetAllAsync(CancellationToken.None);

            Assert.Contains(initialFavorites[0], items);
            Assert.Contains(initialFavorites[1], items);
        }
        finally
        {
            DeleteTestFile(testFilePath);
        }
    }

    private static void DeleteTestFile(string testFile)
    {
        if (File.Exists(testFile))
        {
            File.Delete(testFile);
        }
    }
}
