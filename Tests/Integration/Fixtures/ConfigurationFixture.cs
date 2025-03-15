using Microsoft.Extensions.Configuration;

namespace Tests.Integration.Fixtures;

public class ConfigurationFixture
{
    public ConfigurationFixture()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    internal HttpClient HttpClient { get; } = new();
    internal IConfigurationRoot Configuration { get; }
}
