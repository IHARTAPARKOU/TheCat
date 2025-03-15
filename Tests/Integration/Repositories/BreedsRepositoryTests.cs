using DataAccess.Repositories;
using DataAccess.Services;
using Tests.Integration.Fixtures;

namespace Tests.Integration.Repositories;

public class BreedsRepositoryTests : IClassFixture<ConfigurationFixture>
{
    private readonly BreedsRepository breedsRepository;

    public BreedsRepositoryTests(ConfigurationFixture configuration)
    {
        var connectionService = new ConfigurationService(configuration.Configuration);
        breedsRepository = new(configuration.HttpClient, connectionService);
    }

    [Fact]
    public async Task GetAsyncLimitTest()
    {
        var result = await breedsRepository.GetWithLimitAsync(1, 0, CancellationToken.None);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetAsyncByNameTest()
    {
        var result = await breedsRepository.GetByIdAsync("ragd", CancellationToken.None);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetFactsAsyncTest()
    {
        //This is a premium feature
        var exception = await Assert.ThrowsAsync<HttpRequestException>(() => breedsRepository.GetFactsAsync("ragd", CancellationToken.None));
        Assert.Equal("Response status code does not indicate success: 401 (Unauthorized).", exception.Message);
    }

    [Fact]
    public async Task SearchAsyncTest()
    {
        var result = await breedsRepository.SearchAsync("air", CancellationToken.None);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task SearchWithImageAsyncTest()
    {
        var result = await breedsRepository.SearchAsync("air", attachImage: true, CancellationToken.None);
        Assert.NotEmpty(result);
    }
}