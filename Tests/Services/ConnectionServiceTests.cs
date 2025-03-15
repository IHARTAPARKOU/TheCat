using DataAccess.Services;
using Tests.Integration.Fixtures;

namespace Tests.Services;

public class ConnectionServiceTests(ConfigurationFixture configuration) : IClassFixture<ConfigurationFixture>
{
    private readonly ConfigurationService connectionService = new(configuration.Configuration);

    [Fact]
    public void TheCatApiKeyTest()
    {
        Assert.NotEmpty(connectionService.TheCatApiKey);
    }

    [Fact]
    public void TheCatApiBaseUriTest()
    {
        Assert.Equal("https://api.thecatapi.com/v1", connectionService.TheCatApiBaseUri);
    }
}
